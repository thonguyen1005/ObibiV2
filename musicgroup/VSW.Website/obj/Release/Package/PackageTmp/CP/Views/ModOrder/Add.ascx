<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModOrderModel;
    var item = ViewBag.Data as ModOrderEntity;
    var list = ViewBag.listItem as List<ModOrderDetailEntity>;
    var listItem = item.GetOrderDetail();
    if (list != null && item.ID < 1)
    {
        listItem = list;
    }

    string html = !string.IsNullOrEmpty(item.Name) ? item.Name + "\n" : "";
    html += !string.IsNullOrEmpty(item.Email) ? item.Email + "\n" : "";
    html += !string.IsNullOrEmpty(item.Phone) ? item.Phone + "\n" : "";
    html += !string.IsNullOrEmpty(item.Address) ? item.Address + "\n" : "";
    html += !string.IsNullOrEmpty(item.Content) ? item.Content + "\n\n" : "";
    var listShowRoom = ModAddressService.Instance.GetAllCacheAll();
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="ChangeStatus" name="ChangeStatus" value="<%=model.ChangeStatus %>" />
    <input type="hidden" id="ShowroomOld" name="ShowroomOld" value="<%=model.ShowroomOld %>" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Đơn hàng <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
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
                <div class="form-horizontal form-row-seperated">
                    <div class="portlet">
                        <div class="portlet-title">
                            <div class="caption"></div>
                            <div class="actions btn-set">
                                <%--<button type="button" class="btn default" onclick="javascript:window.history.back()" data-toggle="tooltip" data-placement="bottom" data-original-title="Trở lại trang trước"><i class="fa fa-angle-left"></i>&nbsp;Back</button>--%>
                                <button type="button" class="btn blue" onclick="vsw_exec_cmd('apply')" data-toggle="tooltip" data-placement="bottom" data-original-title="Lưu"><i class="fa fa-check"></i>&nbsp;Lưu</button>
                                <button type="button" class="btn blue" onclick="vsw_exec_cmd('save')" data-toggle="tooltip" data-placement="bottom" data-original-title="Lưu  &amp; đóng"><i class="fa fa-save"></i>&nbsp;Lưu  &amp; đóng</button>
                                <button type="button" class="btn default" onclick="vsw_exec_cmd('cancel')" data-toggle="tooltip" data-placement="bottom" data-original-title="Đóng"><i class="fa fa-ban"></i>&nbsp;Đóng</button>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="row">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-6">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Khách hàng</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Họ và tên:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Name" value="<%=item.Name %>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Điện thoại:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Phone" value="<%=item.Phone %>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Email:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Email" value="<%=item.Email %>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Địa chỉ:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Address" value="<%=item.Address %>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tỉnh/Thành phố:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" value="<%=GetName(item.GetCity()) + " - " + GetName(item.GetDistrict()) + " - " + GetName(item.GetWard())%>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <%if (item.AddressMore)
                                                    { %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Địa chỉ giao hàng:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Address2" value="<%=item.Address2 %>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tỉnh/Thành phố:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" value="<%=GetName(item.GetCity2()) + " - " + GetName(item.GetDistrict2()) + " - " + GetName(item.GetWard2())%>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <%} %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ghi chú:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control" rows="5" name="Content" readonly="readonly"><%=item.Content%></textarea>
                                                    </div>
                                                </div>
                                                <%if (item.WebUserID > 0)
                                                    { %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 text-right">Tài khoản:</label>
                                                    <div class="col-md-9">
                                                        <b><%=item.GetWebUser() != null ? item.GetWebUser().Name : "" %></b>
                                                    </div>
                                                </div>
                                                <%} %>
                                                <%if (item.Invoice)
                                                    { %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 text-right"></label>
                                                    <div class="col-md-9">
                                                        <b>Thông tin xuất hóa đơn</b>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 text-right">Tên công ty:</label>
                                                    <div class="col-md-9">
                                                        <b><%=item.CompanyName %></b>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 text-right">Địa chỉ công ty:</label>
                                                    <div class="col-md-9">
                                                        <b><%=item.CompanyAddress %></b>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 text-right">Mã số thuế công ty:</label>
                                                    <div class="col-md-9">
                                                        <b><%=item.CompanyTax %></b>
                                                    </div>
                                                </div>
                                                <%} %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12 col-sm-12 col-md-12 col-lg-6">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Đơn hàng</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Trạng thái:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-select select2" name="StatusID">
                                                            <option value="0">Root</option>
                                                            <%= Utils.ShowDdlMenuByType2("Status", model.LangID, item.StatusID)%>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mã đơn hàng:</label>
                                                    <div class="col-md-9">
                                                        <%=item.Code %>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Phí đơn hàng:</label>
                                                    <div class="col-md-9">
                                                        <%= string.Format("{0:#,##0}", item.Total)%>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Phí vận chuyển:</label>
                                                    <div class="col-md-9">
                                                        <%= string.Format("{0:#,##0}", item.Fee)%>
                                                    </div>
                                                </div>
                                                <%if (item.SaleMoney > 0)
                                                    { %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Giảm giá:</label>
                                                    <div class="col-md-9">
                                                        Mã: <%=item.SaleCode %> - <%= string.Format("{0:#,##0}", item.SaleMoney)%>
                                                    </div>
                                                </div>
                                                <%} %>
                                                <%if (item.SaleCustomer > 0)
                                                    { %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Giảm giá theo khách hàng:</label>
                                                    <div class="col-md-9">
                                                        <%= string.Format("{0:#,##0}", item.SaleCustomer)%>
                                                    </div>
                                                </div>
                                                <%} %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tổng tiền:</label>
                                                    <div class="col-md-9">
                                                        <b><%= string.Format("{0:#,##0}", item.Total + item.Fee - item.SaleMoney - item.SaleCustomer - item.SaleMoneyPoint)%></b>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">IP:</label>
                                                    <div class="col-md-9">
                                                        <%=item.IP %>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ngày mua:</label>
                                                    <div class="col-md-9">
                                                        <%=string.Format("{0:HH:mm - dd/MM/yyyy}", item.Created) %>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Hình thức TT:</label>
                                                    <div class="col-md-9">
                                                        <%= Utils.GetNameByConfigkey("Mod.Payment", item.Payment)%>
                                                    </div>
                                                </div>
                                                <%if (!string.IsNullOrEmpty(item.BankPay))
                                                    { %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 text-right">Ngân hàng TT:</label>
                                                    <div class="col-md-9">
                                                        <b><%=item.BankPay %></b>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 text-right">Trạng thái TT:</label>
                                                    <div class="col-md-9">
                                                        <b style="<%= item.StatusPay ? "color:#22e020": "color:red;"%>"><%=item.StatusPay ? "Đã thanh toán" : "Chưa thanh toán" %></b>
                                                    </div>
                                                </div>
                                                <%} %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Nhận hàng:</label>
                                                    <div class="col-md-9">
                                                        <%= Utils.GetNameByConfigkey("Mod.Receive", item.Receive)%>
                                                    </div>
                                                </div>
                                                <%if (item.ReceiveValue > 0)
                                                    { %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 text-right">Showroom:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-control" name="ReceiveValue" id="ReceiveValue">
                                                            <%for (int i = 0; listShowRoom != null && i < listShowRoom.Count; i++)
                                                                {
                                                            %>
                                                            <option <%if (item.ReceiveValue == listShowRoom[i].ID || (item.ReceiveValue <= 0 && i == 0))
                                                                {%>
                                                                selected="selected" <%}%> value="<%=listShowRoom[i].ID %>"><%=listShowRoom[i].Name %></option>
                                                            <%} %>
                                                        </select>
                                                    </div>
                                                </div>
                                                <%} %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Sản phẩm</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="dataTables_wrapper">
                                                <div class="table-scrollable">
                                                    <table class="table table-striped table-hover table-bordered dataTable">
                                                        <thead>
                                                            <tr>
                                                                <th class="sorting text-center w1p">#</th>
                                                                <th class="sorting"><%= GetSortUnLink("Tên sản phẩm", "Name")%></th>
                                                                <th class="sorting text-center w10p desktop"><%= GetSortUnLink("Ảnh", "File")%></th>
                                                                <th class="sorting text-center w10p desktop">Ưu đãi</th>
                                                                <th class="sorting text-center w10p desktop"><%= GetSortUnLink("Giá bán", "Price")%></th>
                                                                <th class="sorting text-center w10p desktop"><%= GetSortUnLink("Số lượng", "Quantity")%></th>
                                                                <th class="sorting text-center w10p desktop"><%= GetSortUnLink("Thành tiền", "PriceAll")%></th>
                                                                <th class="sorting text-center w1p">#</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <%
                                                                for (var i = 0; listItem != null && i < listItem.Count; i++)
                                                                {
                                                                    var product = ModProductService.Instance.GetByID(listItem[i].ProductID);
                                                                    if (product == null) continue;

                                                                    html += (i + 1) + ". " + product.Name + " - Số lượng: " + listItem[i].Quantity + "\n";
                                                                    var color = ModProductClassifyDetailService.Instance.CreateQuery().Where(o => o.ID == listItem[i].ColorID).ToSingle_Cache();
                                                                    var size = ModProductClassifyDetailService.Instance.CreateQuery().Where(o => o.ID == listItem[i].SizeID).ToSingle_Cache();
                                                            %>
                                                            <tr>
                                                                <td class="text-center"><%= i + 1%></td>
                                                                <td>
                                                                    <a href="/{CPPath}/ModProduct/Add.aspx/RecordID/<%= product.ID %>" target="_blank"><%= product.Name%></a>
                                                                    <ul class="list-quatang">
                                                                        <%if (color != null)
                                                                            {
                                                                        %>
                                                                        <li>-&nbsp;<%=color.GetParent().Name %> : <%=color.Name %> </li>
                                                                        <%} %>
                                                                        <%if (size != null)
                                                                            {
                                                                        %>
                                                                        <li>-&nbsp;<%=size.GetParent().Name %> : <%=size.Name %> </li>
                                                                        <%} %>
                                                                    </ul>
                                                                </td>
                                                                <td class="text-center desktop">
                                                                    <%= Utils.GetMedia(product.File, 40, 40)%>
                                                                </td>
                                                                <td class="text-center desktop">
                                                                    <%=listItem[i].Other %>
                                                                </td>
                                                                <td class="text-center desktop"><%= string.Format("{0:#,##0}", listItem[i].Price) %></td>
                                                                <td class="text-center desktop"><%= string.Format("{0:#,##0}", listItem[i].Quantity) %></td>
                                                                <td class="text-center desktop"><%= string.Format("{0:#,##0}", listItem[i].PriceAll <= 0 ?  (listItem[i].Price * listItem[i].Quantity) : listItem[i].PriceAll) %></td>
                                                                <td class="text-center desktop"><%= listItem[i].ID%></td>
                                                            </tr>
                                                            <%} %>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 col-sm-12 justify-content-center d-flex">
                                                        <span class="help-block text-primary">Tổng tiền: <%=string.Format("{0:#,##0}", item.Total) %> VNĐ</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Tiện ích</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Nội dung:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control" rows="10" id="html"><%=html%></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">&nbsp;</label>
                                                    <div class="col-md-9">
                                                        <button type="button" class="btn btn-primary" onclick="copyToClipboard('#html'); return false">Copy</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</form>
<%--<%if (item.ID > 0)
    { %>
<script type="text/javascript">
    $(document).ready(function () {
        get_child('<%=item.CityID%>', '<%=item.DistrictID%>', '.list-district');
        get_child('<%=item.DistrictID%>', '<%=item.WardID%>', '.list-ward');
    });
</script>
<%} %>--%>