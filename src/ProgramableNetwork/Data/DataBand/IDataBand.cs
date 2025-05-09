﻿using Mafi;
using Mafi.Core.Entities;
using System.Collections.Generic;

namespace ProgramableNetwork
{
    public interface IDataBand
    {
        EntityContext Context { set; get; }
        DataBandProto Prototype { get; }
        IEnumerable<IDataBandChannel> Channels { get; }
        Computing RequiredComputation { get; }
        Electricity RequiredPower { get; }

        void Update();
        void CreateChannel();
        void RemoveChannel(IDataBandChannel channel);
        void initContext(Antena antena);
    }
}