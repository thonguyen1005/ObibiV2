using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Modules
{
    public interface IModule
    {
        int Priority { get; }

        IConfiguration Configuration { get; set; }

        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Giving each module a chance to configure the application
        /// </summary>
        /// <param name="resolver"></param>
        void Configure(IServiceProvider resolver);

        /// <summary>
        ///  Giving each module a chance to run startup logic
        /// </summary>
        void Initialize(IServiceProvider resolver);
    }
}
