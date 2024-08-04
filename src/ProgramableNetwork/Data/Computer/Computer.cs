﻿using Mafi;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Ports.Io;
using System;
using Mafi.Serialization;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Factory.Transports;
using System.Collections.Generic;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Base;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Collections.ImmutableCollections;

namespace ProgramableNetwork
{
    [GenerateSerializer(false, null, 0)]
    public class Computer : LayoutEntity, IAreaSelectableEntity, IEntityWithCloneableConfig, IEntityWithSimUpdate, IUnityConsumingEntity, IElectricityConsumingEntity
    {
        private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction = delegate(object obj, BlobWriter writer)
	    {
		    ((Computer) obj).SerializeData(writer);
        };
        private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction = delegate (object obj, BlobReader reader)
	    {
		    ((Computer) obj).DeserializeData(reader);
        };

        public Computer(EntityId id, ComputerProto proto, TileTransform transform, EntityContext context)
            : base(id, proto, transform, context)
        {
            Prototype = proto;
            ErrorMessage = "";
            Instructions = new List<Instruction>();
            m_unityConsumer = context.UnityConsumerFactory.CreateConsumer(this);
            m_electricConsumer = context.ElectricityConsumerFactory.CreateConsumer(this);
        }

        [DoNotSave(0, null)]
        private ComputerProto m_proto;
        [DoNotSave(0, null)]
        private Mafi.Core.Entities.Static.StaticEntityProto.ID m_protoId;

        [DoNotSave(0, null)]
        public new ComputerProto Prototype
        {
            get
            {
                return m_proto;
            }
            protected set
            {
                m_proto = value;
                m_protoId = m_proto.Id;
                base.Prototype = value;
            }
        }

        [DoNotSave(0, null)]
        public override bool CanBePaused => true;
        [DoNotSave(0, null)]
        public bool CanWorkOvertime => true;

        public void AddToConfig(EntityConfigData data)
        {
            data.SetArray<Instruction>("instructions", Instructions.ToImmutableArray(), (i, w) => i.SerializeData(w, true));
        }

        public void ApplyConfig(EntityConfigData data)
        {
            SetPaused(true);

            var instructions = data.GetArray<Instruction>("instructions", (i) => Instruction.Deserialize(i, out var ins) ? ins : Instruction.Invalid());
            Instructions.Clear();

            foreach (var itemf in instructions)
            {
                var item = itemf;
                item.Recontext(this);
                item.ValidateEntities(this);

                if (item.Prototype.InstructionLevel > Prototype.InstructionLevel)
                    item = Instruction.Invalid(this);

                Instructions.Add(item);
            }
        }

        public static void Serialize(Computer value, BlobWriter writer)
        {
            if (writer.TryStartClassSerialization(value))
            {
                writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
            }
        }

        public static Computer Deserialize(BlobReader reader)
        {
            if (reader.TryStartClassDeserialization(out Computer value, (Func<BlobReader, Type, Computer>)null))
            {
                reader.EnqueueDataDeserialization(value, s_deserializeDataDelayedAction);
            }
            return value;
        }

        [DoNotSave(0, null)]
        private readonly int SerializerVersion = 0;
        [DoNotSave(0, null)]
        private Program m_program;

        [InitAfterLoad(InitPriority.Normal)]
        [OnlyForSaveCompatibility(null)]
        private void initContexts(int saveVersion)
        {
            foreach (var item in Instructions)
            {
                item.Recontext(this);
            }

            Prototype = Context.ProtosDb.Get<ComputerProto>(m_protoId).ValueOrThrow("Invalid computer proto");
            m_electricConsumer = Context.ElectricityConsumerFactory.CreateConsumer(this);
        }

        protected override void SerializeData(BlobWriter writer)
        {
            base.SerializeData(writer);
            writer.WriteString(m_protoId.Value);
            writer.WriteInt(SerializerVersion);
            writer.WriteInt(Instructions.Count);

            foreach (var instruction in Instructions)
            {
                instruction.SerializeData(writer);
            }

            writer.WriteString(ErrorMessage ?? "");
        }

