using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_ORDERDETAIL")]
    public class MOD_ORDERDETAILEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("OrderID")]
        public int OrderID { get; set; }
        [Column("ProductID")]
        public int ProductID { get; set; }
        [Column("SizeID")]
        public int SizeID { get; set; }
        [Column("ColorID")]
        public int ColorID { get; set; }
        [Column("Quantity")]
        public int Quantity { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Price")]
        public int Price { get; set; }
        [Column("PriceAll")]
        public long PriceAll { get; set; }
        [Column("Other")]
        public string Other { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

