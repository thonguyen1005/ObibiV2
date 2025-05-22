<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModProductVoteModel;
    var listItem = ViewBag.Data as List<ModProductVoteEntity>;
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Câu hỏi</h3>
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
                                            <th class="sorting text-center"><%= GetSortLink("Tiêu đề", "Name")%></th>
                                            <th class="sorting text-center" style="width: 20%"><%= GetSortLink("Tiêu đề Có", "NameYes")%></th>
                                            <th class="sorting text-center" style="width: 20%"><%= GetSortLink("Tiêu đề Không", "NameNo")%></th>
                                            <th class="sorting text-center" style="width: 8%"><%= GetSortLink("Có", "Yes")%></th>
                                            <th class="sorting text-center" style="width: 8%"><%= GetSortLink("Không", "No")%></th>
                                            <%--<th class="sorting text-center" style="width: 10%"><%= GetSortLink("Hiển thị", "TypeView")%></th>--%>
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
                                                <a href="javascript:VSWRedirect('Add', <%= listItem[i].ID %>)">
                                                    <%if (!string.IsNullOrEmpty(listItem[i].Name))
                                                        { %>
                                                    <%= listItem[i].Name%>
                                                    <%}
                                                        else
                                                        { %>
                                                    <%= GetName(listItem[i].GetMenu()) %>
                                                    <%} %>
                                                </a>
                                            </td>
                                            <td class="text-center">
                                                <%= listItem[i].NameYes%>
                                            </td>
                                            <td class="text-center">
                                                <%= listItem[i].NameNo%>
                                            </td>
                                            <td class="text-center">
                                                <input type="number" class="form-control" onchange="$('#cb<%=i %>').prop('checked', true);" name="Yes<%= listItem[i].ID %>" value="<%= listItem[i].Yes %>" />
                                            </td>
                                            <td class="text-center">
                                                <input type="number" class="form-control" onchange="$('#cb<%=i %>').prop('checked', true);" name="No<%= listItem[i].ID %>" value="<%= listItem[i].No %>" />
                                            </td>
                                            <%--<td class="text-center">
                                                <%= listItem[i].TypeView == 1 ? "Biểu đồ tròn" : "Biểu đồ đánh giá ngang"%>
                                            </td>--%>
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
        var VSWController = "FormProductVote";

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
