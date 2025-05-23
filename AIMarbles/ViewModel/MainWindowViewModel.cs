using AIMarbles.Core;
using AIMarbles.Core.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Windows.Data;

namespace AIMarbles.ViewModel
{
    internal partial class MainWindowViewModel : ViewModelBase
   {
        // this will move into a service
        public State<IEnumerable<CanvasObjectViewModelBase>> ItemsState;

        [ObservableProperty]
        public IEnumerable<CanvasObjectViewModelBase> _items = new List<CanvasObjectViewModelBase>();

        [ObservableProperty]
        private CanvasObjectViewModelBase? _selectedDraggableItem;
        public MainWindowViewModel() { 
            ItemsState = new State<IEnumerable<CanvasObjectViewModelBase>>(new List<CanvasObjectViewModelBase>());
            SubscribeToItemsState();
        }

        [RelayCommand]
        private void AddAction()
        {
            ItemsState.SetState(ItemsState.CurrentValue.Append(new ActionViewModel { X = 50, Y = 50 }));
        }

        [RelayCommand]
        private void AddTrigger()
        {
            ItemsState.SetState(ItemsState.CurrentValue.Append(new TriggerViewModel { X = 50, Y = 50 }));
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

            IEnumerable<CanvasObjectViewModelBase> items = ItemsState.CurrentValue;

            if (!items.Contains(SelectedDraggableItem)) {
                return;
            }

            List<CanvasObjectViewModelBase> itemsList = items.ToList();
            itemsList.Remove(SelectedDraggableItem);
            ItemsState.SetState(itemsList);
        }

        private void SubscribeToItemsState()
        {
            AddDisposable(
                ItemsState.Subscribe(newItems =>
            {
               Trace.WriteLine($"Count {newItems.Count()}");
                Items = newItems;
            }
            ));
        }
    }
}
