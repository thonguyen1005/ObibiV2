<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<%
    var listItem = ViewBag.Data as List<SysPageEntity>;
    var page = ViewBag.Page as SysPageEntity;
    if (page == null || listItem == null) return;
    if (listItem.Count == 0) return;
%>
<div class="container">
    <div class="dok-news-block">
        <div class="dok-heading">
            <h1 class="mb-0"><%=ViewPage.CurrentPage.Name %></h1>
        </div>
        <div class="nav-newscate overflow-hidden">
            <div class="swiper-wrapper">
                <%for (int i = 0; listItem != null && i < listItem.Count; i++)
                    {%>
                <div class="swiper-slide nav-newscate-item <%=ViewPage.IsPageActived(listItem[i]) ? "active" : "" %>">
                    <a href="<%=ViewPage.GetPageURL(listItem[i]) %>" title="<%=listItem[i].Name %>"><span><%=listItem[i].Name %></span></a>
                </div>
                <%} %>
            </div>
        </div>
    </div>
</div>
