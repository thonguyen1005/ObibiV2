<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>

<%
    var page = ViewBag.Page as SysPageEntity;
    var listItem = ViewBag.Data as List<ModNewsEntity>;
    string Title = ViewBag.Title;
    if (page == null || listItem == null) return;
%>
<section class="dok-news mb-4">
    <div class="container">
        <div class="row">
            <div class="col-12">
                <div class="dok-heading">
                    <h2><%=!string.IsNullOrEmpty(Title) ? Title : page.Name %></h2>
                </div>
            </div>
            <div class="col-md-6">
                <div class="d-flex flex-column item news-item-large mb-md-0 mb-5">
                    <div class="img">
                        <a href="<%=ViewPage.GetURL(listItem[0].Code) %>" title="<%=listItem[0].Name %>">
                            <img class="img-fluid"
                                src="<%=Utils.GetWebPFile(listItem[0].File, 4 , 800, 500) %>"
                                alt="<%=listItem[0].Name %>">
                        </a>
                    </div>
                    <div class="meta">
                        <%if (listItem[0].AuthorID > 0)
                            {
                                var author = listItem[0].GetAuthor();
                        %>
                        <a href="<%=ViewPage.GetURL("author/"+author.Code) %>">
                            <i class="fa fa-user me-2"></i><%=author.Name %>
                        </a>
                        <span class="mx-3">|</span>
                        <%} %>
                        <span>
                            <i class="fa fa-calendar-alt me-2"></i><%=Utils.FormatDate(listItem[0].Published) %>
                        </span>
                    </div>
                    <div class="name">
                        <a href="<%=ViewPage.GetURL(listItem[0].Code) %>" title="<%=listItem[0].Name %>">
                            <%=listItem[0].Name %>
                        </a>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="d-flex flex-column">
                    <%for (var i = 1; listItem != null && i < listItem.Count; i++)
                        {%>
                    <div class="item news-item-small">
                        <div class="img">
                            <a href="<%=ViewPage.GetURL(listItem[i].Code) %>" title="<%=listItem[i].Name %>">
                                <img class="img-fluid"
                                    src="<%=Utils.GetWebPFile(listItem[i].File, 4 , 144, 80) %>"
                                    alt="<%=listItem[i].Name %>">
                            </a>
                        </div>
                        <div class="wrapper">
                            <div class="name">
                                <a href="<%=ViewPage.GetURL(listItem[i].Code) %>" title="<%=listItem[i].Name %>"><%=listItem[i].Name %></a>
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
    </div>
</section>
