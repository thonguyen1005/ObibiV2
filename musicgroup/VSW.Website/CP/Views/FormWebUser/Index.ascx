<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModWebUserModel;
    var listItem = ViewBag.Data as List<ModWebUserEntity>;
	var listWebUserMenu = ModWebUserMenuService.Instance.CreateQuery().Where(o => o.Activity == true).OrderByAsc(o => o.Order).ToList();
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Thành viên</h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Thành viên</a>
                </li>
            </ul>
            <div class="page-toolbar">
                <div class="btn-group">
                    <a href="/" class="btn green" target="_blank"><i class="icon-screen-desktop"></i>Xem Website</a>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <%= ShowMessage()%>

                <div class="portlet portlet">
                    <div class="portlet-title">
                        <div class="actions btn-set">
                            <%=GetListCommand("export|Xuất Excel,new|Thêm,edit|Sửa,delete|Xóa,config|Xóa cache")%>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
                            <div class="row desktop">
                                <div class="col-12 col-sm-12 col-md-4 col-lg-4">
                                    <div class="dataTables_search">
                                        <input type="text" class="form-control input-inline input-sm" id="filter_search" value="<%= model.SearchText %>" placeholder="Nhập từ khóa cần tìm" onchange="VSWRedirect();return false;" />
                                        <div class="table-group-actions d-inline-block">
                                            <label>Loại tài khoản</label>
                                            <select id="filter_type" class="form-control input-inline input-sm" onchange="VSWRedirect()" size="1">
                                                <option value="0">(Tất cả)</option>
                                                <option value="1" <%=model.Type == 1 ? "selected" : ""%>>Mua buôn</option>
												<option value="2" <%=model.Type == 2 ? "selected" : ""%>>Mua lẻ</option>
                                            </select>
                                        </div>
										<div class="table-group-actions d-inline-block">
                                            <label>Loại khách hàng</label>
                                            <select id="filter_type2" class="form-control input-inline input-sm" onchange="VSWRedirect()" size="1">
                                                <option value="0">(Tất cả)</option>
												<%for (int i = 0; listWebUserMenu != null && i < listWebUserMenu.Count; i++)
												{%>
													<option value="<%=listWebUserMenu[i].ID %>" <%=((listWebUserMenu[i].ID == model.Type2) ? "selected" : "") %>><%=listWebUserMenu[i].Name %></option>
												<%} %>
                                            </select>
                                        </div>
										<button type="button" class="btn blue" onclick="VSWRedirect();return false;">Tìm kiếm</button>
                                    </div>
                                </div>
                            </div>

                            <div class="table-scrollable">
                                <table class="table table-striped table-hover table-bordered dataTable">
                                    <thead>
                                        <tr>
                                            <th class="sorting text-center w1p">#</th>
                                            <th class="sorting text-center w1p">Chọn</th>
                                            <th class="sorting text-center title"><%= GetSortLink("Tên đăng nhập", "UserName")%></th>
                                            <th class="sorting text-center desktop"><%= GetSortLink("Họ và tên", "Name")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Email", "Email")%></th>
                                            <th class="sorting text-center w8p"><%= GetSortLink("Điện thoại", "Phone")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Ngày tạo", "Created")%></th>
                                            <th class="sorting text-center w1p"><%= GetSortLink("IP", "IP")%></th>
                                            <th class="sorting text-center w1p"><%= GetSortLink("ID", "ID")%></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%for (var i = 0; listItem != null && i < listItem.Count; i++)
                                            {
                                        %>
                                        <tr>
                                            <td class="text-center"><%= i + 1%></td>
                                            <td class="text-center"><a href="javascript:CloseWebUser(<%= listItem[i].ID %>,'<%= listItem[i].UserName %>')">Chọn</a></td>

                                            <td class="text-center">
                                                <a href="javascript:CloseWebUser(<%= listItem[i].ID %>,'<%= listItem[i].UserName %>')"><%= listItem[i].UserName%></a>
                                                <br />
                                                <%if (!string.IsNullOrEmpty(listItem[i].File))
                                                    { %>
                                                <%= Utils.GetMedia(listItem[i].File, 40, 40)%>
                                                <%} %>
                                            </td>
                                            <td class="text-center desktop">
                                                <a href="javascript:CloseWebUser(<%= listItem[i].ID %>,'<%= listItem[i].UserName %>')"><%= listItem[i].Name %></a>
                                                <%if (listItem[i].Type2 > 0)
                                                    { %>
                                                <br />
                                                <%= GetName(listItem[i].WebUserMenu) %>
                                                <%} %>
                                            </td>
                                            <td class="text-center desktop"><a href="javascript:CloseWebUser(<%= listItem[i].ID %>,'<%= listItem[i].UserName %>')"><%= listItem[i].Email %></a></td>
                                            <td class="text-center"><%= listItem[i].Phone %></td>
                                            <td class="text-center desktop"><%= string.Format("{0:dd-MM-yyyy HH:mm}", listItem[i].Created) %></td>
                                            <td class="text-center desktop"><%= listItem[i].IP %></td>
                                            <td class="text-center"><%= listItem[i].ID%></td>
                                        </tr>
                                        <%} %>
                                    </tbody>
                                </table>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-sm-12 justify-content-center d-flex ">
                                    <%= GetPagination(model.PageIndex, model.PageSize, model.TotalRecord)%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        var VSWController = "FormWebUser";

        var VSWArrVar = [
            "filter_lang", "LangID",
            "limit", "PageSize",
			"filter_type", "Type",
			"filter_type2", "Type2",
			"limit", "PageSize"
        ];

        var VSWArrVar_QS = [
            "filter_search", "SearchText"
        ];

        var VSWArrQT = [
            "<%= model.PageIndex + 1 %>", "PageIndex",
            "<%= model.Sort %>", "Sort"
        ];

        var VSWArrDefault = [
            "1", "PageIndex",
            "1", "LangID",
            "20", "PageSize"
        ];
    </script>

</form>
