﻿using Mafi;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using Mafi.Core.Prototypes;

namespace ProgramableNetwork.Python
{
    public class Import : IStatement
    {
        public readonly QualifiedName name;
        public readonly List<Token> exportedItems;

        public Import(QualifiedName name, List<Token> exportedItems)
        {
            this.name = name;
            this.exportedItems = exportedItems;
        }

        public void Execute(IDictionary<string, dynamic> context)
        {
            string name = this.name.Concat;

            if (name == "Core.categories") // ignore other types
                context["DefaultCategories"] = Category.Categories();

            else if (name == "Core.entities")
            {
                exportedItems.Select(argument =>
                {
                    EntityType entityType = (EntityType)Enum.Parse(typeof(EntityType), argument.value);

                    switch (entityType)
                    {
                        case EntityType.Entity: return (argument.value, typeof(Entity));
                        case EntityType.StaticEntity: return (argument.value, typeof(StaticEntity));
                        case EntityType.StorageBase: return (argument.value, typeof(StorageBase));
                        case EntityType.Controller: return (argument.value, typeof(Controller));
                        case EntityType.Antena: return (argument.value, typeof(Antena));
                        case EntityType.Machine: return (argument.value, typeof(Machine));
                        case EntityType.SettlementHousingModule: return (argument.value, typeof(SettlementHousingModule));
                        case EntityType.SettlementFoodModule: return (argument.value, typeof(SettlementFoodModule));
                        case EntityType.SettlementTransformer: return (argument.value, typeof(SettlementTransformer));
                        case EntityType.SettlementWasteModule: return (argument.value, typeof(SettlementWasteModule));
                        case EntityType.SettlementServiceModule: return (argument.value, typeof(SettlementServiceModule));

                        default:
                            throw new NotImplementedException($"Entity type '{entityType}' is not implemented");
                    }
                }).Call(p => context[p.value] = p.Item2).ToList();
            }

            else if (name == "Core.fields")
            {
                // TODO generation by fields
            }

            else if (name == "Core.io")
            {
                exportedItems.Select(argument =>
                {
                    if (argument.value == "Input")
                    {
                        return (argument.value, new Constructor(
                            (dynamic[] args) => new ModuleConnectorProtoDefinition(
                                (string)(object)args[0],
                                (string)(object)args[1]
                            )));
                    }
                    else if (argument.value == "Output")
                    {
                        return (argument.value, new Constructor(
                            (dynamic[] args) => new ModuleConnectorProtoDefinition(
                                (string)(object)args[0],
                                (string)(object)args[1]
                            )));
                    }
                    else if (argument.value == "Display")
                    {
                        return (argument.value, new Constructor(
                            (dynamic[] args) => new ModuleConnectorProtoDefinition(
                                (string)(object)args[0],
                                (string)(object)args[1],
                                (int)(object)args[3],
                                (string)(object)args[4]
                            )));
                    }
                    else
                    {
                        throw new NotImplementedException($"IO type '{argument.value}' is not implemented");
                    }
                }).Call(p => context[p.value] = p.Item2).ToList();
            }

            else if (name == "Core.module")
            {
                context["Module"] = typeof(Module);
                context["DefaultControllers"] = new Dictionary<string, StaticEntityProto.ID>()
                {
                    { "Controller", NewIds.Controllers.Controller }
                };
            }

            else
            {
                throw new System.NotImplementedException();
            }
        }
    }
}