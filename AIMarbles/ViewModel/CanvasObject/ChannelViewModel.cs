using AIMarbles.Core;
using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.Core.Pipeline;
using AIMarbles.Extension;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Reactive.Linq;

namespace AIMarbles.ViewModel
{
    public partial class ChannelViewModel : OperatorViewModelBase<int>
    {
        [ObservableProperty]
        private int _channelValue;


        public State<int> ChannelState = new State<int>(0);

        override protected List<Type> _allowedConnectionsList() => [];
        public ChannelViewModel(
            ICanvasObjectService canvasObjectService,
            IMarbleMachineEngine marbleMachineEngine
            ) : base(canvasObjectService, marbleMachineEngine)
        {
            base.ValueState = ChannelState;
            SubscribeToChannelValueChanges();
        }
            
        private void SubscribeToChannelValueChanges()
        {
            AddDisposables([
                this.ObserveProperty(vm => vm.ChannelValue)
                    .StartWith(1)
                    .Subscribe(value =>
                    {
                        ChannelValue = value;
                        ChannelState.SetState(value);
                        Trace.WriteLine($"ViewModel: Channel value changed to: {value}");
                    })
            ]);
        }

    }
}
