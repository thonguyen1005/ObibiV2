using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{
    [Table("MOD_ORDER")]
    public class MOD_ORDEREntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("StatusID")]
        public int StatusID { get; set; }
        [Column("WebUserID")]
        public int WebUserID { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Total")]
        public long Total { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Phone")]
        public string Phone { get; set; }
        [Column("Address")]
        public string Address { get; set; }
        [Column("CityID")]
        public int CityID { get; set; }
        [Column("DistrictID")]
        public int DistrictID { get; set; }
        [Column("WardID")]
        public int WardID { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("Payment")]
        public int Payment { get; set; }
        [Column("StatusPay")]
        public bool StatusPay { get; set; }
        [Column("BankPay")]
        public string BankPay { get; set; }
        [Column("Invoice")]
        public bool Invoice { get; set; }
        [Column("CompanyName")]
        public string CompanyName { get; set; }
        [Column("CompanyAddress")]
        public string CompanyAddress { get; set; }
        [Column("CompanyTax")]
        public string CompanyTax { get; set; }
        [Column("Receive")]
        public int Receive { get; set; }
        [Column("ReceiveValue")]
        public int ReceiveValue { get; set; }
        [Column("IP")]
        public string IP { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }
        [Column("SaleCode")]
        public string SaleCode { get; set; }
        [Column("SaleMoney")]
        public long SaleMoney { get; set; }
        [Column("SalePercent")]
        public double SalePercent { get; set; }
        [Column("Fee")]
        public long Fee { get; set; }
        [Column("PayError")]
        public string PayError { get; set; }
        [Column("PayTranID")]
        public string PayTranID { get; set; }
        [Column("ShipDate")]
        public DateTime ShipDate { get; set; }
        [Column("ShipNote")]
        public string ShipNote { get; set; }
        [Column("OrderNote")]
        public string OrderNote { get; set; }
        [Column("ShipPartner")]
        public string ShipPartner { get; set; }
        [Column("ShipLabel")]
        public string ShipLabel { get; set; }
        [Column("ShipFee")]
        public long ShipFee { get; set; }
        [Column("ShipInsuranceFee")]
        public long ShipInsuranceFee { get; set; }
        [Column("ShipPickTime")]
        public string ShipPickTime { get; set; }
        [Column("ShipDeliverTime")]
        public string ShipDeliverTime { get; set; }
        [Column("ShipCheck")]
        public bool ShipCheck { get; set; }
        [Column("ShipType")]
        public int ShipType { get; set; }
        [Column("SaleCustomer")]
        public long SaleCustomer { get; set; }
        [Column("SaleMoneyPoint")]
        public long SaleMoneyPoint { get; set; }
        [Column("Point")]
        public long Point { get; set; }
        [Column("OrderNews")]
        public bool OrderNews { get; set; }
        [Column("Address2")]
        public string Address2 { get; set; }
        [Column("CityID2")]
        public int CityID2 { get; set; }
        [Column("DistrictID2")]
        public int DistrictID2 { get; set; }
        [Column("WardID2")]
        public int WardID2 { get; set; }
        [Column("AddressMore")]
        public bool AddressMore { get; set; }
        /*------------------------------------*/
        #endregion
    }
}

