<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<% 
    //string UrlRidirectAllProperty = ViewPage.ViewBag.UrlRidirectAllProperty;
    //string ProductPropertyName = ViewPage.ViewBag.ProductPropertyName;
    int BrandID = VSW.Core.Global.Convert.ToInt(ViewPage.ViewBag.BrandID);
    string brandCode = "";
    string brandName = "";
%>
<section class="dok-breadcrumb">
    <div class="container">
        <div class="breadcrumb" id="breadcrumb">
            <%= Utils.GetMapPage(ViewPage.CurrentPage) %>
            <%if (BrandID > 0)
                {
                    var brand = WebMenuService.Instance.GetByID_Cache(BrandID);
                    if (brand != null)
                    {
                        brandName = brand.Name;
                        brandCode = brand.Code;

            %>
            <span class="mx-2"><i class="fa fa-chevron-right fz-12"></i></span>
            <a href="<%=ViewPage.GetURL(ViewPage.CurrentPage.Code + "-"+brandCode+Setting.Sys_PageExt) %>" title="<%=ViewPage.CurrentPage.Name+(!string.IsNullOrEmpty(brandName) ? (" "+brandName) : "") %>"><%=ViewPage.CurrentPage.Name+(!string.IsNullOrEmpty(brandName) ? (" "+brandName) : "") %></a>
            <% 
                    }
                }
            %>
        </div>
    </div>
</section>
<%--<%if (!string.IsNullOrEmpty(ProductPropertyName))
                {
                    string brandName = "";
                    if (BrandID > 0)
                    {
                        brandName = ViewPage.ViewBag.BrandName;
                        if (string.IsNullOrEmpty(brandName))
                        {
                            var brand = WebMenuService.Instance.GetByID_Cache(BrandID);
                            if (brand != null) brandName = brand.Name;
                        }
                    }
            %>
            <span class="mx-2"><i class="fa fa-chevron-right fz-12"></i></span>
            <a href="<%=UrlRidirectAllProperty %>" title="<%=ViewPage.CurrentPage.Name+(!string.IsNullOrEmpty(brandName) ? (" "+brandName) : "")+" "+ ProductPropertyName %>"><%=ViewPage.CurrentPage.Name+(!string.IsNullOrEmpty(brandName) ? (" "+brandName) : "")+" "+ ProductPropertyName %></a>
            <%}%>--%>
