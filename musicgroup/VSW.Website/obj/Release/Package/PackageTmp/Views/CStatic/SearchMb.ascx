<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<div class="dok-header-search d-lg-none d-block">
    <form method="post" id="formSearchmb" name="formSearchmb" onsubmit="doSearch('<%=ViewPage.SearchUrl %>', $('#keywordMb')); return false;">
        <input type="search" name="keywordMb" id="keywordMb" placeholder="Bạn cần tìm gì hôm nay?"
               autocomplete="off">
        <button type="button" onclick="doSearch('<%=ViewPage.SearchUrl %>', $('#keywordMb')); return false;"><i class="far fa-magnifying-glass"></i></button>
    </form>
</div>