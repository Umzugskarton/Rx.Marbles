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
        public State<IEnumerable<DraggableView>> ItemsState;

        [ObservableProperty]
        public IEnumerable<DraggableView> _items = new List<DraggableView>();

        [ObservableProperty]
        private DraggableView? _selectedDraggableItem;
        public MainWindowViewModel() { 
            ItemsState = new State<IEnumerable<DraggableView>>(new List<DraggableView>());
            SubscribeToItemsState();
        }

        [RelayCommand]
        private void AddAction()
        {
            ItemsState.SetState(ItemsState.CurrentValue.Append(new DraggableView { X = 50, Y = 50 }));
        }

        [RelayCommand]
        private void AddTrigger()
        {
            ItemsState.SetState(ItemsState.CurrentValue.Append(new DraggableView { X = 50, Y = 50 }));
        }

        [RelayCommand]
        private void SelectItem(DraggableView item)
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

            IEnumerable<DraggableView> items = ItemsState.CurrentValue;

            if (!items.Contains(SelectedDraggableItem)) {
                return;
            }

            List<DraggableView> itemsList = items.ToList();
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
