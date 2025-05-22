<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<% 
    var item = ViewBag.Data as ModProductEntity;
    var listItem = ViewBag.Other as List<ModProductEntity>;
    var listPhienBan = ViewBag.PhienBan as List<ModProductEntity>;
    var listFile = item.GetFile();
    var modelBuy = VSW.Core.Global.DAO<GetSumModel>.Populate(ModOrderDetailService.Instance.ExecuteReader("select sum([Quantity]) as Total from " + ModOrderDetailService.Instance.TableName + " where [ProductID] = " + item.ID));
    var countBuy = modelBuy != null ? modelBuy.FirstOrDefault().Total : 0;
    //var listVideo = item.GetVideo();
    //var listFAQ = item.GetFAQ();
    int countVideo = ModProductVideoService.Instance.CreateQuery().Where(o => o.ProductID == item.ID).Count().ToValue_Cache().ToInt();
    var listProductClassify = item.GetProductClassify();
    var listProductClassifyDetail = item.GetProductClassifyDetail();
    var listProductClassifyDetailPrice = item.GetProductClassifyDetailPrice();
    var hasProperty = listProductClassify != null && listProductClassify.Count > 0;
    var listvote = ModVoteService.Instance.CreateQuery().Where(o => o.ProductID == item.ID && o.Type == "Product" && o.Activity == true).ToList();
    var listCommnet = (listvote != null ? listvote.Where(o => o.ParentID == 0 && o.Vote <= 0).OrderByDescending(o => o.Created).ToList() : new List<ModVoteEntity>());
    var listRating = (listvote != null ? listvote.Where(o => o.ParentID == 0 && o.Vote > 0).OrderByDescending(o => o.Created).ToList() : new List<ModVoteEntity>());
    double SumStar = (listRating != null ? listRating.Sum(o => o.Vote) : 0);
    int CountStar = (listRating != null ? listRating.Count : 0);
    int CountStar5 = (listRating != null ? listRating.Where(o => o.Vote == 5).Count() : 0);
    int CountStar4 = (listRating != null ? listRating.Where(o => o.Vote >= 4 && o.Vote < 5).Count() : 0);
    int CountStar3 = (listRating != null ? listRating.Where(o => o.Vote >= 3 && o.Vote < 4).Count() : 0);
    int CountStar2 = (listRating != null ? listRating.Where(o => o.Vote >= 2 && o.Vote < 3).Count() : 0);
    int CountStar1 = (listRating != null ? listRating.Where(o => o.Vote >= 1 && o.Vote < 2).Count() : 0);
    var StarTb = Math.Round(VSW.Core.Global.Convert.ToDouble(SumStar) / VSW.Core.Global.Convert.ToDouble(CountStar == 0 ? 1 : CountStar), 1);
    var Star5Tb = Math.Round(VSW.Core.Global.Convert.ToDouble(CountStar5) / VSW.Core.Global.Convert.ToDouble(CountStar == 0 ? 1 : CountStar) * 100);
    var Star4Tb = Math.Round(VSW.Core.Global.Convert.ToDouble(CountStar4) / VSW.Core.Global.Convert.ToDouble(CountStar == 0 ? 1 : CountStar) * 100);
    var Star3Tb = Math.Round(VSW.Core.Global.Convert.ToDouble(CountStar3) / VSW.Core.Global.Convert.ToDouble(CountStar == 0 ? 1 : CountStar) * 100);
    var Star2Tb = Math.Round(VSW.Core.Global.Convert.ToDouble(CountStar2) / VSW.Core.Global.Convert.ToDouble(CountStar == 0 ? 1 : CountStar) * 100);
    var Star1Tb = (CountStar > 0 ? (100 - Star5Tb - Star4Tb - Star3Tb - Star2Tb) : 0);
    var listImagesVote = ModVoteFileService.Instance.CreateQuery().Where(o => o.ProductID == item.ID && o.Show == true && o.Activity == true).OrderByAsc(o => o.Order).ToList_Cache();
    if (listImagesVote != null)
    {
        listImagesVote.ForEach(
            x =>
            {
                x.Commnet = x.GetCommnet();
            }
            );
    }
    //var arrPromotion = !string.IsNullOrEmpty(item.Promotion) ? item.Promotion.Split(new string[] { "<br/>" }, StringSplitOptions.None).ToList() : new List<string>() { "" };
    //var arrSummary = !string.IsNullOrEmpty(item.Summary) ? item.Summary.Split(new string[] { "<br/>" }, StringSplitOptions.None).ToList() : new List<string>() { "" };
    //var arrSpecifications = !string.IsNullOrEmpty(item.Specifications) ? item.Specifications.Split(new string[] { "<br/>" }, StringSplitOptions.None).ToList() : new List<string>() { "" };
    var lstPromotion = new List<ModPromotionEntity>();
    var lstPromotionProduct = ModPromotionProductService.Instance.CreateQuery().Where(o => o.ProductID == item.ID).ToList();
    for (int i = 0; lstPromotionProduct != null && i < lstPromotionProduct.Count; i++)
    {
        var promotion = lstPromotionProduct[i].GetPromotion();
        if (promotion.FromDate > DateTime.MinValue && promotion.FromDate > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)) continue;
        if (promotion.ToDate > DateTime.MinValue && promotion.ToDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1)) continue;
        lstPromotion.Add(promotion);
    }
    var a = WebMenuService.Instance.GetChildIDForWeb_Cache("Product", item.MenuID, ViewPage.CurrentLang.ID);
    var lstPromotionAll = ModPromotionService.Instance.CreateQuery()
        .Where(o => o.Activity == true)
        .Where(" MenuID = 0 or (MenuID > 0 and MenuID in (" + WebMenuService.Instance.GetChildIDForWeb_Cache("Product", item.MenuID, ViewPage.CurrentLang.ID) + "))")
        .Where(o => o.BrandID == 0 || o.BrandID == item.BrandID)
        .Where(" FromDate is null or (FromDate is not null and FromDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "')")
        .Where(" ToDate is null or (ToDate is not null and ToDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "')")
        .ToList();
    if (lstPromotionAll != null) lstPromotion.AddRange(lstPromotionAll);
    string Summary = !string.IsNullOrEmpty(item.Summary) ? item.Summary.Replace("<p>&nbsp;</p>", "") : "";
%>

<input type="hidden" id="ProductID" name="ProductID" value="<%=item.ID %>" />
<input type="hidden" id="ReturnPath" name="ReturnPath" value="<%=ViewPage.ReturnPath %>" />

<div id="scroll-to-item" class="dok-product-toc d-lg-none d-block" data-sticky data-sticky-class="toc-sticky">
    <ul>
        <li><a href="#dok-box-product-mobile">Tổng quan</a></li>
        <li><a href="#dok-to-thong-so">Thông số kỹ thuật</a></li>
        <li><a href="#dok-categories-info">Mô tả</a></li>
        <li><a href="#dok-to-danh-gia">Đánh giá</a></li>
        <li><a href="#dok-to-hoi-dap">Hỏi đáp</a></li>
    </ul>
</div>
<section class="dok-detail-product">
    <div class="container">
        <div class="box-product-top box-product-desktop d-lg-block d-none">
            <div class="d-flex align-items-center flex-1">
                <div class="d-flex align-items-center">
                    <div class="box-product-name">
                        <h1><%=item.Name %> </h1>
                    </div>
                    <div class="box-rating">
                        <div class="product__box-rating">
                            <div>
                                <%for (int z = 0; z < item.Star; z++)
                                    {%>
                                <div class="ic-star is-active">
                                    <i class="fas fa-star-sharp"></i>
                                </div>
                                <%} %>
                                <%for (int z = 0; z < 5 - item.Star; z++)
                                    {%>
                                <div class="ic-star">
                                    <i class="fas fa-star-sharp"></i>
                                </div>
                                <%} %>
                            </div>
                        </div>
                        <div class="text-primary"><a href="#dok-reviews">(<%=CountStar %> đánh giá)</a></div>
                        <%if (countBuy > 0)
                            { %>
                        <span class="text-secondary mx-2"><span class="mx-2">|</span> Đã bán <span class="text-primary"><%=countBuy %></span> </span>
                        <%} %>
                    </div>
                </div>
                <div class="box-stock ms-auto">
                    <span class="text-success"><i class="fa fa-check-circle"></i>&nbsp;&nbsp;Còn hàng</span>
                    <!-- <span class="text-warning"><i class="fa fa-clock"></i> Chờ đặt hàng</span> -->
                    <!-- <span class="text-danger"><i class="fa fa-times-circle"></i> Hết hàng</span> -->
                </div>
            </div>
        </div>

        <!-- Modal Menu Mobile -->
        <div id="dok-product-modal-preview" class="modal fade product-modal-preview" tabindex="-1"
            aria-hidden="true">
            <div class="modal-dialog modal-xl modal-dialog-scrollable">
                <div id="dok-product-modal-tab" class="modal-content dok-product-modal-tab dok-custom-tab">
                    <div class="modal-header">
                        <div class="dok-tab-nav">
                            <ul class="nav">
                                <li class="dok-tab-item active"><a href="#tab-image">Ảnh sản phẩm</a>
                                </li>
                                <%if (countVideo > 0)
                                    { %>
                                <li class="dok-tab-item tagVideo"><a href="#tab-video">Video sản phẩm</a></li>
                                <%} %>
                                <li class="dok-tab-item"><a href="#tab-technical">Thông số kỹ thuật</a>
                                </li>
                            </ul>
                        </div>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"
                            aria-label="Close">
                        </button>
                    </div>
                    <div class="modal-body position-relative">
                        <div class="dok-tab-content">
                            <div id="tab-image" class="dok-tab-pane active">
                                <div class="tab-mobile d-lg-none d-block">
                                    <div class="dok-product-img-main-mobile">
                                        <img class="img-fluid"
                                            src="<%=Utils.GetWebPFile(item.File, 4, 800, 500) %>" alt="<%=item.Name %>" />
                                        <%for (int i = 0; listFile != null && i < listFile.Count; i++)
                                            { %>
                                        <img class="img-fluid"
                                            src="<%=Utils.GetWebPFile(listFile[i].File, 4, 800, 500) %>" alt="<%=item.Name %> <%=i+1%>" />
                                        <%} %>
                                    </div>
                                </div>
                                <div class="tab-desktop d-lg-block d-none">
                                    <div class="swiper dok-product-img-main dok-product-img-main2-js">
                                        <div class="swiper-wrapper">
                                            <div class="swiper-slide">
                                                <img class="img-fluid"
                                                    src="<%=Utils.GetWebPFile(item.File, 4, 800, 500) %>" alt="<%=item.Name %>" />
                                            </div>
                                            <%for (int i = 0; listFile != null && i < listFile.Count; i++)
                                                { %>
                                            <div class="swiper-slide">
                                                <img class="img-fluid"
                                                    src="<%=Utils.GetWebPFile(listFile[i].File, 4, 800, 500) %>" alt="<%=item.Name %> <%=i+1%>" />
                                            </div>
                                            <%} %>
                                        </div>
                                        <div class="swiper-button-next"></div>
                                        <div class="swiper-button-prev"></div>
                                        <div class="swiper-pagination-number"></div>
                                    </div>

                                    <div thumbsslider="" class="swiper dok-product-thumbnail dok-product-thumbnail2-js">
                                        <div class="swiper-wrapper">
                                            <div class="swiper-slide">
                                                <img class="img-fluid"
                                                    src="<%=Utils.GetWebPFile(item.File, 4, 55, 55) %>" alt="<%=item.Name %>" />
                                            </div>
                                            <%for (int i = 0; listFile != null && i < listFile.Count; i++)
                                                { %>
                                            <div class="swiper-slide">
                                                <img class="img-fluid"
                                                    src="<%=Utils.GetWebPFile(listFile[i].File, 4, 55, 55) %>" alt="<%=item.Name %> <%=i+1%>" />
                                            </div>
                                            <%} %>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tab-video" class="dok-tab-pane boxVideo">
                            </div>
                            <div id="tab-technical" class="dok-tab-pane">
                                <%=item.Specifications %>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="dok-product-block">
            <div class="row">
                <div class="col-xl-7 col-lg-6 col-md-12">
                    <div class="dok-product-image">
                        <div class="swiper dok-product-img-main dok-product-img-main-js">
                            <div class="swiper-wrapper">
                                <%for (int i = 0; listFile != null && i < listFile.Count; i++)
                                    { %>
                                <div class="swiper-slide">
                                    <a data-bs-toggle="modal" data-bs-target="#dok-product-modal-preview"
                                        data-tab="image" data-bs-custom-class="beautifier"
                                        href="<%=Utils.GetWebPFile(listFile[i].File, 4, 800, 500)%>">
                                        <img class="img-fluid" src="<%=Utils.GetWebPFile(listFile[i].File, 4, 800, 500)%>" />
                                    </a>
                                </div>
                                <%} %>
                            </div>
                            <div class="swiper-button-next"></div>
                            <div class="swiper-button-prev"></div>
                            <div class="swiper-pagination-number"></div>
                        </div>
                        <div class="d-flex">
                            <%if (countVideo > 0)
                                { %>
                            <div class="video-thumb">
                                <a href="javascript:void(0);" class="text-nowrap" data-bs-toggle="modal"
                                    data-bs-target="#dok-product-modal-preview" data-tab="video">
                                    <span><i class="fa-brands fa-youtube"></i></span>
                                </a>
                                <span><%=countVideo %> video</span>
                            </div>
                            <%} %>
                            <div thumbsslider="" class="swiper dok-product-thumbnail dok-product-thumbnail-js">
                                <div class="swiper-wrapper">
                                    <%for (int i = 0; listFile != null && i < listFile.Count; i++)
                                        { %>
                                    <div class="swiper-slide">
                                        <img class="img-fluid" src="<%=Utils.GetWebPFile(listFile[i].File, 4, 55, 55)%>" />
                                    </div>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="dok-box-product-mobile" class="box-product-mobile d-lg-none d-block">
                        <div class="box-product-price mb-0">
                            <strong class="price productPrice">
                                <span>
                                    <%if (item.PriceView > 0)
                                        { %>
                                    <%=Utils.FormatMoney(item.PriceView) %> đ
                                            <%}
                                                else
                                                { %>
                                                {RS:Lienhe}
                                            <%} %>
                                </span>
                                <%if (item.PriceView2 > 0)
                                    { %>
                                <del><span><%=Utils.FormatMoney(item.PriceView2) %>₫</span></del>
                                <%} %>
                            </strong>
                        </div>

                        <div class="box-product-name">
                            <h1><%=item.Name %></h1>
                        </div>

                        <div class="d-flex align-items-center justify-content-between my-4">
                            <div class="d-flex align-items-center">
                                <b class="me-1"><i class="fa fa-star text-warning"></i><%=item.Star %></b>
                                <div class="text-primary">
                                    <a href="#dok-reviews">(<%=CountStar %> đánh giá)</a>
                                </div>
                                <%if (countBuy > 0)
                                    { %>
                                <span class="text-secondary mx-3">|</span>
                                <div>
                                    Đã bán: <a class="text-primary fw-bold"><%=countBuy %></a>
                                </div>
                                <%} %>
                            </div>
                            <div class="box-stock">
                                <span class="text-success"><i class="fa fa-check-circle"></i>&nbsp;&nbsp;Còn hàng</span>
                                <!-- <span class="text-warning"><i class="fa fa-clock"></i> Chờ đặt hàng</span> -->
                                <!-- <span class="text-danger"><i class="fa fa-times-circle"></i> Hết hàng</span> -->
                            </div>
                        </div>
                        <ul class="box-product-extrainfo">
                            <%if (!string.IsNullOrEmpty(item.DonVi))
                                { %>
                            <li>
                                <strong>Đơn vị</strong> <%=item.DonVi %>
                            </li>
                            <%} %>
                            <%if (!string.IsNullOrEmpty(item.TinhTrang))
                                { %>
                            <li>
                                <strong>Tình trạng loa</strong> <%=item.TinhTrang %>
                            </li>
                            <%} %>
                            <%if (!string.IsNullOrEmpty(item.NguonGoc))
                                { %>
                            <li>
                                <strong>Nguồn gốc/xuất xứ</strong> <%=item.NguonGoc %>
                            </li>
                            <%} %>
                            <%if (!string.IsNullOrEmpty(item.BaoHanh))
                                { %>
                            <li>
                                <strong>Bảo hành</strong> <%=item.BaoHanh %>
                            </li>
                            <%} %>
                        </ul>
                        {RS:ServiceDetailPolicy}
                        <%if (listProductClassify != null)
                            { %>
                        <div class="box-buy-options py-2 mb-3">
                            <%if (listProductClassify != null)
                                { %>
                            <%for (int i = 0; i < listProductClassify.Count; i++)
                                {
                                    var lstItem = listProductClassifyDetail.Where(o => o.ClassifyID == listProductClassify[i].ID).ToList();
                            %>
                            <div class="box-product-filter">
                                <p class="fz-14 fw-medium text-uppercase mb-3"><%=listProductClassify[i].Name %></p>
                                <div class="product-filter-list">
                                    <%for (int z = 0; lstItem != null && z < lstItem.Count; z++)
                                        {%>
                                    <label>
                                        <input type="radio" name="<%=i== 0 ? "Color" : "Size" %>" value="<%=lstItem[z].ID %>" onclick="ChangePrice();" />
                                        <span class="radio-btn">
                                            <%if (!string.IsNullOrEmpty(lstItem[z].File))
                                                { %>
                                            <img title="<%=lstItem[z].Name %>" alt="<%=lstItem[z].Name %>"
                                                src="<%=Utils.GetWebPFile(lstItem[z].File, 4, 30,30) %>">
                                            <%} %>
                                            <span class="d-flex text-start flex-column">
                                                <b class="text"><%=lstItem[z].Name %></b>
                                            </span>
                                        </span>
                                    </label>
                                    <%} %>
                                </div>
                            </div>
                            <% } %>
                            <%} %>
                        </div>
                        <%} %>

                        <%if (listPhienBan != null && listPhienBan.Count > 0)
                            { %>
                        <div class="box-buy-options py-2 mb-3">
                            <div class="box-product-filter">
                                <p class="fz-14 fw-medium text-uppercase mb-3">Lựa chọn phiên bản</p>
                                <div class="product-filter-list">
                                    <%for (int z = 0; listPhienBan != null && z < listPhienBan.Count; z++)
                                        {%>
                                    <label>
                                        <a href="<%=ViewPage.GetURL(listPhienBan[z].MenuID, listPhienBan[z].Code) %>" class="radio-btn">
                                            <%if (!string.IsNullOrEmpty(listPhienBan[z].File))
                                                { %>
                                            <img title="<%=listPhienBan[z].Name %>" alt="<%=listPhienBan[z].Name %>"
                                                src="<%=Utils.GetWebPFile(listPhienBan[z].File, 4, 30,30) %>">
                                            <%} %>
                                            <span class="d-flex text-start flex-column">
                                                <b class="text"><%=listPhienBan[z].Name %></b>
                                            </span>
                                        </a>
                                    </label>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                        <%} %>
                        <div class="box-order-button-container my-3">
                            <div class="d-flex justify-content-between align-items-center">
                                <button class="btn-cta order-button addcartnow" data-id="<%=item.ID %>" data-hasproperty="<%=hasProperty.ToString().ToLower() %>">
                                    <strong>MUA NGAY</strong>
                                    <span>(Giao nhanh từ 2 giờ hoặc nhận tại cửa hàng)</span>
                                </button>
                                <button class="btn-cta add-to-cart-button addcart addToCartAjax" data-id="<%=item.ID %>" data-hasproperty="<%=hasProperty.ToString().ToLower() %>" data-bs-toggle="modal"
                                    data-bs-target="#dok-add-to-cart-desktop">
                                    <i class="far fa-cart-plus"></i>
                                    <span>Thêm vào giỏ</span>
                                </button>
                            </div>
                        </div>
                        <%if (lstPromotion != null && lstPromotion.Count > 0)
                            { %>
                        <div class="box-product-promotion">
                            <div class="box-product-promotion-header ">
                                <div class="icon"><i class="fa-solid fa-gift"></i></div>
                                <span>Khuyến mãi</span>
                            </div>
                            <div class="box-product-promotion-content">
                                <%for (int i = 0; i < lstPromotion.Count; i++)
                                    {
                                        if (string.IsNullOrEmpty(lstPromotion[i].Content)) continue;
                                        string content = lstPromotion[i].Content.Replace("<p>&nbsp;</p>", "");
                                %>
                                <%=content %>
                                <%} %>
                            </div>
                        </div>
                        <%} %>
                        <p class="my-lg-0 my-3 fz-12">
                            <i class="fa-solid fa-bolt-lightning text-warning me-2"></i>Đã có
                           
                            <span class="text-primary"><%=item.View %></span> người quan tâm tới sản phẩm này và
                          
                            <%if (countBuy > 0)
                                { %>
                            <span class="text-primary"><%=countBuy %></span> người đã mua. Mua ngay!
                            <%} %>
                        </p>
                        <hr>
                        <%if (!string.IsNullOrEmpty(item.Summary))
                            {%>
                        <div id="dok-to-dac-diem-noi-bat" class="box-warranty-info">
                            <div class="box-title text-center">
                                <p>Đặc điểm nổi bật</p>
                            </div>
                            <div class="item-warranty-info">
                                <%=Summary %>
                            </div>
                        </div>

                        <hr>
                        <%} %>
                        <div id="dok-to-thong-so" class="dok-widget dok-widget-thong-so">
                            <h2 class="fw-bold fz-18 mb-3">Thông số kỹ thuật</h2>
                            <%=item.Specifications %>
                            <a class="readmore-btn mt-4 mb-0 down" href="javascript:void(0);"
                                data-bs-toggle="modal" data-bs-target="#dok-product-modal-preview"
                                data-tab="technical"><span>Xem cấu hình chi tiết</span>
                            </a>
                        </div>
                    </div>
                    <div class="box-product-desktop d-lg-block d-none">
                        <%if (!string.IsNullOrEmpty(item.Summary))
                            {%>
                        <div class="box-warranty-info">
                            <div class="box-title text-center">
                                <p>Đặc điểm nổi bật</p>
                            </div>
                            <div class="item-warranty-info">
                                <%=Summary %>
                            </div>
                        </div>
                        <%} %>
                     {RS:DetailPolicy}
                    </div>
                </div>

                <div class="col-xl-5 col-lg-6 col-xs-md">
                    <div class="box-product-desktop d-lg-block d-none">
                        <div class="d-flex justify-content-between ">
                            <%if (item.BrandID > 0)
                                {
                                    string url = "";
                                    var brand = item.GetBrand();
                                    var brandMenu = SysPageService.Instance.CreateQuery().Where(o => o.BrandID == item.BrandID && o.MenuID == 2).ToSingle_Cache();
                                    if (brandMenu != null)
                                    {
                                        url = ViewPage.GetPageURL(brandMenu);
                                    }
                                    else
                                    {
                                        url = ViewPage.GetURL("thuong-hieu-" + brand.Code);
                                    }
                            %>
                            <div>
                                Thương hiệu: <a href="url" class="text-primary fw-bold"><%=brand.Name %></a>
                            </div>
                            <%} %>
                            <div>
                                Mã SP: <span class="text-dark fw-bold"><%=item.Model %></span>
                            </div>
                        </div>
                        <hr>
                        <div class="box-product-price">
                            <span class="me-3 fz-14">Giá bán:</span>
                            <strong class="price productPrice">
                                <span>
                                    <%if (item.PriceView > 0)
                                        { %>
                                    <%=Utils.FormatMoney(item.PriceView) %> đ
                                            <%}
                                                else
                                                { %>
                                                {RS:Lienhe}
                                            <%} %>
                                </span>
                                <%if (item.PriceView2 > 0)
                                    { %>
                                <del><span><%=Utils.FormatMoney(item.PriceView2) %> ₫</span></del>
                                <%} %>
                            </strong>
                        </div>
                        <%if (listProductClassify != null)
                            { %>
                        <%for (int i = 0; i < listProductClassify.Count; i++)
                            {
                                var lstItem = listProductClassifyDetail.Where(o => o.ClassifyID == listProductClassify[i].ID).ToList();
                        %>
                        <div class="box-product-filter">
                            <p class="fz-14 fw-medium text-uppercase mb-3"><%=listProductClassify[i].Name %></p>
                            <div class="product-filter-list">
                                <%for (int z = 0; lstItem != null && z < lstItem.Count; z++)
                                    {%>
                                <label>
                                    <input type="radio" name="<%=i== 0 ? "Color" : "Size" %>" value="<%=lstItem[z].ID %>" onclick="ChangePrice();" />
                                    <span class="radio-btn">
                                        <%if (!string.IsNullOrEmpty(lstItem[z].File))
                                            { %>
                                        <img title="<%=lstItem[z].Name %>" alt="<%=lstItem[z].Name %>"
                                            src="<%=Utils.GetWebPFile(lstItem[z].File, 4, 30,30) %>">
                                        <%} %>
                                        <span class="d-flex text-start flex-column">
                                            <b class="text"><%=lstItem[z].Name %></b>
                                        </span>
                                    </span>
                                </label>
                                <%} %>
                            </div>
                        </div>
                        <% } %>
                        <%} %>
                        <%if (listPhienBan != null && listPhienBan.Count > 0)
                            { %>
                        <div class="box-buy-options py-2 mb-3">
                            <div class="box-product-filter">
                                <p class="fz-14 fw-medium text-uppercase mb-3">Lựa chọn phiên bản</p>
                                <div class="product-filter-list">
                                    <%for (int z = 0; listPhienBan != null && z < listPhienBan.Count; z++)
                                        {%>
                                    <label>
                                        <a href="<%=ViewPage.GetURL(listPhienBan[z].MenuID, listPhienBan[z].Code) %>" class="radio-btn">
                                            <%if (!string.IsNullOrEmpty(listPhienBan[z].File))
                                                { %>
                                            <img title="<%=listPhienBan[z].Name %>" alt="<%=listPhienBan[z].Name %>"
                                                src="<%=Utils.GetWebPFile(listPhienBan[z].File, 4, 30,30) %>">
                                            <%} %>
                                            <span class="d-flex text-start flex-column">
                                                <b class="text"><%=listPhienBan[z].Name %></b>
                                            </span>
                                        </a>
                                    </label>
                                    <%} %>
                                </div>
                            </div>
                        </div>
                        <%} %>
                        <ul class="box-product-extrainfo">
                            <%if (!string.IsNullOrEmpty(item.DonVi))
                                { %>
                            <li>
                                <strong>Đơn vị</strong> <%=item.DonVi %>
                            </li>
                            <%} %>
                            <%if (!string.IsNullOrEmpty(item.TinhTrang))
                                { %>
                            <li>
                                <strong>Tình trạng loa</strong> <%=item.TinhTrang %>
                            </li>
                            <%} %>
                            <%if (!string.IsNullOrEmpty(item.NguonGoc))
                                { %>
                            <li>
                                <strong>Nguồn gốc/xuất xứ</strong> <%=item.NguonGoc %>
                            </li>
                            <%} %>
                            <%if (!string.IsNullOrEmpty(item.BaoHanh))
                                { %>
                            <li>
                                <strong>Bảo hành</strong> <%=item.BaoHanh %>
                            </li>
                            <%} %>
                        </ul>
                        <%if (lstPromotion != null && lstPromotion.Count > 0)
                            { %>
                        <div class="box-product-promotion">
                            <div class="box-product-promotion-header ">
                                <div class="icon"><i class="fa-solid fa-gift"></i></div>
                                <span>Khuyến mãi</span>
                            </div>
                            <div class="box-product-promotion-content">
                                <%for (int i = 0; i < lstPromotion.Count; i++)
                                    {
                                        if (string.IsNullOrEmpty(lstPromotion[i].Content)) continue;
                                        string content = lstPromotion[i].Content.Replace("<p>&nbsp;</p>", "");
                                %>
                                <%=content %>
                                <%} %>
                            </div>
                        </div>
                        <%} %>
                        <div class="box-order-button-container my-3">
                            <div class="d-flex justify-content-between align-items-center">
                                <button class="btn-cta order-button addcartnow" data-id="<%=item.ID %>" data-hasproperty="<%=hasProperty.ToString().ToLower() %>">
                                    <strong>MUA NGAY</strong>
                                    <span>(Giao nhanh từ 2 giờ hoặc nhận tại cửa hàng)</span>
                                </button>
                                <button class="btn-cta add-to-cart-button addcart addToCartAjax" data-id="<%=item.ID %>" data-hasproperty="<%=hasProperty.ToString().ToLower() %>" data-bs-toggle="modal"
                                    data-bs-target="#dok-add-to-cart-desktop">
                                    <i class="far fa-cart-plus"></i>
                                    <span>Thêm vào giỏ</span>
                                </button>
                            </div>
                        </div>
                        {RS:BannerTraGopDetailProduct}
                        <p class="my-lg-0 my-3 fz-12">
                            <i class="fa-solid fa-bolt-lightning text-warning me-2"></i>Đã có                           
                            <span class="text-primary"><%=item.View %></span> người quan tâm tới sản phẩm này và                          
                            <%if (countBuy > 0)
                                { %>
                            <span class="text-primary"><%=countBuy %></span> người đã mua. Mua ngay!
                            <%} %>
                        </p>

                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
<div class="container">
    <div class="row">
        <div class="col-md-8">
            <hr class="mobile-line" />
            <div id="dok-categories-info" class="dok-categories-info mb-4">
                <div class="_bd">
                    <div class="short custom-scrollbar" style="max-height: 500px; overflow: hidden;">
                        <div class="_ct">
                            <%=Utils.GetHtmlForSeo(item.Content) %>
                        </div>
                        <div class="show_light"></div>
                    </div>
                </div>
                <a class="show" href="javascript:void(0);">
                    <span>Xem thêm</span>
                </a>
            </div>

            <div class="hide" id="boxFaq">
                <hr class="mobile-line" />
                <div class="dok-faqs mb-4">
                    <h2 class="title">Câu hỏi thường gặp</h2>
                    <div class="accordion" id="accordion">
                    </div>
                </div>
            </div>
            <hr class="mobile-line" />

            <div id="dok-to-danh-gia">
                <div id="dok-reviews" class="dok-reviews dok-box-s">
                    <p class="dok-box-s-title">
                        Đánh giá của khách hàng về <%=item.Name %>
                    </p>
                    <div id="reviews" class="scrollto">
                        <div class="box-border bg-white">
                            <div class="rating " id="danhgia">
                                <p class="rating__title"><%=item.Name %></p>
                                <div class="rating-star ">
                                    <div class="rating-left">
                                        <div class="rating-top">
                                            <p class="point"><%=StarTb%></p>
                                            <div class="list-star">
                                                <%for (int z = 0; z < Math.Round(StarTb, 0); z++)
                                                    {%>
                                                <i class="fa fa-star"></i>
                                                <%} %>
                                                <%for (int z = 0; z < 5 - Math.Round(StarTb, 0); z++)
                                                    {%>
                                                <i class="fa fa-star none"></i>
                                                <%} %>
                                            </div>
                                            <a href="<%=ViewPage.GetURL(item.MenuID, item.Code) %>?view=danhgia" class="rating-total"><%=CountStar %> đánh giá</a>
                                        </div>
                                        <ul class="rating-list">
                                            <li>
                                                <div class="number-star">5 <i class="fa fa-star black"></i></div>
                                                <div class="timeline-star">
                                                    <p class="timing" style="width: <%=Star5Tb%>%"></p>
                                                </div>
                                                <p class="number-percent"><%=Star5Tb%>%</p>
                                            </li>
                                            <li>
                                                <div class="number-star">4 <i class="fa fa-star black"></i></div>
                                                <div class="timeline-star">
                                                    <p class="timing" style="width: <%=Star4Tb%>%"></p>
                                                </div>
                                                <p class="number-percent"><%=Star4Tb%>%</p>
                                            </li>
                                            <li>
                                                <div class="number-star">3 <i class="fa fa-star black"></i></div>
                                                <div class="timeline-star">
                                                    <p class="timing" style="width: <%=Star3Tb%>%"></p>
                                                </div>
                                                <p class="number-percent"><%=Star3Tb%>%</p>
                                            </li>
                                            <li>
                                                <div class="number-star">
                                                    2 <i class="fa fa-star black"></i>
                                                </div>
                                                <div class="timeline-star">
                                                    <p class="timing" style="width: <%=Star2Tb%>%"></p>
                                                </div>
                                                <p class="number-percent"><%=Star2Tb%>%</p>
                                            </li>
                                            <li>
                                                <div class="number-star">
                                                    1 <i class="fa fa-star black"></i>
                                                </div>
                                                <div class="timeline-star">
                                                    <p class="timing" style="width: <%=Star1Tb%>%"></p>
                                                </div>
                                                <p class="number-percent"><%=Star1Tb%>%</p>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="rating-right">
                                        <div class="rating-img ">
                                            <ul class="rating-img-list">
                                                <%for (int i = 0; listImagesVote != null && i < listImagesVote.Count; i++)
                                                    {
                                                        var cm = listRating.Where(o => o.ID == listImagesVote[i].CommentID).FirstOrDefault();
                                                %>
                                                <li class="js-Showcmt">
                                                    <a href="<%=Utils.GetWebPFile(listImagesVote[i].File, 4, 50,50) %>"
                                                        data-fancybox="review" data-caption="<%=cm != null ? cm.Content : "Rating"+(i+1) %>">
                                                        <img class="lazyload"
                                                            src="<%=Utils.GetWebPFile(listImagesVote[i].File, 4, 50,50) %>"
                                                            alt="Image Rating <%=(i+1) %>">
                                                    </a>
                                                </li>
                                                <%} %>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="comment comment--all ratingLst">
                                <%for (int i = 0; listRating != null && i < listRating.Count; i++)
                                    {
                                        var listVoteDetail = ModVoteDetailService.Instance.CreateQuery()
                                                            .Where(o => o.CommentID == listRating[i].ID)
                                                            .ToList_Cache();

                                        var listSub = listvote.Where(o => o.ParentID == listRating[i].ID).OrderBy(o => o.Created).ToList();
                                %>
                                <div class="comment__item par">
                                    <div class="item-top">
                                        <p class="txtname"><%=listRating[i].Name %></p>
                                        <%if (listRating[i].DaMua)
                                            { %>
                                        <p class="tickbuy">
                                            <i class="icondetail-tickbuy"></i>
                                            {RS:DaMua}
                                        </p>
                                        <%} %>
                                    </div>
                                    <div class="item-rate">
                                        <div class="comment-star">
                                            <%for (int z = 0; z < Math.Round(listRating[i].Vote, 0); z++)
                                                {%>
                                            <i class="fa fa-star"></i>
                                            <%} %>
                                            <%for (int z = 0; z < 5 - Math.Round(listRating[i].Vote, 0); z++)
                                                {%>
                                            <i class="fa fa-star none"></i>
                                            <%} %>
                                        </div>
                                    </div>
                                    <div class="comment-content">
                                        <p class="cmt-txt">
                                            <%=listRating[i].Content %>
                                        </p>
                                    </div>
                                    <%if (listImagesVote != null)
                                        {
                                            var listImg = listImagesVote.Where(o => o.CommentID == listRating[i].ID && o.Show == true).ToList();%>
                                    <%if (listImg != null)
                                        { %>
                                    <div class="comment-content">
                                        <%for (int j = 0; listImg != null && j < listImg.Count; j++)
                                            {%>
                                        <div class="cmt-img">
                                            <a href="https://bcec.vn/upload/image/loa-bluetooth-alpha-work-w88-1.webp"
                                                data-fancybox="review"
                                                data-caption="<%=listRating[i].Content %>">
                                                <img class="lazyload"
                                                    src="<%=Utils.GetWebPFile(listImg[j].File, 4, 125, 125) %>"
                                                    alt="Image Rating <%=i+1 %>_<%=j+1 %>">
                                            </a>
                                        </div>
                                        <%} %>
                                    </div>
                                    <%} %>
                                    <%} %>
                                    <%if (!string.IsNullOrEmpty(listRating[i].AdminNote))
                                        { %>
                                    <p class="support">
                                        <%=listRating[i].AdminNote %>
                                    </p>
                                    <%} %>
                                    <div class="item-click">
                                        <a href="javascript:likeRating(<%=listRating[i].ID %>,0);" class="click-like" data-like="0" id="CommentLike_<%=listRating[i].ID %>">
                                            <i class="icondetail-likewhite"></i>
                                            Hữu ích
                                        </a>
                                        <a href="javascript:showRatingCmtChild('r-<%=listRating[i].ID %>')" class="click-cmt">
                                            <i class="fa fa-comments-o"></i>
                                            <span class="cmtr"><%=listSub != null ? listSub.Count : 0 %> thảo luận</span>
                                        </a>
                                        <div class="rr-<%=listRating[i].ID %> reply hide">
                                            <input name="CommentContent<%=listRating[i].ID %>" id="CommentContent<%=listRating[i].ID %>" placeholder="Nhập thảo luận của bạn">
                                            <a href="javascript:ratingRelply(<%=item.ID %>,<%=listRating[i].ID %>,'Product');" class="rrSend">Gửi</a>
                                            <div class="ifrl hide">
                                                <span></span>| <a href="javascript:showReplyConfirmPopup()">Sửa tên</a>
                                            </div>
                                        </div>
                                    </div>
                                    <%for (int z = 0; listSub != null && z < listSub.Count; z++)
                                        {%>
                                    <div class="comment__item rp-<%=listRating[i].ID %> childC__item hide">
                                        <div class="item-top">
                                            <p class="txtname"><%=listSub[z].Name %></p>
                                            <%if (!string.IsNullOrEmpty(listSub[z].ChucVu))
                                                { %>
                                            <span class="qtv"><%=listSub[z].ChucVu %></span>
                                            <%} %>
                                        </div>
                                        <div class="item-rate">
                                        </div>
                                        <div class="comment-content">
                                            <p class="cmt-txt">
                                                <%=listSub[z].Content %>
                                            </p>
                                        </div>
                                        <div class="item-click">
                                            <a href="javascript:likeRating(<%=listSub[z].ID %>,0);" class="click-like" data-like="0" id="CommentLike_<%=listSub[z].ID %>">
                                                <i class="icondetail-likewhite"></i>
                                                Hữu ích
                                            </a>
                                        </div>
                                    </div>
                                    <%} %>
                                </div>
                                <%} %>
                            </div>
                            <div class="comment-btn">
                                <a href="javascript:showInputRating();" class="comment-btn__item blue"><i class="fa fa-star"></i>Viết đánh giá</a>
                            </div>
                            <div class="read-assess hide">
                                <div class="read-assess__top">
                                    <p class="read-assess__title">Đánh giá</p>
                                    <div class="read-assess-close btn-closemenu" onclick="hideInputRating()"><i class="fa fa-close"></i>Đóng</div>
                                </div>
                                <div class="read-assess-main">
                                    <form method="post" name="ratingform" id="ratingform" enctype="multipart/form-data">
                                        <input type="hidden" id="ProductVoteID" name="ProductVoteID" value="<%=item.ID %>" />
                                        <input type="hidden" id="VoteType" name="VoteType" value="Product" />
                                        <input type="hidden" id="StarVote" name="StarVote" value="0" />
                                        <div class="box-cmt-popup">
                                            <div class="info-pro">
                                                <div class="img-cmt">
                                                    <img class="lazyload" src="<%=Utils.GetUrlFile(item.File) %>" alt="<%=item.Name %>">
                                                </div>
                                                <div class="text-pro">
                                                    <h3><%=item.Name %></h3>
                                                </div>
                                            </div>
                                            <div class="select-star">
                                                <p class="txt01">Bạn cảm thấy sản phẩm này như thế nào? (chọn sao nhé):</p>
                                                <ul class="ul-star">
                                                    <li data-val="1">
                                                        <i class="fa fa-star iconratingnew-star--big"></i>
                                                        <p>Rất tệ</p>
                                                    </li>
                                                    <li data-val="2">
                                                        <i class="fa fa-star iconratingnew-star--big"></i>
                                                        <p>Tệ</p>
                                                    </li>
                                                    <li data-val="3">
                                                        <i class="fa fa-star iconratingnew-star--big"></i>
                                                        <p>Bình thường</p>
                                                    </li>
                                                    <li data-val="4">
                                                        <i class="fa fa-star iconratingnew-star--big"></i>
                                                        <p>Tốt</p>
                                                    </li>
                                                    <li data-val="5">
                                                        <i class="fa fa-star iconratingnew-star--big"></i>
                                                        <p>Rất tốt</p>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="read-assess-form">
                                            <div class="textarea">
                                                <textarea class="ct" name="VoteContent" id="VoteContent" placeholder="Mời bạn chia sẻ thêm một số cảm nhận ..."></textarea>
                                                <div class="textarea-bottom clearfix">
                                                    <a href="javascript:void(0)" class="send-img" onclick="$('#FileVote').click();">
                                                        <i class="fa fa-camera"></i>
                                                        Gửi hình chụp thực tế
                                                    </a>
                                                    <input type="file" name="FileVote" id="FileVote" class="hide" onchange="loadFile(this, '.resRtImg')" multiple accept="image/x-png, image/gif, image/jpeg">
                                                    <ul class="resRtImg hide"></ul>
                                                </div>
                                            </div>
                                            <div class="list-input box-input-comment">
                                                <input type="text" name="VoteOrderCode" id="VoteOrderCode" class="input-assess" placeholder="Mã đơn hàng đã mua" style="margin: 2px 0px;" value="">
                                            </div>
                                            <div class="list-input box-input-comment">
                                                <input type="text" name="VoteName" id="VoteName" class="input-assess" placeholder="Họ và tên (bắt buộc)" value="">
                                                <input type="text" name="VotePhone" id="VotePhone" class="input-assess" placeholder="Số điện thoại (bắt buộc)" value="">
                                                <input type="text" name="VoteEmail" id="VoteEmail" class="input-assess" placeholder="Email (không bắt buộc)" value="">
                                            </div>
                                            <a class="submit-assess" href="javascript:submitCommentRating()" style="margin-bottom: 10px;">Gửi đánh giá ngay</a>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <hr class="mobile-line" />
            <div id="dok-to-hoi-dap">
                <div id="block-comment-cps" class="dok-comments dok-box-s comment-container">
                    <div class="d-flex align-items-center flex-wrap mb-3">
                        <p class="dok-box-s-title mb-0">
                            Hỏi đáp về sản phẩm                                   
                        </p>
                        <a href="javascript:void(0);" class="btn btn-secondary btn-sub2 ms-auto"
                            data-bs-toggle="modal" data-bs-target="#dok-hoi-dap">Viết câu hỏi
                        </a>
                    </div>

                    <div class="modal fade" id="dok-hoi-dap" tabindex="-1" aria-labelledby=""
                        aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title fz-18 fw-bold text-center" id="">Đặt câu hỏi cho OBIBI</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                                        aria-label="Close">
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <form name="comment_form" id="comment_form" method="post">
                                        <div class="mb-3">
                                            <textarea class="form-control" rows="4" cols="50"
                                                placeholder="Xin mời bạn để lại câu hỏi, OBIBI.VN sẽ giải đáp nhanh..."
                                                name="CommentContent0" id="CommentContent0"></textarea>
                                        </div>
                                        <div class="row box-input-comment">
                                            <div class="col-md-6 mb-3">
                                                <input type="text" class="form-control"
                                                    placeholder="Họ tên (Bắt buộc)" name="CommentName0" id="CommentName0">
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <input type="text" class="form-control"
                                                    placeholder="Số điện thoại" name="CommentPhone0" id="CommentPhone0">
                                            </div>
                                        </div>
                                    </form>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-bs-dismiss="modal">Huỷ</button>
                                    <button type="button" onclick="AddComment(<%=item.ID %>, 0, 0, 'Product'); return false;" class="btn btn-secondary btn-sub2">
                                        Gửi câu hỏi</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal fade" id="dok-tra-loi" tabindex="-1" aria-labelledby=""
                        aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title fz-18 fw-bold text-center" id="">Trả lời câu hỏi</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal"
                                        aria-label="Close">
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <form name="reply_form" id="reply_form" method="post">
                                        <input type="hidden" name="ReplyIndex" id="ReplyIndex" value="0" />
                                        <div class="mb-3">
                                            <textarea class="form-control" rows="4" cols="50"
                                                placeholder="Nhập nội dung trả lời"
                                                name="ReplyContent" id="ReplyContent"></textarea>
                                        </div>
                                        <div class="row box-input-comment">
                                            <div class="col-md-6 mb-3">
                                                <input type="text" class="form-control"
                                                    placeholder="Họ tên (Bắt buộc)" name="ReplyName" id="ReplyName">
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <input type="text" class="form-control"
                                                    placeholder="Số điện thoại" name="ReplyPhone" id="ReplyPhone">
                                            </div>
                                        </div>
                                    </form>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-bs-dismiss="modal">Huỷ</button>
                                    <button type="button" onclick="AddCommentReply(<%=item.ID %>, 'Product'); return false;" class="btn btn-secondary btn-sub2">Gửi trả lời</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="box-border">
                        <div class="comment comment--all ratingLst">

                            <%for (int i = 0; listCommnet != null && i < listCommnet.Count; i++)
                                {
                                    var sunComment = listvote.Where(o => o.ParentID == listCommnet[i].ID).OrderByDescending(o => o.Created).ToList();
                            %>
                            <div class="comment__item par">
                                <div class="item-top">
                                    <p class="txtname"><%=listCommnet[i].Name %></p>
                                </div>
                                <div class="comment-content">
                                    <div class="cmt-txt">
                                        <%=listCommnet[i].Content %>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center mt-4">
                                    <div class="text-secondary">
                                        <%= string.Format("{0:dd-MM-yyyy HH:mm}", listCommnet[i].Created) %>
                                    </div>
                                    <button type="button" class="btn-rep-cmt" onclick="showReplyForm(<%=listCommnet[i].ID %>);">
                                        <div><i class="fas fa-messages"></i></div>
                                        &nbsp;Trả lời
                                    </button>
                                </div>
                                <%if (sunComment != null && sunComment.Count > 0)
                                    { %>
                                <ul class="listrep-at">
                                    <%for (int z = 0; sunComment != null && z < sunComment.Count; z++)
                                        {
                                    %>
                                    <li>
                                        <div class="comava-at qtv-at"><%=sunComment[z].Name.Substring(0,1) %></div>
                                        <div class="combody-at">
                                            <strong><%=sunComment[z].Name %> </strong>
                                            <p><%=sunComment[z].Content %> </p>
                                        </div>
                                    </li>
                                    <%} %>
                                </ul>
                                <%} %>
                            </div>
                            <%} %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4 box-product-desktop d-lg-block d-none">
            <div class="dok-widget-list">
                <div class="dok-widget dok-widget-thong-so">
                    <h2 class="fw-bold fz-18 mb-3">Thông số kỹ thuật</h2>
                    <%=item.Specifications %>
                    <a class="readmore-btn mt-4 mb-0 down" href="javascript:void(0);" data-bs-toggle="modal"
                        data-bs-target="#dok-product-modal-preview" data-tab="technical">
                        <span>Xem cấu hình chi tiết</span>
                    </a>
                </div>
            </div>
        </div>
    </div>
</div>
<%if (listItem != null && listItem.Count > 0)
    { %>
<section class="dok-products mt-4">
    <div class="container">
        <div class="dok-heading">
            <h2>Sản phẩm tương tự</h2>
        </div>
        <div class="dok-products-slider dok-slider-box swiper">
            <div class="swiper-wrapper">
                <%for (int i = 0; listItem != null && i < listItem.Count; i++)
                    {
                        string url = ViewPage.GetURL(listItem[i].MenuID, listItem[i].Code);
                        var countvote = ModVoteService.Instance.CreateQuery().Where(o => o.ProductID == listItem[i].ID && o.Type == "Product" && o.Activity == true).Count().ToValue_Cache().ToInt();
                        var countBuyItem = VSW.Core.Global.DAO<GetSumModel>.Populate(ModOrderDetailService.Instance.ExecuteReader("select sum([Quantity]) from " + ModOrderDetailService.Instance.TableName + " where [ProductID] = " + listItem[i].ID));
                        var hasPropertyItem = ModProductClassifyService.Instance.CreateQuery().Where(o => o.ProductID == listItem[i].ID).Count().ToValue_Cache().ToBool();
                %>
                <div class="swiper-slide">
                    <div class="product">
                        <div class="product-info-container">
                            <div class="product-info">
                                <a href="<%=url %>" title="<%=listItem[i].Name %>" class="product__link button__link" rel="">
                                    <div class="product__image">
                                        <img src="<%=Utils.GetWebPFile(listItem[i].File, 4, 358, 358) %>"
                                            width="358" height="358"
                                            alt="<%=listItem[i].Name %>"
                                            class="product__img">
                                    </div>
                                    <div class="product__name">
                                        <h3 class="mb-0"><%=listItem[i].Name %></h3>
                                    </div>
                                    <%if (countBuyItem != null && countBuyItem.Count > 0)
                                        { %>
                                    <div class="block-smem-price"><%=countBuyItem[0].Total %> sản phẩm đã bán</div>
                                    <%} %>
                                    <div class="bottom-div">
                                        <div class="product__box-rating">
                                            <div>
                                                <%for (int z = 0; z < listItem[i].Star; z++)
                                                    {%>
                                                <div class="ic-star is-active">
                                                    <i class="fas fa-star-sharp"></i>
                                                </div>
                                                <%} %>
                                                <%for (int z = 0; z < 5 - listItem[i].Star; z++)
                                                    {%>
                                                <div class="ic-star">
                                                    <i class="fas fa-star-sharp"></i>
                                                </div>
                                                <%} %>
                                            </div>
                                        </div>
                                        <%if (countvote > 0)
                                            { %>
                                        <div class="label-rate text-primary">
                                            (<%=countvote %> <span>đánh giá </span>)                                               
                                        </div>
                                        <%} %>
                                    </div>
                                    <div class="block-box-price">
                                        <div class="box-info__box-price">
                                            <p class="product__price--show">
                                                <%if (listItem[i].PriceView > 0)
                                                    { %>
                                                <%=Utils.FormatMoney(listItem[i].PriceView) %>đ
                                                        <%}
                                                            else
                                                            { %>{RS:Lienhe}
                                                        <%} %>
                                            </p>
                                            <%if (listItem[i].PriceView2 > 0)
                                                { %>
                                            <p class="product__price--through">
                                                <%=Utils.FormatMoney(listItem[i].PriceView2) %>đ                                                   
                                            </p>
                                            <%if (listItem[i].SellOffPercent > 0)
                                                { %>
                                            <div class="product__price--percent">
                                                <p class="product__price--percent-detail">
                                                    Giảm&nbsp;<%=listItem[i].SellOffPercent %>%                                                       
                                                </p>
                                            </div>
                                            <%} %>
                                            <%} %>
                                        </div>
                                    </div>
                                </a>
                            </div>
                            <a class="block-add-to-cart addToCartAjax" data-id="<%=listItem[i].ID %>" data-hasproperty="<%=hasPropertyItem.ToString().ToLower() %>" href="javascript:void(0)">Thêm giỏ hàng
                            </a>
                            <div class="install-0-tag">
                                Trả góp 0%
                            </div>
                        </div>
                    </div>
                </div>
                <%} %>
            </div>

            <div class="swiper-button-next"></div>
            <div class="swiper-button-prev"></div>
        </div>
    </div>
</section>
<%} %>
<nav id="bottom-product" class="bottom-nav up stop">
    <div class="container">
        <ul>
            <li><a href="javascript:void(0)" data-bs-toggle="modal" data-bs-target="#dok-menu-mobile" class="dok-category-toggle"><i class="fa fa-list-alt"></i><span>Danh mục</span></a></li>
            <li><a href="<%=ViewPage.FeedbackUrl %>"><i class="fa fa-phone-rotary"></i><span>Tư vấn</span></a></li>
            <li><a href="<%=ViewPage.ViewCartUrl %>" class="bottom-nav-button bottom-nav-cart"><span>Giỏ hàng</span></a></li>
            <li><a href="javascript:void(0);" class="bottom-nav-button bottom-nav-buy addcartnow" data-id="<%=item.ID %>" data-hasproperty="<%=hasProperty.ToString().ToLower() %>"><span>Mua ngay</span></a></li>
        </ul>
    </div>
</nav>
<div class="locationbox__overlay"></div>

<script type="text/javascript">
    var isDetailProduct = 1;
</script>
