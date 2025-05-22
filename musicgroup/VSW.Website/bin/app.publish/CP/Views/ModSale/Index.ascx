<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModSaleModel;
    var listItem = ViewBag.Data as List<ModSaleEntity>;
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Mã giảm giá</h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Mã giảm giá</a>
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
                            <%=GetDefaultListCommandV2()%>
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
                                        <button type="button" class="btn blue" onclick="VSWRedirect();return false;">Tìm kiếm</button>
                                    </div>
                                </div>
                            </div>
                            <div class="portlet portlet mt-2 hide" id="boxActionHide">
                                <div class="portlet-title">
                                    <div class="actions btn-set" style="float:left">
                                        <span class="mr-2">Đã chọn: <span id="countChose">0</span></span> 
                                        <%=GetDefaultListHideCommandV2()%>
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
                                            <th class="sorting text-center"><%= GetSortLink("Tên chương trình", "Name")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Mã giảm giá", "Code")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Từ ngày", "DateStart")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Đến ngày", "DateEnd")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("% giảm", "Percent")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Số tiền", "Money")%></th>
                                            <th class="sorting text-center desktop"><%= GetSortLink("Số lượng", "Count")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Số lượng SD", "CountUse")%></th>
                                            <th class="sorting text-center w1p"><%= GetSortLink("Duyệt", "Activity")%></th>
                                            <th class="sorting text-center w1p"><%= GetSortLink("ID", "ID")%></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%for (var i = 0; listItem != null && i < listItem.Count; i++)
                                            { %>
                                        <tr>
                                            <td class="text-center"><%= i + 1%></td>
                                            <td class="text-center">
                                                <%= GetCheckbox(listItem[i].ID, i)%>
                                            </td>
                                            <td>
                                                <a href="javascript:VSWRedirect('Add', <%= listItem[i].ID %>)"><%= listItem[i].Name%></a>
                                            </td>
                                            <td>
                                                <a href="javascript:VSWRedirect('Add', <%= listItem[i].ID %>)"><%= listItem[i].Code%></a>
                                            </td>
                                            <td class="text-center">
                                                <%= string.Format("{0:dd-MM-yyyy}", listItem[i].DateStart) %>
                                            </td>
                                            <td class="text-center">
                                                <%= string.Format("{0:dd-MM-yyyy}", listItem[i].DateEnd) %>
                                            </td>
                                            <td class="text-center">
                                                <%=Utils.FormatFloat(listItem[i].Percent) %> %
                                            </td>
                                            <td class="text-center">
                                                <%=Utils.FormatMoney(listItem[i].Money) %> VNĐ
                                            </td>
                                            <td class="text-center">
                                                <%= listItem[i].Count %>
                                            </td>
                                            <td class="text-center">
                                                <%= listItem[i].CountUse %>
                                            </td>
                                            <td class="text-center">
                                                <%= GetPublish(listItem[i].ID, listItem[i].Activity)%>
                                            </td>
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

        var VSWController = "ModSale";

        var VSWArrVar = [
            "limit", "PageSize"
        ];

        var VSWArrVar_QS = [
            "filter_search", "SearchText",
            'filter_fromdate', 'FromDate',
            'filter_todate', 'ToDate'
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


