<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>

<% 
    var listItem = ViewBag.Data as List<ModAdvEntity>;
    if (listItem == null) return;
%>
<hr class="mobile-line" />

<div class="dok-widget dok-widget-banner p-0">
    <%for (int i = 0; listItem != null && i < listItem.Count; i++)
        {%>
    <a class="<%=i> 0 ? "mt-2" : "" %>" href="<%=!string.IsNullOrEmpty(listItem[i].URL) ? listItem[i].URL : "javascript:void(0)" %>" title="<%=listItem[i].Name %>" target="<%=listItem[i].Target %>" rel="nofollow">
        <img class="img-fluid" src="<%=Utils.GetWebPFile(listItem[i].File, 4, 1390, 450) %>" alt="<%=listItem[i].Name %>">
    </a>
    <%} %>
</div>
