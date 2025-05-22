<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<% 
    var listModule = VSW.Lib.Web.Application.CPModules.Where(o => o.ShowInMenu == true).OrderBy(o => o.Order).ToList();

    //logs
    var listUserLog = CPUserLogService.Instance.CreateQuery().Take(10).OrderByDesc(o => o.ID).ToList_Cache();

    //bai viet
    var listNews = ModNewsService.Instance.CreateQuery().Take(10).OrderByDesc(o => o.Order).ToList_Cache();
    var listNewsView = ModNewsService.Instance.CreateQuery().Take(10).OrderByDesc(o => o.View).ToList_Cache();

    //san pham
    var listProduct = ModProductService.Instance.CreateQuery().Take(10).OrderByDesc(o => o.Order).ToList_Cache();
    var listProductView = ModProductService.Instance.CreateQuery().Take(10).OrderByDesc(o => o.View).ToList_Cache();

    //don hang
    var listOrder = ModOrderService.Instance.CreateQuery().Where(o => o.OrderNews == true).Take(10).OrderByDesc(o => o.ID).ToList_Cache();

    //quang cao/lien ket
    int TotalProduct = ModProductService.Instance.CreateQuery().Select(o => o.ID).Count().ToValue_Cache().ToInt(0);
    int TotalNews = ModNewsService.Instance.CreateQuery().Select(o => o.ID).Count().ToValue_Cache().ToInt(0);
    int TotalOrder = ModOrderService.Instance.CreateQuery().Where(o => o.OrderNews == true).Select(o => o.ID).Count().ToValue_Cache().ToInt(0);
    int TotalAdv = ModAdvService.Instance.CreateQuery().Select(o => o.ID).Count().ToValue_Cache().ToInt(0);
%>

<%= ShowMessage()%>

