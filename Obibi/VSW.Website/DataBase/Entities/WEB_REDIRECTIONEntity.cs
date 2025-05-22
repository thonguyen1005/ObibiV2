using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("WEB_REDIRECTION")]
    public class WEB_REDIRECTIONEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("Url")]
        public string Url { get; set; }
        [Column("Redirect")]
        public string Redirect { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

