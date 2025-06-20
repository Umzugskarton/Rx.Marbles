using AIMarbles.Core;
using AIMarbles.Core.ConductorCore;
using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.Extension;
using AIMarbles.Model.Enum;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Reactive.Linq;
namespace AIMarbles.ViewModel
{
    public partial class MetronomViewModel : CanvasObjectViewModelBase
    {
        [ObservableProperty]
        private int _bpm;

        [ObservableProperty]
        private TimeSignature _selectedTimeSignature;

        public State<int> BpmState = new State<int>(0);
        public State<TimeSpan> TempoState = new State<TimeSpan>(TimeSpan.Zero);

        protected override List<Type> _allowedConnectionsList() => [typeof(DelayOperatorViewModel), typeof(NoteViewModel), typeof(ChannelViewModel)];

        public MetronomViewModel(
            ICanvasObjectService canvasObjectService,
            IMarbleMachineEngine marbleMachineEngine
            ) : base(canvasObjectService, marbleMachineEngine)
        {
            SubscribeToMetronomProperties();
        }


        private void SubscribeToMetronomProperties() 
        {
            AddDisposables([

                // Observe changes to SelectedTimeSignature and update BeatsPerBar

                this.ObserveProperty(vm => vm.SelectedTimeSignature)
                    .CombineLatest(this.ObserveProperty(vm => vm.Bpm))
                    .Subscribe(pair => 
                     {
                         var (signature, bpm) = pair;
                        if (bpm <= 0) { bpm = 120; }
                        // Calculate the tempo in milliseconds per beat
                        var tempo = MetronomeCalculator.CalculateBeatIntervalMs(bpm, signature);
                        TempoState.SetState(tempo);
                        Trace.WriteLine($"ViewModel: TempoState set to: {tempo}");

                    }),

                this.ObserveProperty(vm => vm.SelectedTimeSignature)
                    .StartWith(TimeSignature.CommonTime_4_4)
                    .Subscribe(timeSignature =>
                        SelectedTimeSignature = timeSignature
                    ),

                this.ObserveProperty(vm => vm.Bpm)
                    .StartWith(120)
                    .Subscribe(bpm => Bpm = bpm)
            ]);

        }
    }
}
