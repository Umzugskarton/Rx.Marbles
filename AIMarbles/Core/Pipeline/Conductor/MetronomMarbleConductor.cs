using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface.Pipeline;
using AIMarbles.Model;
using AIMarbles.MusicTheory;
using System.Reactive.Linq;

namespace AIMarbles.Core.Pipeline.Conductor
{
    internal class MetronomMarbleConductor : IMarbleMachineConductor
    {
        private IObservable<MIDIMarble> _marbleSource;
        private State<TimeSpan> _timeSignatureState;

       private MIDIMarble startMarble = new MIDIMarble
            {
                Pitch = new Pitch(NoteName.C, 4),
                Velocity = 100,
                DurationMs = 500,
                Channel = 1
            };

        private ActorId _actorId;
        public ActorId ActorId => _actorId;
        public string Name => $"Metronome [{_actorId}] — Time Signature: {_timeSignatureState.CurrentValue}";

        public IObservable<MIDIMarble> MarbleSource => _marbleSource;

        public MetronomMarbleConductor(ActorId actorId, State<TimeSpan> timeSignatureState)
        {
            _actorId=actorId;
            _timeSignatureState = timeSignatureState;
            _marbleSource = CreateMarbleSource(timeSignatureState);
        }

        private IObservable<MIDIMarble> CreateMarbleSource(State<TimeSpan> timeSignatureState)
        {
            return timeSignatureState.AsObservable()
                .SelectMany(ts =>
                    Observable.Interval(ts)
                        .Select(_ => startMarble.Clone())
                        .Do(marble =>
                        {
                            marble.Timestamp = DateTime.Now;
                        })
            );
        }
    }
}
