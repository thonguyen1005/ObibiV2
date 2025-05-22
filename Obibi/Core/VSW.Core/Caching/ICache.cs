using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Caching
{
    public interface ICache : IDisposable
    {

        bool HasKey(string key);

        /// <summary>
        /// Adds a value to cache with a given key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">Expire time (Use TimeSpan.Zero to hold value with no expiration)</param>
        void Set(string key, object value, TimeSpan? expiration = null);

        ///// <summary>
        ///// Reads the value with specified key from the local cache.</summary>
        ///// <typeparam name="TItem">Data type</typeparam>
        ///// <param name="key">Key</param>
        //ICacheData<TItem> GetItem<TItem>(string key);


        ///// <summary>
        ///// Reads the value with specified key from the local cache.</summary>
        ///// <typeparam name="TItem">Data type</typeparam>
        ///// <param name="key">Key</param>
        //ICacheData<TItem> GetItem<TItem>(string key, Func<TItem> loader, TimeSpan? expiration = null);


        /// <summary>
        /// Reads the value with specified key from the local cache.</summary>
        /// <typeparam name="TItem">Data type</typeparam>
        /// <param name="key">Key</param>
        TItem Get<TItem>(string key);


        /// <summary>
        /// Reads the value with specified key from the local cache.</summary>
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="key"></param>
        /// <param name="loader"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        TItem Get<TItem>(string key, Func<TItem> loader, TimeSpan? expiration = null);


        /// <summary>
        /// Search the values with given key pattern. If value didn't exist in cache, 
        /// return null. 
        /// </summary>
        /// <param name="keyPattern">Key patttern</param>
        Dictionary<string, TItem> Search<TItem>(string keyPattern);

        /// <summary>
        /// Search the values with given key pattern. If value didn't exist in cache, 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Dictionary<string, TItem> GetMany<TItem>(params string[] keys);

        /// <summary>
        /// Removes the value with specified key from the local cache. If the value doesn't exist, no error is raised.
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(string key);


        /// <summary>
        /// Removes the value with specified key from the local cache. If the value doesn't exist, no error is raised.
        /// </summary>
        /// <param name="keys">Keys</param>
        void Removes(params string[] keys);

        /// <summary>
        /// Removes the value with specified key from the local cache. If the value doesn't exist, no error is raised.
        /// </summary>
        /// <param name="prefix">Key</param>
        void RemoveWithPrefix(string prefix);

        /// <summary>
        /// Removes all items from the cache (avoid expect unit tests).
        /// </summary>
        void Clear();
    }
}
