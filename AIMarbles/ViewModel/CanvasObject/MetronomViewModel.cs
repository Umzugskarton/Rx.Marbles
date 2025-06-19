using AIMarbles.Core;
using AIMarbles.Core.Interface;

namespace AIMarbles.ViewModel
{
    public partial class MetronomViewModel: CanvasObjectViewModelBase
    {
        protected override List<Type> _allowedConnectionsList() => [typeof(DelayOperatorViewModel), typeof(NoteViewModel), typeof(ChannelViewModel)];
        public MetronomViewModel(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine): base(canvasObjectService, marbleMachineEngine) { }

    }
}
