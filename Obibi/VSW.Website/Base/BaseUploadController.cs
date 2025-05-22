using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using VSW.Core.Services;

namespace VSW.Website
{
    public abstract class BaseUploadController : BaseController
    {
        public BaseUploadController(IWorkingContext context) : base(context)
        {
        }
    }
}
