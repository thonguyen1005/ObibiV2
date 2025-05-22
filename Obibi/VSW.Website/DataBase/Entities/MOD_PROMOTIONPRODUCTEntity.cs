using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PROMOTIONPRODUCT")]
    public class MOD_PROMOTIONPRODUCTEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("PromotionID")]
        public int PromotionID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

