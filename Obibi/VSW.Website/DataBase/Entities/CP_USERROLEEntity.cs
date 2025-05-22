using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("CP_USERROLE")]
    public class CP_USERROLEEntity
    {
        #region Autogen by VSW
        [Column("UserID")]
        public int UserID { get; set; }
        [Column("RoleID")]
        public int RoleID { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

