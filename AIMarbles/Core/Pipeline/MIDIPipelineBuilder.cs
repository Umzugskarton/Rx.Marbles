using AIMarbles.Core.Interface.Pipeline;
using AIMarbles.Model;
using System.Reactive.Linq;

namespace AIMarbles.Core.Pipeline
{
    public class MIDIPipelineBuilder
    {
        private IObservable<MIDIMarble>? _currentObservable;
        List<IMarbleOperator?> _operators = new List<IMarbleOperator?>();

        public MIDIPipelineBuilder(IMarbleMachineConductor? conductor, List<IMarbleOperator?> operators)
        {
            _currentObservable=conductor?.MarbleSource;
            _operators=operators;
        }

        /// <summary>
        /// Builds and returns the final IObservable<MIDIMarble> sequence.
        /// </summary>
        public MIDIPipelineBuilder Build()
        {
            _currentObservable ??= Observable.Empty<MIDIMarble>(); // Initialize if null

            foreach (var item in _operators)
            {
                if(item == null)
                {
                    continue; // Skip null operators
                }

                _currentObservable = item.Apply(_currentObservable);
            }

            return this;
        }

        /// <summary>
        /// Builds the observable pipeline using all the provided operators.
        /// </summary>
        /// <param name="midiOperator">The operator to apply.</param>
        /// <returns>The builder instance for chaining (no type change).</returns>
        public MIDIPipelineBuilder Add(IMarbleOperator midiOperator)
        {
            _operators.Add(midiOperator);
            return this;
        }

    }
}
