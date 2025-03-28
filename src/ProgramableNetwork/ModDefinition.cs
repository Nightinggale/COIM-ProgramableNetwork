﻿using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Mods;
using Newtonsoft.Json;
using ProgramableNetwork.Data.Mod;
using System;

namespace ProgramableNetwork
{
    public sealed class ModDefinition : DataOnlyMod {

        public static readonly string ModName = "Programable Network";

        // Name of this mod. It will be eventually shown to the player.
        public override string Name => ModName;

        // Version, currently unused.
        public override int Version => 9;

        public bool IsBeingLoaded => throw new NotImplementedException();


        // Mod constructor that lists mod dependencies as parameters.
        // This guarantee that all listed mods will be loaded before this mod.
        // It is a good idea to depend on both `Mafi.Core.CoreMod` and `Mafi.Base.BaseMod`.
        public ModDefinition(CoreMod coreMod, BaseMod baseMod) {
            // You can use Log class for logging. These will be written to the log file
            // and can be also displayed in the in-game console with command `also_log_to_console`.
            Log.Info($"{ModName}: constructed");
        }


        public override void RegisterPrototypes(ProtoRegistrator registrator) {
            Log.Info($"{ModName}: registering prototypes");
            // Test of serialization
            try
            {
                JsonConvert.DeserializeObject<EntityInfo>(
                    JsonConvert.SerializeObject(new EntityInfo()
                    {
                        Id = 5,
                        Prototype = "proto",
                        X = 0,
                        Y = 0
                    }));
            }
            catch (Exception e)
            {
                Log.Exception(e);
                throw;
            }

            // Register all prototypes here.

            // Registers all products from this assembly. See ExampleModIds.Products.cs for examples.
            registrator.RegisterAllProducts();
            //registrator.RegisterData<Terrain>();

            // Use data class registration to register other protos such as machines, recipes, etc.
            registrator.RegisterData<Modules>();
            registrator.RegisterData<PyModules>();
            registrator.RegisterData<DataBands>();
            registrator.RegisterData<Entities>();
            registrator.RegisterData<ControllerNotification>();

            // Registers all research from this assembly. See ExampleResearchData.cs for examples.
            registrator.RegisterDataWithInterface<IResearchNodesData>();
        }
    }
}