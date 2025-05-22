using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSW.Website.Extensions
{
    public class CustomDistributedSessionStore : ISessionStore
    {
        private DistributedSessionStore _store;
        public CustomDistributedSessionStore(DistributedSessionStore store)
        {
            _store = store;
        }

        public Microsoft.AspNetCore.Http.ISession Create(string sessionKey, TimeSpan idleTimeout, TimeSpan ioTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey)
        {
            sessionKey = "SESSION" + ":" + sessionKey;

            var rs = _store.Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey);
            return rs;
        }
    }
}
