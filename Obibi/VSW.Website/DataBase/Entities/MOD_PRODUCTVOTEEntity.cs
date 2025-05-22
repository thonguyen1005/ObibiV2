using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PRODUCTVOTE")]
    public class MOD_PRODUCTVOTEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Yes")]
        public int Yes { get; set; }
        [Column("No")]
        public int No { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("MenuID")]
        public int MenuID { get; set; }
        [Column("TypeView")]
        public int TypeView { get; set; }
        [Column("NameYes")]
        public string NameYes { get; set; }
        [Column("NameNo")]
        public string NameNo { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

