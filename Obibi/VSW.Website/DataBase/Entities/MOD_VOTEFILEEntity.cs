using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_VOTEFILE")]
    public class MOD_VOTEFILEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("CommentID")]
        public int CommentID { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Show")]
        public bool Show { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

