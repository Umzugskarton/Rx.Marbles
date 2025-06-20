using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.Core.Interface.Pipeline;
using AIMarbles.Core.Interface.Service;
using AIMarbles.Core.Pipeline;
using AIMarbles.Core.Pipeline.Conductor;
using AIMarbles.Core.Pipeline.Operator;
using AIMarbles.Extension;
using AIMarbles.Model;
using AIMarbles.ViewModel;
using AIMarbles.ViewModel.CanvasObject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Core.Service
{
    // TODO: Move into a separate file
    public record Sequence(LinkedList<IMarbleMachineActor> sequence, SequenceId id);

    public class MarbleMachineManager : IMarbleMachineManager
    {

        // Lieber doch States dann können wir auf einzelne pipeline änderungen direkt subscriben
        private ObservableRxDictionary<SequenceId, MIDIPipelineBuilder> _readyPipes;
        private readonly IDictionary<Type, Type> _translateMarbleTypes = new Dictionary<Type, Type>()
        {
            { typeof(MetronomViewModel), typeof(MetronomMarbleConductor) },
            { typeof(DelayOperatorViewModel), typeof(DelayOperator) },
            { typeof(TransposeOperatorViewModel), typeof(TransposeOperator) },
            { typeof(ChannelViewModel), typeof(ChannelSelectOperator) },
            { typeof(NoteViewModel), typeof(NoteSelectOperator) },
        };

        private IDictionary<SequenceId, LinkedList<IMarbleMachineActor>> _sequences;
        private IDictionary<ActorId, HashSet<SequenceId>> _actorInSequences;

        private IDictionary<ActorId, IMarbleMachineActor> _actors;
        public MarbleMachineManager()
        {
            _actors = new Dictionary<ActorId, IMarbleMachineActor>();
            _sequences = new Dictionary<SequenceId, LinkedList<IMarbleMachineActor>>();
            _actorInSequences = new Dictionary<ActorId, HashSet<SequenceId>>();
            _readyPipes = new ObservableRxDictionary<SequenceId, MIDIPipelineBuilder>();
        }


        /// <summary>
        /// Regosters an actor with the manager. The actor is created from the given view model type.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>bool, true if succeeded, False if not</returns>
        public bool registerActor(CanvasObjectViewModelBase viewModel)
        {
            if (_actors.ContainsKey(viewModel.ActorId))
            {
                Trace.WriteLine($"Actor with ID {viewModel.ActorId} already exists.");
                return false;
            }

            _translateMarbleTypes.TryGetValue(viewModel.GetType(), out Type? actorType);

            if (actorType == null)
            {
                Trace.WriteLine($"No actor type found for {viewModel.GetType()}");
                return false;
            }

            IMarbleMachineActor? actor = Activator.CreateInstance(actorType, viewModel) as IMarbleMachineActor;

            if (actor == null)
            {
                Trace.WriteLine($"Failed to create actor of type {actorType}");
                return false;
            }

            _actors.Add(viewModel.ActorId, actor);

            return true;
        }

        /// <summary>
        /// Registers all resulting sequences for a connection between two actors and/or their already present sequence.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool RegisterConnection(CanvasObjectViewModelBase from, CanvasObjectViewModelBase to)
        {
            _actors.TryGetValue(from.ActorId, out IMarbleMachineActor? fromActor);
            _actors.TryGetValue(to.ActorId, out IMarbleMachineActor? toActor);

            if (fromActor == null || toActor == null)
            {
                Trace.WriteLine($"Actor not found for connection from {from.ActorId} to {to.ActorId}");
                return false;
            }

            List<Sequence> beforeSequences = GetAllBefore(fromActor);
            List<Sequence> afterSequences = GetAllAfter(toActor);
            HashSet<SequenceId> updatedAndCreatedIds = new HashSet<SequenceId>();

            //Update all sequences that are before the 'from' actor to include the first 'to' actor sequence
            beforeSequences.ForEach((tuple) =>
            {
                LinkedList<IMarbleMachineActor> updatedSequence = new(tuple.sequence.Concat(afterSequences[0].sequence));
                _sequences[tuple.id] = updatedSequence;
                updatedAndCreatedIds.Add(tuple.id);
            });

            afterSequences.RemoveAt(0); // Remove the first sequence as it is already included in the before sequences

            // For every sequence after the first 'to' actor sequence, we have to create a new sequence that includes the 'from' actor and the current 'to' actor sequence.
            foreach (var sequence in afterSequences)
            {
                beforeSequences.ForEach((tuple) =>
                {
                    LinkedList<IMarbleMachineActor> newSequence = new(tuple.sequence.Concat(sequence.sequence));
                    var newSequenceId = new SequenceId();
                    _sequences.Add(newSequenceId, newSequence);
                    updatedAndCreatedIds.Add(newSequenceId);
                });
            }

            _actorInSequences[from.ActorId] = updatedAndCreatedIds;
            _actorInSequences[to.ActorId] = updatedAndCreatedIds;

            CheckAndBuildPipe(updatedAndCreatedIds.ToList());

            return true;
        }

        public bool ReleaseConnection(CanvasObjectViewModelBase from, CanvasObjectViewModelBase to)
        {

            _actors.TryGetValue(from.ActorId, out IMarbleMachineActor? fromActor);
            _actors.TryGetValue(to.ActorId, out IMarbleMachineActor? toActor);

            if (fromActor == null || toActor == null)
            {
                Trace.WriteLine($"Actor not found for connection from {from.ActorId} to {to.ActorId}");
                return false;
            }

            List<SequenceId> sequencesToUpdate = new List<SequenceId>();

            // Very unperformant // refactor to use actorInSequences perhaps
            _sequences.Where(sequence =>
                {
                    var fromActorEntry = sequence.Value.Find(fromActor);
                    return fromActorEntry?.Next?.Value is IMarbleMachineActor nextActor && nextActor.ActorId == toActor.ActorId;

                })
                .ToList()
                .ForEach(sequence =>
                {
                    var oldSequence = _sequences[sequence.Key];
                    var fromSequence = oldSequence.GetAllBefore(fromActor);
                    var toSequence = oldSequence.GetAllAfter(toActor);

                    if (_actorInSequences[from.ActorId].Count > 1)
                    {
                        _sequences.Remove(sequence.Key);
                    }
                    else
                    {
                        _sequences[sequence.Key] = fromSequence;
                    }

                    var newSequenceId = new SequenceId();
                    _sequences.Add(newSequenceId, toSequence);

                });


            return true;
        }

        private void CheckAndBuildPipe(List<SequenceId> checkIds)
        {
            var sequences = checkIds.Select(id => new Sequence(_sequences[id], id)).Where(sequence => sequence.sequence != null);
            foreach (var sequence in sequences)
            {
                if (sequence.sequence.Count < 2
                    || sequence.sequence.First?.Value is not IMarbleMachineConductor
                    || sequence.sequence.Last?.Value is not ChannelSelectOperator)
                { continue; }
                //if (_activePipes.TryGetValue(sequence.id, out IDisposable? pipeSubscription)) { pipeSubscription?.Dispose(); }

                IMarbleMachineConductor? marbleMachineConductor = sequence.sequence.First.Value as IMarbleMachineConductor;
                List<IMarbleOperator?> marbleOperators = sequence.sequence
                    .Where(actor => actor is IMarbleOperator)
                    .Select(actor => actor as IMarbleOperator)
                    .ToList();

                var marblePipe = new MIDIPipelineBuilder(marbleMachineConductor, marbleOperators);

                _readyPipes[sequence.id] = marblePipe;
            }
        }

        private List<Sequence> GetAllAfter(IMarbleMachineActor actor)
        {
            return GetSequencesAroundActor(actor, (sequence, actor) => sequence.GetAllAfter(actor));
        }


        private List<Sequence> GetAllBefore(IMarbleMachineActor actor)
        {
            return GetSequencesAroundActor(actor, (sequence, actor) => sequence.GetAllBefore(actor));
        }

        private List<Sequence> GetSequencesAroundActor(
            IMarbleMachineActor actor,
            Func<LinkedList<IMarbleMachineActor>, IMarbleMachineActor, LinkedList<IMarbleMachineActor>> listExtractionMethod)
        {
            List<Sequence> newSequences = new List<Sequence>();


            if (!_actorInSequences.TryGetValue(actor.ActorId, out HashSet<SequenceId>? sequencesFrom))
            {
                // Adds the source actor as a new sequence
                var newSequence = new LinkedList<IMarbleMachineActor>();
                newSequence.AddFirst(actor);
                newSequences.Add(new Sequence(newSequence, new SequenceId()));
                return newSequences;
            }

            foreach (var sequenceId in sequencesFrom)
            {
                if (!_sequences.TryGetValue(sequenceId, out LinkedList<IMarbleMachineActor>? sequence)) { continue; }
                newSequences.Add(new Sequence(listExtractionMethod(sequence, actor), sequenceId));
            }
            return newSequences;
        }

        public IDisposable SubscribeToAllPipeChanges(Action<IDictionary<SequenceId, MIDIPipelineBuilder>> onChange) =>
            _readyPipes.DictionaryChanged
                .Subscribe(onChange);

        public IDisposable SubscribeToPipeCreations(Action<DictionaryAddEventArgs<SequenceId, MIDIPipelineBuilder>> onChange)
        {
            return _readyPipes.Added.Subscribe(onChange);
        }

        public IDisposable SubscribeToPipeRemovals(Action<DictionaryRemoveEventArgs<SequenceId, MIDIPipelineBuilder>> onChange)
        {
            return _readyPipes.Removed.Subscribe(onChange);
        }

        public IDisposable SubscribeToPipeUpdates(Action<DictionaryReplaceEventArgs<SequenceId, MIDIPipelineBuilder>> onChange)
        {
            return _readyPipes.Replaced.Subscribe(onChange);
        }
    }
}
