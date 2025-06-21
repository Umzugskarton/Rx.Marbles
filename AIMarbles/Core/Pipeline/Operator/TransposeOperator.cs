using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface.Pipeline;
using AIMarbles.Model;
using AIMarbles.MusicTheory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Core.Pipeline.Operator
{
    class TransposeOperator : IMarbleOperator
    {
        private readonly State<int> _semitonesState;

        private ActorId _actorId;
        public ActorId ActorId => _actorId;
        public string Name => $"Transpose {_actorId.ToString()}:::{_semitonesState.CurrentValue}";


        public TransposeOperator(ActorId actorId, State<int> semitonesState)
        {
            _actorId = actorId;
            _semitonesState = semitonesState;
        }

        public IObservable<MIDIMarble> Apply(IObservable<MIDIMarble> source)
        {
            return source
                .WithLatestFrom(_semitonesState.AsObservable(), (marble, semitones) => new { marble, semitones})
                .Select(pair =>
            {
                var newMarble = pair.marble.Clone();
                newMarble.Pitch.AddSemitones(pair.semitones);

                return newMarble;
            });
        }
    }
}
