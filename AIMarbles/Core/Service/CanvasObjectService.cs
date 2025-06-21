using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.Core.Interface.Factory;
using AIMarbles.Core.Interface.Service;
using AIMarbles.ViewModel;
using System.Diagnostics;
using System.Reactive.Linq;

namespace AIMarbles.Core.Service
{
    internal class CanvasObjectService : ICanvasObjectService
    {
        private IMarbleMachineManager _marbleMachineManager;
        private State<List<CanvasObjectViewModelBase>> _canvasObjectsState;
        private State<List<ConnectionViewModel>> _connectionsState;
        private State<CanvasObjectViewModelBase?> _currentFromConnectorState;
        private IObservable<bool> _isConnectionModeActive;

        private readonly IViewModelFactory<ChannelViewModel> _channelViewModelFactory;
        private readonly IViewModelFactory<NoteViewModel> _noteViewModelFactory;
        private readonly IViewModelFactory<MetronomViewModel> _metronomViewModelFactory;
        private readonly IViewModelFactory<ConnectionViewModel> _connectionViewModelFactory;
        private readonly IViewModelFactory<DelayOperatorViewModel> _operatorViewModelFactory;

        public CanvasObjectService(
            IMarbleMachineManager marbleMachineManager,
            IViewModelFactory<ChannelViewModel> channelViewModelFactory,
            IViewModelFactory<NoteViewModel> noteViewModelFactory,
            IViewModelFactory<MetronomViewModel> metronomViewModelFactory,
            IViewModelFactory<ConnectionViewModel> connectionViewModelFactory,
            IViewModelFactory<DelayOperatorViewModel> operatorViewModelFactory
        )
        {
            _marbleMachineManager = marbleMachineManager;
            _canvasObjectsState = new State<List<CanvasObjectViewModelBase>>(new List<CanvasObjectViewModelBase>());
            _currentFromConnectorState = new State<CanvasObjectViewModelBase?>(null);
            _connectionsState = new State<List<ConnectionViewModel>>(new List<ConnectionViewModel>());
            _isConnectionModeActive = _currentFromConnectorState.AsObservable().Select(state => state != null);
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
            var newAction = CreateCanvasObject(_noteViewModelFactory);
            _marbleMachineManager.RegisterActor(newAction);
            AddCanvasObject(newAction);
        }

        public void AddChannel()
        {
            var newChannel = CreateCanvasObject(_channelViewModelFactory);
            _marbleMachineManager.RegisterActor(newChannel);
            AddCanvasObject(newChannel);
        }

        public void AddMetronom()
        {
            var newMetronom = CreateCanvasObject(_metronomViewModelFactory);
            _marbleMachineManager.RegisterActor(newMetronom);
            AddCanvasObject(newMetronom);
        }

        public void AddOperator()
        {
            
            var newOperator = CreateCanvasObject(_operatorViewModelFactory);
            AddCanvasObject(newOperator);
        }

        private T CreateCanvasObject<T>(IViewModelFactory<T> factory) where T: CanvasObjectViewModelBase 
        { 
            var newObject = factory.Create();
            newObject.X = 50;
            newObject.Y = 50;
            return newObject;
        }

        public void EnterConnectionMode(CanvasObjectViewModelBase from)
        {
            var newConnection = _connectionViewModelFactory.Create();
            newConnection.From = from;
            AddConnection(newConnection);
            _currentFromConnectorState.SetState(from);
        }

        public void CancelConnectionMode()
        {
            var fromConnector = _currentFromConnectorState.CurrentValue;
            if(fromConnector == null) { return; }
            var newList = _connectionsState.CurrentValue.ToList();
            if(newList.Count <= 0) { return; }
            newList.RemoveAt(newList.Count - 1);
            _connectionsState.SetState(newList);
            _currentFromConnectorState.SetState(null);
        }

        public void RegisterLink(CanvasObjectViewModelBase to)
        {
            var newConnection = _connectionsState.CurrentValue.Last();
            newConnection.To = to;
            var fromConnector = _currentFromConnectorState.CurrentValue;
            if(fromConnector == null) { return; }
            _marbleMachineManager.RegisterConnection(fromConnector, to);
            _currentFromConnectorState.SetState(null);
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

        public IDisposable SubscribeToCurrentConnection(Action<ConnectionViewModel?> onChange) 
            => _isConnectionModeActive
            .Select(isActive => isActive ? _connectionsState.CurrentValue.LastOrDefault() : null)
            .Subscribe(onChange);

        public IDisposable SubscribeToIsConnectionModeActiveByTypeState(Type ToConnectorType, Action<bool> onChange)
            => _currentFromConnectorState.AsObservable()
            .Select(fromConnector => fromConnector?.isConnectionAllowed(ToConnectorType) ?? false).Subscribe(onChange);

        public IDisposable SubscribeToIsConnectionModeActiveState(Action<bool> onChange)
            => _currentFromConnectorState.AsObservable()
            .Select(fromConnector => fromConnector != null).Subscribe(onChange);

        public IDisposable SubscribeToConnectionsState(Action<List<ConnectionViewModel>> onChange)
            => _connectionsState.Subscribe(onChange);
        
    }
}
