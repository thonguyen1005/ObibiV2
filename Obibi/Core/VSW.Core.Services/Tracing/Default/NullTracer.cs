using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Tracing.Default
{
    public class NullSpan: DefaultSpan
    {
        public NullSpan() :base("", null, null , null)
        {

        }
    }

    public class NullTransaction : DefaultTransaction
    {
        public NullTransaction() : base(null, null)
        {

        }
    }
}
