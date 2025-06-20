using AIMarbles.Core.Interface;
using AIMarbles.Core.Interface.Service;
using AIMarbles.Model;

namespace AIMarbles.Core.Service
{
    public class MarbleMachineEngine : IMarbleMachineEngine
    {

        private readonly IMarbleMachineManager _marbleManager;
        private readonly IMidiOutputService _midiOutputService;

        private IDictionary<SequenceId, IDisposable> _marbleMachine = new Dictionary<SequenceId, IDisposable>();
        public MarbleMachineEngine(IMarbleMachineManager marbleManager, IMidiOutputService midiOutputService)
        {
            _marbleManager = marbleManager;
            _midiOutputService = midiOutputService;
            SubscribeToPipelines();
        }

        private void SendToMidiOut(MIDIMarble marble)
        {
            if (marble == null) return;
            _midiOutputService.SendToMidiOut(marble);
        }

        private void SubscribeToPipelines()
        {
            _marbleManager.SubscribeToPipeCreations(addEvent =>
            {
                _marbleMachine.Add(addEvent.Key, addEvent.Value.Build().Subscribe(SendToMidiOut));
            });

            _marbleManager.SubscribeToPipeRemovals(addEvent =>
            {
                _marbleMachine[addEvent.Key].Dispose();
                _marbleMachine.Remove(addEvent.Key);
            });

            _marbleManager.SubscribeToPipeUpdates(addEvent =>
            {
                _marbleMachine[addEvent.Key].Dispose();
                _marbleMachine[addEvent.Key] = addEvent.NewValue.Build().Subscribe(SendToMidiOut);
            });
        }

        public void Dispose()
        {
            foreach (var machine in _marbleMachine)
            {
                machine.Value.Dispose();
            }
            _marbleMachine.Clear();
            _midiOutputService.Dispose();
        }

    }
}
