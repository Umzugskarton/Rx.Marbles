using CommunityToolkit.Mvvm.ComponentModel;
using System.Reactive.Disposables;

namespace AIMarbles.Core
{
    abstract internal partial class ViewModelBase : ObservableObject, IDisposable
    {
        CompositeDisposable _disposables = new CompositeDisposable();

        public void AddDisposables(CompositeDisposable disposables)
        {
            _disposables.Add(disposables);
        }

        public void AddDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public void Dispose()
        {
            foreach (var item in _disposables)
            {
                item.Dispose();
            }
        }

    }
}
