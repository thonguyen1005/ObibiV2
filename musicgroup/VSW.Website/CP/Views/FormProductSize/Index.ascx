<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModProductSizeModel;
    var listItem = ViewBag.Data as List<ModProductSizeEntity>;
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Kích cỡ và màu sắc</h3>
        <div class="row">
            <div class="col-md-12">
                <%= ShowMessage()%>

                <div class="portlet portlet">
                    <div class="portlet-title">
                        <div class="actions btn-set">
                            <%=GetDefaultListCommand("saveprice|Cập nhật")%>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
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
                                            <th class="sorting text-center"><%= GetSortLink("Kích cỡ", "SizeID")%></th>
                                            <th class="sorting text-center"><%= GetSortLink("Màu sắc", "ColorID")%></th>
                                            <th class="sorting text-center" style="width: 8%"><%= GetSortLink("Cân nặng(gram)", "Weight")%></th>
                                            <th class="sorting text-center" style="width: 11%"><%= GetSortLink("Giá bán", "Price")%></th>
                                            <th class="sorting text-center" style="width: 11%"><%= GetSortLink("Giá thị trường", "Price2")%></th>
                                            <th class="sorting text-center" style="width: 11%"><%= GetSortLink("Giá khuyến mại", "PricePromotion")%></th>
                                            <th class="sorting text-center" style="width:8%;">Hình ảnh</th>
                                            <th class="sorting text-center w1p"><%= GetSortLink("Duyệt", "Activity")%></th>
                                            <th class="sorting text-center w10p hidden-sm hidden-col">
                                                <%= GetSortLink("Sắp xếp", "Order")%>
                                                <a href="javascript:vsw_exec_cmd('saveorder')" class="saveorder" data-toggle="tooltip" data-placement="bottom" data-original-title="Lưu sắp xếp"><i class="fa fa-save"></i></a>
                                            </th>
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
                                                <a href="javascript:VSWRedirect('Add', <%= listItem[i].ID %>)"><%= GetName(listItem[i].getSize())%></a>
                                            </td>
                                            <td>
                                                <a href="javascript:VSWRedirect('Add', <%= listItem[i].ID %>)"><%= GetName(listItem[i].getColor())%></a>
                                            </td>
                                            <td class="text-center">
                                                <input type="number" class="form-control" onchange="$('#cb<%=i %>').prop('checked', true);" name="Weight<%= listItem[i].ID %>" value="<%= listItem[i].Weight %>" />
                                            </td>
                                            <td class="text-center">
                                                <input type="number" class="form-control" onchange="$('#cb<%=i %>').prop('checked', true);" name="Price<%= listItem[i].ID %>" value="<%= listItem[i].Price %>" />
                                            </td>
                                            <td class="text-center">
                                                <input type="number" class="form-control" onchange="$('#cb<%=i %>').prop('checked', true);" name="Price2<%= listItem[i].ID %>" value="<%= listItem[i].Price2 %>" />
                                            </td>
                                            <td class="text-center">
                                                <input type="number" class="form-control" onchange="$('#cb<%=i %>').prop('checked', true);" name="PricePromotion<%= listItem[i].ID %>" value="<%= listItem[i].PricePromotion %>" />
                                            </td>
                                            <td class="text-center">
                                                <a href="javascript:void(0)" onclick="ShowProductSizeImageForm('<%= listItem[i].ID %>','<%= listItem[i].ProductID %>'); return false"><i class="fa fa-search"></i></a>
                                            </td>
                                            <td class="text-center">
                                                <%= GetPublish(listItem[i].ID, listItem[i].Activity)%>
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
        var VSWController = "FormProductSize";

        var VSWArrVar = [
            "limit", "PageSize"
        ];

        var VSWArrVar_QS = [
        ];

        var VSWArrQT = [
            "<%= model.PageIndex + 1 %>", "PageIndex",
            "<%= model.ProductID %>", "ProductID",
            "<%= model.Sort %>", "Sort"
        ];

        var VSWArrDefault = [
            "1", "PageIndex",
            "1", "LangID",
            "20", "PageSize"
        ];
    </script>

</form>
