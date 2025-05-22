using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{
    [Table("CP_ACCESS")]
    public class CP_ACCESSEntity
    {
        #region Autogen by VSW
        [Column("RefCode")]
        public string RefCode { get; set; }
        [Column("RoleID")]
        public int RoleID { get; set; }
        [Column("UserID")]
        public int UserID { get; set; }
        [Column("Type")]
        public string Type { get; set; }
        [Column("Value")]
        public int Value { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

