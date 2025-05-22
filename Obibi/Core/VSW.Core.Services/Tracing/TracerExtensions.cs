using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Tracing
{
    public static class TracerExtensions
    {
        public static string GetTraceId(this ITracer tracer)
        {
            if (tracer == null || tracer.CurrentTransaction == null)
            {
                return "";
            }

            return tracer.CurrentTransaction.TraceId;
        }
    }
}
