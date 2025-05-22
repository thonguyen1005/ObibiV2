using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_ADDRESS")]
    public class MOD_ADDRESSEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Phone")]
        public string Phone { get; set; }
        [Column("Address")]
        public string Address { get; set; }
        [Column("Facebook")]
        public string Facebook { get; set; }
        [Column("FBName")]
        public string FBName { get; set; }
        [Column("Zalo")]
        public string Zalo { get; set; }
        [Column("Map")]
        public string Map { get; set; }
        [Column("MapAddress")]
        public string MapAddress { get; set; }
        [Column("LatLong")]
        public string LatLong { get; set; }
        [Column("Published")]
        public DateTime Published { get; set; }
        [Column("Updated")]
        public DateTime Updated { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("ShowroomID")]
        public int ShowroomID { get; set; }
        [Column("CityID")]
        public int CityID { get; set; }
        [Column("DistrictID")]
        public int DistrictID { get; set; }
        [Column("WardID")]
        public int WardID { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

