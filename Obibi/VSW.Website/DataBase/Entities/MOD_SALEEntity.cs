using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_SALE")]
    public class MOD_SALEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("DateStart")]
        public DateTime DateStart { get; set; }
        [Column("DateEnd")]
        public DateTime DateEnd { get; set; }
        [Column("Money")]
        public long Money { get; set; }
        [Column("Status")]
        public bool Status { get; set; }
        [Column("Published")]
        public DateTime Published { get; set; }
        [Column("Percent")]
        public double Percent { get; set; }
        [Column("Auto")]
        public bool Auto { get; set; }
        [Column("Count")]
        public int Count { get; set; }
        [Column("CountUse")]
        public int CountUse { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("WebUserID")]
        public int WebUserID { get; set; }
        [Column("MoneyMin")]
        public long MoneyMin { get; set; }
        [Column("AutoLogin")]
        public bool AutoLogin { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

