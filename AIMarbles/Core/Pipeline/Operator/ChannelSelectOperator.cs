using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface.Pipeline;
using AIMarbles.Model;
using System.Reactive.Linq;

namespace AIMarbles.Core.Pipeline.Operator
{
    internal class ChannelSelectOperator: IMarbleOperator
    {        
        private readonly State<int> _channelState;

        private ActorId _actorId;
        public ActorId ActorId => _actorId;
        public string Name => $"Channel {_actorId.ToString()}::{_channelState.CurrentValue}"; 


        public ChannelSelectOperator(ActorId actorId, State<int> channelState)
        {
            _actorId = actorId;
            _channelState = channelState;
        }

        public IObservable<MIDIMarble> Apply(IObservable<MIDIMarble> source)
        {
            return source
                .WithLatestFrom(_channelState.AsObservable(), (marble, channel) => new { marble, channel})
                .Select(pair =>
            {
                var newMarble = pair.marble.Clone();
                newMarble.Channel = pair.channel;

                return newMarble;
            });
        }

    }
}
