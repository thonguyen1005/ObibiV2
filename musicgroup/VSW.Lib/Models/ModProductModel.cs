using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }
        [DataInfo]
        public string MenuListID { get; set; }

        [DataInfo]
        public int BrandID { get; set; }

        [DataInfo]
        public int State { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string Model { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public long Price { get; set; }

        [DataInfo]
        public long Price2 { get; set; }

        [DataInfo]
        public long View { get; set; }
        [DataInfo]
        public string Tags { get; set; }
        [DataInfo]
        public string Summary { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public string Specifications { get; set; }

        [DataInfo]
        public string Custom { get; set; }

        [DataInfo]
        public string PageTitle { get; set; }

        [DataInfo]
        public string PageDescription { get; set; }

        [DataInfo]
        public string PageKeywords { get; set; }

        [DataInfo]
        public DateTime Published { get; set; }

        [DataInfo]
        public DateTime Updated { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public string Colors { get; set; }
        [DataInfo]
        public string Sizes { get; set; }
        [DataInfo]
        public int SizeID { get; set; }
        [DataInfo]
        public int ColorID { get; set; }
        [DataInfo]
        public int Status { get; set; }
        [DataInfo]
        public int Star { get; set; }
        [DataInfo]
        public long Weight { get; set; }
        [DataInfo]
        public DateTime DatePromotion { get; set; }
        [DataInfo]
        public long PricePromotion { get; set; }
        [DataInfo]
        public bool IsGift { get; set; }
        [DataInfo]
        public string TinhTrang { get; set; }
        [DataInfo]
        public string Promotion { get; set; }
        [DataInfo]
        public string Info { get; set; }
        [DataInfo]
        public string Promotion2 { get; set; }
        [DataInfo]
        public int GroupMenuID { get; set; }
        [DataInfo]
        public string PhienBan { get; set; }
        [DataInfo]
        public string TaiSaoChon { get; set; }
        [DataInfo]
        public int CPUserID { get; set; }
        [DataInfo]
        public string SchemaJson { get; set; }
        [DataInfo]
        public string InfoAdd { get; set; }
        [DataInfo]
        public string BaoHanh { get; set; }
        [DataInfo]
        public string NguonGoc { get; set; }
        [DataInfo]
        public string DonVi { get; set; }

        [DataInfo]
        public int SoLuong { get; set; }
        public List<ModTSKTModel> listTSKT { get; set; }
        public bool Check { get; set; }
        #endregion Autogen by VSW

        public ModProductEntity()
        {
            Items.SetValue("IsName", true);
            Items.SetValue("IsSummary", true);
        }
        private List<string> _ogetTag = null;
        public List<string> getTag()
        {
            if (_ogetTag == null)
            {
                string[] arrTag = this.Tags.Split(',');
                _ogetTag = new List<string>();

                for (int i = 0; arrTag != null && i < arrTag.Length; i++)
                    if (!string.IsNullOrEmpty(arrTag[i]))
                        _ogetTag.Add(arrTag[i]);
            }

            if (_ogetTag == null)
                _ogetTag = new List<string>();

            return _ogetTag;
        }

        private WebMenuEntity _oMenu;
        public WebMenuEntity GetMenu()
        {
            if (_oMenu == null && MenuID > 0)
                _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            return _oMenu ?? (_oMenu = new WebMenuEntity());
        }
        private WebMenuEntity _oGroupMenu;
        public WebMenuEntity GetGroupMenu()
        {
            if (_oGroupMenu == null && GroupMenuID > 0)
                _oGroupMenu = WebMenuService.Instance.GetByID_Cache(GroupMenuID);

            return _oGroupMenu ?? (_oGroupMenu = new WebMenuEntity());
        }

        private WebMenuEntity _oBrand;
        public WebMenuEntity GetBrand()
        {
            if (_oBrand == null && BrandID > 0)
                _oBrand = WebMenuService.Instance.GetByID_Cache(BrandID);

            return _oBrand ?? (_oBrand = new WebMenuEntity());
        }
        private WebMenuEntity _oStatus;
        public WebMenuEntity GetStatus()
        {
            if (_oStatus == null && Status > 0)
                _oStatus = WebMenuService.Instance.GetByID_Cache(Status);

            return _oStatus ?? (_oStatus = new WebMenuEntity());
        }
        public void UpView()
        {
            View++;
            ModProductService.Instance.Save(this, o => o.View);
        }

        private long _PriceView;
        public long PriceView
        {
            get
            {
                return _PriceView = (DatePromotion >= DateTime.Now && PricePromotion > 0 ? PricePromotion : Price);
            }
        }
        private long _PriceView2;
        public long PriceView2
        {
            get
            {
                return _PriceView2 = (Price > PriceView ? Price : 0);
            }
        }

        private long _oSellOff;
        public long SellOff
        {
            get
            {
                if (_oSellOff == 0 && PriceView2 > PriceView)
                    _oSellOff = PriceView2 - PriceView;

                return _oSellOff;
            }
        }

        private long _oSellOffPercent;
        public long SellOffPercent
        {
            get
            {
                if (_oSellOffPercent == 0 && PriceView2 > 0 && SellOff > 0)
                    _oSellOffPercent = SellOff * 100 / PriceView2;

                return _oSellOffPercent;
            }
        }

        private List<ModProductFileEntity> _oGetFile;
        public List<ModProductFileEntity> GetFile()
        {
            if (_oGetFile == null)
                _oGetFile = ModProductFileService.Instance.CreateQuery()
                                                .Where(o => o.ProductID == ID)
                                                .OrderByAsc(o => o.Order)
                                                .ToList_Cache();

            return _oGetFile ?? (_oGetFile = new List<ModProductFileEntity>());
        }

        private List<ModGiftEntity> _oGetGift;
        public List<ModGiftEntity> GetGift()
        {
            _oGetGift = new List<ModGiftEntity>();
            if (_oGetGift == null || _oGetGift.Count < 1)
            {
                var listItem = ModProductGiftService.Instance.CreateQuery()
                                                .Select(o => o.GiftID)
                                                .Where(o => o.ProductID == ID)
                                                .ToList_Cache();

                for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var item = ModGiftService.Instance.GetByID_Cache(listItem[i].GiftID);
                    if (item != null)
                        _oGetGift.Add(item);
                }
            }

            return _oGetGift;
        }
        private List<ModGiftEntity> _oGetGift2;
        public List<ModGiftEntity> GetGift2()
        {
            _oGetGift = new List<ModGiftEntity>();

            var listGiftByMenuAndHang = ModGiftService.Instance.CreateQuery()
                                        .Where(o => o.Activity == true)
                                        .WhereIn(MenuID > 0, o => o.MenuProductID, WebMenuService.Instance.GetChildIDForWeb_Cache("Product", MenuID, 1))
                                        .Where(BrandID > 0, o => (o.BrandID == BrandID || o.BrandID == 0))
                                        .ToList_Cache();
            if (listGiftByMenuAndHang != null && listGiftByMenuAndHang.Count > 0)
            {
                return listGiftByMenuAndHang;
            }

            if (_oGetGift == null || _oGetGift.Count < 1)
            {
                var listItem = ModProductGiftService.Instance.CreateQuery()
                                                .Select(o => o.GiftID)
                                                .Where(o => o.ProductID == ID)
                                                .ToList_Cache();

                for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var item = ModGiftService.Instance.GetByID_Cache(listItem[i].GiftID);
                    if (item != null)
                        _oGetGift.Add(item);
                }
            }

            return _oGetGift;
        }
        private List<ModProductEntity> _oGetOther;
        public List<ModProductEntity> GetOther()
        {
            _oGetOther = new List<ModProductEntity>();
            if (_oGetOther == null || _oGetOther.Count < 1)
            {
                var listItem = ModProductOtherService.Instance.CreateQuery()
                                                .Select(o => o.ProductOtherID)
                                                .Where(o => o.ProductID == ID)
                                                .OrderByAsc(o => new { o.Order, o.ID })
                                                .ToList_Cache();

                for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var item = ModProductService.Instance.GetByID_Cache(listItem[i].ProductOtherID);
                    if (item != null)
                        _oGetOther.Add(item);
                }
            }

            return _oGetOther;
        }
        private List<ModVideoEntity> _oGetVideo;
        public List<ModVideoEntity> GetVideo()
        {
            _oGetVideo = new List<ModVideoEntity>();
            if (_oGetVideo == null || _oGetVideo.Count < 1)
            {
                var listItem = ModProductVideoService.Instance.CreateQuery()
                                                .Select(o => o.VideoID)
                                                .Where(o => o.ProductID == ID)
                                                .OrderByAsc(o => new { o.Order, o.ID })
                                                .ToList_Cache();

                for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var item = ModVideoService.Instance.GetByID_Cache(listItem[i].VideoID);
                    if (item != null)
                        _oGetVideo.Add(item);
                }
            }

            return _oGetVideo;
        }
        private List<ModFAQEntity> _oGetFAQ;
        public List<ModFAQEntity> GetFAQ()
        {
            _oGetFAQ = new List<ModFAQEntity>();
            if (_oGetFAQ == null || _oGetFAQ.Count < 1)
            {
                var listItem = ModProductFAQService.Instance.CreateQuery()
                                                .Select(o => o.FAQID)
                                                .Where(o => o.ProductID == ID)
                                                .OrderByAsc(o => new { o.Order, o.ID })
                                                .ToList_Cache();

                for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var item = ModFAQService.Instance.GetByID_Cache(listItem[i].FAQID);
                    if (item != null)
                        _oGetFAQ.Add(item);
                }
            }

            return _oGetFAQ;
        }
        private List<ModProductEntity> _oGetOld;
        public List<ModProductEntity> GetOld()
        {
            _oGetOld = new List<ModProductEntity>();
            if (_oGetOld == null || _oGetOld.Count < 1)
            {
                var listItem = ModProductOldService.Instance.CreateQuery()
                                                .Select(o => o.ProductOldID)
                                                .Where(o => o.ProductID == ID)
                                                .OrderByAsc(o => new { o.Order, o.ID })
                                                .ToList_Cache();

                for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var item = ModProductService.Instance.GetByID_Cache(listItem[i].ProductOldID);
                    if (item != null)
                        _oGetOld.Add(item);
                }
            }

            return _oGetOld;
        }
        private CPUserEntity _oUser;

        public CPUserEntity GetUser()
        {
            if (_oUser == null && CPUserID > 0)
                _oUser = CPUserService.Instance.GetByID(CPUserID);

            return _oUser ?? (_oUser = new CPUserEntity());
        }
        private List<ModProductVoteEntity> _oGetVote;
        public List<ModProductVoteEntity> GetVote()
        {
            _oGetVote = new List<ModProductVoteEntity>();
            if (_oGetVote == null || _oGetVote.Count < 1)
            {
                _oGetVote = ModProductVoteService.Instance.CreateQuery()
                                                .Where(o => o.ProductID == ID)
                                                .OrderByAsc(o => new { o.Order, o.ID })
                                                .ToList_Cache();
            }
            if (_oGetVote == null) _oGetVote = new List<ModProductVoteEntity>();
            return _oGetVote;
        }

        private List<ModProductClassifyEntity> _oGetProductClassify;
        public List<ModProductClassifyEntity> GetProductClassify()
        {
            _oGetProductClassify = new List<ModProductClassifyEntity>();
            if (_oGetProductClassify == null || _oGetProductClassify.Count < 1)
            {
                _oGetProductClassify = ModProductClassifyService.Instance.CreateQuery()
                                                .Where(o => o.ProductID == ID)
                                                .OrderByAsc(o => new { o.ID })
                                                .ToList_Cache();
            }
            if (_oGetProductClassify == null) _oGetProductClassify = new List<ModProductClassifyEntity>();
            return _oGetProductClassify;
        }

        private List<ModProductClassifyDetailEntity> _oGetProductClassifyDetail;
        public List<ModProductClassifyDetailEntity> GetProductClassifyDetail()
        {
            _oGetProductClassifyDetail = new List<ModProductClassifyDetailEntity>();
            if (_oGetProductClassifyDetail == null || _oGetProductClassifyDetail.Count < 1)
            {
                _oGetProductClassifyDetail = ModProductClassifyDetailService.Instance.CreateQuery()
                                                .Where(o => o.ProductID == ID)
                                                .OrderByAsc(o => new { o.ID })
                                                .ToList_Cache();
            }
            if (_oGetProductClassifyDetail == null) _oGetProductClassifyDetail = new List<ModProductClassifyDetailEntity>();
            return _oGetProductClassifyDetail;
        }


        private List<ModProductClassifyDetailPriceEntity> _oGetProductClassifyDetailPrice;
        public List<ModProductClassifyDetailPriceEntity> GetProductClassifyDetailPrice()
        {
            _oGetProductClassifyDetailPrice = new List<ModProductClassifyDetailPriceEntity>();
            if (_oGetProductClassifyDetailPrice == null || _oGetProductClassifyDetailPrice.Count < 1)
            {
                _oGetProductClassifyDetailPrice = ModProductClassifyDetailPriceService.Instance.CreateQuery()
                                                .Where(o => o.ProductID == ID)
                                                .OrderByAsc(o => new { o.ClassifyDetailID1, o.ClassifyDetailID2 })
                                                .ToList_Cache();
            }
            if (_oGetProductClassifyDetailPrice == null) _oGetProductClassifyDetailPrice = new List<ModProductClassifyDetailPriceEntity>();
            return _oGetProductClassifyDetailPrice;
        }
    }

    public class ModProductService : ServiceBase<ModProductEntity>
    {
        #region Autogen by VSW

        public ModProductService() : base("[Mod_Product]")
        {
            //DBExecuteMode = DBExecuteType.DataReader;
            //DBType = DBType.SQL2012;
        }

        private static ModProductService _instance;

        public static ModProductService Instance => _instance ?? (_instance = new ModProductService());

        #endregion Autogen by VSW

        public ModProductEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModProductEntity GetByModel(string model)
        {
            if (string.IsNullOrEmpty(model)) return null;
            return CreateQuery()
               .Where(o => o.Model == model && o.Model != "")
               .ToSingle();
        }
        public ModProductEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }

        public ModProductEntity GetDataSelectByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .Select(o => new { o.ID, o.Name, o.Code, o.File, o.Price, o.Price2, o.Summary, o.MenuID, o.Activity })
               .ToSingle_Cache();
        }

        public bool Exists(string query)
        {
            return CreateQuery()
                           .Where(query)
                           .Count()
                           .ToValue()
                           .ToBool();
        }
    }
}