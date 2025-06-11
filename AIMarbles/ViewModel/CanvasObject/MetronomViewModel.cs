using AIMarbles.Core;
using AIMarbles.Core.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace AIMarbles.ViewModel
{
    public partial class MetronomViewModel: CanvasObjectViewModelBase
    {
        protected override List<Type> _allowedConnectionsList() => [typeof(OperatorViewModel), typeof(ChannelViewModel)];

        [ObservableProperty]
        private bool isConnectionModeActive = false;
        
        public MetronomViewModel(ICanvasObjectService canvasObjectService): base(canvasObjectService)
        {
            SubscribeToConnectionModeState();
        }

        private void SubscribeToConnectionModeState()
        {
            AddDisposable(
                    _canvasObjectService.SubscribeToIsConnectionModeActiveState(isActive => {
                        Trace.WriteLine($"Is Active {isActive}");
                        IsConnectionModeActive = isActive;
                    })
                );
        }

    }
}
