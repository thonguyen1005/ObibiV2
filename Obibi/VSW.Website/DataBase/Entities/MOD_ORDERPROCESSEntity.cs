using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_ORDERPROCESS")]
    public class MOD_ORDERPROCESSEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("OrderID")]
        public int OrderID { get; set; }
        [Column("Date")]
        public DateTime Date { get; set; }
        [Column("StatusID")]
        public int StatusID { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

