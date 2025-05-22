using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PRODUCTCLASSIFYDETAIL")]
    public class MOD_PRODUCTCLASSIFYDETAILEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("ClassifyID")]
        public int ClassifyID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("File")]
        public string File { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

