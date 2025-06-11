using AIMarbles.Core;
using AIMarbles.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.ViewModel
{
    internal class OperatorViewModel: CanvasObjectViewModelBase
    {
        public OperatorViewModel(ICanvasObjectService canvasObjectService) : base(canvasObjectService)
        {
        }

        protected override List<Type> _allowedConnectionsList()=> [typeof(OperatorViewModel), typeof(ChannelViewModel)];

    }
}
