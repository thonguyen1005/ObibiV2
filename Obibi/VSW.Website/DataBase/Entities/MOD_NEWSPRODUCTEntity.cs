using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_NEWSPRODUCT")]
    public class MOD_NEWSPRODUCTEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("NewsID")]
        public int NewsID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

