using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB.Common.Internal.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VSW.Website
{
    public class Startup : WebStartUp
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            base.OnConfigureServices(services);
        }


        protected override void OnConfigure(IApplicationBuilder app)
        {
            base.OnConfigure(app);
        }
    }
}
