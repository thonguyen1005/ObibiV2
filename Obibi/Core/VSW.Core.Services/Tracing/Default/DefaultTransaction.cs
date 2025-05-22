using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace VSW.Core.Services.Tracing.Default
{
    public class DefaultTransaction : ITransaction
    {
        public string Name { get; private set; }

        public ITransactionContext Context { get; private set; }
        public string Id { get; private set; }

        public string TraceId { get; private set; }

        public string ParentId { get; private set; }

        public string Type { get; private set; }

        private ILogger _logger;
        private DateTime _startTime;
        private bool isEnded = false;

        public DefaultTransaction(ILogger logger, string name, string type = null)
        {
            Id = StringExtensions.NewId();
            TraceId = Id;
            _logger = logger;
            _startTime = DateTimeHelper.Now;
            Name = name;
            Type = type;
            Context = new TransactionContext();
            Log("Start");
        }

        public void CaptureError(string message, string culprit, StackFrame[] frames = null)
        {
            Log("Error " + Environment.NewLine + "- Message: {0}" + Environment.NewLine + "- Source: {1}" + Environment.NewLine + "- Detail: {2}",
                 message, culprit, frames.IsEmpty() ? "" : frames.Select(x => x.ToString()).Join(Environment.NewLine)
            );
        }

        public void CaptureException(Exception e)
        {
            Log("Exception " + Environment.NewLine + "- Detail: {0}", e);
        }

        public void Dispose()
        {
            End();
        }

        public void End()
        {
            if (isEnded)
            {
                return;
            }

            isEnded = true;
        }

        public void Log(string msg, params object[] args)
        {
            if (_logger == null)
            {
                return;
            }

            _logger.LogInformation($"Instrumentation(Type: Transaction Id: {Id} ParentId: {ParentId} TraceId: {TraceId}) {msg}", args);
        }

        public string SerializeTracingData()
        {
            return null;
        }

        public ISpan StartSpan(string name, string type, string subType = null, string action = null)
        {
            return new DefaultSpan(name, this, _logger, type: type, subType: subType, action: action);
        }
    }
}
