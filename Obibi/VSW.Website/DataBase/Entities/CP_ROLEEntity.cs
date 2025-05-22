using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("CP_ROLE")]
    public class CP_ROLEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Lock")]
        public bool Lock { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

