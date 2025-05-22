<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModProductModel;
    var listItem = ViewBag.Data as List<ModProductEntity>;
%>
<form id="vswForm" name="vswForm" method="post">
    <div class="page-content-wrapper">
        <h3 class="page-title">Sản phẩm</h3>
        <div class="row">
            <div class="col-md-12">
                <%= ShowMessage()%>
                <div class="portlet portlet">
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
                            <div class="table-scrollable">
                                <table class="table table-striped table-hover table-bordered dataTable">
                                    <thead>
                                        <tr>
                                            <th class="sorting text-center w1p">#</th>
                                            <th class="sorting"><%= GetSortLink("Tiêu đề", "Name")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Ảnh", "File")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Giá bán", "Price")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Giá khuyến mại", "PricePromotion")%></th>
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
                                            <td>
                                                <a href="/CP/ModProduct/Add.aspx/<%= listItem[i].ID %>"><%= listItem[i].Name%></a>
                                                <p class="smallsub desktop">(<span>Sku</span>: <%= listItem[i].Model%>)</p>
                                            </td>
                                            <td class="text-center desktop">
                                                <%= Utils.GetMedia(listItem[i].File, 40, 40)%>
                                            </td>
                                            <td class="text-center desktop"><%= string.Format("{0:#,##0}", listItem[i].Price) %></td>
                                            <td class="text-center desktop"><%= string.Format("{0:#,##0}", listItem[i].PricePromotion) %></td>
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

    var VSWController = "FormProductGroup";

    var VSWArrVar = [
        "filter_lang", "LangID",
        "limit", "PageSize"
    ];

    var VSWArrVar_QS = [
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
