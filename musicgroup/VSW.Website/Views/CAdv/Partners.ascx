<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>

<% 
    var listItem = ViewBag.Data as List<ModAdvEntity>;
    if (listItem == null) return;
    string Title = ViewBag.Title;
%>
<section class="dok-partners mb-4">
    <div class="container">
        <div>
            <div class="dok-heading">
                <h2><%=Title %></h2>
            </div>
            <div class="dok-partner-list swiper">
                <div class="swiper-wrapper">
                    <%for (int i = 0; listItem != null && i < listItem.Count; i++)
                        {%>
                    <div class="swiper-slide">
                        <a href="<%=!string.IsNullOrEmpty(listItem[i].URL) ? listItem[i].URL : "javascript:void(0)" %>" title="<%=listItem[i].Name %>" target="<%=listItem[i].Target %>" rel="nofollow" class="d-block radius-10 position-relative overflow-hidden">
                            <img class="img-fluid" src="<%=Utils.GetWebPFile(listItem[i].File, 4, 478, 283) %>" alt="<%=listItem[i].Name %>">
                        </a>
                    </div>
                    <%} %>
                </div>
                <div class="swiper-button-next"></div>
                <div class="swiper-button-prev"></div>
            </div>
        </div>
    </div>
</section>
