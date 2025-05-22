<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<%
    var item = ViewBag.Data as ModOrderEntity;
    var model = ViewBag.Model as MViewCartModel;
    var cart = new Cart();
    long total = 0;
    long quantity = 0;
    string address = "";
    string ToCity = "";
    if (!item.AddressMore)
    {
        if (string.IsNullOrEmpty(item.Address))
        {
            if (item.WardID > 0)
            {
                address = item.GetWard().Name;
            }
            if (item.DistrictID > 0)
            {
                address += (!string.IsNullOrEmpty(address) ? ", " : "") + item.GetDistrict().Name;
            }
            if (item.CityID > 0)
            {
                ToCity = item.GetCity().Name;
                address += (!string.IsNullOrEmpty(address) ? ", " : "") + ToCity;
            }
        }
    }
    else
    {
        if (string.IsNullOrEmpty(item.Address2))
        {
            if (item.WardID2 > 0)
            {
                address = item.GetWard2().Name;
            }
            if (item.DistrictID2 > 0)
            {
                address += (!string.IsNullOrEmpty(address) ? ", " : "") + item.GetDistrict2().Name;
            }
            if (item.CityID2 > 0)
            {
                ToCity = item.GetCity2().Name;
                address += (!string.IsNullOrEmpty(address) ? ", " : "") + ToCity;
            }
        }
    }
