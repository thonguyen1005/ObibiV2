using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_SUBSCRIBE")]
    public class MOD_SUBSCRIBEEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Phone")]
        public string Phone { get; set; }
        [Column("IP")]
        public string IP { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