        protected override void DeserializeData(BlobReader reader)
        {
            base.DeserializeData(reader);
            m_protoId = new Mafi.Core.Entities.Static.StaticEntityProto.ID(reader.ReadString());
            int version = reader.ReadInt();

            CurrentInstruction = 0;

            this.Instructions = new List<Instruction>();
            int countOfInstructions = reader.ReadInt();
            for (int i = 0; i < countOfInstructions; i++)
            {
                if (Instruction.Deserialize(reader, out Instruction instruction))
                    this.Instructions.Add(instruction);
                else
                    this.Instructions.Add(Instruction.Invalid());
            }
            ErrorMessage = reader.ReadString();

            reader.RegisterInitAfterLoad(this, "initContexts", InitPriority.Normal);
        }

        [DoNotSave(0, null)]
        public Upoints MonthlyUnityConsumed => 0.Upoints();

        [DoNotSave(0, null)]
        public Upoints MaxMonthlyUnityConsumed => 0.Upoints();

        public Proto.ID UpointsCategoryId => IdsCore.UpointsCategories.Boost;

        [DoNotSave(0, null)]
        public Option<UnityConsumer> UnityConsumer => m_unityConsumer;
        [DoNotSave(0, null)]
        private UnityConsumer m_unityConsumer;

        [DoNotSave(0, null)]
        public int CurrentInstruction { get; private set; }

        [DoNotSave(0, null)]
        public Electricity PowerRequired { get; private set; } = Electricity.Zero;

        [DoNotSave(0, null)]
        public Option<IElectricityConsumerReadonly> ElectricityConsumer => ((IElectricityConsumerReadonly)m_electricConsumer).SomeOption();
        [DoNotSave(0, null)]
        private IElectricityConsumer m_electricConsumer;

        [DoNotSave(0, null)]
        public List<Instruction> Instructions { get; private set; }
        [DoNotSave(0, null)]
        public string ErrorMessage { get; private set; }

        [DoNotSave(0, null)]
        public bool IsDebug { get; private set; }

        [DoNotSave(0, null)]
        public bool WaitForUser { get; private set; }

        public void SimUpdate()
        {
            if (IsNotEnabled && IsNotPaused)
            {
                return;
            }

            if (!m_electricConsumer.TryConsume())
            {
                return;
            }

            if (Instructions.Count == 0)
            {
                CurrentInstruction = 0;
                PowerRequired = Prototype.IddlePower;
                m_program = null;
                return;
            }

            int instructionIndex = CurrentInstruction;
            m_program = m_program ?? new Program(this);
            var instruction = Instructions[CurrentInstruction];

            if (!WaitForUser)
            {
                try
                {
                    if (instruction.GetLength() > Prototype.UsableTime)
                    {
                        SetPaused(true);
                        instruction.SetError(NewIds.Texts.WatchDogStop);
                        return;
                    }

                    instruction.Run(m_program);

                    ErrorMessage = "";
                    if (m_program.ContinueInstruction != null)
                    {
                        if (m_program.ContinueInstruction == 0)
                        {
                            m_program.ContinueInstruction = null;
                            CurrentInstruction = Instructions.Count;
                        }
                        else
                        {
                            for (int i = 0; i < Instructions.Count; i++)
                            {
                                if (m_program.ContinueInstruction == Instructions[i].UniqueId)
                                {
                                    m_program.ContinueInstruction = null;
                                    CurrentInstruction = i;
                                    break;
                                }
                            }

                            if (m_program.ContinueInstruction != null)
                            {
                                throw new ProgramException(NewIds.Texts.InvalidInstruction);
                            }
                        }
                    }
                    else
                    {
                        CurrentInstruction++;
                    }
                }
                catch (ProgramException e)
                {
                    instruction.SetError(e.Message);
                    ErrorMessage = $"{instructionIndex:D3}: {e.Message.Value}";
                    Log.Debug($"{ModDefinition.ModName}: {e.Message}");
                    Log.Debug(e.StackTrace);
                    SetPaused(true);
                }
                catch (Exception e)
                {
                    instruction.SetError(NewIds.Texts.UnknownError);
                    Log.Error($"{ModDefinition.ModName}: Error during execution of program");
                    Log.Exception(e);
                    ErrorMessage = $"{instructionIndex:D3}: {e.Message}";
                    SetPaused(true);
                }
            }

            if (CurrentInstruction == Instructions.Count)
                CurrentInstruction = 0;

            PowerRequired = 
                Prototype.IddlePower +
                Prototype.WorkingPower
                .ScaledBy((100 - 100.0 * instruction.GetLength() / Prototype.UsableTime).Percent());

            if (IsDebug && !WaitForUser)
            {
                WaitForUser = true;
            }

            if (!IsDebug && WaitForUser)
            {
                WaitForUser = false;
            }

        }
    }
}
