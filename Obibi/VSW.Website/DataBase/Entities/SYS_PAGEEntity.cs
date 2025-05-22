using System;
using LinqToDB.Mapping;
using System.Collections.Generic;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.DataBase.Entities
{
    [Table("SYS_PAGE")]
    public class SYS_PAGEEntity : IPageInterface
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("TemplateID")]
        public int TemplateID { get; set; }
        [Column("TemplateMobileID")]
        public int TemplateMobileID { get; set; }
        [Column("TemplateTabletID")]
        public int TemplateTabletID { get; set; }
        [Column("ModuleCode")]
        public string ModuleCode { get; set; }
        [Column("LangID")]
        public int LangID { get; set; }
        [Column("MenuID")]
        public int MenuID { get; set; }
        [Column("ParentID")]
        public int ParentID { get; set; }
        [Column("BrandID")]
        public int BrandID { get; set; }
        [Column("State")]
        public int State { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Icon")]
        public string Icon { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("Summary")]
        public string Summary { get; set; }
        [Column("LinkTitle")]
        public string LinkTitle { get; set; }
        [Column("PageHeading")]
        public string PageHeading { get; set; }
        [Column("PageTitle")]
        public string PageTitle { get; set; }
        [Column("PageCanonical")]
        public string PageCanonical { get; set; }
        [Column("PageDescription")]
        public string PageDescription { get; set; }
        [Column("PageKeywords")]
        public string PageKeywords { get; set; }
        [Column("TopContent")]
        public string TopContent { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("Custom")]
        public string Custom { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }
        [Column("Updated")]
        public DateTime Updated { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("AliasName")]
        public string AliasName { get; set; }
        /*------------------------------------*/
        public Custom Items { get; set; }
        #endregion
    }
}

