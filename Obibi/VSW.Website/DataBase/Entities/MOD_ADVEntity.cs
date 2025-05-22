using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_ADV")]
    public class MOD_ADVEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("MenuID")]
        public int MenuID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("AdvCode")]
        public string AdvCode { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("Summary")]
        public string Summary { get; set; }
        [Column("Width")]
        public int Width { get; set; }
        [Column("Height")]
        public int Height { get; set; }
        [Column("AddInTag")]
        public string AddInTag { get; set; }
        [Column("URL")]
        public string URL { get; set; }
        [Column("Target")]
        public string Target { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

