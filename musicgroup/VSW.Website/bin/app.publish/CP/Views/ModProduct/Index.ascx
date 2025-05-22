<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModProductModel;
    var listItem = ViewBag.Data as List<ModProductEntity>;
%>
<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Sản phẩm</h3>
        <div class="row">
            <div class="col-md-12">
                <%= ShowMessage()%>

                <div class="portlet portlet">
                    <div class="row portlet-title">
                        <div class="col-md-8">
                            <div class="tabbable-line transparent" style="float: left">
                                <ul class="nav-tabs clear" style="border: none">
                                    <li class="<%= model.Status < 1 ? "active" : "" %>">
                                        <a href="/{CPPath}/ModProduct/Index.aspx">Tất cả</a>
                                    </li>
                                    <li class="<%= model.Status == 1 ? "active" : "" %>">
                                        <a href="/{CPPath}/ModProduct/Index.aspx?Status=1">Đang bán</a>
                                    </li>
                                    <li class="<%= model.Status == 2 ? "active" : "" %>">
                                        <a href="/{CPPath}/ModProduct/Index.aspx?Status=2">Dừng hoạt động</a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="col-md-4 actions btn-set justify-content-end">
                            <button type="button" class="btn btn-primary" onclick="VSWRedirect('Add')" data-toggle="tooltip" data-placement="bottom" data-original-title="Thêm mới"><i class="fa fa-plus mr-1"></i>Thêm mới</button>
                            <button type="button" class="btn purple" onclick="vsw_exec_cmd('config')" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa cache"><i class="fa fa-undo mr-1"></i>Xóa cache</button>

                        </div>
                    </div>
                    <div class="card card-search">
                        <div class="card-header header-elements-inline">
                            <p class="card-title font-size-s">
                                <i style="width: 26px; height: 26px; line-height: 26px; font-size: 12px;" class="icon-search4 d-flex-inline align-items-center justify-content-center rounded-round bg-color mr-2"></i>
                                Tìm kiếm      
                            </p>
                            <div class="header-elements">
                                <div class="list-icons">
                                    <a class="list-icons-item" data-action="collapse"></a>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Từ khóa: </label>
                                        <input type="text" class="form-control" id="filter_search" value="<%= model.SearchText %>" placeholder="Nhập từ khóa cần tìm" onchange="VSWRedirect();return false;" />
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Chuyên mục</label>
                                        <select id="filter_menu" class="form-select select2" onchange="VSWRedirect()" size="1">
                                            <option value="0">(Tất cả)</option>
                                            <%= Utils.ShowDdlMenuByType("Product", model.LangID, model.MenuID)%>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Thương hiệu</label>
                                        <select id="filter_brand" class="form-control select2" onchange="VSWRedirect()" size="1">
                                            <option value="0">(Tất cả)</option>
                                            <%= Utils.ShowDdlMenuByType("Brand", model.LangID, model.BrandID)%>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Vị trí</label>
                                        <select id="filter_state" class="form-select select2" onchange="VSWRedirect()" size="1">
                                            <option value="0">(Tất cả)</option>
                                            <%= Utils.ShowDdlByConfigkey("Mod.ProductState", model.State)%>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- <div class="portlet-title">
                        <div class="actions btn-set">
                            <input class="form-control input-inline input-sm" type="text" name="Excel" id="Excel" value="<%=model.Excel %>" style="width: 200px; display: inline-block;" maxlength="255" />
                            <input class="form-control input-inline input-sm" style="width: 100px; display: inline-block;" type="button" onclick="ShowFileForm('Excel'); return false;" value="Chọn File" />
                            <button type="button" style="padding: 4px 10px; font-size: 13px; line-height: 1.5; border-radius: 0; cursor: pointer; margin: 0 0 0 5px;" class="btn blue" onclick="vsw_exec_cmd('import')" data-toggle="tooltip" data-placement="bottom" data-original-title="Update Price Excel"><i class="fa fa-plus-circle"></i>Cập nhật sản phẩm</button>
                        </div>
                    </div>--%>
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
                            <div class="portlet portlet mt-2 hide" id="boxActionHide">
                                <div class="portlet-title">
                                    <div class="actions btn-set">
                                        <span class="des mr-2">Đã chọn: <span id="countChose">0</span></span>
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
                                            <th class="sorting"><%= GetSortLink("Tên sản phẩm", "Name")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Ảnh", "File")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Giá bán", "Price")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Chuyên mục", "MenuID")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Vị trí", "State")%></th>
                                            <th class="sorting text-center w1p"><%= GetSortLink("Duyệt", "Activity")%></th>
                                            <th class="sorting text-center w10p desktop">
                                                <%= GetSortLink("Sắp xếp", "Order")%>
                                                <a href="javascript:vsw_exec_cmd('saveorder')" class="saveorder" data-toggle="tooltip" data-placement="bottom" data-original-title="Lưu sắp xếp"><i class="fa fa-save"></i></a>
                                            </th>
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
                                                <p class="smallsub desktop">(<span>Mã</span>: <%= listItem[i].Code%>) - <a href="/<%=listItem[i].Code + Setting.Sys_PageExt %>" style="color: red;" target="_blank">Xem website</a></p>
                                            </td>
                                            <td class="text-center desktop">
                                                <%= Utils.GetMedia(listItem[i].File, 40, 40)%>
                                            </td>
                                            <td class="text-center desktop"><%= string.Format("{0:#,##0}", listItem[i].Price) %></td>
                                            <td class="text-center desktop">
                                                <%= GetName(listItem[i].GetMenu()) %>
                                            </td>
                                            <td class="text-center desktop"><%= Utils.ShowNameByConfigkey2("Mod.ProductState", listItem[i].State)%></td>
                                            <td class="text-center">
                                                <%= GetPublish(listItem[i].ID, listItem[i].Activity)%>
                                            </td>
                                            <td class="text-center desktop">
                                                <%= GetOrder(listItem[i].ID, listItem[i].Order)%>
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

        var VSWController = "ModProduct";

        var VSWArrVar = [
            "filter_menu", "MenuID",
            "filter_group_menu", "GroupMenuID",
            "filter_brand", "BrandID",
            "filter_user", "UserID",
            "filter_state", "State",
            "filter_lang", "LangID",
            "limit", "PageSize",
            "filter_status", "Status2"
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
