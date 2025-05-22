using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PRODUCTFAQ")]
    public class MOD_PRODUCTFAQEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("FAQID")]
        public int FAQID { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("Promotion")]
        public string Promotion { get; set; }
        [Column("Info")]
        public string Info { get; set; }
        [Column("Promotion2")]
        public string Promotion2 { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

