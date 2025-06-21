using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace AIMarbles.Core.Helper;

public class State<T>
{
    private BehaviorSubject<T> _state;

    public State(T initialState)
    {
        _state = new BehaviorSubject<T>(initialState);
    }

    public T CurrentValue => _state.Value;

    public IObservable<T> AsObservable() { return _state.AsObservable(); }
    public IDisposable Subscribe(Action<T> onNext)
    {
        return _state.Subscribe(onNext);
    }

    public void SetState(T newState)
    {
        _state.OnNext(newState);
        Trace.WriteLine($"State<{typeof(T).Name}> (Hash: {this.GetHashCode()}): SetState called with {newState}");
    }
}


