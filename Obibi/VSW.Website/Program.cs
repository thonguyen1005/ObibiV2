using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VSW.Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new WebAppLoader(WebAppLoader.DefaultWebConfigRootPath);
            app.Run<Startup>(args);
        }
    }
}
