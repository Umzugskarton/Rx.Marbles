﻿using AIMarbles.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Core.Interface.Pipeline
{
    public interface IMarbleMachineConductor : IMarbleMachineActor
    {
        IObservable<MIDIMarble> MarbleSource { get; }
    }
}
