using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_WEBUSER")]
    public class MOD_WEBUSEREntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("UserName")]
        public string UserName { get; set; }
        [Column("Password")]
        public string Password { get; set; }
        [Column("TempPassword")]
        public string TempPassword { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("Files")]
        public string Files { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Phone")]
        public string Phone { get; set; }
        [Column("Address")]
        public string Address { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("Facebook")]
        public string Facebook { get; set; }
        [Column("Skype")]
        public string Skype { get; set; }
        [Column("Zalo")]
        public string Zalo { get; set; }
        [Column("IP")]
        public string IP { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("Birthday")]
        public DateTime Birthday { get; set; }
        [Column("Gender")]
        public bool Gender { get; set; }
        [Column("Type")]
        public int Type { get; set; }
        [Column("CMND")]
        public string CMND { get; set; }
        [Column("CityID")]
        public int CityID { get; set; }
        [Column("DistrictID")]
        public int DistrictID { get; set; }
        [Column("WardID")]
        public int WardID { get; set; }
        [Column("Website")]
        public string Website { get; set; }
        [Column("CompanyName")]
        public string CompanyName { get; set; }
        [Column("CompanyAddress")]
        public string CompanyAddress { get; set; }
        [Column("CompanyTax")]
        public string CompanyTax { get; set; }
        [Column("Type2")]
        public int Type2 { get; set; }
        [Column("Point")]
        public long Point { get; set; }
        [Column("DateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

