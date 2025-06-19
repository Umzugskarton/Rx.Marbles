using AIMarbles.Core.Helper;
using AIMarbles.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Core.Pipeline.Operator
{
    internal class ChannelSelectOperator
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
