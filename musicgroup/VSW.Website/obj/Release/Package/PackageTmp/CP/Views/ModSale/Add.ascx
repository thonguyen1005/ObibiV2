<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModSaleModel;
    var item = ViewBag.Data as ModSaleEntity;

    var listDetail = item.GetDetail();
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Mã giảm giá <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Mã giảm giá</a>
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
                                <%= GetDefaultAddCommand()%>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="row">
                                <div class="col-12 col-sm-12 col-md-8 col-lg-8">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Thông tin chung</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tên chương trình:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="Name" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mã giảm giá:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Code" value="<%=item.Code %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ngày bắt đầu:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" style="width: 150px;" class="form-control date" name="DateStart" id="DateStart" value="<%=(item.DateStart > DateTime.MinValue ? string.Format("{0:dd-MM-yyyy}",item.DateStart) : "") %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ngày kết thúc:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" style="width: 150px;" class="form-control date" name="DateEnd" id="DateEnd" value="<%=(item.DateEnd > DateTime.MinValue ? string.Format("{0:dd-MM-yyyy}",item.DateEnd) : "") %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Số lượng:</label>
                                                    <div class="col-md-9">
                                                        <div class="form-inline">
                                                            <input type="number" style="width: 150px;" class="form-control price" name="Count" value="<%=item.Count %>" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Số tiền giảm:</label>
                                                    <div class="col-md-9">
                                                        <div class="form-inline">
                                                            <input type="number" style="width: 150px;" class="form-control price" name="Money" value="<%=item.Money %>" />
                                                            <span class="help-block text-primary"><%=Utils.NumberToWord(item.Money.ToString())%></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Phần trăm giảm:</label>
                                                    <div class="col-md-9">
                                                        <div class="form-inline">
                                                            <input type="number" style="width: 150px;" class="form-control price" name="Percent" value="<%=item.Percent %>" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Số tiền tối thiểu:</label>
                                                    <div class="col-md-9">
                                                        <div class="form-inline">
                                                            <input type="number" style="width: 150px;" class="form-control price" name="MoneyMin" value="<%=item.MoneyMin %>" />
                                                            <span class="help-block text-primary"><%=Utils.NumberToWord(item.MoneyMin.ToString())%></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ghi chú:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="Content" value="<%=item.Content %>" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12 col-sm-12 col-md-12 col-lg-4">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Thuộc tính</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group">
                                                    <label class="portlet-title-sub">Tự động dùng</label>
                                                    <div class="radio-list">
                                                        <label class="radioPure radio-inline">
                                                            <input type="radio" name="Auto" <%= item.Auto ? "checked": "" %> value="1" />
                                                            <span class="outer"><span class="inner"></span></span><i>Có</i>
                                                        </label>
                                                        <label class="radioPure radio-inline">
                                                            <input type="radio" name="Auto" <%= !item.Auto ? "checked": "" %> value="0" />
                                                            <span class="outer"><span class="inner"></span></span><i>Không</i>
                                                        </label>
                                                    </div>
                                                </div>
                                                <%if (CPViewPage.UserPermissions.Approve)
                                                    {%>
                                                <div class="form-group">
                                                    <label class="portlet-title-sub">Duyệt</label>
                                                    <div class="radio-list">
                                                        <label class="radioPure radio-inline">
                                                            <input type="radio" name="Activity" <%= item.Activity ? "checked": "" %> value="1" />
                                                            <span class="outer"><span class="inner"></span></span><i>Có</i>
                                                        </label>
                                                        <label class="radioPure radio-inline">
                                                            <input type="radio" name="Activity" <%= !item.Activity ? "checked": "" %> value="0" />
                                                            <span class="outer"><span class="inner"></span></span><i>Không</i>
                                                        </label>
                                                    </div>
                                                </div>
                                                <%} %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%if (listDetail != null && listDetail.Count > 0)
                                    { %>
                                <div class="col-12 col-sm-12 col-md-12 col-lg-8">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Đơn hàng đã sử dụng</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <%for (int i = 0; listDetail != null && i < listDetail.Count; i++)
                                                    { %>
                                                <div class="form-group">
                                                    <p><b><%=item.GetOrder(listDetail[i].OrderID) %> - <%=Utils.FormatMoney(listDetail[i].Money) %></b></p>
                                                </div>
                                                <%  } %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%} %>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</form>

