using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Caching
{
    public interface IGenericCache<TKey, TValue>
    {
        bool HasKey(TKey key);

        /// <summary>
        /// Adds a value to cache with a given key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">Expire time (Use TimeSpan.Zero to hold value with no expiration)</param>
        void Add(TKey key, TValue value);

        /// <summary>
        /// Reads the value with specified key from the local cache.</summary>
        /// <typeparam name="TItem">Data type</typeparam>
        /// <param name="key">Key</param>
        TValue Get(TKey key);

        TValue this[TKey key] { get; }

        /// <summary>
        /// Search the values with given key pattern. If value didn't exist in cache, 
        /// return null. 
        /// </summary>
        /// <param name="keyPattern">Key patttern</param>
        Dictionary<TKey, TValue> GetMany<TItem>(params TKey[] keys);

        /// <summary>
        /// Removes the value with specified key from the local cache. If the value doesn't exist, no error is raised.
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(TKey key);


        /// <summary>
        /// Removes the value with specified key from the local cache. If the value doesn't exist, no error is raised.
        /// </summary>
        /// <param name="key">Key</param>
        void Removes(params TKey[] keys);

        /// <summary>
        /// Removes all items from the cache (avoid expect unit tests).
        /// </summary>
        void Clear();
    }
}
