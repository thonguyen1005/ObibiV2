using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_SALEDETAIL")]
    public class MOD_SALEDETAILEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("OrderID")]
        public int OrderID { get; set; }
        [Column("SaleID")]
        public int SaleID { get; set; }
        [Column("Money")]
        public long Money { get; set; }
        [Column("Published")]
        public DateTime Published { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

