using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services
{
    public abstract class BaseService
    {
        protected IWorkingContext Context { get; set; }

        public BaseService(IWorkingContext context)
        {
            Context = context;
        }
    }
}
