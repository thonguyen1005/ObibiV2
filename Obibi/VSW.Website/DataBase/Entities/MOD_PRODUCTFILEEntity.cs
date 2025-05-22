using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PRODUCTFILE")]
    public class MOD_PRODUCTFILEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("Default")]
        public bool Default { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("ProductSizeID")]
        public string ProductSizeID { get; set; }
        [Column("IsAvatar")]
        public bool IsAvatar { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

