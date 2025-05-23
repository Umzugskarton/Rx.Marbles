using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace AIMarbles.Core.Helpers;

public class State<T>
{
    private BehaviorSubject<T> _state;

    public State(T initialState)
    {
        _state = new BehaviorSubject<T>(initialState);
    }

    public T CurrentValue => _state.Value;

    public IDisposable Subscribe(Action<T> onNext)
    {
        return _state.Subscribe(onNext);
    }

    public void SetState(T newState)
    {
        _state.OnNext(newState);
    }
}

