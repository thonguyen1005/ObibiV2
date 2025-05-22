using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_AUTOLINKS")]
    public class MOD_AUTOLINKSEntity
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
        [Column("Target")]
        public string Target { get; set; }
        [Column("Quantity")]
        public int Quantity { get; set; }
        [Column("Published")]
        public DateTime Published { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

