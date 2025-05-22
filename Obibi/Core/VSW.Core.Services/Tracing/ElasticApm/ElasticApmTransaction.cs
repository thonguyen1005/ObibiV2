using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace VSW.Core.Services.Tracing.ElasticApm
{
    public class ElasticApmTransaction : ITransaction
    {
        private Elastic.Apm.Api.ITransaction _transaction;
        private readonly ILogger<ElasticApmTracer> _logger;

        public string Name { get { return _transaction?.Name; } }

        public string Id { get { return _transaction?.Id; } }

        public string TraceId { get { return _transaction?.TraceId; } }

        public string ParentId { get { return _transaction?.ParentId; } }

        public ITransactionContext Context { get; private set; }

        public string Type { get { return _transaction?.Type; } }

        public ILogger GetLogger()
        {
            return _logger;
        }

        internal ElasticApmTransaction(Elastic.Apm.Api.ITransaction transaction, ILogger<ElasticApmTracer> logger)
        {
            _transaction = transaction;
            _logger = logger;
            Context = new ElasticApmTransactionContext(_transaction.Context);
        }


        public void CaptureError(string message, string culprit, StackFrame[] frames = null)
        {
            _transaction?.CaptureError(message, culprit, frames);
        }

        public void CaptureException(Exception e)
        {
            _transaction?.CaptureException(e);
        }

        public void Dispose()
        {
            if (_transaction == null)
            {
                return;
            }
            End();
        }

        public void End()
        {
            _transaction?.End();
            _transaction = null;
        }

        public void Log(string msg, params object[] args)
        {
            if (_transaction == null || _logger == null)
            {
                return;
            }
            _logger.LogInformation($"Instrumentation(Type: Transaction Id: {_transaction.Id} ParentId: {_transaction.ParentId} TraceId: {_transaction.TraceId}) {msg}", args);
        }

        public string SerializeTracingData()
        {
            string result = _transaction?.OutgoingDistributedTracingData.SerializeToString();
            return result;
        }

        public ISpan StartSpan(string name, string type, string subType = null, string action = null)
        {
            if (_transaction == null)
            {
                return null;
            }

            var innerSpan = _transaction.StartSpan(name, type, subType, action);
            var r = new ElasticApmSpan(this, innerSpan);
            return r;
        }
    }
}
