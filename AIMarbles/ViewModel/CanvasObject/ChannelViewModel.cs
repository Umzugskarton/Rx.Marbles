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
        public ChannelViewModel(ICanvasObjectService canvasObjectService) : base(canvasObjectService)
        {
        }

        override protected List<Type> _allowedConnectionsList() => [typeof(OperatorViewModel), typeof(ChannelViewModel)];

    }
}
