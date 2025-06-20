using AIMarbles.Core;
using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;

namespace AIMarbles.ViewModel.CanvasObject
{
    public partial class TransposeOperatorViewModel: CanvasObjectViewModelBase
    {
        private static readonly int _defaultTransposeValue = 0; // Default transpose value

        [ObservableProperty]
        public int _transposeValue;

        public State<int> TransposeValueState = new State<int>(_defaultTransposeValue);
        protected override List<Type> _allowedConnectionsList()=> [typeof(DelayOperatorViewModel),typeof(TransposeOperatorViewModel), typeof(ChannelViewModel)];

        public TransposeOperatorViewModel(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine) : base(canvasObjectService, marbleMachineEngine) 
        {            
            AddDisposable(
                this.ObserveProperty(vm => vm.TransposeValue)
                .StartWith(_defaultTransposeValue)
                .Subscribe( transposeValue => 
                { 
                    Console.WriteLine($"ViewModel: TransposeValue changed to: {transposeValue}");
                    _transposeValue = transposeValue;
                    TransposeValueState.SetState(transposeValue);
                })

            );
        }

    }
}
