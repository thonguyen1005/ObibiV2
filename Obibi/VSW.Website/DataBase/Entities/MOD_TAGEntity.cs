using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_TAG")]
    public class MOD_TAGEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Link")]
        public string Link { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Keywords")]
        public string Keywords { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("View")]
        public int View { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

