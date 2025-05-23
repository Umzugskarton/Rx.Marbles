using AIMarbles.Core.Helpers;
using AIMarbles.ViewModel;

namespace AIMarbles.Core.Service
{
    internal class CanvasObjectService
    {
        private State<List<CanvasObjectViewModelBase>> _canvasObjectsState;

        public CanvasObjectService()
        {
            _canvasObjectsState = new State<List<CanvasObjectViewModelBase>>(new List<CanvasObjectViewModelBase>());
        }


        public IEnumerable<CanvasObjectViewModelBase> GetCanvasObjects()
        {
            return _canvasObjectsState.CurrentValue;
        }

        public void AddAction()
        {
            var newAction = new ActionViewModel { X = 50, Y = 50 };
            AddCanvasObject(newAction);
        }

        public void AddTrigger()
        {
            var newTrigger = new TriggerViewModel { X = 50, Y = 50 };
            AddCanvasObject(newTrigger);
        }

        public void AddOperator()
        {
            var newOperator = new OperatorViewModel { X = 50, Y = 50 };
            AddCanvasObject(newOperator);
        }

        public void RemoveCanvasObject(CanvasObjectViewModelBase item)
        {
            if (item == null) return;
            if (!_canvasObjectsState.CurrentValue.Contains(item)) return;
            var newList = _canvasObjectsState.CurrentValue.ToList();
            newList.Remove(item);
            _canvasObjectsState.SetState(newList);
        }

        private void AddCanvasObject(CanvasObjectViewModelBase item)
        {
            if (item == null) return;
            if (_canvasObjectsState.CurrentValue.Contains(item)) return;
            var newList = _canvasObjectsState.CurrentValue.ToList();
            newList.Add(item);
            _canvasObjectsState.SetState(newList);
        }

        public IDisposable SubscribeToCanvasObjects(Action<List<CanvasObjectViewModelBase>> onChange)
              => _canvasObjectsState.Subscribe(onChange);
    }
}
