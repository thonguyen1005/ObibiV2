<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<% 
    var model = ViewBag.Model as ModOrderModel;
    var listItem = ViewBag.Data as List<ModOrderEntity>;
    var lstStatus = WebMenuService.Instance.GetByParentID_Cache(11976);
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Đơn hàng</h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Đơn hàng</a>
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
                    <div class="row portlet-title">
                        <%if (lstStatus != null)
                            { %>
                        <div class="col-md-8">
                            <div class="tabbable-line transparent">
                                <ul class="nav-tabs clear" style="border: none; margin-bottom: 0px;">
                                    <li class="<%= model.StatusID < 1 ? "active" : "" %>">
                                        <a href="/{CPPath}/ModOrder/Index.aspx">Tất cả</a>
                                    </li>
                                    <%for (int i = 0; i < lstStatus.Count; i++)
                                        {%>
                                    <li class="<%=lstStatus[i].ID == model.StatusID ? "active" : "" %>">
                                        <a href="/{CPPath}/ModOrder/Index.aspx?StatusID=<%=lstStatus[i].ID %>"><%=lstStatus[i].Name %></a>
                                    </li>
                                    <% } %>
                                </ul>
                            </div>
                        </div>
                        <%} %>
                        <div class="col-md-4 text-right actions">
                            <%=GetListCommand("export|Xuất Excel|btn-success,config|Xóa cache|purple")%>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
                            <div class="row hidden-sm hidden-col">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="dataTables_search">
                                        <input type="text" class="form-control input-inline input-sm" id="filter_search" value="<%= model.SearchText %>" placeholder="Nhập từ khóa cần tìm" onchange="VSWRedirect();return false;" />
                                        <input type="text" class="form-control input-inline input-sm date" id="filter_fromdate" value="<%= model.FromDate %>" placeholder="Từ ngày" />
                                        <input type="text" class="form-control input-inline input-sm date" id="filter_todate" value="<%= model.ToDate %>" placeholder="Đến ngày" />
                                        <%--<div class="table-group-actions d-inline-block">
                                            <label>Trạng thái</label>
                                            <select id="filter_status" class="form-select select2" onchange="VSWRedirect()" size="1">
                                                <option value="0">(Tất cả)</option>
                                                <%= Utils.ShowDdlMenuByType("Status", model.LangID, model.StatusID)%>
                                            </select>
                                        </div>--%>
                                        <button type="button" class="btn blue" onclick="VSWRedirect();return false;">Tìm kiếm</button>
                                    </div>
                                </div>
                                <div class="col-12 col-sm-12 col-md-8 col-lg-8 text-right pull-right"></div>
                            </div>
                            <div class="portlet portlet mt-2 hide" id="boxActionHide">
                                <div class="portlet-title">
                                    <div class="actions">
                                        <span class="mr-2">Đã chọn: <span id="countChose">0</span></span>
                                        <%=GetListCommand("delete|Xóa|btn-danger")%>
                                    </div>
                                </div>
                            </div>
                            <div class="table-scrollable">
                                <table class="table table-striped table-hover table-bordered dataTable">
                                    <thead>
                                        <tr>
                                            <th class="sorting text-center w1p">#</th>
                                            <th class="sorting_disabled text-center w1p">
                                                <label class="itemCheckBox itemCheckBox-sm">
                                                    <input type="checkbox" name="toggle" onclick="checkAll(<%= model.PageSize %>)">
                                                    <i class="check-box"></i>
                                                </label>
                                            </th>
                                            <th class="sorting text-center" style="width: 13%"><%= GetSortLink("Mã đơn hàng", "Code")%></th>
                                            <th class="sorting text-center hidden-sm hidden-col"><%= GetSortLink("Họ và tên", "Name")%></th>
                                            <th class="sorting text-center w10p"><%= GetSortLink("Tổng tiền", "Total")%></th>
                                            <th class="sorting text-center w10p hidden-sm hidden-col"><%= GetSortLink("Trạng thái", "StatusID")%></th>
                                            <th class="sorting text-center w10p hidden-sm hidden-col" style="width: 12%;"><%= GetSortLink("Trạng thái TT", "StatusPay")%></th>
                                            <th class="sorting text-center w10p hidden-sm hidden-col" style="width: 8%;"><%= GetSortLink("Xuất HĐ", "Invoice")%></th>
                                            <th class="sorting text-center w10p hidden-sm hidden-col"><%= GetSortLink("Ngày mua", "Created")%></th>
                                            <th class="sorting text-center w10p hidden-sm hidden-col"><%= GetSortLink("IP", "IP")%></th>
                                            <th class="sorting text-center w1p"><%= GetSortLink("ID", "ID")%></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%for (var i = 0; listItem != null && i < listItem.Count; i++)
                                            { %>
                                        <tr style="<%=(listItem[i].OrderNews ? "font-weight: bold;": "")%>">
                                            <td class="text-center"><%= i + 1%></td>
                                            <td class="text-center">
                                                <%= GetCheckbox(listItem[i].ID, i)%>
                                            </td>
                                            <td>
                                                <a href="javascript:VSWRedirect('Add', <%= listItem[i].ID %>)"><%= listItem[i].Code%></a>
                                            </td>
                                            <td class="text-center hidden-sm hidden-col"><a href="javascript:VSWRedirect('Add', <%= listItem[i].ID %>)"><%= listItem[i].Name%></a></td>
                                            <td class="text-center"><%= string.Format("{0:#,##0}", listItem[i].Total + listItem[i].Fee - listItem[i].SaleMoney) %></td>
                                            <td class="text-center hidden-sm hidden-col"><%= GetName(listItem[i].GetStatus()) %></td>
                                            <td class="text-center hidden-sm hidden-col"><b style="<%= listItem[i].StatusPay ? "color:#22e020;": "color:red;"%>"><%= listItem[i].StatusPay ? "Đã thanh toán" : "Chưa thanh toán" %></b></td>
                                            <td class="text-center hidden-sm hidden-col"><%= listItem[i].Invoice ? "Có" : "Không" %></td>
                                            <td class="text-center hidden-sm hidden-col"><%= string.Format("{0:dd-MM-yyyy HH:mm}", listItem[i].Created) %></td>
                                            <td class="text-center hidden-sm hidden-col"><%= listItem[i].IP%></td>
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

        var VSWController = 'ModOrder';

        var VSWArrVar = [
            'filter_lang', 'LangID',
            'limit', 'PageSize',
            'filter_status', 'StatusID'
        ];

        var VSWArrVar_QS = [
            'filter_search', 'SearchText',
            'filter_fromdate', 'FromDate',
            'filter_todate', 'ToDate'
        ];

        var VSWArrQT = [
            '<%= model.PageIndex + 1 %>', 'PageIndex',
            '<%= model.Sort %>', 'Sort'
        ];

        var VSWArrDefault = [
            '1', 'PageIndex',
            '1', 'LangID',
            '20', 'PageSize'
        ];
    </script>

</form>
