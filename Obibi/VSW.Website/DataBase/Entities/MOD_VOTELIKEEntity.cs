using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_VOTELIKE")]
    public class MOD_VOTELIKEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("CommentID")]
        public int CommentID { get; set; }
        [Column("IP")]
        public string IP { get; set; }
        [Column("Like")]
        public bool Like { get; set; }
        [Column("UnLike")]
        public bool UnLike { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

