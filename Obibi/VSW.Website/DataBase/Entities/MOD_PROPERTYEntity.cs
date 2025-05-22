using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PROPERTY")]
    public class MOD_PROPERTYEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("MenuID")]
        public int MenuID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("PropertyID")]
        public int PropertyID { get; set; }
        [Column("PropertyValueID")]
        public int PropertyValueID { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

