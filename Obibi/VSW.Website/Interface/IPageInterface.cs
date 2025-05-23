using System;
using VSW.Website.Models;

namespace VSW.Website.Interface
{
    public interface IPageInterface
    {
        int ID
        {
            get;
            set;
        }
        int ParentID { get; set; }
        string Code
        {
            get;
            set;
        }
        string Name
        {
            get;
            set;
        }
        int MenuID
        {
            get;
            set;
        }
        int BrandID
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
        int TemplateID
        {
            get;
            set;
        }
        int TemplateMobileID
        {
            get;
            set;
        }
        int TemplateTabletID
        {
            get;
            set;
        }
        string ModuleCode
        {
            get;
            set;
        }
        string Custom
        {
            get;
            set;
        }
        string LinkTitle
        {
            get;
            set;
        }
        string PageTitle
        {
            get;
            set;
        }
        string PageHeading
        {
            get;
            set;
        }
        string PageKeywords
        {
            get;
            set;
        }
        string PageDescription
        {
            get;
            set;
        }
        string PageCanonical
        {
            get;
            set;
        }
        public string TopContent { get; set; }
        public string Content { get; set; }
        Custom Items { get; set; }
    }
}
