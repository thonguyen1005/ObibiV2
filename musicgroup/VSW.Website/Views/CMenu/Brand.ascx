<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<%
    var listItem = ViewBag.Data as List<SysPageEntity>;
    var page = ViewBag.Page as SysPageEntity;
    if (page == null || listItem == null) return;
    if (listItem.Count == 0) return;
    string Title = ViewBag.Title;
%>
<section class="dok-brands mb-4">
    <div class="container">
        <div>
            <div class="dok-heading">
                <h2><%=!string.IsNullOrEmpty(Title) ? Title : page.Name %></h2>
            </div>
            <div class="dok-brand-list swiper">
                <div class="swiper-wrapper">
                    <%for (int i = 0; listItem != null && i < listItem.Count; i += 2)
                        {%>
                    <div class="swiper-slide">
                        <div class="category-wrap-item">
                            <a class="category-item" href="<%=ViewPage.GetPageURL(listItem[i]) %>" title="<%=listItem[i].Name %>">
                                <img src="<%=Utils.GetWebPFile(listItem[i].File, 4, 245, 50) %>"
                                    height="50" alt="<%=listItem[i].Name %>" loading="lazy" class="filter-brand__img">
                            </a>
                            <%if (listItem.Count > i + 1)
                                { %>
                            <a class="category-item" href="<%=ViewPage.GetPageURL(listItem[i+1]) %>" title="<%=listItem[i+1].Name %>">
                                <img src="<%=Utils.GetWebPFile(listItem[i+1].File, 4, 245, 50) %>"
                                    height="50" alt="<%=listItem[i+1].Name %>" loading="lazy" class="filter-brand__img">
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
</section>
