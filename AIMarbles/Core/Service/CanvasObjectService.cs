using AIMarbles.Core.Helpers;
using AIMarbles.Core.Interface;
using AIMarbles.Core.Interface.Factory;
using AIMarbles.ViewModel;
using System.Diagnostics;
using System.Reactive.Linq;

namespace AIMarbles.Core.Service
{
    internal class CanvasObjectService : ICanvasObjectService
    {
        private State<List<CanvasObjectViewModelBase>> _canvasObjectsState;
        private State<List<ConnectionViewModel>> _connectionsState;
        private State<CanvasObjectViewModelBase?> _fromConnectorSetState;
        private IObservable<bool> _isConnectionModeActive;

        private readonly IViewModelFactory<ChannelViewModel> _channelViewModelFactory;
        private readonly IViewModelFactory<NoteViewModel> _noteViewModelFactory;
        private readonly IViewModelFactory<MetronomViewModel> _metronomViewModelFactory;
        private readonly IViewModelFactory<ConnectionViewModel> _connectionViewModelFactory;
        private readonly IViewModelFactory<OperatorViewModel> _operatorViewModelFactory;

        public CanvasObjectService(
            IViewModelFactory<ChannelViewModel> channelViewModelFactory,
            IViewModelFactory<NoteViewModel> noteViewModelFactory,
            IViewModelFactory<MetronomViewModel> metronomViewModelFactory,
            IViewModelFactory<ConnectionViewModel> connectionViewModelFactory,
            IViewModelFactory<OperatorViewModel> operatorViewModelFactory
        )
        {
            _canvasObjectsState = new State<List<CanvasObjectViewModelBase>>(new List<CanvasObjectViewModelBase>());
            _fromConnectorSetState = new State<CanvasObjectViewModelBase?>(null);
            _connectionsState = new State<List<ConnectionViewModel>>(new List<ConnectionViewModel>());
            _isConnectionModeActive = _fromConnectorSetState.AsObservable().Select(state => state != null);
            _channelViewModelFactory = channelViewModelFactory;
            _noteViewModelFactory = noteViewModelFactory;
            _metronomViewModelFactory = metronomViewModelFactory;
            _connectionViewModelFactory = connectionViewModelFactory;
            _operatorViewModelFactory = operatorViewModelFactory;
        }

        public IEnumerable<CanvasObjectViewModelBase> GetCanvasObjects()
        {
            return _canvasObjectsState.CurrentValue;
        }

        public void AddNote()
        {
            var newAction = _noteViewModelFactory.Create();
            newAction.X = 50;
            newAction.Y = 50;
            AddCanvasObject(newAction);
        }

        public void AddChannel()
        {
            var newChannel = _channelViewModelFactory.Create();
            newChannel.X = 50;
            newChannel.Y = 50;
            AddCanvasObject(newChannel);
        }

        public void AddMetronom()
        {
            var newMetronom = _metronomViewModelFactory.Create();
            newMetronom.X = 50;
            newMetronom.Y = 50;
            AddCanvasObject(newMetronom);
        }

        public void AddOperator()
        {
            var newOperator = _operatorViewModelFactory.Create();
            newOperator.X = 50;
            newOperator.Y = 50;
            AddCanvasObject(newOperator);
        }

        public void EnterConnectionMode(CanvasObjectViewModelBase from)
        {
            var newConnection = _connectionViewModelFactory.Create();
            newConnection.From = from;
            AddConnection(newConnection);
            _fromConnectorSetState.SetState(from);
        }

        public void RegisterLink(CanvasObjectViewModelBase to)
        {
            var newConnection = _connectionsState.CurrentValue.Last();
            newConnection.To = to;
            _fromConnectorSetState.SetState(null);
        }

        public void RemoveSelectedCanvasObjects()
        {
            _canvasObjectsState.CurrentValue
                .Where(canvasObject => canvasObject.IsSelected)
                .ToList()
                .ForEach(RemoveCanvasObject);
        }

        public void SelectCanvasObject(CanvasObjectViewModelBase canvasObject)
        {
            var objects = _canvasObjectsState.CurrentValue;
            foreach (var obj in objects)
            {
                obj.IsSelected = (obj == canvasObject);
            }
            _canvasObjectsState.SetState(objects.ToList());
        }

        public void SelectCanvasObjects(CanvasObjectViewModelBase[] canvasObjects)
        {
            var objects = _canvasObjectsState.CurrentValue;
            foreach (var obj in objects)
            {
                obj.IsSelected = canvasObjects.Contains(obj);
            }
            _canvasObjectsState.SetState(objects.ToList());
        }

        private void RemoveCanvasObject(CanvasObjectViewModelBase item)
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

        private void AddConnection(ConnectionViewModel connection)
        {
            if (connection == null) return;
            if (_connectionsState.CurrentValue.Contains(connection)) return;
            var newList = _connectionsState.CurrentValue.ToList();
            newList.Add(connection);
            _connectionsState.SetState(newList);
        }
        public IDisposable SubscribeToCanvasObjectsState(Action<List<CanvasObjectViewModelBase>> onChange)
              => _canvasObjectsState.Subscribe(onChange);

        public IDisposable SubscribeToActiveConnection(Action<ConnectionViewModel?> onChange) 
            => _isConnectionModeActive
            .Select(isActive => isActive ? _connectionsState.CurrentValue.LastOrDefault() : null)
            .Subscribe(onChange);

        public IDisposable SubscribeToIsConnectionModeActiveState(Action<bool> onChange)
            => _isConnectionModeActive.Subscribe(onChange);

        public IDisposable SubscribeToConnectionsState(Action<List<ConnectionViewModel>> onChange)
            => _connectionsState.Subscribe(onChange);
        
    }
}
