using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_VIDEO")]
    public class MOD_VIDEOEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("MenuID")]
        public int MenuID { get; set; }
        [Column("State")]
        public int State { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Summary")]
        public string Summary { get; set; }
        [Column("PageTitle")]
        public string PageTitle { get; set; }
        [Column("PageDescription")]
        public string PageDescription { get; set; }
        [Column("PageKeywords")]
        public string PageKeywords { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("Custom")]
        public string Custom { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("View")]
        public int View { get; set; }
        [Column("Published")]
        public DateTime Published { get; set; }
        [Column("Updated")]
        public DateTime Updated { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("Image")]
        public string Image { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

