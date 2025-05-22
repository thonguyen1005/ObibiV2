<%@ Control Language="C#" AutoEventWireup="true" %>

<% 
    var listModule = VSW.Lib.Web.Application.CPModules.Where(o => o.ShowInMenu == true).OrderBy(o => o.Order).ToList();

    var permissionsProduct = CPLogin.CurrentUser.GetPermissionsByModule("ModProduct");
    var permissionsGitf = CPLogin.CurrentUser.GetPermissionsByModule("ModGitf");
    var permissionsPromotion = CPLogin.CurrentUser.GetPermissionsByModule("ModPromotion");

    var permissionsOrder = CPLogin.CurrentUser.GetPermissionsByModule("ModOrder");
    var permissionsWebUser = CPLogin.CurrentUser.GetPermissionsByModule("ModWebUser");
    var permissionsWebUserMenu = CPLogin.CurrentUser.GetPermissionsByModule("ModWebUserMenu");
%>
<aside id="layout-menu" class="layout-menu menu-vertical menu bg-menu-theme">
    <div class="app-brand demo">
        <a href="/{CPPath}/" class="app-brand-link">
            <span class="app-brand-logo demo">
                <img src="/{CPPath}/Content/skins/img/logo.png" style="width: 180px;" alt="logo" />
            </span>
            <%--<span class="app-brand-text demo menu-text fw-bold ms-2">OBIBI</span>--%>
        </a>
        <%--<a href="javascript:void(0);" class="layout-menu-toggle menu-link text-large ms-auto d-block">
            <i class="bx bx-chevron-left bx-sm align-middle"></i>
        </a>--%>
    </div>
    <div class="menu-inner-shadow"></div>
    <ul class="menu-inner py-1">
        <!-- Dashboards -->
        <li class="menu-item <%=Request.RawUrl.Contains("/Home/") || Request.RawUrl == "/CP/" || Request.RawUrl == "/CP/Default.aspx" ? "active" : "" %>">
            <a href="/{CPPath}/" class="menu-link">
                <i class="menu-icon tf-icons bx bx-home-circle"></i>
                <div data-i18n="Home">Home</div>
            </a>
        </li>
        <%if (CPLogin.CurrentUser.IsAdministrator || (permissionsProduct != null && permissionsProduct.View))
            { %>
        <li class="menu-item <%=Request.RawUrl.Contains("ModProduct") || Request.RawUrl.Contains("ModGift") || Request.RawUrl.Contains("ModPromotion") ? "active open" : "" %>">
            <a href="javascript:;" class="menu-link menu-toggle">
                <i class="menu-icon tf-icons bx bx-detail"></i>
                <div data-i18n="Sản phẩm">Sản phẩm</div>
            </a>
            <ul class="menu-sub">
                <li class="menu-item <%=Request.RawUrl.Contains("ModProduct/Index.aspx") ? "active" : "" %>">
                    <a href="/{CPPath}/ModProduct/Index.aspx" class="menu-link">
                        <div data-i18n="Danh sách sản phẩm">Quản lý sản phẩm</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("ModProduct/Add.aspx") ? "active" : "" %>">
                    <a href="/{CPPath}/ModProduct/Add.aspx" class="menu-link">
                        <div data-i18n="Thêm mới sản phẩm">Thêm sản phẩm mới</div>
                    </a>
                </li>
                <%if (permissionsPromotion != null && permissionsPromotion.View)
                    { %>
                <li class="menu-item <%=Request.RawUrl.Contains("ModPromotion") ? "active" : "" %>">
                    <a href="/{CPPath}/ModPromotion/Index.aspx" class="menu-link">
                        <div data-i18n="Quản lý Khuyến mại">Quản lý Khuyến mại</div>
                    </a>
                </li>
                <%} %>
                <%--<%if (permissionsGitf != null && permissionsGitf.View)
                    { %>
                <li class="menu-item <%=Request.RawUrl.Contains("ModGift") ? "active" : "" %>">
                    <a href="/{CPPath}/ModGift/Index.aspx" class="menu-link">
                        <div data-i18n="Quản lý quà tặng">Quản lý quà tặng</div>
                    </a>
                </li>
                <%} %>--%>
            </ul>
        </li>
        <%} %>
        <%if (CPLogin.CurrentUser.IsAdministrator || (permissionsOrder != null && permissionsOrder.View))
            { %>
        <li class="menu-item <%=Request.RawUrl.Contains("ModOrder") ? "active open" : "" %>">
            <a href="javascript:;" class="menu-link menu-toggle">
                <i class="menu-icon fa fa-shopping-cart"></i>
                <div data-i18n="Đơn hàng">Đơn hàng</div>
            </a>
            <ul class="menu-sub">
                <li class="menu-item <%=Request.RawUrl.Contains("ModOrder") && (!Request.QueryString.AllKeys.Contains("StatusID") || (Request.QueryString.AllKeys.Contains("StatusID") && VSW.Core.Global.Convert.ToInt(Request.QueryString["StatusID"]) != 11982))  ? "active" : "" %>">
                    <a href="/{CPPath}/ModOrder/Index.aspx" class="menu-link">
                        <div data-i18n="Quản lý đơn hàng">Quản lý đơn hàng</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("ModOrder") && (Request.QueryString.AllKeys.Contains("StatusID") && VSW.Core.Global.Convert.ToInt(Request.QueryString["StatusID"]) == 11982)  ? "active" : "" %>">
                    <a href="/{CPPath}/ModOrder/Index.aspx?StatusID=11982" class="menu-link">
                        <div data-i18n="Quản lý yêu cầu hủy">Quản lý yêu cầu hủy</div>
                    </a>
                </li>
            </ul>
        </li>
        <%} %>
        <%if (CPLogin.CurrentUser.IsAdministrator || (permissionsWebUser != null && permissionsWebUser.View))
            { %>
        <li class="menu-item <%=Request.RawUrl.Contains("ModWebUser") ? "active open" : "" %>">
            <a href="javascript:;" class="menu-link menu-toggle">
                <i class="menu-icon fa fa-user"></i>
                <div data-i18n="Sản phẩm">Khách hàng</div>
            </a>
            <ul class="menu-sub">
                <li class="menu-item <%=Request.RawUrl.Contains("ModWebUser/") ? "active" : "" %>">
                    <a href="/{CPPath}/ModWebUser/Index.aspx" class="menu-link">
                        <div data-i18n="Khách hàng">Khách hàng</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("ModWebUserMenu/") ? "active" : "" %>">
                    <a href="/{CPPath}/ModWebUserMenu/Index.aspx" class="menu-link">
                        <div data-i18n="Phân loại khách hàng">Phân loại khách hàng</div>
                    </a>
                </li>
            </ul>
        </li>
        <%} %>
        <li class="menu-item <%=(listModule.Where(o=> o.Code != "ModProduct" && o.Code != "ModGift" && o.Code != "ModPromotion"  && o.Code != "ModOrder" && o.Code != "ModWebUser" && o.Code != "ModWebUserMenu" && Request.RawUrl.Contains(o.Code)).Any() ? "active open" : "") %>">
            <a href="javascript:;" class="menu-link menu-toggle">
                <i class="menu-icon fa fa-folder"></i>
                <div data-i18n="{RS:MenuTop_Management}">{RS:MenuTop_Management}</div>
            </a>
            <ul class="menu-sub">
                <%for (var i = 0; i < listModule.Count; i++)
                    {
                        if (!CPLogin.CurrentUser.IsAdministrator)
                        {
                            var permission = CPLogin.CurrentUser.GetPermissionsByModule(listModule[i].Code);
                            if (permission == null) continue;
                            if (!permission.View) continue;
                        }
                        if (listModule[i].Code == "ModProduct" || listModule[i].Code == "ModGift" || listModule[i].Code == "ModOrder" || listModule[i].Code == "ModWebUser" || listModule[i].Code == "ModWebUserMenu") continue;
                        string icon = "folder";
                        if (listModule[i].Code == "ModAdv") icon = "money";
                        else if (listModule[i].Code == "ModNews") icon = "list-alt";
                        else if (listModule[i].Code == "ModFile") icon = "upload";
                        else if (listModule[i].Code == "ModComment") icon = "comments";
                        else if (listModule[i].Code == "ModOrder") icon = "shopping-cart";
                        else if (listModule[i].Code == "ModProduct") icon = "th";
                        else if (listModule[i].Code == "ModVideo") icon = "film";
                        else if (!string.IsNullOrEmpty(listModule[i].CssClass)) icon = listModule[i].CssClass;
                %>
                <li class="menu-item <%=Request.RawUrl.Contains(listModule[i].Code+"/") ? "active" : "" %>">
                    <a href="/{CPPath}/<%=listModule[i].Code%>/Index.aspx" class="menu-link">
                        <div data-i18n="<%=listModule[i].Name%>"><i class="menu-icon-sub fa fa-<%=icon %>"></i><%=listModule[i].Name%></div>
                    </a>
                </li>
                <%} %>
            </ul>
        </li>
        <%if (CPLogin.CurrentUser.IsAdministrator)
            { %>
        <li class="menu-item <%=(Request.RawUrl.Contains("SysPage") || Request.RawUrl.Contains("SysTemplate")  || Request.RawUrl.Contains("SysSite") ? "active open" : "") %>">
            <a href="javascript:;" class="menu-link menu-toggle">
                <i class="menu-icon fa fa-pencil-square-o"></i>
                <div data-i18n="{RS:MenuTop_Design}">{RS:MenuTop_Design}</div>
            </a>
            <ul class="menu-sub">
                <li class="menu-item <%=Request.RawUrl.Contains("SysPage/") ? "active" : "" %>">
                    <a href="/{CPPath}/SysPage/Index.aspx" class="menu-link">
                        <div data-i18n="{RS:MenuTop_Page}"><i class="menu-icon-sub fa fa-th"></i>{RS:MenuTop_Page}</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("SysTemplate/") ? "active" : "" %>">
                    <a href="/{CPPath}/SysTemplate/Index.aspx" class="menu-link">
                        <div data-i18n="{RS:MenuTop_Template}"><i class="menu-icon-sub fa fa-columns"></i>{RS:MenuTop_Template}</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("SysSite/") ? "active" : "" %>">
                    <a href="/{CPPath}/SysSite/Index.aspx" class="menu-link">
                        <div data-i18n="Site"><i class="menu-icon-sub fa fa-sitemap"></i>Site</div>
                    </a>
                </li>
            </ul>
        </li>
        <li class="menu-item <%=(Request.RawUrl.Contains("SysMenu") || Request.RawUrl.Contains("SysRedirection") || Request.RawUrl.Contains("SysProperty") || Request.RawUrl.Contains("SysRole") || Request.RawUrl.Contains("SysUser") || Request.RawUrl.Contains("SysResource") || Request.RawUrl.Contains("SysUserLog") ? "active open" : "") %>">
            <a href="javascript:;" class="menu-link menu-toggle">
                <i class="menu-icon fa fa-gears"></i>
                <div data-i18n="{RS:MenuTop_System}">{RS:MenuTop_System}</div>
            </a>
            <ul class="menu-sub">
                <li class="menu-item <%=Request.RawUrl.Contains("SysMenu/") ? "active" : "" %>">
                    <a href="/{CPPath}/SysMenu/Index.aspx" class="menu-link">
                        <div data-i18n="Chuyên mục"><i class="menu-icon-sub fa fa-folder"></i>Chuyên mục</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("SysRedirection/") ? "active" : "" %>">
                    <a href="/{CPPath}/SysRedirection/Index.aspx" class="menu-link">
                        <div data-i18n="Redicrect 301"><i class="menu-icon-sub fa fa-folder"></i>Redicrect 301</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("SysProperty/") ? "active" : "" %>">
                    <a href="/{CPPath}/SysProperty/Index.aspx" class="menu-link">
                        <div data-i18n="Thuộc tính"><i class="menu-icon-sub fa fa-folder"></i>Thuộc tính</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("SysRole/") ? "active" : "" %>">
                    <a href="/{CPPath}/SysRole/Index.aspx" class="menu-link">
                        <div data-i18n="Nhóm người sử dụng"><i class="menu-icon-sub fa fa-users"></i>Nhóm người sử dụng</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("SysUser/") ? "active" : "" %>">
                    <a href="/{CPPath}/SysUser/Index.aspx" class="menu-link">
                        <div data-i18n="Người sử dụng"><i class="menu-icon-sub fa fa-user"></i>Người sử dụng</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("SysResource/") ? "active" : "" %>">
                    <a href="/{CPPath}/SysResource/Index.aspx" class="menu-link">
                        <div data-i18n="Tài nguyên"><i class="menu-icon-sub fa fa-globe"></i>Tài nguyên</div>
                    </a>
                </li>
                <li class="menu-item <%=Request.RawUrl.Contains("SysUserLog/") ? "active" : "" %>">
                    <a href="/{CPPath}/SysUserLog/Index.aspx" class="menu-link">
                        <div data-i18n="Nhật ký đăng nhập"><i class="menu-icon-sub fa fa-calendar"></i>Nhật ký đăng nhập</div>
                    </a>
                </li>
            </ul>
        </li>
        <%} %>
    </ul>
</aside>
