using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("WEB_RESOURCE")]
    public class WEB_RESOURCEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("LangID")]
        public int LangID { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Value")]
        public string Value { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

