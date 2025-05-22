<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<script runat="server">
    public string SortMode
    {
        get
        {
            string sort = VSW.Core.Web.HttpQueryString.GetValue("sort").ToString().ToLower().Trim();

            if (sort == "update" || sort == "new" || sort == "view" || sort == "price_asc" || sort == "price_desc")
                return sort;
            return "update";
        }
    }
    public string GetURL(string sKey, string sValue)
    {
        string strUrl = string.Empty;
        string strKey = string.Empty;
        string strValue = string.Empty;
        for (int i = 0; i < ViewPage.PageViewState.Count; i++)
        {
            strKey = ViewPage.PageViewState.AllKeys[i];
            strValue = ViewPage.PageViewState[strKey].ToString();
            if (strKey.ToLower() == sKey.ToLower() || strKey.ToLower() == "vsw" || strKey.ToLower() == "v" || strKey.ToLower() == "s" || strKey.ToLower() == "w" || strKey.ToLower().Contains("web."))
                continue;
            if (strUrl == string.Empty)
                strUrl = "?" + strKey + "=" + HttpContext.Current.Server.UrlEncode(strValue);
            else
                strUrl += "&" + strKey + "=" + HttpContext.Current.Server.UrlEncode(strValue);
        }
        strUrl += (strUrl == string.Empty ? "?" : "&") + sKey + "=" + sValue;

        return strUrl;
    }
    public string GetRemoveURL(string sKey, string sValue)
    {
        string strUrl = string.Empty;
        string strKey = string.Empty;
        string strValue = string.Empty;
        for (int i = 0; i < ViewPage.PageViewState.Count; i++)
        {
            strKey = ViewPage.PageViewState.AllKeys[i];
            strValue = ViewPage.PageViewState[strKey].ToString();
            if (strKey.ToLower() == sKey.ToLower() || strKey.ToLower() == "vsw" || strKey.ToLower() == "v" || strKey.ToLower() == "s" || strKey.ToLower() == "w" || strKey.ToLower().Contains("web."))
                continue;
            if (strUrl == string.Empty)
                strUrl = "?" + strKey + "=" + HttpContext.Current.Server.UrlEncode(strValue);
            else
                strUrl += "&" + strKey + "=" + HttpContext.Current.Server.UrlEncode(strValue);
        }

        return (string.IsNullOrEmpty(strUrl) ? ViewPage.GetPageURL(ViewPage.CurrentPage) : strUrl);
    }
</script>

<% 
    if (ViewPage.CurrentPage.ModuleCode != "MProduct") return;
    var page = ViewPage.CurrentPage;
    var listMenu = SysPageService.Instance.GetByParent_Cache(page.ID);
    if ((listMenu == null || listMenu.Count == 0) && page.ParentID > 1)
    {
        listMenu = SysPageService.Instance.GetByParent_Cache(page.ParentID);
        page = SysPageService.Instance.GetByID(page.ParentID);
    }
    var ListBrandOfProduct = ViewPage.ViewBag.ListBrand as List<ModProductEntity>;
    List<WebMenuEntity> listBrand = null;
    if (ListBrandOfProduct != null)
    {
        var listB = ListBrandOfProduct.Select(o => o.BrandID).ToList().ToArray();
        string joinB = VSW.Core.Global.Array.ToString(listB);
        listBrand = WebMenuService.Instance.CreateQuery()
                        .WhereIn(o => o.ID, joinB)
                        .ToList();
    }

    var listItem = ViewBag.Data as Dictionary<WebPropertyEntity, List<WebPropertyEntity>>;
    var model = ViewPage.ViewBag.Model as MProductModel;
%>


