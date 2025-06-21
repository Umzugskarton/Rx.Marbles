using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface.Pipeline;
using AIMarbles.Model;
using AIMarbles.MusicTheory;
using System.Diagnostics;
using System.Reactive.Linq;

namespace AIMarbles.Core.Pipeline.Operator
{
    internal class NoteSelectOperator : IMarbleOperator
    {

        private readonly State<Pitch> _pitchState;
       
        private ActorId _actorId;
        public ActorId ActorId => _actorId;
        public string Name => $"Note {_actorId.ToString()}::{_pitchState.CurrentValue}";

        public NoteSelectOperator(ActorId actorId, State<Pitch> pitchState)
        {
            _actorId = actorId;
            _pitchState = pitchState;
            pitchState.AsObservable().Subscribe(test => Trace.WriteLine($"Changed in operator to {test.ToString()}"));
        }

        public IObservable<MIDIMarble> Apply(IObservable<MIDIMarble> source)
        {
            return source
                .WithLatestFrom(_pitchState.AsObservable())
                .Select(pair =>
                {

                    var (marble, pitch) = pair;
                    Trace.WriteLine($" Note {pitch.ToString()}");

                    var newMarble = marble.Clone();
                    newMarble.Pitch = pitch;

                    return newMarble;
                });
        }
    }
}
