<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<div class="container">
    <div class="article-detail">
        <div class="dok-heading">
            <h1 class="mb-0"><%=ViewPage.CurrentPage.Name %></h1>
        </div>
        <hr>
        <div class="article-content">
            <%=Utils.GetHtmlForSeo(ViewPage.CurrentPage.Content) %>
        </div>
    </div>
</div>
