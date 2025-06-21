using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface.Pipeline;
using AIMarbles.Model;
using AIMarbles.MusicTheory;
using System.Diagnostics;
using System.Reactive.Linq;

namespace AIMarbles.Core.Pipeline.Conductor
{
    internal class MetronomMarbleConductor : IMarbleMachineConductor
    {
        private readonly IObservable<MIDIMarble> _marbleSource;
        private readonly State<TimeSpan> _timeSignatureState;

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
            timeSignatureState.AsObservable().Subscribe(test => Trace.WriteLine($"Changed in operator to {test.ToString()}"));
        }

        private IObservable<MIDIMarble> CreateMarbleSource(State<TimeSpan> timeSignatureState)
        {
            return timeSignatureState.AsObservable()
                    .Select(ts =>
                        {
                            return Observable.Interval(ts)
                                    .Select(_ => startMarble.Clone())
                                    .Do(marble =>
                                    {
                                        marble.Timestamp = DateTime.Now;
                                    });
                    })
                    .Switch();
        }

    }
}
