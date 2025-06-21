using AIMarbles.ViewModel;

namespace AIMarbles.Core.Interface
{
    public interface ICanvasObjectService
    {
        IEnumerable<CanvasObjectViewModelBase> GetCanvasObjects();

        void AddChannel();

        void AddMetronom();

        void AddNote();

        void AddOperator();

        void SelectCanvasObject(CanvasObjectViewModelBase canvasObject);
        void SelectCanvasObjects(CanvasObjectViewModelBase[] canvasObjects);
        void RemoveSelectedCanvasObjects();
        void EnterConnectionMode(CanvasObjectViewModelBase from);
        void CancelConnectionMode();
        void RegisterLink(CanvasObjectViewModelBase to);
        IDisposable SubscribeToCanvasObjectsState(Action<List<CanvasObjectViewModelBase>> onChange);
        IDisposable SubscribeToCurrentConnection(Action<ConnectionViewModel?> onChange);
        IDisposable SubscribeToConnectionsState(Action<List<ConnectionViewModel>> onChange);
        IDisposable SubscribeToIsConnectionModeActiveByTypeState(Type ToConnectorType, Action<bool> onChange);
        IDisposable SubscribeToIsConnectionModeActiveState(Action<bool> onChange);
    }
}
