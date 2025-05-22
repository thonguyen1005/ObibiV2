using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PROMOTION")]
    public class MOD_PROMOTIONEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("FromDate")]
        public DateTime FromDate { get; set; }
        [Column("ToDate")]
        public DateTime ToDate { get; set; }
        [Column("MenuID")]
        public int MenuID { get; set; }
        [Column("BrandID")]
        public int BrandID { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

