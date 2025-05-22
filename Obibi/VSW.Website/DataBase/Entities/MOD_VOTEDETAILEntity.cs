using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_VOTEDETAIL")]
    public class MOD_VOTEDETAILEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("ProductVoteID")]
        public int ProductVoteID { get; set; }
        [Column("CommentID")]
        public int CommentID { get; set; }
        [Column("Vote")]
        public bool Vote { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

