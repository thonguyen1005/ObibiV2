using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VSW.Core.Services.Tracing.Default
{
    public class DefaultTracer : ITracer
    {
        public ITransaction CurrentTransaction { get; private set; }
        public ISpan CurrentSpan { get; private set; }
        private ILogger _logger;

        public DefaultTracer(ILogger<DefaultTracer> logger)
        {
            _logger = logger;
        }

        public object DeserializeTracingData(string tracingData)
        {
            return null;
        }

        public ISpan StartChildSpan(string name, string type, string subType = null, string action = null)
        {
            if(CurrentSpan == null)
            {
                return StartSpan(name, type, subType, action);
            }

            return CurrentSpan.StartChildSpan(name, type, subType, action);
        }

        public ISpan StartSpan(string name, string type, string subType = null, string action = null)
        {
            if(CurrentTransaction == null)
            {
                StartTransaction(name, type);
            }

            return CurrentTransaction.StartSpan(name, type, subType, action);
        }

        public ITransaction StartTransaction(string name, string type, object tracingData = null)
        {
            var trans = new DefaultTransaction(_logger, name, type);
            CurrentTransaction = trans;
            return trans;
        }
    }
}
