using AIMarbles.Core;
using AIMarbles.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.ViewModel
{
    public class ChannelViewModel : CanvasObjectViewModelBase
    {
        public ChannelViewModel(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine) : base(canvasObjectService, marbleMachineEngine)
        {
        }

        override protected List<Type> _allowedConnectionsList() => [];

    }
}
