﻿using Mafi.Core.Entities;
using Mafi.Collections;
using Mafi.Serialization;
using System.Collections.Generic;
using Mafi;

namespace ProgramableNetwork
{
    public partial class DataBands
    {
        public class UnkownnDataBandType : IDataBand, IDataBandTyped<IDataBandChannel>
        {
            public UnkownnDataBandType(EntityContext context, DataBandProto proto)
            {
                Context = context;
                Prototype = proto;
            }

            public EntityContext Context { set; get; }
            public DataBandProto Prototype { get; private set; }

            public IEnumerable<IDataBandChannel> Channels { get; } = new Lyst<IDataBandChannel>();

            public Computing RequiredComputation => Computing.Zero;

            public Electricity RequiredPower => Electricity.Zero;

            public void CreateChannel()
            {
            }

            public void initContext(Antena antena)
            {
                Prototype = Context.ProtosDb.Get<DataBandProto>(DataBand_Unknown).ValueOrThrow("Unknown signal not found");
            }

            public void Update()
            {
                // do nothing
            }

            public static void Serialize(UnkownnDataBandType dataBand, BlobWriter writer)
            {
            }

            public static UnkownnDataBandType Deserialize(BlobReader reader)
            {
                return new UnkownnDataBandType(null, null);
            }

            public void RemoveChannel(IDataBandChannel channel)
            {
                // do nothing
            }
        }
    }
}
