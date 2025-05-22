
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
namespace VSW.Core.Services.Tracing.ElasticApm
{
    public class ElasticApmSpan : ISpan
    {
        private Elastic.Apm.Api.ISpan _span;

        public string Action { get { return _span?.Action; } }

        public string SubType { get { return _span?.Subtype; } }

        public string Type { get { return _span?.Type; } }

        internal ElasticApmSpan(ITransaction transaction, Elastic.Apm.Api.ISpan span)
        {
            Transaction = transaction;
            _span = span;
            Context = new ElasticApmSpanContext(span.Context);
        }

        public ITransaction Transaction { get; private set; }

        public string TransactionId
        {
            get
            {
                return Transaction != null ? Transaction.Id : null;
            }
        }

        public string Name { get { return _span?.Name; } }

        public string Id { get { return _span?.Id; } }

        public string TraceId { get { return Transaction != null ? Transaction.TraceId : null; } }

        public string ParentId { get { return _span?.ParentId; } }

        public ISpanContext Context { get; private set; }



        public void CaptureError(string message, string culprit, StackFrame[] frames = null)
        {
            _span?.CaptureError(message, culprit, frames);
        }

        public void CaptureException(Exception e)
        {
            _span?.CaptureException(e);
        }

        public void Dispose()
        {
            if (_span == null)
            {
                return;
            }

            End();
        }

        public void End()
        {
            _span?.End();
            _span = null;
        }

        public void Log(string msg, params object[] args)
        {
            var logger = (Transaction as ElasticApmTransaction).GetLogger();
            if (logger == null)
            {
                return;
            }
            logger.LogInformation($"Instrumentation(Type: Span Id: {_span.Id} TransactionId: {_span.TransactionId} ParentId: {_span.ParentId} TraceId: {_span.TraceId}) {msg}", args);
        }

        public string SerializeTracingData()
        {
            string result = Transaction?.SerializeTracingData();
            return result;
        }

        public ISpan StartChildSpan(string name, string type, string subType = null, string action = null)
        {
            var innerChildSpand = _span.StartSpan(name, type, subType, action);
            return new ElasticApmSpan(Transaction, innerChildSpand);
        }
    }
}
