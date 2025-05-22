using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_AUTHOR")]
    public class MOD_AUTHOREntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("Banner")]
        public string Banner { get; set; }
        [Column("Facebook")]
        public string Facebook { get; set; }
        [Column("Zalo")]
        public string Zalo { get; set; }
        [Column("Tiktok")]
        public string Tiktok { get; set; }
        [Column("Youtube")]
        public string Youtube { get; set; }
        [Column("Position")]
        public string Position { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("Story")]
        public string Story { get; set; }
        [Column("PageTitle")]
        public string PageTitle { get; set; }
        [Column("PageDescription")]
        public string PageDescription { get; set; }
        [Column("PageKeywords")]
        public string PageKeywords { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

