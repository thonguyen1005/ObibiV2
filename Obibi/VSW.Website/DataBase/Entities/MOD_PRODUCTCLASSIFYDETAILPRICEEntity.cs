using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PRODUCTCLASSIFYDETAILPRICE")]
    public class MOD_PRODUCTCLASSIFYDETAILPRICEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("ClassifyDetailID1")]
        public int ClassifyDetailID1 { get; set; }
        [Column("ClassifyDetailID2")]
        public int ClassifyDetailID2 { get; set; }
        [Column("Price")]
        public long Price { get; set; }
        [Column("Price2")]
        public long Price2 { get; set; }
        [Column("SoLuong")]
        public int SoLuong { get; set; }
        [Column("Sku")]
        public string Sku { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

