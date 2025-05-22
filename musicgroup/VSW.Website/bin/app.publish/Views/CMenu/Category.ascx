<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<%
    var listItem = ViewBag.Data as List<SysPageEntity>;
    var page = ViewBag.Page as SysPageEntity;
    if (page == null || listItem == null) return;
    if (listItem.Count == 0) return;
    string Title = ViewBag.Title;
%>
<section class="dok-categories mb-4">
    <div class="container">
        <div>
            <div class="dok-heading">
                <h2><%=!string.IsNullOrEmpty(Title) ? Title : page.Name %></h2>
            </div>
            <div class="dok-category-list swiper">
                <div class="swiper-wrapper">
                    <%for (int i = 0; listItem != null && i < listItem.Count; i += 2)
                        {%>
                    <div class="swiper-slide">
                        <div class="category-wrap-item">
                            <a class="category-item" href="<%=ViewPage.GetPageURL(listItem[i]) %>" title="<%=listItem[i].Name %>">
                                <div class="cate-img">
                                    <img class="img-fluid"
                                        src="<%=Utils.GetWebPFile(listItem[i].File, 4, 106, 106) %>"
                                        alt="<%=listItem[i].Name %>">
                                </div>
                                <h4 class="cate-title"><%=listItem[i].Name %></h4>
                            </a>
                            <%if (listItem.Count > i + 1)
                                { %>
                            <a class="category-item" href="<%=ViewPage.GetPageURL(listItem[i+1]) %>" title="<%=listItem[i+1].Name %>">
                                <div class="cate-img">
                                    <img class="img-fluid"
                                        src="<%=Utils.GetWebPFile(listItem[i+1].File, 4, 106, 106) %>"
                                        alt="<%=listItem[i+1].Name %>">
                                </div>
                                <h4 class="cate-title"><%=listItem[i+1].Name %></h4>
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
