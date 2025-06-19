using AIMarbles.Core;
using AIMarbles.Core.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace AIMarbles.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public PalleteViewModel PalleteVM { get; }
        private readonly ICanvasObjectService _canvasObjectService;

        [ObservableProperty]
        private IEnumerable<CanvasObjectViewModelBase> _items = new List<CanvasObjectViewModelBase>();

        [ObservableProperty]
        private IEnumerable<ConnectionViewModel> _connections = new List<ConnectionViewModel>();

        [ObservableProperty]
        private ConnectionViewModel? _activeConnection;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CancelConnectionModeCommand))]
        private bool _isConnectionModeActive = false;

        public MainWindowViewModel
            (
                PalleteViewModel palleteViewModel,
                ICanvasObjectService canvasObjectService
            )
        {
            PalleteVM = palleteViewModel;
            _canvasObjectService = canvasObjectService;
            SubscribeToItemsState();
        }

        [RelayCommand]
        private void SelectItem(CanvasObjectViewModelBase canvasObject)
        {
            Trace.WriteLine($"Selecting canvasObject {canvasObject.Name}");
            _canvasObjectService.SelectCanvasObject(canvasObject);
        }

        [RelayCommand]
        private void CancelConnectionMode() {
            _canvasObjectService.CancelConnectionMode();
        }

        private bool CanCancelConnectionMode()
        {
            return IsConnectionModeActive;
        }

        private void SubscribeToItemsState()
        {
            AddDisposables(
                [
                    _canvasObjectService.SubscribeToCanvasObjectsState(newItems =>
                    {
                        Trace.WriteLine($"Count {newItems.Count()}");
                        Items = newItems;
                    }),

                    _canvasObjectService.SubscribeToConnectionsState(newConnections =>
                    {
                        Trace.WriteLine($"Count Connections {newConnections.Count()}");
                        Connections = newConnections;
                    }),

                    _canvasObjectService.SubscribeToCurrentConnection(newConnection =>{
                        Trace.WriteLine($"Setting ActiveConnection to {newConnection?.Name ?? "Null"}");
                        ActiveConnection = newConnection;
                    }),

                    _canvasObjectService.SubscribeToIsConnectionModeActiveState(isActive => IsConnectionModeActive = isActive  ),
                ]
            );
        }
    }
}