<div class="page-content-wrapper">
    <div class="page-bar justify-content-between">
        <ul class="breadcrumb">
            <li class="breadcrumb-item">
                <i class="fa fa-home"></i>
                <a href="/{CPPath}/">Home</a>
            </li>
        </ul>
        <div class="page-toolbar">
            <div class="btn-group">
                <a href="/" class="btn green" target="_blank"><i class="icon-screen-desktop"></i>Xem Website</a>
            </div>
        </div>
    </div>

    <div class="row">
        <%if (listModule.Find(o => o.Code == "ModProduct") != null)
            {%>
        <div class="col-lg-3 col-md-3 col-sm-6 col-12">
            <div class="dashboard-stat blue-madison">
                <div class="visual">
                    <i class="fa fa-th"></i>
                </div>
                <div class="details">
                    <div class="number"><%=TotalProduct %></div>
                    <div class="desc">Sản phẩm</div>
                </div>
                <a href="/{CPPath}/ModProduct/Index.aspx" class="more">Xem thêm<i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <%} %>

        <%if (listModule.Find(o => o.Code == "ModOrder") != null)
            {%>
        <div class="col-lg-3 col-md-3 col-sm-6 col-12">
            <div class="dashboard-stat red-intense">
                <div class="visual">
                    <i class="fa fa-shopping-cart"></i>
                </div>
                <div class="details">
                    <div class="number"><%=TotalOrder %></div>
                    <div class="desc">Đơn hàng mới</div>
                </div>
                <a href="/{CPPath}/ModOrder/Index.aspx" class="more">Xem thêm<i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <%} %>

        <%if (listModule.Find(o => o.Code == "ModNews") != null)
            {%>
        <div class="col-lg-3 col-md-3 col-sm-6 col-12">
            <div class="dashboard-stat purple-plum">
                <div class="visual">
                    <i class="fa fa-list-alt"></i>
                </div>
                <div class="details">
                    <div class="number"><%=TotalNews %></div>
                    <div class="desc">Bài viết</div>
                </div>
                <a href="/{CPPath}/ModNews/Index.aspx" class="more">Xem thêm<i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <%} %>

        <%if (listModule.Find(o => o.Code == "ModAdv") != null)
            {%>
        <div class="col-lg-3 col-md-3 col-sm-6 col-12">
            <div class="dashboard-stat green-haze">
                <div class="visual">
                    <i class="fa fa-money"></i>
                </div>
                <div class="details">
                    <div class="number"><%=TotalAdv %></div>
                    <div class="desc">Quảng cáo - Liên kết</div>
                </div>
                <a href="/{CPPath}/ModAdv/Index.aspx" class="more">Xem thêm<i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <%} %>
    </div>
    <div class="clear"></div>

    <div class="row">
        <div class="col-lg-3 col-md-3 col-sm-6 col-12">
            <div class="dashboard-stat blue-madison">
                <div class="visual">
                    <i class="fa fa-folder"></i>
                </div>
                <div class="details">
                    <div class="number"><a href="/{CPPath}/SysMenu/Index.aspx">Chuyên mục</a></div>
                </div>
                <a href="/{CPPath}/SysMenu/Index.aspx" class="more">Xem thêm<i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <div class="col-lg-3 col-md-3 col-sm-6 col-12">
            <div class="dashboard-stat red-intense">
                <div class="visual">
                    <i class="fa fa-columns"></i>
                </div>
                <div class="details">
                    <div class="number"><a href="/{CPPath}/SysPage/Index.aspx">Trang</a></div>
                </div>
                <a href="/{CPPath}/SysPage/Index.aspx" class="more">Xem thêm<i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <div class="col-lg-3 col-md-3 col-sm-6 col-12">
            <div class="dashboard-stat purple-plum">
                <div class="visual">
                    <i class="fa fa-user"></i>
                </div>
                <div class="details">
                    <div class="number"><a href="/{CPPath}/SysUser/Index.aspx">Người sử dụng</a></div>
                </div>
                <a href="/{CPPath}/SysUser/Index.aspx" class="more">Xem thêm<i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
        <div class="col-lg-3 col-md-3 col-sm-6 col-12">
            <div class="dashboard-stat green-haze">
                <div class="visual">
                    <i class="fa fa-globe"></i>
                </div>
                <div class="details">
                    <div class="number"><a href="/{CPPath}/SysResource/Index.aspx">Tài nguyên</a></div>
                </div>
                <a href="/{CPPath}/SysResource/Index.aspx" class="more">Xem thêm<i class="fa fa-arrow-circle-right"></i></a>
            </div>
        </div>
    </div>
    <div class="clear"></div>

    <div class="row">
        <%--box sản phẩm--%>
        <div class="col-md-6 col-sm-12">
            <div class="portlet box blue-steel box-content">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-th"></i>Sản phẩm
                    </div>
                </div>
                <div class="portlet-body">
                    <div class="tabbable-line">
                        <ul class="nav-tabs clear">
                            <li class="active" data-href="#p-new">
                                <a href="javascript:;">Sản phẩm mới</a>
                            </li>
                            <li class="" data-href="#p-view">
                                <a href="javascript:;">Xem nhiều nhất</a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane active" id="p-new">
                                <div class="table-responsive">
                                    <div class="table-responsive">
                                        <table class="table table-striped table-hover table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>Sản phẩm</th>
                                                    <th class="text-center w20p">Giá bán</th>
                                                    <th class="text-center w10p"></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <%for (int i = 0; listProduct != null && i < listProduct.Count; i++)
                                                    {%>
                                                <tr>
                                                    <td>
                                                        <a href="/<%=listProduct[i].Code %><%=Setting.Sys_PageExt %>" target="_blank"><%=listProduct[i].Name %></a>
                                                    </td>
                                                    <td class="text-right"><%=string.Format("{0:#,##0}", listProduct[i].Price) %></td>
                                                    <td class="text-center">
                                                        <a href="/{CPPath}/ModProduct/Add.aspx/RecordID/<%=listProduct[i].ID %>" class="btn default btn-sm  green-stripe" target="_blank">Edit</a>
                                                    </td>
                                                </tr>
                                                <%} %>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane" id="p-view">
                                <div class="table-responsive">
                                    <div class="table-responsive">
                                        <table class="table table-striped table-hover table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>Sản phẩm</th>
                                                    <th class="text-center w20p">Giá bán</th>
                                                    <th class="text-center w10p"></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <%for (int i = 0; listProductView != null && i < listProductView.Count; i++)
                                                    {%>
                                                <tr>
                                                    <td>
                                                        <a href="/<%=listProductView[i].Code %><%=Setting.Sys_PageExt %>" target="_blank"><%=listProductView[i].Name %></a>
                                                    </td>
                                                    <td class="text-right"><%=string.Format("{0:#,##0}", listProductView[i].Price) %></td>
                                                    <td class="text-center">
                                                        <a href="/{CPPath}/ModProduct/Add.aspx/RecordID/<%=listProductView[i].ID %>" class="btn default btn-sm  green-stripe" target="_blank">Edit</a>
                                                    </td>
                                                </tr>
                                                <%} %>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--end box sản phẩm--%>

        <%--box đơn hàng--%>
        <div class="col-md-6 col-sm-12">
            <div class="portlet box red-sunglo box-content">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-bar-chart-o"></i>Đơn hàng mới
                    </div>
                </div>
                <div class="portlet-body">
                    <div class="table-responsive">
                        <table class="table table-striped table-hover table-bordered">
                            <thead>
                                <tr>
                                    <th>Mã đơn hàng</th>
                                    <th class="text-center w20p">Ngày mua</th>
                                    <th class="text-center w20p"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <%for (int i = 0; listOrder != null && i < listOrder.Count; i++)
                                    {%>
                                <tr>
                                    <td>
                                        <a href="/{CPPath}/ModOrder/Add.aspx/RecordID/<%=listOrder[i].ID %>" target="_blank"><%=listOrder[i].Code %></a>
                                    </td>
                                    <td class="text-center"><%=string.Format("{0:dd-MM-yyyy HH:mm}", listOrder[i].Created) %></td>
                                    <td class="text-center">
                                        <a href="/{CPPath}/ModOrder/Add.aspx/RecordID/<%=listOrder[i].ID %>" class="btn default btn-sm  green-stripe" target="_blank">Xem đơn</a>
                                    </td>
                                </tr>
                                <%} %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <%--end box đơn hàng--%>
    </div>
    <div class="clear"></div>

    <div class="row">
        <%--box bài viết--%>
        <div class="col-md-6 col-sm-12">
            <div class="portlet box blue-steel box-content" style="height: 100%">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-list-alt"></i>Bài viết
                    </div>
                </div>
                <div class="portlet-body">
                    <div class="tabbable-line">
                        <ul class="nav-tabs clear">
                            <li class="active" data-href="#n-new">
                                <a href="javascript:;">Mới</a>
                            </li>
                            <li class="" data-href="#n-view">
                                <a href="javascript:;">Xem nhiều nhất</a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div class="tab-pane active" id="n-new">
                                <div class="table-responsive">
                                    <table class="table table-striped table-hover table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Tiêu đề</th>
                                                <th class="text-center w20p">Ngày</th>
                                                <th class="text-center w10p"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <%for (int i = 0; listNews != null && i < listNews.Count; i++)
                                                {%>
                                            <tr>
                                                <td>
                                                    <a href="/<%=listNews[i].Code %><%=Setting.Sys_PageExt %>" target="_blank"><%=listNews[i].Name %></a>
                                                </td>
                                                <td class="text-center"><%=string.Format("{0:dd-MM-yyyy HH:mm}", listNews[i].Published) %></td>
                                                <td class="text-center">
                                                    <a href="/{CPPath}/ModNews/Add.aspx/RecordID/<%=listNews[i].ID %>" class="btn default btn-sm  green-stripe" target="_blank">Edit</a>
                                                </td>
                                            </tr>
                                            <%} %>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="tab-pane" id="n-view">
                                <div class="table-responsive">
                                    <table class="table table-striped table-hover table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Tiêu đề</th>
                                                <th class="text-center w20p">Ngày</th>
                                                <th class="text-center w10p"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <%for (int i = 0; listNewsView != null && i < listNewsView.Count; i++)
                                                {%>
                                            <tr>
                                                <td>
                                                    <a href="/<%=listNewsView[i].Code %><%=Setting.Sys_PageExt %>" target="_blank"><%=listNewsView[i].Name %></a>
                                                </td>
                                                <td class="text-center"><%=string.Format("{0:dd-MM-yyyy HH:mm}", listNewsView[i].Published) %></td>
                                                <td class="text-center">
                                                    <a href="/{CPPath}/ModNews/Add.aspx/RecordID/<%=listNewsView[i].ID %>" class="btn default btn-sm  green-stripe" target="_blank">Edit</a>
                                                </td>
                                            </tr>
                                            <%} %>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--end box bài viết--%>

        <%--box đăng nhập--%>
        <div class="col-md-6 col-sm-12">
            <div class="portlet box green-haze box-logs">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-sign-in"></i>Đăng nhập gần nhất
                    </div>
                </div>
                <div class="portlet-body">
                    <div class="table-responsive">
                        <table class="table table-hover ">
                            <thead>
                                <tr>
                                    <th class="w20p">Tài khoản</th>
                                    <th>Ghi chú</th>
                                    <th>IP</th>
                                    <th>Ngày</th>
                                    <th>ID</th>
                                </tr>
                            </thead>
                            <tbody>
                                <%for (var i = 0; listUserLog != null && i < listUserLog.Count; i++)
                                    { %>
                                <tr>
                                    <td><%= listUserLog[i].GetUser().LoginName%></td>
                                    <td><%= listUserLog[i].Note%></td>
                                    <td><%= listUserLog[i].IP%></td>
                                    <td><%= string.Format("{0:dd-MM-yyyy HH:mm}", listUserLog[i].Created)%></td>
                                    <td><%= listUserLog[i].ID%></td>
                                </tr>
                                <%} %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <%--end box đăng nhập--%>
    </div>
    <div class="clear"></div>
</div>
