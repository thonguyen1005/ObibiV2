<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModFeedbackModel;
    var item = ViewBag.Data as ModFeedbackEntity;
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Liên hệ <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Liên hệ</a>
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
                                <%= GetTinyAddCommand()%>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="row">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Thông tin chung</div>
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
                                                    <label class="col-md-3 col-form-label text-right">Địa điểm thuê:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Address" value="<%=item.DiaChiThue %>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Số lượng người-diện tích:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="SoNguoi" value="<%=item.SoNguoi %>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ngày bắt đầu:</label>
                                                    <div class="col-md-9">
                                                        <input type="datetime-local" class="form-control" name="FromDate" value="<%=item.FromDate %>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ngày kết thúc:</label>
                                                    <div class="col-md-9">
                                                        <input type="datetime-local" class="form-control" name="ToDate" value="<%=item.ToDate %>" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Loại thiết bị:</label>
                                                    <div class="col-md-9">
                                                        <%=Utils.ShowRadioByByType2("LoaiThietBi", model.LangID, item.ThietBiID, "ThietBiID") %>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Môi trường:</label>
                                                    <div class="col-md-9">
                                                        <%=Utils.ShowRadioByByType2("MoiTruong", model.LangID, item.MoiTruongID, "MoiTruongID") %>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Nội dung:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control" rows="5" name="Content" readonly="readonly"><%=item.Content%></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ngày gửi liên hệ:</label>
                                                    <div class="col-md-9">
                                                        <%= string.Format("{0:dd-MM-yyyy HH:mm}", item.Created) %>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">IP:</label>
                                                    <div class="col-md-9">
                                                        <%= item.IP %>
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
