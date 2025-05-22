using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using VSW.Core;

namespace VSW.Website
{
    public class PermissionAttribute : ActionFilterAttribute
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PermissionAttribute(string code, string name, string description = null)
        {
            Code = code;
            Name = name;
            Description = description;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
