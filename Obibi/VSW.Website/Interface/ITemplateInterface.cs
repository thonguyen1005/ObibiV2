using System;
using VSW.Website.Models;

namespace VSW.Website.Interface
{
    public interface ITemplateInterface
	{
		int ID
		{
			get;
			set;
        }
        string Custom
        {
            get;
            set;
        }
        string File
        {
            get;
            set;
        }
        int LangID
        {
            get;
            set;
        }
        Custom Items
		{
			get;
            set;
        }
	}
}
