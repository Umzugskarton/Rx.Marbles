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
        void RegisterLink(CanvasObjectViewModelBase to);
        IDisposable SubscribeToCanvasObjectsState(Action<List<CanvasObjectViewModelBase>> onChange);
        IDisposable SubscribeToActiveConnection(Action<ConnectionViewModel?> onChange);
        IDisposable SubscribeToConnectionsState(Action<List<ConnectionViewModel>> onChange);
        IDisposable SubscribeToIsConnectionModeActiveState(Action<bool> onChange);
    }
}
