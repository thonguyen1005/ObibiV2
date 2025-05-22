<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as SysSiteModel;
    var listItem = ViewBag.Data as List<SysSiteEntity>;
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />
    <div class="page-content-wrapper">
        <h3 class="page-title">Site</h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Site</a>
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
                            <div class="portlet portlet mt-2 hide" id="boxActionHide">
                                <div class="portlet-title">
                                    <div class="actions btn-set" style="float: left">
                                        <span class="mr-2">Đã chọn: <span id="countChose">0</span></span>
                                        <%=GetSortListHideCommandV2()%>
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
                                            <th class="sorting"><%= GetSortLink("Tên site", "Name")%></th>
                                            <th class="sorting text-center"><%= GetSortLink("Mã", "Code")%></th>
                                            <th class="sorting text-center"><%= GetSortLink("Ngôn ngữ", "LangID")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Mặc định", "Default")%></th>
                                            <th class="sorting text-center w10p desktop">
                                                <%= GetSortLink("Sắp xếp", "Order")%>
                                                <a href="javascript:vsw_exec_cmd('saveorder')" class="saveorder" data-toggle="tooltip" data-placement="bottom" data-original-title="Lưu sắp xếp"><i class="fa fa-save"></i></a>
                                            </th>
                                            <th class="sorting text-center w1p"><%= GetSortLink("ID", "ID")%></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%for (var i = 0; listItem != null && i < listItem.Count; i++){ %>
                                        <tr>
                                            <td class="text-center"><%= i + 1%></td>
                                            <td class="text-center">
                                                <%= GetCheckbox(listItem[i].ID, i)%>
                                            </td>
                                            <td>
                                                <a href="javascript:VSWRedirect('Add', <%= listItem[i].ID %>)"><%= listItem[i].Name%></a>
                                            </td>
                                            <td class="text-center"><%=listItem[i].Code %></td>
                                            <td class="text-center"><%= GetName(SysLangService.Instance.GetByID(listItem[i].LangID)) %></td>
                                            <td class="text-center hidden-sm hidden-col">
                                                <%= GetDefault(listItem[i].ID, listItem[i].Default)%>
                                            </td>
                                            <td class="text-center hidden-sm hidden-col">
                                                <%= GetOrder(listItem[i].ID, listItem[i].Order)%>
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

        var VSWController = "SysSite";

        var VSWArrVar = [
            "limit", "PageSize"
        ];

        var VSWArrQT = [
            "<%= model.PageIndex + 1 %>", "PageIndex",
            "<%= model.Sort %>", "Sort"
        ];

        var VSWArrDefault = [
            "1", "PageIndex",
            "20", "PageSize"
        ];
    </script>

</form>