<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<%
    var model = ViewBag.Model as MCompleteModel;
    var order = ModOrderService.Instance.GetByID_Cache(model.OrderID);
%>
<<div class="container">
    <div class="cart-box">
        <div class="alert-success-new"><i class="fad fa-bag-shopping"></i><strong>ĐẶT HÀNG THÀNH CÔNG</strong></div>
        <%if (order != null)
            {
                string address = order.Address + (order.WardID > 0 ? " - " + order.GetWard().Name : "") + (order.DistrictID > 0 ? " - " + order.GetDistrict().Name : "") + (order.CityID > 0 ? " - " + order.GetCity().Name : "");
        %>
        <div>
            <h4 class="p-cart-title pd">Thông tin đơn hàng</h4>
            <div class="p-cart-box">
                <table class="p-cart-table">
                    <tr>
                        <td class="text-black-50 fw-medium">Mã đơn hàng</td>
                        <td class="ps-4"><%=order.Code %></td>
                    </tr>

                    <tr>
                        <td class="text-black-50 fw-medium">Người nhận</td>
                        <td class="ps-4"><%=order.Name %>, <%=order.Phone %></td>
                    </tr>
                    <tr>
                        <td class="text-black-50 fw-medium">Địa chỉ</td>
                        <td class="ps-4"><%=address %></td>
                    </tr>
                    <tr>
                        <td class="text-black-50 fw-medium">Tổng tiền</td>
                        <td class="ps-4"><span class="price-color fw-bold"><%=Utils.FormatMoney(order.Total) %>đ</span></td>
                    </tr>
                </table>
            </div>
        </div>
        <%--<div class="p-cart-box border-0 pb-0">
            <h4 class="order-infor-alert"> Đơn hàng chưa được thanh toán </h4>
        </div>
        <div>
            <h4 class="p-cart-title pd">Chọn hình thức thanh toán</h4>
            <div class="p-cart-box">
                <div class="d-flex flex-column formality-pay-new">
                    <div class="form-check d-flex align-items-center mb-3">
                        <input class="form-check-input" type="radio" name="payment-menthod" id="pay-1"
                            onchange="change_payment_menthod('cod');" checked>
                        <label class="form-check-label mb-0 ms-2" for="pay-1">
                            <i class="choose-payment-normal-at-store"></i> Thành toán tiền mặt khi nhận hàng
                        </label>
                    </div>

                    <div class="form-check d-flex align-items-center mb-3">
                        <input class="form-check-input" type="radio" name="payment-menthod" id="pay-2"
                            onchange="change_payment_menthod('ngan-hang');">
                        <label class="form-check-label mb-0 ms-2" for="pay-2">
                            <i class="choose-payment-bank"></i>Chuyển khoản ngân hàng
                        </label>
                    </div>

                    <div class="form-check d-flex align-items-center mb-3">
                        <input class="form-check-input" type="radio" name="payment-menthod" id="pay-3"
                            onchange="change_payment_menthod('3');">
                        <label class="form-check-label mb-0 ms-2" for="pay-3">
                            <i class="choose-payment-atm"></i>Qua thẻ ATM (có Internet Banking)
                        </label>
                    </div>

                    <div class="form-check d-flex align-items-center mb-3">
                        <input class="form-check-input" type="radio" name="payment-menthod" id="pay-4"
                            onchange="change_payment_menthod('4');">
                        <label class="form-check-label mb-0 ms-2" for="pay-4">
                            <i class="choose-payment-VMJ"></i>Qua thẻ quốc tế Visa, Master, JCB
                            <span class="listcard">
                                <i class="cartnew-visa"></i>
                                <i class="cartnew-mastercard"></i>
                                <i class="cartnew-jcb"></i>
                            </span>
                        </label>
                    </div>

                    <div class="form-check d-flex align-items-center mb-3">
                        <input class="form-check-input" type="radio" name="payment-menthod" id="pay-5"
                            onchange="change_payment_menthod('5');">
                        <label class="form-check-label mb-0 ms-2" for="pay-5">
                            <i class="choose-payment-MOMO"></i>Ví MoMo
                        </label>
                    </div>

                    <div class="form-check d-flex align-items-center mb-3">
                        <input class="form-check-input" type="radio" name="payment-menthod" id="pay-6"
                            onchange="change_payment_menthod('6');">
                        <label class="form-check-label mb-0 ms-2" for="pay-6">
                            <i class="choose-payment-QR-Code"></i>Qua VNPAY-QR
                        </label>
                    </div>
                </div>
            </div>


            <div id="form-ngan-hang" class="p-cart-box" style="display: none;">
                <div>
                    <div class="bank-img mb-2">
                        <img class="img-fluid" height="40"
                            src="https://gcs.tripi.vn/public-tripi/tripi-feed/img/474066HCx/logo-techcombank-dep_045648046.png"
                            alt="">
                    </div>
                    <div class="row">
                        <div class="col-md-9">
                            <div class="bank-info">
                                <ul>
                                    <li>
                                        <div class="text-wrap">
                                            <span>Tên ngân hàng:</span>
                                            <span class="text">Teckcombank</span>
                                        </div>
                                        <button class="copy">Sao chép</button>
                                    </li>
                                    <li>
                                        <div class="text-wrap">
                                            <span>Chi nhánh:</span>
                                            <span class="text">Chi nhánh Mễ Trì</span>
                                        </div>

                                        <button class="copy">Sao chép</button>
                                    </li>
                                    <li>
                                        <div class="text-wrap">
                                            <span>Tên chủ thẻ:</span>
                                            <span class="text">LÊ VĂN A</span>
                                        </div>

                                        <button class="copy">Sao chép</button>
                                    </li>
                                    <li>
                                        <div class="text-wrap">
                                            <span>Số tài khoản:</span>
                                            <span class="text">454571186647</span>
                                        </div>
                                        <button class="copy">Sao chép</button>
                                    </li>
                                    <li>
                                        <div class="text-wrap">
                                            <span>Nội dung:</span>
                                            <span class="text">NA123ABC</span>
                                        </div>
                                        <button class="copy">Sao chép</button>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="bank-qr mt-2 mt-md-0">
                                <img class="img-fluid"
                                    src="https://docs.lightburnsoftware.com/img/QRCode/ExampleCode.png"
                                    alt="">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- <div class="p-cart-box">
            <div class="d-flex flex-column w-100 text-center">
                <a class="btn-cta order-button mb-3 d-flex align-items-center justify-content-center"
                    data-bs-toggle="modal" data-bs-target="#dok-buy-success"><strong
                        class="text-uppercase text-dark">Xác nhận</strong>
                </a>
            </div>
        </div> -->

        <div class="p-cart-box">
            <div class="d-flex align-items-center justify-content-center">
                <button class="btn btn-pay" data-bs-toggle="modal" data-bs-target="#dok-buy-success">
                    <strong>Xác nhận </strong>
                </button>
            </div>
        </div>--%>
        <%} %>
    </div>
</div>
