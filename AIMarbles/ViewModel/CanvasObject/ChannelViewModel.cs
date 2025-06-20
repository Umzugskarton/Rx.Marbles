using AIMarbles.Core;
using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.Extension;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace AIMarbles.ViewModel
{
    public partial class ChannelViewModel : CanvasObjectViewModelBase
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
            SubscribeToChannelValueChanges();
        }
            
        private void SubscribeToChannelValueChanges()
        {
            AddDisposables([
                this.ObserveProperty(vm => vm.ChannelValue)
                    .Subscribe(value =>
                    {
                        ChannelState.SetState(value);
                        Trace.WriteLine($"ViewModel: Channel value changed to: {value}");
                    })
            ]);
        }

    }
}
