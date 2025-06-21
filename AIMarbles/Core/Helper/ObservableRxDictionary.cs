using System;
using System.Collections;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;

namespace AIMarbles.Core.Helper
{
    // Defining custom event args for dictionary changes for better granularity
    public class DictionaryAddEventArgs<TKey, TValue> : EventArgs
    {
        public TKey Key { get; }
        public TValue Value { get; }
        public DictionaryAddEventArgs(TKey key, TValue value) { Key = key; Value = value; }
    }

    public class DictionaryRemoveEventArgs<TKey, TValue> : EventArgs
    {
        public TKey Key { get; }
        public TValue Value { get; } // Value that was removed
        public DictionaryRemoveEventArgs(TKey key, TValue value) { Key = key; Value = value; }
    }

    public class DictionaryReplaceEventArgs<TKey, TValue> : EventArgs
    {
        public TKey Key { get; }
        public TValue OldValue { get; }
        public TValue NewValue { get; }
        public DictionaryReplaceEventArgs(TKey key, TValue oldValue, TValue newValue)
        {
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class ObservableRxDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDisposable where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        // Subjects for different types of changes
        private readonly Subject<DictionaryAddEventArgs<TKey, TValue>> _added = new Subject<DictionaryAddEventArgs<TKey, TValue>>();
        private readonly Subject<DictionaryRemoveEventArgs<TKey, TValue>> _removed = new Subject<DictionaryRemoveEventArgs<TKey, TValue>>();
        private readonly Subject<DictionaryReplaceEventArgs<TKey, TValue>> _replaced = new Subject<DictionaryReplaceEventArgs<TKey, TValue>>();
        private readonly Subject<Unit> _cleared = new Subject<Unit>(); // Unit is Rx's void equivalent
        private readonly Subject<IDictionary<TKey, TValue>> _dictionaryChanged = new Subject<IDictionary<TKey, TValue>>(); // For any change

        public ObservableRxDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public ObservableRxDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        // Expose observable streams
        public IObservable<DictionaryAddEventArgs<TKey, TValue>> Added => _added.AsObservable();
        public IObservable<DictionaryRemoveEventArgs<TKey, TValue>> Removed => _removed.AsObservable();
        public IObservable<DictionaryReplaceEventArgs<TKey, TValue>> Replaced => _replaced.AsObservable();
        public IObservable<Unit> Cleared => _cleared.AsObservable();
        public IObservable<IDictionary<TKey, TValue>> DictionaryChanged => _dictionaryChanged.Select(dict => dict).AsObservable(); // A general stream for all changes

        public IObservable<object> AllChanges => Observable.Merge(
            _added.Select(args => (object)args),
            _removed.Select(args => (object)args),
            _replaced.Select(args => (object)args),
            _cleared.Select(_ => (object)"Cleared") 
        );


        // IDictionary<TKey, TValue> Implementation (with Rx notifications)

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                if (_dictionary.TryGetValue(key, out TValue? oldValue))
                {
                    if (!EqualityComparer<TValue>.Default.Equals(oldValue, value))
                    {
                        _dictionary[key] = value;
                        _replaced.OnNext(new DictionaryReplaceEventArgs<TKey, TValue>(key, oldValue, value));
                        _dictionaryChanged.OnNext(this);
                    }
                }
                else
                {
                    _dictionary[key] = value;
                    _added.OnNext(new DictionaryAddEventArgs<TKey, TValue>(key, value));
                    _dictionaryChanged.OnNext(this);
                }
            }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;
        public ICollection<TValue> Values => _dictionary.Values;
        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            _added.OnNext(new DictionaryAddEventArgs<TKey, TValue>(key, value));
            _dictionaryChanged.OnNext(this);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)_dictionary).Add(item);
            _added.OnNext(new DictionaryAddEventArgs<TKey, TValue>(item.Key, item.Value));
            _dictionaryChanged.OnNext(this);
        }

        public void Clear()
        {
            if (_dictionary.Count > 0)
            {
                _dictionary.Clear();
                _cleared.OnNext(Unit.Default);
                _dictionaryChanged.OnNext(this);
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_dictionary).Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            if (_dictionary.TryGetValue(key, out TValue? removedValue))
            {
                bool result = _dictionary.Remove(key);
                if (result)
                {
                    _removed.OnNext(new DictionaryRemoveEventArgs<TKey, TValue>(key, removedValue));
                    _dictionaryChanged.OnNext(this);
                }
                return result;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_dictionary.Contains(item) && _dictionary.Remove(item.Key))
            {
                _removed.OnNext(new DictionaryRemoveEventArgs<TKey, TValue>(item.Key, item.Value));
                _dictionaryChanged.OnNext(this);
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Dispose all subjects when the dictionary is no longer needed
        public void Dispose()
        {
            _added.OnCompleted();
            _added.Dispose();
            _removed.OnCompleted();
            _removed.Dispose();
            _replaced.OnCompleted();
            _replaced.Dispose();
            _cleared.OnCompleted();
            _cleared.Dispose();
            _dictionaryChanged.OnCompleted();
            _dictionaryChanged.Dispose();
        }
    }
}
