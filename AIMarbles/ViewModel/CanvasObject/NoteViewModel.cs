using AIMarbles.Core;
using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.Extension;
using AIMarbles.Model;
using AIMarbles.MusicTheory;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Reactive.Bindings.Extensions;
using System.Diagnostics;
using System.Reactive.Linq;

namespace AIMarbles.ViewModel
{
    public partial class NoteViewModel: CanvasObjectViewModelBase
    {

        private static readonly Pitch _defaultPitch = new Pitch(NoteName.C, 4); // Default pitch C4

        [ObservableProperty]
        public NoteName _noteName;

        [ObservableProperty]
        public int _octave;

        public State<Pitch> PitchState = new State<Pitch>(_defaultPitch.Copy());

        protected override List<Type> _allowedConnectionsList() => [typeof(DelayOperatorViewModel), typeof(ChannelViewModel)];

        [RelayCommand]
        private void IncrementOctave()
        {
            Octave++;
            Trace.WriteLine($"ViewModel: Octave incremented to: {Octave}");
        }

        [RelayCommand]
        private void DecrementOctave() 
        {
            Octave--;
            Trace.WriteLine($"ViewModel: Octave decremented to: {Octave}");
        }

        public NoteViewModel(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine) : base(canvasObjectService, marbleMachineEngine)
        {
            AddDisposables([
                this.ObserveProperty(vm => vm.NoteName)
                .CombineLatest(this.ObserveProperty(vm => vm.Octave))
                .Select(tuple => new Pitch(tuple.First, tuple.Second))
                .StartWith(_defaultPitch.Copy())
                .Subscribe( newPitch =>
                {
                    _noteName = newPitch.Note;
                    _octave = newPitch.Octave;
                    Console.WriteLine($"ViewModel: Pitch changed to: {newPitch}");
                    PitchState.SetState(newPitch);
                })
]
            );
        }

    }
}
