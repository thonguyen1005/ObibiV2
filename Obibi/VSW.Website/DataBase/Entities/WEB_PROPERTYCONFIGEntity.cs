using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("WEB_PROPERTYCONFIG")]
    public class WEB_PROPERTYCONFIGEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("MenuID")]
        public int MenuID { get; set; }
        [Column("PropertyID")]
        public int PropertyID { get; set; }
        [Column("BrandID")]
        public int BrandID { get; set; }
        [Column("IsShowMenu")]
        public bool IsShowMenu { get; set; }
        [Column("ShowFilterFast")]
        public bool ShowFilterFast { get; set; }
        [Column("ShowBreadCrumb")]
        public bool ShowBreadCrumb { get; set; }
        [Column("PropertyParentID")]
        public int PropertyParentID { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

