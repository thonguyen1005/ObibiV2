<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>

<%
    var page = ViewBag.Page as SysPageEntity;
    var listItem = ViewBag.Data as List<ModNewsEntity>;
    string Title = ViewBag.Title;
    if (page == null || listItem == null) return;
%>

<hr class="mobile-line" />

<div class="dok-widget dok-widget-hot-news">
    <h4 class="fw-bold fz-18 mb-3"><%=Title %></h4>
    <div class="dok-news">
        <div class="d-flex flex-column">
            <%for (var i = 0; listItem != null && i < listItem.Count; i++)
                {
                    string url = ViewPage.GetURL(listItem[i].Code);
            %>
            <div class="item news-item-small">
                <div class="img">
                    <a href="<%=url %>" title="<%=listItem[i].Name %>">
                        <img class="img-fluid"
                            src="<%=Utils.GetWebPFile(listItem[i].File, 4 , 144, 80) %>"
                            alt="<%=listItem[i].Name %>">
                    </a>
                </div>
                <div class="wrapper">
                    <div class="name">
                        <a href="<%=url %>" title="<%=listItem[i].Name %>"><%=listItem[i].Name %></a>
                    </div>
                    <div class="meta">
                        <%if (listItem[i].AuthorID > 0)
                            {
                                var author = listItem[i].GetAuthor();
                        %>
                        <a href="<%=ViewPage.GetURL("author/"+author.Code) %>">
                            <i class="fa fa-user me-2"></i><%=author.Name %>
                                </a>
                        <span class="mx-3">|</span>
                        <%} %>
                        <span>
                            <i class="fa fa-calendar-alt me-2"></i><%=Utils.FormatDate(listItem[i].Published) %>
                                </span>
                    </div>
                </div>
            </div>
            <%} %>
        </div>
    </div>
</div>
