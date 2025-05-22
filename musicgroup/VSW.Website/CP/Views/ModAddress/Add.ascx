<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModAddressModel;
    var item = ViewBag.Data as ModAddressEntity;
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Showroom <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Showroom</a>
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
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Thông tin chung</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tên showroom:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Name" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Địa chỉ Latlong:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="LatLong" value="<%=item.LatLong %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Email:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Email" value="<%=item.Email %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Điện thoại:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control" name="Phone"><%=item.Phone %></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Địa chỉ:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Address" value="<%=item.Address %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Zalo:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Zalo" value="<%=item.Zalo %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Facebook</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Facebook" value="<%=item.Facebook %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tên FB</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="FBName" value="<%=item.FBName %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Chuyên mục:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-select select2" name="ShowroomID" id="ShowroomID" onchange="GetProperties(this.value)">
                                                            <option value="0">Root</option>
                                                            <%= Utils.ShowDdlShowroomByType("Showroom", model.LangID, item.ShowroomID)%>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tỉnh thành:</label>
                                                    <div class="col-md-9">
                                                        <select name="CityID" id="CityID" class="form-select select2" onchange="get_child(this.value, $('#DistrictID').val(), '.list-district')">
                                                            <option value="0">Chọn thành phố</option>
                                                            <%=Utils.ShowDdlMenuByType2("City", 1, item.CityID) %>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Quận huyện:</label>
                                                    <div class="col-md-9">
                                                        <select name="DistrictID" id="DistrictID" class="form-select select2 list-district" onchange="get_child(this.value, $('#WardID').val(), '.list-ward')">
                                                            <option value="0">Chọn quận / huyện</option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Xã phường:</label>
                                                    <div class="col-md-9">
                                                        <select name="WardID" id="WardID" class="form-select select2 list-ward">
                                                            <option value="0">Chọn phường / xã</option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Bản đồ:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control" rows="5" name="Map"><%=item.Map%></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Địa chỉ Bản đồ:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="MapAddress" value="<%=item.MapAddress %>" />
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
<script type="text/javascript">
    $(document).ready(function () {
        get_child('<%=item.CityID%>', '<%=item.DistrictID%>', '.list-district');
        get_child('<%=item.DistrictID%>', '<%=item.WardID%>', '.list-ward');
    });
</script>