%>
<div class="container">
    <div class="cart-breadcrumb d-flex align-items-center justify-content-between mb-2">
        <a href="javascript:void(0)" onclick="goURL('<%=model.returnpath %>','')">
            <i class="fa fa-angle-left"></i>Mua thêm sản phẩm khác
        </a>
        <span>Giỏ hàng của bạn</span>
    </div>
    <form method="post" name="checkout_form" id="checkout_form" class="form-list">
        <div class="cart-box">
            <div class="cart-listing">
                <h4 class="p-cart-title pd">Danh sách sản phẩm</h4>
                <ul>
                    <%for (int i = 0; i < cart.Items.Count; i++)
                        {
                            var product = ModProductService.Instance.GetByID(cart.Items[i].ProductID);
                            if (product == null) continue;
                            var size = ModProductClassifyDetailPriceService.Instance.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
                            long price = product.PriceView;
                            if (size != null)
                            {
                                price = size.Price;
                            }
                            long t = cart.Items[i].Quantity * price;
                            total += t;
                            quantity += cart.Items[i].Quantity;
                            string url = ViewPage.GetURL(product.MenuID, product.Code);
                            var listGift = product.GetGift();

                            var listProductClassify = product.GetProductClassify();
                            var listProductClassifyDetail = product.GetProductClassifyDetail();
                            var listProductClassifyDetailPrice = product.GetProductClassifyDetailPrice();
                    %>
                    <li>
                        <div class="p-cart-img">
                            <a href="<%=url %>" title="<%=product.Name %>">
                                <img src="<%=Utils.GetWebPFile(product.File,4,150,150) %>" alt="<%=product.Name %>"
                                    class="img-fluid">
                            </a>
                            <div class="p-cart-remove">
                                <a href="javascript:;" onclick="delete_cart(<%=i %>,'<%=model.returnpath %>')">
                                    <i class="fa fa-times-circle"></i>Xoá
                            </a>
                            </div>
                        </div>
                        <div class="p-cart-info">
                            <div class="p-cart-top">
                                <h3><%=product.Name %> </h3>
                                <div class="p-cart-price">
                                    <%if (price > 0)
                                        { %>
                                    <span><%=Utils.FormatMoney(price) %>₫</span>
                                    <%}
                                        else
                                        { %>
                                    <span>{RS:LienHe}</span>
                                    <%} %>
                                </div>
                            </div>
                            <div class="p-cart-bottom">
                                <%if (listProductClassify != null)
                                    { %>
                                <div class="p-cart-selection">
                                    <%for (int z = 0; z < listProductClassify.Count; z++)
                                        {
                                            var lstItem = listProductClassifyDetail.Where(o => o.ClassifyID == listProductClassify[z].ID).ToList();
                                            string active = "";
                                            if (z == 0 && cart.Items[i].ColorID > 0)
                                            {
                                                var itemActive = lstItem.Where(o => o.ID == cart.Items[i].ColorID).FirstOrDefault();
                                                if (itemActive != null) active = itemActive.Name;
                                            }else if (cart.Items[i].SizeID > 0)
                                            {
                                                var itemActive = lstItem.Where(o => o.ID == cart.Items[i].SizeID).FirstOrDefault();
                                                if (itemActive != null) active = itemActive.Name;
                                            }
                                    %>
                                    <div class="dropdown">
                                        <a class="nav-link dropdown-toggle text-primary" href="javascript:;" id="Classify<%=z %>"
                                            role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                            <%=listProductClassify[z].Name %>:&nbsp;<%=active %>
                                            
                                        </a>
                                        <ul class="dropdown-menu" aria-labelledby="Classify<%=z %>">
                                            <%for (int k = 0; lstItem != null && k < lstItem.Count; k++)
                                                {%>
                                            <%if (z == 0)
                                                { %>
                                            <li><a class="dropdown-item" href="javascript:;" onclick="update_cart(<%=i%>, <%=cart.Items[i].Quantity %>, <%=lstItem[k].ID %>, <%=cart.Items[i].SizeID %>, '<%=model.returnpath %>');"><%=lstItem[k].Name %></a></li>
                                            <%}
                                                else
                                                { %>
                                            <li><a class="dropdown-item" href="javascript:;" onclick="update_cart(<%=i%>, <%=cart.Items[i].Quantity %>, <%=cart.Items[i].ColorID %>, <%=lstItem[k].ID %>, '<%=model.returnpath %>');"><%=lstItem[k].Name %></a></li>
                                            <%} %>
                                            <%} %>
                                        </ul>
                                    </div>
                                    <% } %>
                                </div>
                                <%} %>
                                <div class="d-flex align-items-center">
                                    <div class="counter-container">
                                        <button type="button" class="decrease">
                                            <i class="far fa-minus fz-14"></i>
                                        </button>
                                        <input type="number" value="<%=cart.Items[i].Quantity %>" name="qtybutton" onchange="load_cart(this.value,'<%=i%>')" class="counter-input" readonly="">
                                        <button type="button" class="increase">
                                            <i class="far fa-plus fz-14"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </li>
                    <%} %>
                </ul>
            </div>
            <div class="p-cart-box">
                <div class="infor-customer">
                    <div class="p-hinh-thuc mb-3">
                        <h4 class="p-cart-title">Hình thức nhận hàng</h4>
                        <div class="p-cart-delivery-menthod">
                            <input type="radio" class="btn-check" name="Receive" id="giao-hang-tan-noi"
                                onchange="change_delivery_menthod('giao-hang');" autocomplete="off" <%=(item.Receive == 1 || item.Receive < 1) ? "checked" : "" %> value="1">
                            <label class="btn" for="giao-hang-tan-noi">Giao hàng tận nơi</label>
                            <input type="radio" class="btn-check" name="Receive" id="nhan-tai-cua-hang"
                                onchange="change_delivery_menthod('nhan-hang');" autocomplete="off" <%=item.Receive == 2 ? "checked" : "" %> value="2">
                            <label class="btn" for="nhan-tai-cua-hang">Nhận tại cửa hàng</label>
                        </div>
                    </div>
                    <div id="form-common-info">
                        <div class="d-flex align-items-center ms-auto mb-3">
                            <div class="form-check d-flex align-items-center mb-0 me-4">
                                <input class="form-check-input" type="radio" name="AnhChi" id="SexNam" value="1" <%=model.AnhChi != 2 ? "checked" : "" %>>
                                <label class="form-check-label mb-0 ms-2" for="SexNam">
                                    Anh
                                </label>
                            </div>
                            <div class="form-check d-flex align-items-center mb-0">
                                <input class="form-check-input" type="radio" name="AnhChi" id="SexNu" value="2" <%=model.AnhChi == 2 ? "checked" : "" %>>
                                <label class="form-check-label mb-0 ms-2" for="SexNu">
                                    Chị                           
                                </label>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <input type="text" class="form-control" name="Name" value="<%=item.Name%>" placeholder="Họ và tên">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <input type="text" class="form-control" name="Phone" value="<%=item.Phone%>" placeholder="Số điện thoại">
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="form-giao-hang">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="mb-3">
                                    <select class="form-control" name="CityID" id="CityID" onchange="get_child(this.value, $('#DistrictID').val(), '.list-district','<option value=0>Chọn quận / huyện</option>');">
                                        <option value="0">Chọn thành phố</option>
                                        <%=Utils.ShowDdlMenuByType2("City", 1, item.CityID) %>
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="mb-3">
                                    <select class="form-control list-district" name="DistrictID" id="DistrictID" onchange="get_child(this.value, $('#WardID').val(), '.list-ward', '<option value=0>Chọn phường / xã</option>');">
                                        <option value="0">Chọn quận / huyện</option>
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="mb-3">
                                    <select class="form-control list-ward" name="WardID" id="WardID">
                                        <option value="0">Chọn phường / xã</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="mb-3">
                                <input type="text" class="form-control" autocomplete="off" name="Address" id="Address" value="<%=item.Address%>" placeholder="Số nhà, tòa nhà, tên thôn xóm, tên đường...">
                            </div>
                            <div class="mb-3">
                                <textarea class="form-control" height="200" autocomplete="off" name="Content" rows="5" placeholder="Ghi chú thêm nếu có"><%=item.Content%></textarea>
                            </div>
                        </div>
                    </div>

                    <div id="form-nhan-hang" style="display: none;">
                        {RS:ThongTinCongTy}
                    </div>
                </div>
            </div>

            <%--<div class="p-cart-box">
                <div class="total-provisional px-0">
                    <ul>
                        <li>
                            <span class="total-product-quantity total-product-coupon">
                                <span class="total-label">Nhập mã giảm giá:</span>
                                <div class="total-discount">
                                    <input type="text" placeholder="" class="form-control" name="" id="coupon-code">
                                    <button onclick="apply_coupon()">Áp dụng</button>
                                </div>
                            </span>
                        </li>
                    </ul>
                </div>
            </div>--%>

            <div class="p-cart-box">
                <%--<h4 class="p-cart-title pd px-0">Chi tiết thanh toán</h4>--%>
                <div class="total-provisional p-0">
                    <ul>
                        <%-- <li>
                            <span class="total-product-quantity">
                                <span class="total-label">Tổng tiền hàng (2 sản phẩm): </span>
                            </span>
                            <span class="temp-total-money"><%=Utils.FormatMoney(total) %>₫</span>
                        </li>

                        <li id="total-fee-ship" style="">
                            <span class="total-product-quantity">
                                <span class="total-label">Phí vận chuyển: </span>
                            </span>
                            <span class="temp-total-money">Miễn phí</span>
                        </li>--%>
                        <%--<li>
                            <span class="total-product-quantity">
                                <span class="total-label">Tổng voucher giảm giá: </span>
                            </span>
                            <span class="temp-total-money">-500.000đ</span>
                        </li>--%>
                        <%-- <li id="total-coupon" style="display: none;">
                            <span class="total-product-quantity">
                                <span class="total-label">Mã giảm giá: </span>
                            </span>
                            <span class="temp-total-money">-250.000đ</span>
                        </li>--%>
                        <li>
                            <span class="total-product-quantity">
                                <span class="total-label fw-bold">Tổng thanh toán:</span>
                            </span>
                            <span class="temp-total-money fw-bold price-color" id="TotalMoney"><%=Utils.FormatMoney(total) %>₫</span>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="p-cart-box">
                <div class="d-flex flex-column w-100 text-center">
                    <%--<div class="form-check mb-3">
                        <input class="form-check-input" type="checkbox" value="" id="check-box-agree">
                        <label class="form-check-label mt-1 ms-2" for="check-box-agree">
                            Bấm đặt hàng đồng nghĩa với bạn đồng ý với việc bạn tuân theo điều khoản của OBIBI.VN
                   
                        </label>
                    </div>--%>
                    <a class="btn-cta order-button mb-3 d-flex align-items-center justify-content-center" href="javascript:void(0)" onclick="document.checkout_form.submit()">
                        <strong class="text-uppercase text-dark">Đặt hàng</strong>
                    </a>
                    <!-- <a class="btn-cta order-button order-button-light" href="tra-gop.html">
                    <strong>Trả góp qua thẻ</strong>
                    <span>Visa, Mastercart, JCB, Amex</span>
                </a> -->
                    <%--<p class="mb-0">Bạn có thể chọn hình thức thành toán sau khi đặt hàng</p>--%>
                </div>
            </div>
        </div>
        <input type="submit" name="_vsw_action[AddPOST]" style="display: none" />
        <input type="hidden" name="_vsw_action[AddPOST]" />
    </form>
</div>
<script type="text/javascript">
    var isCart = 1;
    var CityID = <%=item.CityID%>;
    var DistrictID = <%=item.DistrictID%>;
    var WardID = <%=item.WardID%>;
</script>