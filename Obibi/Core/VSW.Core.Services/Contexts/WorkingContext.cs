using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using VSW.Core.Services.Tracing;

namespace VSW.Core.Services
{
    public class WorkingContext : IWorkingContext
    {
        public IAppSession Session { get; set; }
        public IStringLocalizer Localizer { get; set; }
        public ILogger Logger { get; set; }
        public ITracer Tracer { get; set; }

        public int WorkingLanguageId { get; set; }

        public WorkingContext(IAppSession session, IStringLocalizer localizer, ILogger logger, ITracer tracer)
        {
            Session = session;
            Localizer = localizer;
            Logger = logger;
            Tracer = tracer;
        }
    }

    public class WorkingContext<TCategory> : WorkingContext, IWorkingContext<TCategory>
    {
        public WorkingContext(IAppSession session, IStringLocalizer<TCategory> localizer, ILogger<TCategory> logger, ITracer tracer) : base(session, localizer, logger, tracer)
        {
        }
    }
}
