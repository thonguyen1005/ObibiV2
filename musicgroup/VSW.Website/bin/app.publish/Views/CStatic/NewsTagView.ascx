<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<% 
    string sql = "select top 10 b.* from Mod_NewsTag a join Mod_Tag b on a.TagID = b.ID order by b.[View] desc";
    var lstNewTagsView = VSW.Core.Global.DAO<ModTagEntity>.Populate(ModOrderDetailService.Instance.ExecuteReader(sql));
    if (lstNewTagsView == null) return;
    if (lstNewTagsView.Count == 0) return;
%>
<div class="dok-widget dok-widget-hot-news">
    <h4 class="fw-bold fz-18 mb-3">Chủ đề Hot</h4>
    <ul class="hot-lst">
        <%for (int i = 0; lstNewTagsView != null && i < lstNewTagsView.Count; i++)
            {%>
        <li>
            <a href="<%= ViewPage.GetURL("ntag/"+ lstNewTagsView[i].Code) %>" title="<%=lstNewTagsView[i].Name %>" target="_blank"><%=lstNewTagsView[i].Name %></a>
        </li>
        <% } %>
    </ul>
</div>
