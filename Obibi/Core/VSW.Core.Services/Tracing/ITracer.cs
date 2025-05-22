using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VSW.Core.Services.Tracing
{
    public interface ISegment
    {
        string Name { get; }

        string Type { get; }

        string Id { get; }

        string TraceId { get; }

        string ParentId { get; }

        void CaptureException(Exception e);

        void CaptureError(string message, string culprit, StackFrame[] frames = null);

        void End();

        void Log(string msg, params object[] args);

        string SerializeTracingData();
    }

    public interface ITransaction : IDisposable, ISegment
    {
        ITransactionContext Context { get; }

        ISpan StartSpan(string name, string type, string subType = null, string action = null);

    }

    public interface ISpan : IDisposable, ISegment
    {
        string Action { get; }

        string SubType { get; }

        ITransaction Transaction { get; }

        ISpanContext Context { get; }

        string TransactionId { get; }

        ISpan StartChildSpan(string name, string type, string subType = null, string action = null);

    }

    public interface ITracer
    {
        ITransaction StartTransaction(string name, string type, object tracingData = null);

        /// <summary>
        /// Bắt đầu một span mới theo transaction hiện thời
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="subType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        ISpan StartSpan(string name, string type, string subType = null, string action = null);

        ISpan StartChildSpan(string name, string type, string subType = null, string action = null);

        ITransaction CurrentTransaction { get; }

        ISpan CurrentSpan { get; }

        object DeserializeTracingData(string tracingData);
    }
}
