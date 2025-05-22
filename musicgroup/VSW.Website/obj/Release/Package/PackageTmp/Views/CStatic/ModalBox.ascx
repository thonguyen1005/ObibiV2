<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<% 
    var lstCategory = SysPageService.Instance.GetByParent_Cache(1240, 16);
    var lstBrand = SysPageService.Instance.GetByParent_Cache(21, 0);
    var lstNews = SysPageService.Instance.GetByParent_Cache(1513, 0);
%>
<div id="modal-box">
    <div id="dok-menu-mobile" class="modal fade" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-fullscreen modal-dialog-scrollable box-product-mobile">
            <div class="modal-content">
                <div class="modal-body p-0">
                    <div>
                        <div class="dok-menu-mobile-header">
                            <div class="dok-menu-mobile-header-wrapper">
                                <div class="dok-header-search">
                                    <form method="post" id="formSearchMenu" name="formSearchMenu" onsubmit="doSearch('<%=ViewPage.SearchUrl %>', $('#keywordMobile')); return false;">
                                        <input type="search" name="keywordMobile" id="keywordMobile"
                                            placeholder="Bạn cần tìm gì hôm nay ?" autocomplete="off">
                                        <button type="button" onclick="doSearch('<%=ViewPage.SearchUrl %>', $('#keywordMobile')); return false;"><i class="far fa-magnifying-glass"></i></button>
                                    </form>
                                </div>
                                <a class="dok-menu-mobile-close text-dark d-flex align-items-center fz-14"
                                    href="javascript:void(0)" data-bs-dismiss="modal">
                                    <i class="fz-28 fas fa-rectangle-xmark me-2"></i>
                                    <span>Đóng</span>
                                </a>
                            </div>
                        </div>

                        <div id="dok-menu-mobile-filter">
                            <div class="dok-menu-mobile-filter-box">
                                <h3 class="dok-menu-mobile-title ">Danh mục sản phẩm</h3>
                                <div class="dok-menu-mobile-category dok-slider-box">
                                    <div class="swiper-wrapper">
                                        <%for (int i = 0; lstCategory != null && i < lstCategory.Count; i += 3)
                                            {
                                        %>
                                        <div class="swiper-slide">
                                            <a href="<%=ViewPage.GetPageURL(lstCategory[i]) %>" class="label-menu-item">
                                                <%if (!string.IsNullOrEmpty(lstCategory[i].Icon))
                                                    { %>
                                                <%if (lstCategory[i].Icon.Contains("/"))
                                                    { %>
                                                <img width="25" height="25" class="lazyload" data-src="<%=Utils.GetWebPFile(lstCategory[i].Icon, 4, 25, 25) %>" alt="<%=lstCategory[i].Name %>" />
                                                <%}
                                                    else
                                                    { %>
                                                <i class="ic-cate <%=lstCategory[i].Icon %>"></i>
                                                <%} %>
                                                <%} %>
                                                <span><%=lstCategory[i].Name %></span>
                                            </a>
                                            <%if (lstCategory.Count > i + 1)
                                                { %>
                                            <a href="<%=ViewPage.GetPageURL(lstCategory[i+1]) %>" class="label-menu-item">
                                                <%if (!string.IsNullOrEmpty(lstCategory[i + 1].Icon))
                                                    { %>
                                                <%if (lstCategory[i + 1].Icon.Contains("/"))
                                                    { %>
                                                <img width="25" height="25" class="lazyload" data-src="<%=Utils.GetWebPFile(lstCategory[i+1].Icon, 4, 25, 25) %>" alt="<%=lstCategory[i+1].Name %>" />
                                                <%}
                                                    else
                                                    { %>
                                                <i class="ic-cate <%=lstCategory[i+1].Icon %>"></i>
                                                <%} %>
                                                <%} %>
                                                <span><%=lstCategory[i+1].Name %></span>
                                            </a>
                                            <%} %>
                                            <%if (lstCategory.Count > i + 2)
                                                { %>
                                            <a href="<%=ViewPage.GetPageURL(lstCategory[i+2]) %>" class="label-menu-item">
                                                <%if (!string.IsNullOrEmpty(lstCategory[i + 2].Icon))
                                                    { %>
                                                <%if (lstCategory[i + 2].Icon.Contains("/"))
                                                    { %>
                                                <img width="25" height="25" class="lazyload" src="<%=Utils.GetWebPFile(lstCategory[i+2].Icon, 4, 25, 25) %>" alt="<%=lstCategory[i+2].Name %>" />
                                                <%}
                                                    else
                                                    { %>
                                                <i class="ic-cate <%=lstCategory[i+2].Icon %>"></i>
                                                <%} %>
                                                <%} %>
                                                <span><%=lstCategory[i+2].Name %></span>
                                            </a>
                                            <%} %>
                                        </div>
                                        <%} %>
                                    </div>
                                    <div class="swiper-pagination position-static"></div>
                                </div>
                            </div>
                            <%if (lstBrand != null && lstBrand.Count > 0)
                                {
                            %>
                            <div class="dok-menu-mobile-filter-box">
                                <h3 class="dok-menu-mobile-title">Thương hiệu</h3>
                                <div class="dok-brands">
                                    <div class="dok-brand-list swiper">
                                        <div class="swiper-wrapper">
                                            <%for (int i = 0; lstBrand != null && i < lstBrand.Count; i += 2)
                                                {
                                            %>
                                            <div class="swiper-slide">
                                                <div class="category-wrap-item">
                                                    <a class="category-item" href="<%=ViewPage.GetPageURL(lstBrand[i]) %>" title="<%=lstBrand[i].Name %>">
                                                        <img src="<%=Utils.GetWebPFile(lstBrand[i].File, 4, 245, 50) %>"
                                                            height="50" alt="<%=lstBrand[i].Name %>" loading="lazy" class="filter-brand__img">
                                                    </a>
                                                    <%if (lstBrand.Count > i + 1)
                                                        { %>
                                                    <a class="category-item" href="<%=ViewPage.GetPageURL(lstBrand[i+1]) %>" title="<%=lstBrand[i+1].Name %>">
                                                        <img src="<%=Utils.GetWebPFile(lstBrand[i+1].File, 4, 245, 50) %>"
                                                            height="50" alt="<%=lstBrand[i+1].Name %>" loading="lazy" class="filter-brand__img">
                                                    </a>
                                                    <%} %>
                                                </div>
                                            </div>
                                            <%} %>
                                        </div>
                                        <div class="swiper-pagination position-static"></div>
                                    </div>
                                </div>
                            </div>
                            <%} %>
                            <div class="dok-menu-mobile-filter-box">
                                <h3 class="dok-menu-mobile-title">Tin tức & liên hệ</h3>
                                <div id="dok-menu-mobile-category"
                                    class="dok-menu-mobile-category dok-slider-box">
                                    <div class="swiper-wrapper">
                                        <%for (int i = 0; lstNews != null && i < lstNews.Count; i++)
                                            {
                                        %>
                                        <div class="swiper-slide">
                                            <a href="<%=ViewPage.GetPageURL(lstNews[i]) %>"><%=lstNews[i].Name %></a>
                                        </div>
                                        <%} %>
                                    </div>
                                    <div class="swiper-pagination position-static"></div>
                                </div>
                            </div>

                            <div class="mt-3">
                                {RS:HotlineMobile}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!-- Modal mua ngay destop-->
    <div id="dok-buy-desktop" class="modal fade dok-modal-bottom-up" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-scrollable box-product-mobile">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="fz-18 fw-bold text-center w-100 mb-0">Xác nhận lựa chọn của bạn</h4>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-header" id="boxHeaderProduct">
                </div>
                <div class="modal-body" id="boxBodyProduct">
                </div>
                <div class="modal-footer ">
                    <div class="modal-buy-footer m-0 w-100">
                        <div class="d-flex gap-4">
                            <div class="d-flex align-items-center">
                                <span class="me-3 fz-12 text-nowrap">Số lượng:</span>
                                <div class="counter-container">
                                    <button type="button" class="decrease">
                                        <i class="far fa-minus fz-14"></i>
                                    </button>
                                    <input type="hidden" id="productIDFromProduct" value="" />
                                    <input type="number" id="counterFromProduct" class="counter-input" value="1" readonly>
                                    <button type="button" class="increase">
                                        <i class="far fa-plus fz-14"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="ms-auto w-75">
                                <button type="button" class="btn btn-danger text-white justify-content-center w-100 btnAddToNow">
                                    Mua ngay
                                </button>
                                <button type="button" class="btn btn-danger text-white justify-content-center w-100 btnAddToCart">
                                    Thêm giỏ hàng
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
