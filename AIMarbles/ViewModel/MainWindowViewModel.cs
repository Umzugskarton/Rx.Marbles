using AIMarbles.Core;
using AIMarbles.Core.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace AIMarbles.ViewModel
{
    internal partial class MainWindowViewModel : ViewModelBase
   {
        private readonly CanvasObjectService _canvasObjectService;

        [ObservableProperty]
        public IEnumerable<CanvasObjectViewModelBase> _items = new List<CanvasObjectViewModelBase>();

        [ObservableProperty]
        private CanvasObjectViewModelBase? _selectedDraggableItem;

        public MainWindowViewModel(CanvasObjectService canvasObjectService) {
            _canvasObjectService = canvasObjectService;
            SubscribeToItemsState();
        }

        [RelayCommand]
        private void AddAction()
        {
            _canvasObjectService.AddAction();
        }

        [RelayCommand]
        private void AddTrigger()
        {
            _canvasObjectService.AddTrigger();
        }

        [RelayCommand]
        private void AddOperator()
        {
            _canvasObjectService.AddOperator();
        }

        [RelayCommand]
        private void SelectItem(CanvasObjectViewModelBase item)
        {
            if (item == null) return;

            if(SelectedDraggableItem == item)
            {
                SelectedDraggableItem.IsSelected = false;
                SelectedDraggableItem = null;
                return;
            }
            if (SelectedDraggableItem != null && SelectedDraggableItem != item)
            {
                SelectedDraggableItem.IsSelected = false;
            }

            SelectedDraggableItem = item;
            SelectedDraggableItem.IsSelected = true;
            Trace.WriteLine($"Selected: {SelectedDraggableItem.Name}");
        }

        [RelayCommand]
        private void RemoveItem()
        {
            if (SelectedDraggableItem == null) {
                return;
            }

            _canvasObjectService.RemoveCanvasObject(SelectedDraggableItem);
        }

        private void SubscribeToItemsState()
        {
            AddDisposable(
                _canvasObjectService.SubscribeToCanvasObjects(newItems =>
                {
                    Trace.WriteLine($"Count {newItems.Count()}");
                    Items = newItems;
                }
            ));
        }
    }
}
