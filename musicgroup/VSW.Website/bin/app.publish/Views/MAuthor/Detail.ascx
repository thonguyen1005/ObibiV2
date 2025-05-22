<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<% 
    var item = ViewBag.Data as ModAuthorEntity;
    var listItem = ModNewsService.Instance.CreateQuery().Where(o => o.AuthorID == item.ID).ToList_Cache();
%>
<div class="container">
    <div class="dok-box-s mb-4 bg-white">
        <%if (!string.IsNullOrEmpty(item.Banner))
            { %>
        <div class="author-banner" style="background-image: url('<%=Utils.GetWebPFile(item.Banner, 4 ,1446, 360) %>');"></div>
        <%}
            else
            { %>
        <div class="author-banner" style="background-image: url('/Content/skins/img/Banner-.jpg');"></div>
        <%} %>
        <div class="">
            <div class="author-box px-4 mb-2">
                <div class="author-avatar">
                    <img src="<%=Utils.GetWebPFile(item.File, 4 ,115, 115) %>" alt="<%=item.Name %>" class="img-fluid w-100 h-100">
                </div>
                <div class="d-flex align-items-end w-100 mb-3 ms-4">
                    <div class="author-info">
                        <h1 class="fz-18 fw-bold mb-1"><%=item.Name %></h1>
                        <p class="fz-14 mb-0 text-secondary"><%=item.Position %></p>
                    </div>
                    <div class="author-social ms-auto">
                        <ul class="flex gap-4">
                            <%if (!string.IsNullOrEmpty(item.Facebook))
                                { %>
                            <li><a href="<%=item.Facebook %>" target="_blank" class="fz-20"><i class="fa-brands fa-facebook"></i></a></li>
                            <%} %>
                            <%if (!string.IsNullOrEmpty(item.Zalo))
                                { %>
                            <li><a href="<%=item.Zalo %>" target="_blank" class="fz-20">
                                <img src="/Content/skins/img/zalo.png" alt="zalo" style="width: 25px; vertical-align: top;" /></a></li>
                            <%} %>
                            <%if (!string.IsNullOrEmpty(item.Tiktok))
                                { %>
                            <li><a href="<%=item.Tiktok %>" target="_blank" class="fz-20">
                                <img src="/Content/skins/img/tiktok.png" alt="tiktok" style="width: 25px; vertical-align: top;" /></a></li>
                            <%} %>
                            <%if (!string.IsNullOrEmpty(item.Youtube))
                                { %>
                            <li><a href="<%=item.Youtube %>" target="_blank" class="fz-20"><i class="fa-brands fa-youtube"></i></a></li>
                            <%} %>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <hr class="mobile-line" />
    <div class="row">
        <div class="col-lg-4">
            <%if (!string.IsNullOrEmpty(item.Story))
                { %>
            <div class="dok-widget">
                <h2 class="fw-bold fz-18 mb-3">Thông tin cá nhân</h2>
                <div class="text-justify">
                    <%=item.Story %>
                </div>
            </div>
            <hr class="mobile-line" />
            <%} %>
            <div class="dok-widget">
                <h2 class="fw-bold fz-18 mb-3">Tiểu sử và hoạt động</h2>
                <div class="text-justify">
                    <div class="_bd">
                        <div class="short custom-scrollbar" style="max-height: 354px; overflow: hidden;">
                            <div class="_ct">
                                <%=item.Content %>
                            </div>
                            <div class="show_light"></div>
                        </div>
                    </div>
                    <%if (!string.IsNullOrEmpty(item.Content))
                        { %>
                    <a class="show" href="javascript:void(0);">
                        <span>Xem thêm</span>
                    </a>
                    <%} %>
                </div>
            </div>
            <hr class="mobile-line" />
        </div>
        <div class="col-lg-8">
            <div class="dok-widget-list position-static mt-0">
                <div class="dok-widget dok-widget-hot-news">
                    <h2 class="fw-bold fz-18 mb-3">Bài viết đã chia sẻ</h2>
                    <div class="dok-news">
                        <div class="d-flex flex-column">
                            <%for (int i = 0; listItem != null && i < listItem.Count; i++)
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
                                        <span>
                                            <i class="fa fa-calendar-alt me-2"></i><%=Utils.FormatDate(listItem[i].Published) %>
                                </span>
                                    </div>
                                </div>
                            </div>
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
