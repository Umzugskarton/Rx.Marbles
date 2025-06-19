using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface.Pipeline;
using AIMarbles.Model;
using AIMarbles.MusicTheory;
using System.Reactive.Linq;

namespace AIMarbles.Core.Pipeline.Operator
{
    class NoteSelectOperator : IMarbleOperator
    {

        private readonly State<Pitch> _pitchState;
       
        private ActorId _actorId;
        public ActorId ActorId => _actorId;
        public string Name => $"Note {_actorId.ToString()}::{_pitchState.CurrentValue}";

        public NoteSelectOperator(ActorId actorId, State<Pitch> pitchState)
        {
            _actorId = actorId;
            _pitchState = pitchState;
        }

        public IObservable<MIDIMarble> Apply(IObservable<MIDIMarble> source)
        {
            return source
                .WithLatestFrom(_pitchState.AsObservable(), (marble, pitch) => new { marble, pitch })
                .Select(pair =>
                {
                    var newMarble = pair.marble.Clone();
                    newMarble.Pitch = pair.pitch;

                    return newMarble;
                });
        }
    }
}
