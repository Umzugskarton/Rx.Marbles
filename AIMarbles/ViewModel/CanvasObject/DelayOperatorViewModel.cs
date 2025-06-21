using AIMarbles.Core;
using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.ViewModel.CanvasObject;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.ViewModel
{
    // TODO
    public partial class DelayOperatorViewModel: CanvasObjectViewModelBase
    {
        // TODO Change to TimeSignature 
        [ObservableProperty]
        int _delay;

        override protected List<Type> _allowedConnectionsList()=> [typeof(DelayOperatorViewModel),typeof(TransposeOperatorViewModel), typeof(ChannelViewModel)];

        public DelayOperatorViewModel(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine) : base(canvasObjectService, marbleMachineEngine) { }
    }
}
