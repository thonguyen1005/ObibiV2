<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<% 
    var cart = new Cart();
%>
<nav id="header-simple" class="header-simple">
    <div class="container">
        <ul>
            <li>
                <a href="/">
                    <i class="fa fa-arrow-left"></i>
                </a>
            </li>
            <li class="dok-header-search d-lg-none d-block">
                <form method="post" id="formSearchPopup" onsubmit="doSearch('<%=ViewPage.SearchUrl %>', $('#keywordPopup')); return false;">
                    <input type="search" name="keywordPopup" id="keywordPopup" placeholder="Bạn cần tìm gì hôm nay?" autocomplete="off">
                    <button type="button" onclick="doSearch('<%=ViewPage.SearchUrl %>', $('#keywordPopup')); return false;"><i class="far fa-magnifying-glass"></i></button>
                </form>
            </li>
            <li>
                <a href="https://www.facebook.com/sharer/sharer.php?u=<%=VSW.Core.Web.HttpRequest.Domain + ViewPage.GetURL(ViewPage.CurrentPage.Code) %>" target="_blank">
                    <i class="fa fa-share"></i>
                </a>
            </li>
            <li class="dok-cart droplist mx-0">
                <a href="<%=ViewPage.ViewCartUrl %>" class="d-flex align-items-center">
                    <div class="position-relative">
                        <span class="dok-icon mr-0 fal fa-cart-shopping">
                            <span class="dok-count cart_count"><%=cart.Items.Count %></span>
                        </span>
                    </div>
                </a>
            </li>
        </ul>
    </div>
</nav>
