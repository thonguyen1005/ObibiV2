using System;
using System.Collections.Generic;
using System.Text;
using VSW.Core.Securities;

namespace VSW.Core.Services.Tracing.ElasticApm
{
    public class ElasticApmSpanContext : ISpanContext
    {
        public void AddTag(string key, string value)
        {
            if (_context.Labels.ContainsKey(key))
                _context.Labels[key] = value;
            else
                _context.Labels.Add(key, value);
        }

        public void RemoveTag(string key)
        {
            if (_context.Labels.ContainsKey(key))
                _context.Labels.Remove(key);
        }


        private Elastic.Apm.Api.SpanContext _context;

        public ElasticApmSpanContext(Elastic.Apm.Api.SpanContext context)
        {
            _context = context;
        }
    }

    public class ElasticApmTransactionContext : ITransactionContext<UserIdentity>
    {
        public void AddTag(string key, string value)
        {
            if (_context.Labels.ContainsKey(key))
                _context.Labels[key] = value;
            else
                _context.Labels.Add(key, value);
        }

        public void RemoveTag(string key)
        {
            if (_context.Labels.ContainsKey(key))
                _context.Labels.Remove(key);
        }

        public void AddUser(UserIdentity user)
        {
            if(user == null)
            {
                return;
            }

            _context.User = new Elastic.Apm.Api.User
            {
                Id = user.Id,
                UserName = user.IdentityCode
            };
        }

        private Elastic.Apm.Api.Context _context;

        public ElasticApmTransactionContext(Elastic.Apm.Api.Context context)
        {
            _context = context;
        }
    }
}
