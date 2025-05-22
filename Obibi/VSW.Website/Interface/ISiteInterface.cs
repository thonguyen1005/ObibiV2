using System;

namespace VSW.Website.Interface
{
    public interface ISiteInterface
    {
        string Code
        {
            get;
            set;
        }
        int ID
        {
            get;
            set;
        }
        int LangID
        {
            get;
            set;
        }
        int PageID
        {
            get;
            set;
        }
    }
}
