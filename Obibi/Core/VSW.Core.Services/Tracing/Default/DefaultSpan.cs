using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace VSW.Core.Services.Tracing.Default
{
    public class DefaultSpan : ISpan
    {
        public ITransaction Transaction { get; }

        public ISpanContext Context { get; private set; }

        public string TraceId { get; private set; }

        public string Id { get; private set; }

        public string ParentId { get; private set; }

        public string TransactionId { get; private set; }

        public string Name { get; private set; }
        public string Action { get; private set; }
        public string SubType { get; private set; }

        public string Type { get; private set; }

        private DateTime _startTime;

        private bool isEnded = false;

        private ILogger _logger;
        public DefaultSpan(string name, ITransaction transaction, ILogger logger, ISpan parent = null, string type = null, string subType = null, string action = null)
        {
            Id = StringExtensions.NewId();
            Name = name;
            _logger = logger;
            Transaction = transaction;
            TraceId = Transaction != null ? Transaction.TraceId: "";
            ParentId = parent != null ? parent.Id : "";
            TransactionId = Transaction != null ? Transaction.Id : "";
            _startTime = DateTimeHelper.Now;
            Action = action;
            Type = type;
            SubType = subType;
            Context = new SpanContext();
            Log("Start");
        }

        public void Log(string msg, params object[] args)
        {
            if (_logger == null)
            {
                return;
            }
            _logger.LogInformation($"Instrumentation(Type: Span Id: {Id} - {Name} TransactionId: {TransactionId} ParentId: {ParentId} TraceId: {TraceId}) {msg}", args);
        }

        public void CaptureException(Exception e)
        {
            Log("Exception " + Environment.NewLine + "- Detail: {0}", e);
        }

        public void CaptureError(string message, string culprit, StackFrame[] frames = null)
        {
            Log("Error " + Environment.NewLine + "- Message: {0}" + Environment.NewLine + "- Source: {1}" + Environment.NewLine + "- Detail: {2}",
                message, culprit, frames.IsEmpty() ? "" : frames.Select(x => x.ToString()).Join(Environment.NewLine)
            );
        }

        public void End()
        {
            if (isEnded)
            {
                return;
            }

            var duration = DateTimeHelper.Now.Subtract(_startTime).TotalMilliseconds;
            Log("End <Duration(ms): {0}", duration);
            isEnded = true;
        }

        public ISpan StartChildSpan(string name, string type, string subType = null, string action = null)
        {
            return new DefaultSpan(name, Transaction, _logger, this, action, type, subType);
        }

        public string SerializeTracingData()
        {
            return null;
        }

        public void Dispose()
        {
            End();
        }
    }
}
