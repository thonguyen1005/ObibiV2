using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VSW.Website
{
    public class WebSession : IWebSession
    {

        private Microsoft.AspNetCore.Http.ISession _innerSession;

        private bool HasValue()
        {
            return _innerSession != null;
        }

        public string Id => HasValue() ? _innerSession.Id : null;

        public string UserName { get; set; }

        public bool IsAvailable => HasValue() ? _innerSession.IsAvailable : false;

        public IEnumerable<string> Keys => HasValue() ? _innerSession.Keys : new List<string>();

        public void Clear()
        {
            if (HasValue())
                _innerSession.Clear();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (HasValue())
            {
                await _innerSession.CommitAsync(cancellationToken);
            }
        }

        public async Task LoadAsync(CancellationToken cancellationToken = default)
        {
            if (HasValue())
            {
                await _innerSession.LoadAsync(cancellationToken);
            }
        }

        public void Remove(string key)
        {
            if (HasValue())
            {
                _innerSession.Remove(key);
            }
        }

        public void Set(string key, byte[] value)
        {
            if (HasValue())
            {
                _innerSession.Set(key, value);
            }
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (HasValue())
            {
                return _innerSession.TryGetValue(key, out value);
            }

            value = null;
            return false;
        }

        public WebSession()
        {
            var context = HttpRequestExtensions.GetContext();
            _innerSession = context == null ? null : context.Session;
            if (context != null)
            {
                var user = context.User;
                if (user != null && user.Identity != null)
                {
                    UserName = user.Identity.IsAuthenticated ? user.Identity.Name : null;
                }
            }
        }
    }
}
