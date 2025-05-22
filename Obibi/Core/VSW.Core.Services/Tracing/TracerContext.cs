using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Tracing
{
    public interface ISpanContext
    {
        void AddTag(string key, string value);

        void RemoveTag(string key);
    }

    public interface ITransactionContext
    {
        void AddTag(string key, string value);

        void RemoveTag(string key);

      
    }

    public interface ITransactionContext<TUserInfo>: ITransactionContext
    {
        void AddUser(TUserInfo user);
    }    

    public class SpanContext : ISpanContext
    {
        public SpanContext()
        {
            Tags = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Tags { get; set; }

        public void AddTag(string key, string value)
        {
            Tags.TryAdd(key, value);
        }

        public void RemoveTag(string key)
        {
            Tags.Remove(key);
        }
    }

    public class TransactionContext : ITransactionContext
    {
        public TransactionContext()
        {
            Tags = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Tags { get; set; }

    

        public void AddTag(string key, string value)
        {
            Tags.TryAdd(key, value);
        }

      

        public void RemoveTag(string key)
        {
            Tags.Remove(key);
        }
    }

    public class TransactionContext<TUserInfo> : TransactionContext, ITransactionContext<TUserInfo>
    {
        public TUserInfo User { get; set; }

        public void AddUser(TUserInfo user)
        {
            User = user;
        }
    }
}
