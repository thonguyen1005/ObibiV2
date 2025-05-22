using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PRODUCTCLASSIFY")]
    public class MOD_PRODUCTCLASSIFYEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

