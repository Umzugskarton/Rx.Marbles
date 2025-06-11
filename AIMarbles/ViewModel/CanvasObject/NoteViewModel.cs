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
        public NoteViewModel(ICanvasObjectService canvasObjectService) : base(canvasObjectService)
        {
        }

        protected override List<Type> _allowedConnectionsList() => [typeof(OperatorViewModel), typeof(ChannelViewModel)];
    }
}
