<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModTagModel;
    var listItem = ViewBag.Data as List<ModTagEntity>;
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

  <div class="page-content-wrapper">
        <h3 class="page-title">Tags</h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Tags</a>
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
                            <%=GetTinyListCommand()%>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
                            <div class="row hidden-sm hidden-col">
                                <div class="col-12 col-sm-12 col-md-4 col-lg-4">
                                    <div class="dataTables_search">
                                        <input type="text" class="form-control input-inline input-sm" id="filter_search" value="<%= model.SearchText %>" placeholder="Nhập từ khóa cần tìm" onchange="VSWRedirect();return false;" />
                                        <button type="button" class="btn blue" onclick="VSWRedirect();return false;">Tìm kiếm</button>
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
                                            <th class="sorting"><%= GetSortLink("Tên", "Name")%></th>
                                            <th class="sorting"><%= GetSortLink("Link", "Link")%></th>
                                            <th class="sorting text-center hidden-sm hidden-col" style="width:20%;"><%= GetSortLink("[SEO] Tiêu đề trang", "Title")%></th>
                                            <th class="sorting text-center hidden-sm hidden-col" style="width:20%;"><%= GetSortLink("[SEO] Mô tả trang", "Description")%></th>
                                            <th class="sorting text-center" style="width:20%;"><%= GetSortLink("[SEO] Từ khóa trang", "Keywords")%></th>
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
                                            <td><a href="<%= listItem[i].Link%>" target="_blank"><%= listItem[i].Link%></a></td>
                                            <td class="text-center hidden-sm hidden-col"><%= listItem[i].Title%></td>
                                            <td class="text-center hidden-sm hidden-col"><%= listItem[i].Description%></td>
                                            <td class="text-center hidden-sm hidden-col"><%= listItem[i].Keywords%></td>                                            
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

            var VSWController = 'ModTag';

            var VSWArrVar = [
                "filter_lang", "LangID",
                'limit', 'PageSize'
            ];


            var VSWArrVar_QS = [
                'filter_search', 'SearchText'
            ];

            var VSWArrQT = [
                          '<%= model.PageIndex + 1 %>', 'PageIndex', 
                      '<%= model.Sort %>', 'Sort'
            ];

            var VSWArrDefault =
                [
                    '1', 'PageIndex',
                    '20', 'PageSize'
                ];
    </script>
</form>