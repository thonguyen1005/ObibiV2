<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>

<% 
    var listItem = ViewBag.Data as List<ModAdvEntity>;
    if (listItem == null) return;
%>
<section id="dok-slider-banner" class="mb-4">
    <div class="container-fluid">
        <div class="row">
            <div class="dok-adv-slider swiper">
                <div class="swiper-wrapper">
                    <%for (int i = 0; listItem != null && i < listItem.Count; i++)
                        {%>
                    <div class="swiper-slide">
                        <a href="<%=!string.IsNullOrEmpty(listItem[i].URL) ? listItem[i].URL : "javascript:void(0)" %>" title="<%=listItem[i].Name %>" target="<%=listItem[i].Target %>" rel="nofollow" class="plain box p-0">
                            <img class="img-fluid" src="<%=Utils.GetWebPFile(listItem[i].File, 4, 1390, 450) %>" alt="<%=listItem[i].Name %>">
                        </a>
                    </div>
                    <%} %>
                </div>
                <div class="swiper-button-next"></div>
                <div class="swiper-button-prev"></div>
                <div class="swiper-pagination"></div>
            </div>
        </div>
    </div>
</section>
