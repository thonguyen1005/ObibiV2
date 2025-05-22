using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_NEWSTAG")]
    public class MOD_NEWSTAGEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("TagID")]
        public int TagID { get; set; }
        [Column("NewsID")]
        public int NewsID { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

