using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace VSW.Website.Interface
{
    public interface IViewRenderService
    {
        Task<string> PartialAsync(string viewName, object model, ViewDataDictionary viewData = null);
    }
}
