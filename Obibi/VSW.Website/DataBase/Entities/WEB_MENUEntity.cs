using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{
    [Table("WEB_MENU")]
    public class WEB_MENUEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("LangID")]
        public int LangID { get; set; }
        [Column("ParentID")]
        public int ParentID { get; set; }
        [Column("PropertyID")]
        public int PropertyID { get; set; }
        [Column("State")]
        public int State { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("CityCode")]
        public string CityCode { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("Type")]
        public string Type { get; set; }
        [Column("Summary")]
        public string Summary { get; set; }
        [Column("Custom")]
        public string Custom { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

