using System;
using System.Collections.Generic;

using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductSizeEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }

        [DataInfo]
        public int SizeID { get; set; }
        [DataInfo]
        public int ColorID { get; set; }

        [DataInfo]
        public long Price { get; set; }

        [DataInfo]
        public long Price2 { get; set; }
        [DataInfo]
        public int Order { get; set; }
        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public long PricePromotion { get; set; }
        [DataInfo]
        public long Weight { get; set; }
        #endregion

        private ModProductEntity _oProduct = null;
        public ModProductEntity getProduct()
        {
            if (_oProduct == null && ProductID > 0)
                _oProduct = ModProductService.Instance.GetByID(ProductID);

            if (_oProduct == null)
                _oProduct = new ModProductEntity();

            return _oProduct;
        }

        private ModSizeEntity _oSize = null;
        public ModSizeEntity getSize()
        {
            if (_oSize == null && SizeID > 0)
                _oSize = ModSizeService.Instance.GetByID(SizeID);

            if (_oSize == null)
                _oSize = new ModSizeEntity();

            return _oSize;
        }
        private ModColorEntity _oColor = null;
        public ModColorEntity getColor()
        {
            if (_oColor == null && ColorID > 0)
                _oColor = ModColorService.Instance.GetByID(ColorID);

            if (_oColor == null)
                _oColor = new ModColorEntity();

            return _oColor;
        }
        private long _PriceView;
        public long PriceView(bool Promotion)
        {
            return _PriceView = (Promotion && PricePromotion > 0 ? PricePromotion : Price);
        }
        private long _PriceView2;
        public long PriceView2(bool Promotion)
        {
            return _PriceView2 = (Price > PriceView(Promotion) ? Price : 0);
        }

        private long _oSellOff;
        public long SellOff(bool Promotion)
        {
            if (_oSellOff == 0 && PriceView2(Promotion) > PriceView(Promotion))
                _oSellOff = PriceView2(Promotion) - PriceView(Promotion);

            return _oSellOff;
        }

        private long _oSellOffPercent;
        public long SellOffPercent(bool Promotion)
        {
            if (_oSellOffPercent == 0 && PriceView2(Promotion) > 0 && SellOff(Promotion) > 0)
                _oSellOffPercent = SellOff(Promotion) * 100 / PriceView2(Promotion);

            return _oSellOffPercent;
        }
    }

    public class ModProductSizeService : ServiceBase<ModProductSizeEntity>
    {

        #region Autogen by VSW

        private ModProductSizeService()
            : base("[Mod_ProductSize]")
        {

        }

        private static ModProductSizeService _Instance = null;
        public static ModProductSizeService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModProductSizeService();

                return _Instance;
            }
        }

        #endregion

        public ModProductSizeEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public bool Exists(int product_id, int size_id)
        {
            return base.CreateQuery()
                .Select(o => o.ID)
                .Where(o => o.ProductID == product_id && o.SizeID == size_id)
                .Count()
                .ToValue()
                .ToBool();
        }
        public ModProductSizeEntity GetSingle_Cache(int product_id, int size_id, int color_id)
        {
            return base.CreateQuery()
                .Where(o => o.ProductID == product_id && o.SizeID == size_id && o.ColorID == color_id)
                .ToSingle_Cache();
        }
        public void InsertOrUpdate(int productID, long price, long price2, string Sizes, string Colors, long price3)
        {
            if (string.IsNullOrEmpty(Sizes) && string.IsNullOrEmpty(Colors))
            {
                Delete(o => o.ProductID == productID);
                return;
            }
            string[] arrSize = !string.IsNullOrEmpty(Sizes) ? Sizes.Split('|') : null;
            string[] arrColor = !string.IsNullOrEmpty(Colors) ? Colors.Split('|') : null;
            if ((arrSize == null || arrSize.Length == 0) && (Colors == null || Colors.Length == 0))
            {
                return;
            }
            int order = ModProductSizeService.Instance.CreateQuery()
                    .Where(o => o.ProductID == productID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;

            if (arrSize != null)
            {
                foreach (var size in arrSize)
                {
                    if (string.IsNullOrEmpty(size)) continue;
                    int sizeid = VSW.Core.Global.Convert.ToInt(size);
                    var checksize = CreateQuery().Where(o => o.ProductID == productID && o.SizeID == sizeid && o.SizeID > 0).ToSingle_Cache();
                    if (arrColor != null)
                    {
                        foreach (var color in arrColor)
                        {
                            int colorid = VSW.Core.Global.Convert.ToInt(color);
                            var check = CreateQuery().Where(o => o.ProductID == productID && o.SizeID == sizeid && o.ColorID == colorid).ToSingle_Cache();
                            if (check != null) continue;

                            check = new ModProductSizeEntity()
                            {
                                ProductID = productID,
                                SizeID = sizeid,
                                ColorID = colorid,
                                Price = (checksize != null ? checksize.Price : price),
                                Price2 = (checksize != null ? checksize.Price2 : price2),
                                PricePromotion = (checksize != null ? checksize.PricePromotion : price3),
                                Weight = (checksize != null ? checksize.Weight : 0),
                                Order = order,
                                Activity = true
                            };
                            Save(check);
                            order++;
                        }
                    }
                    else
                    {
                        var item = CreateQuery().Where(o => o.ProductID == productID && o.SizeID == sizeid && o.ColorID == 0).ToSingle_Cache();
                        if (item != null) continue;

                        item = new ModProductSizeEntity()
                        {
                            ProductID = productID,
                            SizeID = sizeid,
                            ColorID = 0,
                            Price = (checksize != null ? checksize.Price : price),
                            Price2 = (checksize != null ? checksize.Price2 : price2),
                            PricePromotion = (checksize != null ? checksize.PricePromotion : price3),
                            Weight = (checksize != null ? checksize.Weight : 0),
                            Order = order,
                            Activity = true
                        };
                        Save(item);
                        order++;
                    }
                }
            }
            else
            {
                foreach (var color in arrColor)
                {
                    int colorid = VSW.Core.Global.Convert.ToInt(color);
                    var check = CreateQuery().Where(o => o.ProductID == productID && o.SizeID == 0 && o.ColorID == colorid).ToSingle_Cache();
                    if (check != null) continue;

                    check = new ModProductSizeEntity()
                    {
                        ProductID = productID,
                        SizeID = 0,
                        ColorID = colorid,
                        Price = price,
                        Price2 = price2,
                        PricePromotion = price3,
                        Order = order,
                        Activity = true
                    };
                    Save(check);
                    order++;
                }
            }
            if (arrSize == null || arrSize.Length == 0)
            {
                Delete(o => o.ProductID == productID && o.SizeID > 0);
            }
            if (Colors == null || Colors.Length == 0)
            {
                Delete(o => o.ProductID == productID && o.ColorID > 0);
            }
            if (!string.IsNullOrEmpty(Sizes))
            {
                var listInDb = CreateQuery()
                                    .Where(o => o.ProductID == productID && o.SizeID > 0)
                                    .WhereNotIn(o => o.SizeID, Sizes.Replace("|", ","))
                                    .ToList();

                for (var i = 0; listInDb != null && i < listInDb.Count; i++)
                {
                    Delete(listInDb[i]);
                }
            }
            if (!string.IsNullOrEmpty(Colors))
            {
                var listInDb = CreateQuery()
                                    .Where(o => o.ProductID == productID && o.ColorID > 0)
                                    .WhereNotIn(o => o.ColorID, Colors.Replace("|", ","))
                                    .ToList();

                for (var i = 0; listInDb != null && i < listInDb.Count; i++)
                {
                    Delete(listInDb[i]);
                }
            }
            if (!string.IsNullOrEmpty(Sizes) && !string.IsNullOrEmpty(Colors))
            {
                Delete(o => o.ProductID == productID && (o.SizeID == 0 || o.ColorID == 0));
                return;
            }
        }
    }
}