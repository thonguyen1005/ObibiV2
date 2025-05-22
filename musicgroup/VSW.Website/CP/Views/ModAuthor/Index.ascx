<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModAuthorModel;
    var listItem = ViewBag.Data as List<ModAuthorEntity>;
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Tác giả</h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Tác giả</a>
                
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
                        <div class="actions btn-set justify-content-end">
                            <%=GetDefaultListCommandV2()%>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Từ khóa: </label>
                                        <input type="text" class="form-control" id="filter_search" value="<%= model.SearchText %>" placeholder="Nhập từ khóa cần tìm" onchange="VSWRedirect();return false;" />
                                    </div>
                                </div>
                            </div>
                            <div class="portlet portlet mt-2 hide" id="boxActionHide">
                                <div class="portlet-title">
                                    <div class="actions btn-set">
                                        <span class="des mr-2">Đã chọn: <span id="countChose">0</span></span>
                                        <%=GetSortListHideCommandV2()%>
                                    </div>
                                </div>
                            </div>
                            <div class="table-scrollable">
                                <table class="table table-striped table-hover table-bordered dataTable">
                                    <thead>
                                        <tr>
                                            <th class="sorting text-center w1p desktop">#</th>
                                            <th class="sorting_disabled text-center w1p">
                                                <label class="itemCheckBox itemCheckBox-sm">
                                                    <input type="checkbox" name="toggle" onclick="checkAll(<%= model.PageSize %>)">
                                                    <i class="check-box"></i>
                                                </label>
                                            </th>
                                            <th class="sorting"><%= GetSortLink("Tên tác giả", "Name")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Ảnh", "File")%></th>
                                            <th class="sorting text-center w70p desktop">Thông tin</th>
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
                                            <td class="text-center">
                                                <%= Utils.GetMedia(listItem[i].File, 40, 40)%>
                                            </td>
                                            <td>
                                                <%=listItem[i].Position %>
                                                <%if (!string.IsNullOrEmpty(listItem[i].Facebook))
                                                    { %>
                                                <a href="<%=listItem[i].Facebook %>" target="_blank" class="fz-20"><i class="fa fa-facebook"></i></a>
                                                <%} %>
                                                <%if (!string.IsNullOrEmpty(listItem[i].Zalo))
                                                    { %>
                                                <a href="<%=listItem[i].Zalo %>" target="_blank" class="fz-20">
                                                    <img src="/Content/skins/img/zalo.png" alt="zalo" style="width: 27px; vertical-align: top;" /></a>
                                                <%} %>
                                                <%if (!string.IsNullOrEmpty(listItem[i].Tiktok))
                                                    { %>
                                                <a href="<%=listItem[i].Tiktok %>" target="_blank" class="fz-20">
                                                    <img src="/Content/skins/img/tiktok.png" alt="tiktok" style="width: 27px; vertical-align: top;" /></a>
                                                <%} %>
                                                <%if (!string.IsNullOrEmpty(listItem[i].Youtube))
                                                    { %>
                                                <a href="<%=listItem[i].Youtube %>" target="_blank" class="fz-20"><i class="fa fa-youtube"></i></a>
                                                <%} %>
                                            </td>
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

        var VSWController = "ModAuthor";

        var VSWArrVar = [
            "filter_menu", "MenuID",
            "filter_state", "State",
            "filter_lang", "LangID",
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
