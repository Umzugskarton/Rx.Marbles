using AIMarbles.Core;
using AIMarbles.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.ViewModel
{
    public class NoteViewModel: CanvasObjectViewModelBase
    {
        protected override List<Type> _allowedConnectionsList() => [typeof(DelayOperatorViewModel), typeof(ChannelViewModel)];
        public NoteViewModel(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine) : base(canvasObjectService, marbleMachineEngine) { }
    }
}
