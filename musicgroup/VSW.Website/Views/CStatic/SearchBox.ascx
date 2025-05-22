<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<% 
    var cart = new Cart();
%>
<div class="dok-header-search d-lg-block d-none">
    <form method="post" id="formSearch" name="formSearch" onsubmit="doSearch('<%=ViewPage.SearchUrl %>', $('#keyword')); return false;">
        <input type="search" name="keyword" id="keyword" placeholder="Bạn cần tìm gì hôm nay?" autocomplete="off">
        <button type="button" onclick="doSearch('<%=ViewPage.SearchUrl %>', $('#keyword')); return false;"><i class="far fa-magnifying-glass"></i></button>
    </form>
</div>

<ul class="dok-list-action list-unstyled d-flex align-items-center justify-content-end">
    {RS:HotlineHeader}
    <li class="dok-cart droplist mx-0">
        <a href="<%=ViewPage.ViewCartUrl %>" class="d-flex align-items-center">
            <div class="position-relative">
                <span class="dok-icon mr-0 fal fa-cart-shopping">
                    <span class="dok-count cart_count"><%=cart.Items.Count %></span>
                </span>
            </div>
        </a>
        <div class="droplist-box d-lg-block d-none">
            <span class="dok-arrow"></span>
            <ul class="dok-cart-list custom-scrollbar list-unstyled">
                <%for (int i = 0; i < cart.Items.Count; i++)
                    {
                        var product = ModProductService.Instance.GetByID_Cache(cart.Items[i].ProductID);
                        string url = ViewPage.GetURL(product.MenuID, product.Code);
                %>
                <li id="item-cart-<%=i %>">
                    <div class="dok-cart-img">
                        <a href="<%=url %>" title="<%=product.Name %>">
                            <img class="img-fluid"
                                src="<%=Utils.GetWebPFile(product.File, 4 ,50, 50) %>"
                                alt="<%=product.Name %>">
                        </a>
                    </div>
                    <div class="dok-cart-info">
                        <p class="dok-cart-title mb-0">
                            <%=product.Name %>
                        </p>
                        <%if (product.PriceView > 0)
                            { %>
                        <p class="dok-cart-price"><%=Utils.FormatMoney(product.PriceView) %>đ</p>
                        <%}
                            else
                            { %>
                        <p class="dok-cart-price">{RS:Lienhe}</p>
                        <%} %>
                    </div>
                    <a class="dok-cart-trash" href="javascript:deleteCart(<%=cart.Items[i].ProductID %>, <%=i %>)">
                        <i class="far fa-trash-alt"></i>
                    </a>
                </li>
                <%} %>
            </ul>
            <div class="dok-cart-footer">
                <span class="fw-medium"><span class="cart_count"><%=cart.Items.Count %></span> sản phẩm</span>
                <a href="<%=ViewPage.ViewCartUrl %>" class="dok-cart-buy">Xem giỏ hàng</a>
            </div>
        </div>
    </li>
    <li class="d-lg-none d-block">
        <a class="dok-header-item-mobile dok-category-toggle" href="javascript:void(0)"
            data-bs-toggle="modal" data-bs-target="#dok-menu-mobile">
            <i class="far fa-bars-staggered"></i>
        </a>
    </li>
</ul>
