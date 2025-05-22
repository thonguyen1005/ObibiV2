using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_URLSEOACTIVITY")]
    public class MOD_URLSEOACTIVITYEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("Url")]
        public string Url { get; set; }
        [Column("UrlRedirect")]
        public string UrlRedirect { get; set; }
        [Column("PageTitle")]
        public string PageTitle { get; set; }
        [Column("PageDescription")]
        public string PageDescription { get; set; }
        [Column("PageKeywords")]
        public string PageKeywords { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("CountSearch")]
        public int CountSearch { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("SchemaJson")]
        public string SchemaJson { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

