using AIMarbles.Core;
using AIMarbles.Core.Interface;

namespace AIMarbles.ViewModel.CanvasObject
{
    public  class TransposeOperatorViewModel: CanvasObjectViewModelBase
    {
        protected override List<Type> _allowedConnectionsList()=> [typeof(DelayOperatorViewModel),typeof(TransposeOperatorViewModel), typeof(ChannelViewModel)];

        public TransposeOperatorViewModel(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine) : base(canvasObjectService, marbleMachineEngine) { }
    }
}
