<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<%if (!string.IsNullOrEmpty(ViewPage.CurrentPage.Content))
    { %>
<hr class="mobile-line" />

<div class="dok-categories-info mb-4">
    <div class="_bd">
        <div class="short custom-scrollbar" style="max-height: 500px; overflow: hidden;">
            <div class="_ct">
                <%=Utils.GetHtmlForSeo(ViewPage.CurrentPage.Content) %>
            </div>
            <div class="show_light"></div>
        </div>
    </div>
    <a class="show" href="javascript:void(0);">
        <span>Xem thêm</span>
    </a>
</div>
<%} %>