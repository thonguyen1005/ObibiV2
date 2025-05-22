namespace VSW.Website.DataBase.Models
{
    public class ModProductModel
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int BrandID { get; set; }
        public int State { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Model { get; set; }
        public string File { get; set; }
        public long Price { get; set; }
        public long Price2 { get; set; }
        public int View { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Specifications { get; set; }
        public string Tags { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageKeywords { get; set; }
        public DateTime Published { get; set; }
        public DateTime Updated { get; set; }
        public int Order { get; set; }
        public bool Activity { get; set; }
        public int Status { get; set; }
        public int Star { get; set; }
        public long Weight { get; set; }
        public string MenuListID { get; set; }
        public string PhienBan { get; set; }
        public int GroupMenuID { get; set; }
        public string TinhTrang { get; set; }
        public int CPUserID { get; set; }
        public string BaoHanh { get; set; }
        public string NguonGoc { get; set; }
        public string DonVi { get; set; }
        public int SoLuong { get; set; }
        public bool HasProperty { get; set; }
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
        public int TotalBuy { get; set; }
        public int CountComment { get; set; }
        public int SizeByCart { get; set; }
        public int ColorByCart { get; set; }
        public int QuantityByCart { get; set; }
        public long PriceByCart { get; set; }
    }
}
