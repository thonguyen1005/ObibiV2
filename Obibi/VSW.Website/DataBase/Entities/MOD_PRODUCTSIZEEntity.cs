using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PRODUCTSIZE")]
    public class MOD_PRODUCTSIZEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("SizeID")]
        public int SizeID { get; set; }
        [Column("Price")]
        public long Price { get; set; }
        [Column("Price2")]
        public long Price2 { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("ColorID")]
        public int ColorID { get; set; }
        [Column("PricePromotion")]
        public long PricePromotion { get; set; }
        [Column("Weight")]
        public long Weight { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

