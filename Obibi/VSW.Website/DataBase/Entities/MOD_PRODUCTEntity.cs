using System;
using LinqToDB.Mapping;
using System.Collections.Generic;

namespace VSW.Website.DataBase.Entities
{               
    [Table("MOD_PRODUCT")]
    public class MOD_PRODUCTEntity
    {
        #region Autogen by VSW
        [Column("ID", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }
        [Column("MenuID")]
        public int MenuID { get; set; }
        [Column("BrandID")]
        public int BrandID { get; set; }
        [Column("State")]
        public int State { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Model")]
        public string Model { get; set; }
        [Column("File")]
        public string File { get; set; }
        [Column("Price")]
        public long Price { get; set; }
        [Column("Price2")]
        public long Price2 { get; set; }
        [Column("View")]
        public int View { get; set; }
        [Column("Summary")]
        public string Summary { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("Specifications")]
        public string Specifications { get; set; }
        [Column("Tags")]
        public string Tags { get; set; }
        [Column("PageTitle")]
        public string PageTitle { get; set; }
        [Column("PageDescription")]
        public string PageDescription { get; set; }
        [Column("PageKeywords")]
        public string PageKeywords { get; set; }
        [Column("Published")]
        public DateTime Published { get; set; }
        [Column("Updated")]
        public DateTime Updated { get; set; }
        [Column("Order")]
        public int Order { get; set; }
        [Column("Activity")]
        public bool Activity { get; set; }
        [Column("Status")]
        public int Status { get; set; }
        [Column("Star")]
        public int Star { get; set; }
        [Column("Weight")]
        public double Weight { get; set; }
        [Column("MenuListID")]
        public string MenuListID { get; set; }
        
        [Column("PhienBan")]
        public string PhienBan { get; set; }
        [Column("GroupMenuID")]
        public int GroupMenuID { get; set; }
        [Column("TinhTrang")]
        public string TinhTrang { get; set; }
        [Column("CPUserID")]
        public int CPUserID { get; set; }
        [Column("BaoHanh")]
        public string BaoHanh { get; set; }
        [Column("NguonGoc")]
        public string NguonGoc { get; set; }
        [Column("DonVi")]
        public string DonVi { get; set; }
        [Column("SoLuong")]
        public int SoLuong { get; set; }
        [Column("HasProperty")]
        public bool HasProperty { get; set; }
        /*------------------------------------*/
        #endregion
        public long PriceView
        {
            get
            {
                return Price;
            }
        }

        public long PriceRoot
        {
            get
            {
                return Price2;
            }
        }
        private long _oSellOff;
        public long SellOff
        {
            get
            {
                if (_oSellOff == 0 && PriceRoot > PriceView)
                    _oSellOff = PriceRoot - PriceView;

                return _oSellOff;
            }
        }

        private long _oSellOffPercent;
        public long SellOffPercent
        {
            get
            {
                if (_oSellOffPercent == 0 && PriceRoot > 0 && SellOff > 0)
                    _oSellOffPercent = SellOff * 100 / PriceRoot;

                return _oSellOffPercent;
            }
        }
    }
}

