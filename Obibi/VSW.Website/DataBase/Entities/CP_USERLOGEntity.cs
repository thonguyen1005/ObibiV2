using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("CP_USERLOG")]
    public class CP_USERLOGEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("UserID")]
        public int UserID { get; set; }
        [Column("IP")]
        public string IP { get; set; }
        [Column("Note")]
        public string Note { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

