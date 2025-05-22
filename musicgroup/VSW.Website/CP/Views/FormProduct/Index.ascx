<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModProductModel;
    var listItem = ViewBag.Data as List<ModProductEntity>;
%>
<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />
    <input type="hidden" name="Type" id="Type" value="<%=model.Type %>" />
    <div class="page-content-wrapper">
        <h3 class="page-title">Sản phẩm</h3>

        <div class="row">
            <div class="col-md-12">
                <%= ShowMessage()%>

                <div class="portlet portlet">
                    <div class="portlet-title">
                        <div class="actions btn-set">
                            <button type="button" class="btn blue" onclick="if(document.vswForm.boxchecked.value>0){CloseProductMulti('<%=model.Type %>');}else{showSwalWarning('Bạn phải chọn sản phẩm.');}" data-toggle="tooltip" data-placement="bottom" data-original-title="Chọn"><i class="fa fa-check-circle mr-1"></i>Chọn</button>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
                            <div class="row desktop">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="dataTables_search">
                                        <input type="text" class="form-control input-inline input-sm" id="filter_search" value="<%= model.SearchText %>" placeholder="Nhập từ khóa cần tìm" onchange="VSWRedirect();return false;" />
                                        <div class="table-group-actions d-inline-block">
                                        <label>Chuyên mục</label>
                                        <select id="filter_menu" class="form-control input-inline input-sm" onchange="VSWRedirect()" size="1">
                                            <option value="0">(Tất cả)</option>
                                            <%= Utils.ShowDdlMenuByType("Product", model.LangID, model.MenuID)%>
                                        </select>
                                    </div>
                                    <div class="table-group-actions d-inline-block">
                                        <label>Thương hiệu</label>
                                        <select id="filter_brand" class="form-control input-inline input-sm" onchange="VSWRedirect()" size="1">
                                            <option value="0">(Tất cả)</option>
                                            <%= Utils.ShowDdlMenuByType2("Brand", model.LangID, model.BrandID)%>
                                        </select>
                                    </div>
                                    <div class="table-group-actions d-inline-block">
                                        <label>Vị trí</label>
                                        <select id="filter_state" class="form-control input-inline input-sm" onchange="VSWRedirect()" size="1">
                                            <option value="0">(Tất cả)</option>
                                            <%= Utils.ShowDdlByConfigkey("Mod.ProductState", model.State)%>
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
                                            <th class="sorting text-center w1p">
                                                <label class="itemCheckBox itemCheckBox-sm">
                                                    <input type="checkbox" name="toggle" onclick="checkAll(<%= model.PageSize %>)">
                                                    <i class="check-box"></i>
                                                </label>
                                            </th>
                                            <th class="sorting text-center w1p">Chọn</th>
                                            <th class="sorting"><%= GetSortLink("Tiêu đề", "Name")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Ảnh", "File")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Chuyên mục", "MenuID")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Ngày đăng", "Published")%></th>
                                            <th class="sorting text-center w1p desktop"><%= GetSortLink("Duyệt", "Activity")%></th>
                                            <th class="sorting text-center w1p"><%= GetSortLink("ID", "ID")%></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%for (var i = 0; listItem != null && i < listItem.Count; i++)
                                            {
                                        %>
                                        <tr>
                                            <td class="text-center"><%= i + 1%></td>
                                            <td class="text-center">
                                                <%= GetCheckbox(listItem[i].ID, i)%>
                                            </td>
                                            <td class="text-center"><a href="javascript:CloseProduct(<%= listItem[i].ID %>, '<%=model.Type %>')">Chọn</a></td>
                                            <td>
                                                <a href="javascript:CloseProduct(<%= listItem[i].ID %>, '<%=model.Type %>')"><%= listItem[i].Name%></a>
                                                <p class="smallsub desktop">(<span>Sku</span>: <%= listItem[i].Model%>)</p>
                                            </td>
                                            <td class="text-center desktop">
                                                <%= Utils.GetMedia(listItem[i].File, 40, 40)%>
                                            </td>
                                            <td class="text-center desktop"><%= GetName(listItem[i].GetMenu()) %></td>
                                            <td class="text-center desktop"><%= string.Format("{0:dd-MM-yyyy HH:mm}", listItem[i].Published) %></td>
                                            <td class="text-center desktop">
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
</form>
<script type="text/javascript">

    var VSWController = "FormProduct";

    var VSWArrVar = [
        "filter_menu", "MenuID",
        "filter_brand", "BrandID",
        "filter_state", "State",
        "filter_lang", "LangID",
        "limit", "PageSize"
    ];

    var VSWArrVar_QS = [
        "filter_search", "SearchText"
    ];

    var VSWArrQT = [
        "<%= model.PageIndex + 1 %>", "PageIndex",
        "<%= model.Type %>", "Type",
        "<%= model.Sort %>", "Sort"
    ];

    var VSWArrDefault = [
        "1", "PageIndex",
        "1", "LangID",
        "20", "PageSize"
    ];
</script>
