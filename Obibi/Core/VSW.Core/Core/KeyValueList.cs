using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
namespace VSW.Core
{
    public class KeyValueList<TKey, TItem> : ICollection, ICollection<TItem>
    {
        protected readonly Dictionary<TKey, TItem> _dictionary;

        protected readonly Func<TItem, TKey> _funcKey;

        public KeyValueList(Func<TItem, TKey> keyFuncs)
        {
            _funcKey = keyFuncs;
            _dictionary = new Dictionary<TKey, TItem>();
        }

        public KeyValueList(int capacity, System.Linq.Expressions.Expression<Func<TItem, TKey>> keyFuncs) : this(keyFuncs.Compile())
        {
        }

        public KeyValueList(Func<TItem, TKey> keyFuncs, ICollection<TItem> pointers) : this(keyFuncs)
        {
            foreach (var ptr in pointers)
            {
                Add(ptr);
            }
        }

        protected virtual TKey RefineKey(TKey key)
        {
            return key;
        }

        public virtual TItem this[TKey key] { get => Find(key); set => AddOrUpdate(value); }

        public int Count => _dictionary.Count;

        public bool IsSynchronized => true;

        public object SyncRoot => true;

        public bool IsReadOnly => false;

        public virtual void Add(TItem item)
        {
            _dictionary.Add(GetKey(item), item);
        }

        public virtual void AddIfNotExist(TItem item)
        {
            if (!Contains(item))
                Add(item);
        }

        public virtual void AddOrUpdate(TItem item)
        {
            if (!Contains(item))
                Add(item);
            else
            {
                Remove(item);
                Add(item);
            }
        }

        public virtual void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(TKey key)
        {
            key = RefineKey(key);
            return _dictionary.ContainsKey(key);
        }

        public virtual bool Contains(TItem item)
        {
            var key = GetKey(item);
            return Contains(key);
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public virtual void CopyTo(TItem[] array, int arrayIndex)
        {
            _dictionary.Values.CopyTo(array, arrayIndex);
        }

        public virtual TItem Find(TKey key)
        {
            key = RefineKey(key);
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key];
            }

            return default;
        }

        public virtual TItem Find(Predicate<TItem> match)
        {
            return _dictionary.Values.FirstOrDefault(x => match(x));
        }

        public virtual List<TItem> FindAll(Predicate<TItem> match)
        {
            return _dictionary.Values.Where(x => match(x)).ToList();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        public virtual TKey GetKey(TItem item)
        {
            return RefineKey(_funcKey(item));
        }

        public virtual bool Remove(TKey key)
        {
            key = RefineKey(key);
            if (_dictionary.ContainsKey(key))
            {
                _dictionary.Remove(key);
                return true;
            }

            return false;
        }

        public virtual bool Remove(TItem item)
        {
            var key = GetKey(item);
            return Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }
    }
}