<input type="hidden" name="menuParentID" id="menuParentID" value="<%=page.MenuID %>" />
<input type="hidden" name="pageParentCode" id="pageParentCode" value="<%=page.Code %>" />
<%if (listBrand != null && listBrand.Count > 0)
    { %>
<div class="dok-brands mb-4">
    <div class="container">
        <div>
            <div class="dok-brand-list swiper">
                <div class="swiper-wrapper">
                    <%for (var i = 0; listBrand != null && i < listBrand.Count; i += 2)
                        {%>
                    <div class="swiper-slide">
                        <div class="category-wrap-item filterBrand">
                            <a class="category-item" href="javascript:addBrandReload(this, <%=listBrand[i].ID %>)" data-id="<%=listBrand[i].ID %>" title="<%=listBrand[i].Name %>">
                                <%if (!string.IsNullOrEmpty(listBrand[i].File))
                                    { %>
                                <img alt="<%=listBrand[i].Name %>" height="30" style="max-height:30px" class="filter-brand__img" src="<%=Utils.GetWebPFile(listBrand[i].File, 4, 245, 50) %>" />
                                <%}
                                    else
                                    { %>
                                <%=listBrand[i].Name %>
                                <%} %>
                            </a>

                            <%if (listBrand.Count > i + 1)
                                { %>
                            <a class="category-item" href="javascript:addBrandReload(this, <%=listBrand[i + 1].ID %>)" data-id="<%=listBrand[i + 1].ID %>" title="<%=listBrand[i + 1].Name %>">
                                <%if (!string.IsNullOrEmpty(listBrand[i + 1].File))
                                    { %>
                                <img alt="<%=listBrand[i + 1].Name %>" height="30" style="max-height:30px" class="filter-brand__img" src="<%=Utils.GetWebPFile(listBrand[i + 1].File, 4, 245, 50) %>" />
                                <%}
                                    else
                                    { %>
                                <%=listBrand[i + 1].Name %>
                                <%} %>
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
</div>
<%} %>
<div class="dok-product-filter desktop d-lg-block d-none">
    <div class="container">
        <div class="dok-filters">
            <div class="dok-filter-item dok-filter-box">
                <div class="dok-combination">
                    <a class="openpopup collapsed" data-bs-toggle="collapse" href="#dok_filter_total"
                        role="button" aria-expanded="false" aria-controls="dok_filter_total">
                        <i class=" far fa-bars-filter"></i>Bộ lọc <span class="filter-count"></span>
                    </a>
                    <div class="collapse custom-scrollbar dok_filter_total" id="dok_filter_total">
                        <button class="close-popup-total">
                            <i class="fa fa-xmark fz-16 me-2"></i>Đóng</button>
                        <div class="wrap custom-scrollbar">
                            <div class="show-total-main">
                                <%if (listBrand != null && listBrand.Count > 0)
                                    { %>
                                <div class="show-total-item warpper-manu-inside">
                                    <p class="show-total-txt">Hãng</p>
                                    <div class="filter-list filterBrand">
                                        <%for (var i = 0; listBrand != null && i < listBrand.Count; i++)
                                            {%>
                                        <a href="javascript:addBrand(this, <%=listBrand[i].ID %>)" data-id="<%=listBrand[i].ID %>" class="c-btnbox filter-manu" title="<%=listBrand[i].Name %>">
                                            <%if (!string.IsNullOrEmpty(listBrand[i].File))
                                                { %>
                                            <img alt="<%=listBrand[i].Name %>" width="68" height="30" class="img-fluid" src="<%=Utils.GetWebPFile(listBrand[i].File, 4, 68, 30) %>" />
                                            <%}
                                                else
                                                { %>
                                            <%=listBrand[i].Name %>
                                            <%} %>
                                        </a>
                                        <%} %>
                                    </div>
                                </div>
                                <div class="filter-border"></div>
                                <%} %>
                                <%if (listMenu != null && listMenu.Count > 0)
                                    { %>
                                <div class="show-total-item clearfix warpper-manu-inside arranged">
                                    <p class="show-total-txt">Phân loại</p>
                                    <div class="filter-list filterMenu">
                                        <%for (var i = 0; listMenu != null && i < listMenu.Count; i++)
                                            {
                                        %>
                                        <a href="javascript:addMenu(this, <%=listMenu[i].MenuID %>, '<%=listMenu[i].Code %>')" data-id="<%=listMenu[i].MenuID %>" class="c-btnbox <%=ViewPage.IsPageActived(listMenu[i]) ? "active" : "" %> filter-manu" title="<%=listMenu[i].Name %>">
                                            <%if (!string.IsNullOrEmpty(listMenu[i].File))
                                                { %>
                                            <img alt="<%=listMenu[i].Name %>" width="68" height="30" class="img-fluid" src="<%=Utils.GetWebPFile(listMenu[i].File, 4, 68, 30) %>" />
                                            <%}
                                                else
                                                { %>
                                            <%=!string.IsNullOrEmpty(listMenu[i].AliasName) ? listMenu[i].AliasName : listMenu[i].Name %>
                                            <%} %>
                                        </a>
                                        <%} %>
                                    </div>
                                </div>
                                <%} %>
                                <%
                                    if (listItem != null)
                                    {
                                        int zz = 0;
                                        foreach (var item in listItem.Keys)
                                        {
                                            var listChildItem = listItem[item];
                                            if (listChildItem == null) continue;
                                            if (listChildItem.Count < 1) continue;
                                            zz++;
                                %>
                                <%if (zz == 3)
                                    { %>
                                <div class="filter-border"></div>
                                <%} %>
                                <div class="show-total-item count-item">
                                    <p class="show-total-txt"><%=item.Name %></p>
                                    <div class="filter-list filterAtr filterProperty filter<%=item.Code %>">
                                        <%for (var i = 0; listChildItem != null && i < listChildItem.Count; i++)
                                            {%>
                                        <a href="javascript:addAtr(this, <%=listChildItem[i].ID %>, '<%=item.Code %>')" data-id="<%=listChildItem[i].ID %>" title="<%=listChildItem[i].Name %>" class="c-btnbox">
                                            <%if (!string.IsNullOrEmpty(listChildItem[i].File))
                                                { %>
                                            <img alt="<%=listChildItem[i].Name %>" width="68" height="30" class="img-fluid" src="<%=Utils.GetWebPFile(listChildItem[i].File, 4, 68, 30) %>" />
                                            <%}
                                                else
                                                { %>
                                            <%=listChildItem[i].Name %>
                                            <%} %>
                                        </a>
                                        <%} %>
                                    </div>
                                </div>
                                <%if (zz == 3) zz = 0;%>
                                <%}
                                    }%>
                            </div>
                        </div>
                        <div class="filter-button filter-button--total" style="display: block;">
                            <span onclick="removeAll();" class="btn-filter-close" title="Bỏ chọn">Bỏ chọn</span>
                            <a href="javascript:;" class="btn-filter-readmore">Xem <b class="total-reloading"></b>kết quả</a>
                        </div>
                    </div>
                </div>
            </div>

            <% if (listBrand != null && listBrand.Count > 0)
                {
            %>
            <div class="dok-filter-item">
                <div class="dropdown filter-box">
                    <a class="dropdown-toggle filter-box-txt" href="javascript:;" data-bs-toggle="dropdown" data-bs-auto-close="false">Hãng</a>

                    <div class="dropdown-menu">
                        <div class="dropdown-content filterBrand">
                            <%for (var i = 0; listBrand != null && i < listBrand.Count; i++)
                                {%>
                            <a href="javascript:addBrand(this, <%=listBrand[i].ID %>)" data-id="<%=listBrand[i].ID %>" data-name="<%=listBrand[i].Name %>" class="c-btnbox filter-manu" title="<%=listBrand[i].Name %>">
                                <%if (!string.IsNullOrEmpty(listBrand[i].File))
                                    { %>
                                <img alt="<%=listBrand[i].Name %>" width="68" height="30" class="img-fluid" src="<%=Utils.GetWebPFile(listBrand[i].File, 4, 68, 30) %>" />
                                <%}
                                    else
                                    { %>
                                <%=listBrand[i].Name %>
                                <%} %>
                            </a>
                            <%} %>
                        </div>
                        <div class="filter-button filter-button--total" style="display: block;">
                            <a href="javascript:removeAllBrand();" class="btn-filter-close" title="Bỏ chọn">Bỏ chọn</a>
                            <a href="javascript:;" class="btn-filter-readmore">Xem <b class="total-reloading"></b>kết quả</a>
                        </div>
                    </div>
                </div>
            </div>
            <%} %>
            <% if (listMenu != null && listMenu.Count > 0)
                {
                    var check = listMenu.Where(o => o.ID == ViewPage.CurrentPage.ID).FirstOrDefault();
            %>
            <div class="dok-filter-item">
                <div class="dropdown filter-menu-out">
                    <a class="dropdown-toggle <%=check != null ? "active" : "" %>" href="javascript:;" data-bs-toggle="dropdown" data-bs-auto-close="false"><%=check != null ? check.Name : "Phân loại" %></a>
                    <div class="dropdown-menu" style="">
                        <div class="dropdown-content filterMenu">
                            <%for (var i = 0; listMenu != null && i < listMenu.Count; i++)
                                {
                            %>
                            <a href="javascript:addMenu(this, <%=listMenu[i].MenuID %>, '<%=listMenu[i].Code %>')" data-id="<%=listMenu[i].MenuID %>" class="c-btnbox <%=ViewPage.IsPageActived(listMenu[i]) ? "active" : "" %> filter-manu" title="<%=listMenu[i].Name %>">
                                <%if (!string.IsNullOrEmpty(listMenu[i].File))
                                    { %>
                                <img alt="<%=listMenu[i].Name %>" width="68" height="30" class="img-fluid" src="<%=Utils.GetWebPFile(listMenu[i].File, 4, 68, 30) %>" />
                                <%}
                                    else
                                    { %>
                                <%=!string.IsNullOrEmpty(listMenu[i].AliasName) ? listMenu[i].AliasName : listMenu[i].Name %>
                                <%} %>
                            </a>
                            <%} %>
                        </div>
                        <div class="filter-button filter-button--total" style="display: block;">
                            <a href="javascript:removeAllMenu(<%=ViewPage.CurrentPage.MenuID %>, '<%=ViewPage.CurrentPage.Code %>');" class="btn-filter-close" title="Bỏ chọn">Bỏ chọn</a>
                            <a href="javascript:;" class="btn-filter-readmore">Xem <b class="total-reloading"></b>kết quả</a>
                        </div>
                    </div>
                </div>
            </div>
            <%} %>
            <%
                if (listItem != null)
                {
                    int z = 0;
                    foreach (var item in listItem.Keys)
                    {
                        z++;
                        var listChildItem = listItem[item];
                        if (listChildItem == null) continue;
                        if (listChildItem.Count < 1) continue;
            %>
            <div class="dok-filter-item">
                <div class="dropdown filter-box">
                    <a class="dropdown-toggle filter-box-txt" href="javascript:;" data-bs-toggle="dropdown" data-bs-auto-close="false" data-name="<%=item.Name %>"><%=item.Name %></a>
                    <div class="dropdown-menu" style="">
                        <div class="dropdown-content filterAtr filterProperty filter<%=item.Code %>">
                            <%for (var i = 0; listChildItem != null && i < listChildItem.Count; i++)
                                {%>
                            <a href="javascript:addAtr(this, <%=listChildItem[i].ID %>, '<%=item.Code %>')" data-id="<%=listChildItem[i].ID %>" data-name="<%=listChildItem[i].Name %>" title="<%=listChildItem[i].Name %>" class="c-btnbox">
                                <%if (!string.IsNullOrEmpty(listChildItem[i].File))
                                    { %>
                                <img alt="<%=listChildItem[i].Name %>" width="68" height="30" class="img-fluid" src="<%=Utils.GetWebPFile(listChildItem[i].File, 4, 68, 30) %>" />
                                <%}
                                    else
                                    { %>
                                <%=listChildItem[i].Name %>
                                <%} %>
                            </a>
                            <%} %>
                        </div>
                        <div class="filter-button filter-button--total" style="display: block;">
                            <a href="javascript:removeAllAtr('<%=item.Code %>');" class="btn-filter-close" title="Bỏ chọn">Bỏ chọn</a>
                            <a href="javascript:;" class="btn-filter-readmore">Xem <b class="total-reloading"></b>kết quả</a>
                        </div>
                    </div>
                </div>
            </div>
            <%}
                } %>
        </div>

        <div class="dok-sort-cate">
            <div class="dok-sort-cate-right">
                <h1 class="sort-total pageProductName"><b class="countProduct"></b>&nbsp;&nbsp;<%=ViewPage.CurrentPage.Name %></h1>
                <%-- <div class="box-checkbox extend">
                    <a href="" class="c-checkitem" rel="nofollow">
                        <span class="tick-checkbox"></span>
                        <p>Giảm giá</p>
                    </a>
                    <a href="" class="c-checkitem" rel="nofollow">
                        <span class="tick-checkbox"></span>
                        <p>Trả Góp 0%</p>
                    </a>
                    <a href="" class="c-checkitem active" rel="nofollow">
                        <span class="tick-checkbox"></span>
                        <p>Mới 2023</p>
                    </a>
                </div>--%>
            </div>
            <div class="dok-sort-cate-left">
                <div class="dok-dropdown dropdown">
                    <button class="dok-dropdown__cta" data-bs-toggle="dropdown" data-bs-auto-close="false">
                        <span>Xếp theo: 
                            <%if (SortMode == "view")
                                { %>
                            Quan tâm nhất
                            <%} %>
                            <%else if (SortMode == "price_asc")
                                { %>
                            Giá thấp đến cao
                            <%} %>
                            <%else if (SortMode == "price_desc")
                                { %>
                            Giá cao xuống thấp
                            <%} %>
                            <%else if (SortMode == "buy")
                                { %>
                            Bán chạy nhất
                            <%} %>
                        </span>
                    </button>
                    <div class="dok-dropdown__content dropdown-menu dropdown-menu-end">
                        <div class="dok-dropdown__item position-desc">
                            <a class="sort-link <%=(SortMode == "view") ? "active" : "" %>" href="<%= ViewPage.GetURL("sort", "view") %>">Quan tâm nhất</a>
                        </div>
                        <div class="dok-dropdown__item price-asc">
                            <a class="sort-link <%=(SortMode == "price_asc") ? "active" : "" %>" href="<%= ViewPage.GetURL("sort", "price_asc") %>">Giá thấp đến cao</a>
                        </div>
                        <div class="dok-dropdown__item price-desc">
                            <a class="sort-link <%=(SortMode == "price_desc") ? "active" : "" %>" href="<%= ViewPage.GetURL("sort", "price_desc") %>">Giá cao xuống thấp</a>
                        </div>
                        <div class="dok-dropdown__item popularity-desc">
                            <a class="sort-link <%=(SortMode == "buy") ? "active" : "" %>" href="<%= ViewPage.GetURL("sort", "buy") %>">Bán chạy nhất</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


<div class="dok-product-filter mobile d-lg-none d-block">
    <div class="mb-filter" data-sticky data-sticky-class="filter-sticky">
        <div class="container">
            <div class="dok-mobile-filters">
                <div class="d-flex align-items-center">
                    <div class="box-checkbox extend">
                        <%--<a href="" class="c-checkitem" rel="nofollow">
                            <span class="tick-checkbox"></span>
                            <p>Giảm giá</p>
                        </a>
                        <a href="" class="c-checkitem" rel="nofollow">
                            <span class="tick-checkbox"></span>
                            <p>Trả Góp 0%</p>
                        </a>
                        <a href="" class="c-checkitem active" rel="nofollow">
                            <span class="tick-checkbox"></span>
                            <p>Mới 2023</p>
                        </a>--%>
                    </div>
                    <div class="ms-auto">
                        <div class="dok-combination">
                            <a class="open-filters text-nowrap text-dark p-3 fw-medium border rounded-3"
                                data-bs-toggle="modal" data-bs-target="#dok-category-mobile-filters">
                                <i class=" far fa-bars-filter"></i>Bộ lọc
                             </a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="position-relative">
                <div class="dok-filters">
                    <% if (listBrand != null && listBrand.Count > 0)
                        {
                    %>
                    <div class="filter-item block-manu lst-brand-mobile">
                        <div class="filter-item__title jsTitle">
                            <div class="arrow-filter"></div>
                            <span>Hãng</span>
                        </div>
                        <div class="filter-show">
                            <div class="filter-list filter-list--hang manu filterBrand" data-rows="3">
                                <%for (var i = 0; listBrand != null && i < listBrand.Count; i++)
                                    {%>
                                <a href="javascript:addBrand(this, <%=listBrand[i].ID %>)" data-id="<%=listBrand[i].ID %>" class="c-btnbox filter-manu" title="<%=listBrand[i].Name %>">
                                    <%if (!string.IsNullOrEmpty(listBrand[i].File))
                                        { %>
                                    <img alt="<%=listBrand[i].Name %>" width="68" height="30" class="lazyload" src="<%=Utils.GetWebPFile(listBrand[i].File, 4, 68, 30) %>" />
                                    <%}
                                        else
                                        { %>
                                    <%=listBrand[i].Name %>
                                    <%} %>
                                </a>
                                <%} %>
                            </div>

                            <button class="btn-filter-show-more show-more" type="button">Xem thêm</button>
                            <div class="filter-button filter-button--total" style="display: block;">
                                <a href="javascript:removeAllBrand();" class="btn-filter-close" title="Bỏ chọn">Bỏ chọn</a>
                                <a href="javascript:;" class="btn-filter-readmore">Xem <b class="total-reloading"></b>kết quả</a>
                            </div>
                        </div>
                    </div>
                    <%} %>
                    <% if (listMenu != null && listMenu.Count > 0)
                        {
                    %>
                    <div class="filter-item block-manu lst-menu-mobile">
                        <div class="filter-item__title jsTitle">
                            <div class="arrow-filter"></div>
                            <span>Phân loại</span>
                        </div>
                        <div class="filter-show">
                            <div class="filter-list filter-list--hang manu menu filterMenu">
                                <%for (var i = 0; listMenu != null && i < listMenu.Count; i++)
                                    {
                                %>
                                <a href="javascript:addMenu(this, <%=listMenu[i].MenuID %>, '<%=listMenu[i].Code %>')" data-id="<%=listMenu[i].MenuID %>" class="c-btnbox <%=ViewPage.IsPageActived(listMenu[i]) ? "active" : "" %> filter-manu" title="<%=listMenu[i].Name %>">
                                    <%if (!string.IsNullOrEmpty(listMenu[i].File))
                                        { %>
                                    <img alt="<%=listMenu[i].Name %>" width="68" height="30" class="lazyload" src="<%=Utils.GetWebPFile(listMenu[i].File, 4, 68, 30) %>" />
                                    <%}
                                        else
                                        { %>
                                    <%=!string.IsNullOrEmpty(listMenu[i].AliasName) ? listMenu[i].AliasName : listMenu[i].Name %>
                                    <%} %>
                                </a>
                                <%} %>
                            </div>
                            <div class="filter-button filter-button--total" style="display: block;">
                                <a href="javascript:removeAllMenu(<%=ViewPage.CurrentPage.MenuID %>, '<%=ViewPage.CurrentPage.Code %>');" class="btn-filter-close" title="Bỏ chọn">Bỏ chọn</a>
                                <a href="javascript:;" class="btn-filter-readmore">Xem <b class="total-reloading"></b>kết quả</a>
                            </div>
                        </div>
                    </div>
                    <%} %>
                    <%
                        if (listItem != null)
                        {
                            int zzz = 0;
                            foreach (var item in listItem.Keys)
                            {
                                zzz++;
                                var listChildItem = listItem[item];
                                if (listChildItem == null) continue;
                                if (listChildItem.Count < 1) continue;
                    %>
                    <div class="filter-item ">
                        <div class="filter-item__title jsTitle lst-<%=item.Code %>-mobile">
                            <div class="arrow-filter"></div>
                            <span><%=item.Name %></span>
                        </div>
                        <div class="filter-show <%=zzz > 4 && zzz < 11 ? "filter-show--right" : "" %>">
                            <div class="filter-list props filterProperty filter<%=item.Code %>">
                                <%for (var i = 0; listChildItem != null && i < listChildItem.Count; i++)
                                    {%>
                                <a href="javascript:addAtr(this, <%=listChildItem[i].ID %>, '<%=item.Code %>')" data-id="<%=listChildItem[i].ID %>" title="<%=listChildItem[i].Name %>" class="c-btnbox">
                                    <%if (!string.IsNullOrEmpty(listChildItem[i].File))
                                        { %>
                                    <img alt="<%=listChildItem[i].Name %>" width="68" height="30" class="lazyload" src="<%=Utils.GetWebPFile(listChildItem[i].File, 4, 68, 30) %>" />
                                    <%}
                                        else
                                        { %>
                                    <%=listChildItem[i].Name %>
                                    <%} %>
                                </a>
                                <%} %>
                            </div>
                            <div class="filter-button filter-button--total" style="display: block;">
                                <a href="javascript:removeAllAtr('<%=item.Code %>');" class="btn-filter-close" title="Bỏ chọn">Bỏ chọn</a>
                                <a href="javascript:;" class="btn-filter-readmore">Xem <b class="total-reloading"></b>kết quả</a>
                            </div>
                        </div>
                    </div>
                    <%}
                        } %>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="dok-sort-cate">
            <div class="dok-sort-cate-right">
                <h2 class="sort-total pageProductName"><b class="countProduct"></b>&nbsp;&nbsp;<%=ViewPage.CurrentPage.Name %></h2>
            </div>
            <div class="dok-sort-cate-left">
                <div class="dok-dropdown dropdown">
                    <button class="dok-dropdown__cta" data-bs-toggle="dropdown" data-bs-auto-close="false">
                        <span>Xếp theo: 
                            <%if (SortMode == "view")
                                { %>
                            Quan tâm nhất
                            <%} %>
                            <%else if (SortMode == "price_asc")
                                { %>
                            Giá thấp đến cao
                            <%} %>
                            <%else if (SortMode == "price_desc")
                                { %>
                            Giá cao xuống thấp
                            <%} %>
                            <%else if (SortMode == "buy")
                                { %>
                            Bán chạy nhất
                            <%} %>
                        </span>
                    </button>
                    <div class="dok-dropdown__content dropdown-menu dropdown-menu-end">
                        <div class="dok-dropdown__item position-desc">
                            <a class="sort-link <%=(SortMode == "view") ? "active" : "" %>" href="<%= ViewPage.GetURL("sort", "view") %>">Quan tâm nhất</a>
                        </div>
                        <div class="dok-dropdown__item price-asc">
                            <a class="sort-link <%=(SortMode == "price_asc") ? "active" : "" %>" href="<%= ViewPage.GetURL("sort", "price_asc") %>">Giá thấp đến cao</a>
                        </div>
                        <div class="dok-dropdown__item price-desc">
                            <a class="sort-link <%=(SortMode == "price_desc") ? "active" : "" %>" href="<%= ViewPage.GetURL("sort", "price_desc") %>">Giá cao xuống thấp</a>
                        </div>
                        <div class="dok-dropdown__item popularity-desc">
                            <a class="sort-link <%=(SortMode == "buy") ? "active" : "" %>" href="<%= ViewPage.GetURL("sort", "buy") %>">Bán chạy nhất</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div id="dok-category-mobile-filters" class="modal fade dok-modal-bottom-up box-mobile-filters"
    tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-fullscreen modal-dialog-scrollable">
        <div class="modal-content">
             <div class="modal-header">
                <div class="filter-search" style="background: none; border: none; height:20px">
                   <%-- <form action="">
                        <i class="far fa-magnifying-glass"></i>
                        <input type="search" name="" id="quick-search" class="quick-search"
                            placeholder="Tìm theo bộ lọc" autocomplete="off">
                    </form>--%>
                </div>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <div id="dok-search-filter" class="show-total-main">
                    <div class="loadersmall"></div>
                    <%if (listBrand != null && listBrand.Count > 0)
                        { %>
                    <div class="f-elem show-total-item">
                        <p class="show-total-txt" data-bs-toggle="collapse" href="#collapse-filter-brand">Hãng</p>
                        <div class="show-total-content collapse show" id="collapse-filter-brand">
                            <div class="filter-list filterBrand" data-rows="3">
                                <%for (var i = 0; listBrand != null && i < listBrand.Count; i++)
                                    {%>
                                <a href="javascript:addBrand(this, <%=listBrand[i].ID %>)" data-id="<%=listBrand[i].ID %>" class="c-btnbox filter-manu" title="<%=listBrand[i].Name %>">
                                    <%if (!string.IsNullOrEmpty(listBrand[i].File))
                                        { %>
                                    <img alt="<%=listBrand[i].Name %>" width="68" height="30" class="img-fluid" src="<%=Utils.GetWebPFile(listBrand[i].File, 4, 68, 30) %>" />
                                    <%}
                                        else
                                        { %>
                                    <%=listBrand[i].Name %>
                                    <%} %>
                                </a>
                                <%} %>
                            </div>
                            <button class="show-more" type="button">Xem thêm</button>
                        </div>
                    </div>
                    <%} %>
                    <%if (listMenu != null && listMenu.Count > 0)
                        { %>
                    <div class="f-elem show-total-item">
                        <p class="show-total-txt" data-bs-toggle="collapse" href="#collapse-filter-menu">Phân loại</p>
                        <div class="show-total-content collapse show" id="collapse-filter-menu">
                            <div class="filter-list filterMenu">
                                <%for (var i = 0; listMenu != null && i < listMenu.Count; i++)
                                    {
                                %>
                                <a href="javascript:addMenu(this, <%=listMenu[i].MenuID %>, '<%=listMenu[i].Code %>')" data-id="<%=listMenu[i].MenuID %>" class="c-btnbox <%=ViewPage.IsPageActived(listMenu[i]) ? "active" : "" %> filter-manu" title="<%=listMenu[i].Name %>">
                                    <%if (!string.IsNullOrEmpty(listMenu[i].File))
                                        { %>
                                    <img alt="<%=listMenu[i].Name %>" width="68" height="30" class="img-fluid" src="<%=Utils.GetWebPFile(listMenu[i].File, 4, 68, 30) %>" />
                                    <%}
                                        else
                                        { %>
                                    <%=!string.IsNullOrEmpty(listMenu[i].AliasName) ? listMenu[i].AliasName : listMenu[i].Name %>
                                    <%} %>
                                </a>
                                <%} %>
                            </div>
                        </div>
                    </div>
                    <%} %>
                    <%
                        if (listItem != null)
                        {
                            foreach (var item in listItem.Keys)
                            {
                                var listChildItem = listItem[item];
                                if (listChildItem == null) continue;
                                if (listChildItem.Count < 1) continue;
                    %>
                    <div class="f-elem show-total-item count-item">
                        <p class="show-total-txt" data-bs-toggle="collapse" href="#collapse-filter-<%=item.Code %>">
                            <%=item.Name %>
                        </p>
                        <div class="show-total-content collapse show" id="collapse-filter-<%=item.Code %>">
                            <div class="filter-list filterAtr filterProperty filter<%=item.Code %>">
                                <%for (var i = 0; listChildItem != null && i < listChildItem.Count; i++)
                                    {%>
                                <a href="javascript:addAtr(this, <%=listChildItem[i].ID %>, '<%=item.Code %>')" data-id="<%=listChildItem[i].ID %>" title="<%=listChildItem[i].Name %>" class="c-btnbox">
                                    <%if (!string.IsNullOrEmpty(listChildItem[i].File))
                                        { %>
                                    <img alt="<%=listChildItem[i].Name %>" width="68" height="30" class="img-fluid" src="<%=Utils.GetWebPFile(listChildItem[i].File, 4, 68, 30) %>" />
                                    <%}
                                        else
                                        { %>
                                    <%=listChildItem[i].Name %>
                                    <%} %>
                                </a>
                                <%} %>
                            </div>
                        </div>
                    </div>
                    <%} %>
                    <%} %>
                </div>
            </div>
            <div class="modal-footer justify-content-between">
                <span onclick="removeAll();" class="btn-filter-close" title="Bỏ chọn">Bỏ chọn</span>
                <a href="javascript:;" class="btn-filter-readmore">Xem <b class="total-reloading"></b>kết quả</a>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    var hasProperty = 1;
</script>
