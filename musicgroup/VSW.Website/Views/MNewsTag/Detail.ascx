<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>

<%
    var listItem = ViewBag.Data as List<ModNewsEntity>;
    var model = ViewBag.Model as MNewsTagModel;
    if (listItem == null) return;
%>

<div class="dok-news mb-lg-0 mb-4">
    <div class="row">
        <div class="col-md-7">
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
                <div class="desc mt-2">
                    <%=listItem[0].Summary %>
                </div>
            </div>
        </div>
        <div class="col-md-5">
            <div class="d-flex flex-column">
                <%for (var i = 1; listItem != null && i < (listItem.Count > 5 ? 5 : listItem.Count); i++)
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
                            <a href="<%=url%>" title="<%=listItem[i].Name %>"><%=listItem[i].Name %></a>
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
    <%if (listItem.Count > 5)
        { %>
    <hr class="my-4" />
    <div class="row">
        <div class="col-12">
            <div class="d-flex flex-column sty-big boxNews">
                <%for (var i = 5; listItem != null && i < listItem.Count; i++)
                    {
                        string url = ViewPage.GetURL(listItem[i].Code);
                %>
                <div class="item news-item-small">
                    <div class="img">
                        <a href="<%=url %>" title="<%=listItem[i].Name %>">
                            <img class="img-fluid"
                                src="<%=Utils.GetWebPFile(listItem[i].File, 4, 300, 187) %>"
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
                            <a href="<%=ViewPage.GetURL("author/" + author.Code) %>">
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
    <%} %>
    <%if (model.TotalRecord > model.PageSize)
        { %>
    <div class="page-sort-cate">
        <nav class="pagination text-center">
            <%= GetPagination(model.page, model.PageSize, model.TotalRecord)%>
        </nav>
    </div>
    <%} %>
    <%--<a class="readmore-btn down mt-4 mb-0 pagination new" href="javascript:void(0)" data-url="<%=ViewPage.CurrentURL %>" rel="nofollow">
        <span>Xem thêm bài viết</span>
    </a>--%>
</div>

