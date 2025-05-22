using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("CP_USER")]
    public class CP_USEREntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("LoginName")]
        public string LoginName { get; set; }
        [Column("Password")]
        public string Password { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Address")]
        public string Address { get; set; }
        [Column("Phone")]
        public string Phone { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("File")]
        public string File { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

