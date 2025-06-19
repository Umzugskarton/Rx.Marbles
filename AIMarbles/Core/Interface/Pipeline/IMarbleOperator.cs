using AIMarbles.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Core.Interface.Pipeline
{
    public interface IMarbleOperator : IMarbleMachineActor
    {
        IObservable<MIDIMarble> Apply(IObservable<MIDIMarble> source);
    }
}
