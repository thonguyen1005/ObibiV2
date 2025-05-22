<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<div class="row">
    <div class="col-md-9 col-sm-9 col-xs-12">
        <VSW:StaticControl Code="CStatic" VSWID="vswStaticBreadcrumb" DefaultLayout="Breadcrumb" runat="server" />
        <div class="blog-desc">
            <div class="bxcontentnews">
                <h1 class="tb-titlel"><%=ViewBag.Title %></h1>
                <%=Utils.GetHtmlForSeo(ViewBag.Text) %>
            </div>
            <!-- end -->
        </div>
        <!-- end -->
    </div>
</div>
