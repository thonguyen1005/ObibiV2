using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("WEB_PROPERTY")]
    public class WEB_PROPERTYEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("LangID")]
        public int LangID { get; set; }
        [Column("ParentID")]
        public int ParentID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("Multiple")]
        public bool Multiple { get; set; }
        [Column("IsSize")]
        public bool IsSize { get; set; }
        [Column("IsColor")]
        public bool IsColor { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("ShowFilterFast")]
        public bool ShowFilterFast { get; set; }
        [Column("IsMenu")]
        public bool IsMenu { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

