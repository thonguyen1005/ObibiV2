using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using VSW.Core.Services.Tracing;

namespace VSW.Core.Services
{
    public interface IWorkingContext
    {
        IAppSession Session { get; set; }

        IStringLocalizer Localizer { get; set; }

        ILogger Logger { get; set; }

        ITracer Tracer { get; set; }

        int WorkingLanguageId { get; set; }
    }

    public interface IWorkingContext<TCategory>: IWorkingContext
    {

    }    
}
