using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface.Pipeline;
using AIMarbles.Model;
using System.Reactive.Linq;

namespace AIMarbles.Core.Pipeline.Operator
{
    class DelayOperator : IMarbleOperator
    {
        private readonly State<TimeSpan> _delayState;

        private ActorId _actorId;
        public ActorId ActorId => _actorId;
        public string Name => $"Delay {_actorId.ToString()}:::{_delayState.CurrentValue}";

        public DelayOperator(State<TimeSpan> delayState, ActorId actorId)
        {
            _delayState = delayState;
            _actorId = actorId;
        }

        public IObservable<MIDIMarble> Apply(IObservable<MIDIMarble> source)
        {
            return source.WithLatestFrom(_delayState.AsObservable(), (marble, delay) => new { marble, delay })
                         .SelectMany((pair) => Observable.Return(pair.marble).Delay(pair.delay));
        }
    }
}
