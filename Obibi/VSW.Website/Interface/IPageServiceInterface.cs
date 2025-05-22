using System;
using VSW.Website.Models;

namespace VSW.Website.Interface
{
    public interface IPageServiceInterface
    {
        Tuple<IPageInterface, string> VSW_Core_GetByID(int id);
        Tuple<IPageInterface, string> VSW_Core_CurrentPage(VQS vqs, int langId);
        string GetUrlByModule(string moduleCode, int langId);
    }
}
