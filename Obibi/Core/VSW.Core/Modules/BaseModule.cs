using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Modules
{
    public abstract class BaseModule : IModule
    {
        public virtual int Priority => PriorityLevel.System;

        protected ILogger Logger { get; set; }

        public IConfiguration Configuration { get; set; }

        public BaseModule()
        {
        }

        public virtual void Configure(IServiceProvider resolver)
        {

        }

        public virtual void ConfigureServices(IServiceCollection services)
        {

        }

        public virtual void Initialize(IServiceProvider resolver)
        {

        }
    }
}
