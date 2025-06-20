using AIMarbles.Core.Helper;
using AIMarbles.Core.Pipeline;
using AIMarbles.Model;

namespace AIMarbles.Core.Interface.Service
{
    public interface IMarbleMachineManager
    {
        IDisposable SubscribeToAllPipeChanges(Action<IDictionary<SequenceId, MIDIPipelineBuilder>> onChange);
        IDisposable SubscribeToPipeCreations(Action<DictionaryAddEventArgs<SequenceId, MIDIPipelineBuilder>> onChange);
        IDisposable SubscribeToPipeRemovals(Action<DictionaryRemoveEventArgs<SequenceId, MIDIPipelineBuilder>> onChange);
        IDisposable SubscribeToPipeUpdates(Action<DictionaryReplaceEventArgs<SequenceId, MIDIPipelineBuilder>> onChange);
    }
}
