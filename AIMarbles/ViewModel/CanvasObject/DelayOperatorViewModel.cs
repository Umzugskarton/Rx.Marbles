using AIMarbles.Core;
using AIMarbles.Core.Interface;
using AIMarbles.ViewModel.CanvasObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.ViewModel
{
    public class DelayOperatorViewModel: CanvasObjectViewModelBase
    {
        override protected List<Type> _allowedConnectionsList()=> [typeof(DelayOperatorViewModel),typeof(TransposeOperatorViewModel), typeof(ChannelViewModel)];

        public DelayOperatorViewModel(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine) : base(canvasObjectService, marbleMachineEngine) { }
    }
}
