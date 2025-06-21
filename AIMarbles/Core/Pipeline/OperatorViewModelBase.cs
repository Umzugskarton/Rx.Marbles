using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Core.Pipeline
{
    public abstract class OperatorViewModelBase<T> : CanvasObjectViewModelBase 
    {
        public required State<T> ValueState { get; set; }
        protected OperatorViewModelBase(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine) : base(canvasObjectService, marbleMachineEngine) { }
    }
}
