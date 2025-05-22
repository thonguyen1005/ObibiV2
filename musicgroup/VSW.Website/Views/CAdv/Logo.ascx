<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>

<% 
    var item = ViewBag.Data as ModAdvEntity;
    if (item == null) return;
%>
<div class="dok-logo">
    <a href="/" title="logo">
        <img class="img-fluid" src="<%=Utils.GetUrlFile(item.File) %>" alt="Logo">
    </a>
</div>
