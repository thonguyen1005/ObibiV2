using System;
using LinqToDB.Mapping;
using System.Collections.Generic;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.DataBase.Entities
{
    [Table("SYS_TEMPLATE")]
    public class SYS_TEMPLATEEntity : ITemplateInterface
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("LangID")]
        public int LangID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("Custom")]
        public string Custom { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Device")]
        public int Device { get; set; }
        /*------------------------------------*/
        public Custom Items { get; set; }
        #endregion
    }
}

