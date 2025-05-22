using System;
using LinqToDB.Mapping;
using System.Collections.Generic;
using VSW.Website.Interface;

namespace VSW.Website.DataBase.Entities
{
    [Table("SYS_SITE")]
    public class SYS_SITEEntity : ISiteInterface
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("PageID")]
        public int PageID { get; set; }
        [Column("LangID")]
        public int LangID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Custom")]
        public string Custom { get; set; }
        [Column("Default")]
        public bool Default { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

