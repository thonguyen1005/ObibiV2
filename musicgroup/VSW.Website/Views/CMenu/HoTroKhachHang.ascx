<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<%
    var listItem = ViewBag.Data as List<SysPageEntity>;
    var page = ViewBag.Page as SysPageEntity;
    if (page == null || listItem == null) return;
    string Title = ViewBag.Title;
%>
<div class="col-md-3">
    <div class="ft-box">
        <p class="ft-title"><%=!string.IsNullOrEmpty(Title) ? Title : page.Name %></p>
        <ul class="ft-list-link">
            <li>
                <p>
                    {RS:HoTroKhachHang}               
                </p>
            </li>
            <%for (int i = 0; listItem != null && i < listItem.Count; i++)
                {%>
            <li><a href="<%=ViewPage.GetPageURL(listItem[i]) %>" title="<%=listItem[i].Name %>"><%=listItem[i].Name %></a></li>
            <%} %>
        </ul>
    </div>
</div>
