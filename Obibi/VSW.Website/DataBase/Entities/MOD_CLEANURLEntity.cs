using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_CLEANURL")]
    public class MOD_CLEANURLEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Type")]
        public string Type { get; set; }
        [Column("Value")]
        public int Value { get; set; }
        [Column("MenuID")]
        public int MenuID { get; set; }
        [Column("LangID")]
        public int LangID { get; set; }
        [Column("CodeLength")]
        public int CodeLength { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

