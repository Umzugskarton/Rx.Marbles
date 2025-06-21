using AIMarbles.Core;
using AIMarbles.Core.ConductorCore;
using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.Core.Pipeline;
using AIMarbles.Extension;
using AIMarbles.Model.Enum;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Reactive.Linq;
namespace AIMarbles.ViewModel
{
    public partial class MetronomViewModel : OperatorViewModelBase<TimeSpan>
    {
        [ObservableProperty]
        private int _bpm;

        [ObservableProperty]
        private TimeSignature _selectedTimeSignature;


        protected override List<Type> _allowedConnectionsList() => [typeof(DelayOperatorViewModel), typeof(NoteViewModel), typeof(ChannelViewModel)];

        public MetronomViewModel(
            ICanvasObjectService canvasObjectService,
            IMarbleMachineEngine marbleMachineEngine
            ) : base(canvasObjectService, marbleMachineEngine)
        {
            base.ValueState = new State<TimeSpan>(TimeSpan.Zero);
            SubscribeToMetronomProperties();
            SelectedTimeSignature = TimeSignature.CommonTime_4_4;
            Bpm = 120;
        }


        private void SubscribeToMetronomProperties() 
        {
            AddDisposable(
                this.ObserveProperty(vm => vm.SelectedTimeSignature).StartWith(TimeSignature.CommonTime_4_4)
                    .CombineLatest(this.ObserveProperty(vm => vm.Bpm).StartWith(120))
                    .Subscribe(pair => 
                     {
                         var (signature, bpm) = pair;
                        if (bpm <= 0) { bpm = 120; }
                        // Calculate the tempo in milliseconds per beat
                        var tempo = MetronomeCalculator.CalculateBeatIntervalMs(bpm, signature);
                        Trace.WriteLine($"ViewModel: TempoState set to: {tempo}");
                         ValueState.SetState(tempo);
                    })

            );

        }
    }
}
