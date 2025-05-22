using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_VOTE")]
    public class MOD_VOTEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Phone")]
        public string Phone { get; set; }
        [Column("Vote")]
        public int Vote { get; set; }
        [Column("WebUserID")]
        public int WebUserID { get; set; }
        [Column("IP")]
        public string IP { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }
        [Column("ParentID")]
        public int ParentID { get; set; }
        [Column("Type")]
        public string Type { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("DaMua")]
        public bool DaMua { get; set; }
        [Column("GioiThieu")]
        public bool GioiThieu { get; set; }
        [Column("NgayMua")]
        public DateTime NgayMua { get; set; }
        [Column("AdminNote")]
        public string AdminNote { get; set; }
        [Column("ChucVu")]
        public string ChucVu { get; set; }
        [Column("OrderCode")]
        public string OrderCode { get; set; }
        [Column("OrderID")]
        public int OrderID { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

