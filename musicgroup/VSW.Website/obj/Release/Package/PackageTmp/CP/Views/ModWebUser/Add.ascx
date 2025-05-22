<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModWebUserModel;
    var item = ViewBag.Data as ModWebUserEntity;
    var listFile = item.GetFile();
	var listWebUserMenu = ModWebUserMenuService.Instance.CreateQuery().Where(o => o.Activity == true).OrderByAsc(o => o.Order).ToList();
%>
<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="RecordID" value="<%=model.RecordID %>" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Khách hàng <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Khách hàng</a>
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
                                <%= GetSortAddCommand()%>
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
                                                    <label class="col-md-3 col-form-label text-right">Loại khách hàng:</label>
                                                    <div class="col-md-9">
                                                        <div class="radio-list">
															<%for (int i = 0; listWebUserMenu != null && i < listWebUserMenu.Count; i++)
																{%>
															<label class="radioPure radio-inline">
																<input type="radio" name="Type2" value="<%=listWebUserMenu[i].ID %>" <%=((listWebUserMenu[i].ID == item.Type2 || (item.Type2 <=0 && i ==0)) ? "checked" : "") %>>
																<span class="outer"><span class="inner"></span></span><i><%=listWebUserMenu[i].Name %></i>
															</label>
															<%} %>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ảnh:</label>
                                                    <div class="col-md-9">
                                                        <%if (!string.IsNullOrEmpty(item.File))
                                                            { %>
                                                        <p class="preview "><%= Utils.GetMedia(item.File, 80, 80)%></p>
                                                        <%}
                                                            else
                                                            { %>
                                                        <p class="preview">
                                                            <img src="" width="80" height="80" />
                                                        </p>
                                                        <%} %>
                                                        <div class="form-inline">
                                                            <input type="text" class="form-control" name="File" id="File" value="<%=item.File %>" />
                                                            <button type="button" class="btn btn-primary" onclick="ShowFileForm('File'); return false">Chọn ảnh</button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tên đăng nhập:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="UserName" id="UserName" value="<%=item.UserName %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mật khẩu:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Password2" id="Password2" value="" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Họ và tên:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Name" id="Name" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Giới tính:</label>
                                                    <div class="col-md-9">
                                                        <div class="radio-list">
                                                            <%=Utils.ShowRadioByConfigkey("Mod.Gender","Gender",(item.Gender ? 1 : 0)) %>
                                                        </div>
                                                    </div>
                                                </div>
												<div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ngày sinh</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control date" name="Birthday" id="Birthday" value="<%=Utils.FormatDate(item.Birthday) %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Email:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Email" id="Email" value="<%=item.Email %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Điện thoại:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Phone" id="Phone" value="<%=item.Phone %>" />
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
                                                        <select name="DistrictID" id="DistrictID" class="form-select select2 list-district" onchange="get_child2(this.value, $('#WardID').val(), '.list-ward')">
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
                                                    <label class="col-md-3 col-form-label text-right">Địa chỉ:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Address" id="Address" value="<%=item.Address %>" />
                                                    </div>
                                                </div>
                                                <%--<div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Loại tài khoản:</label>
                                                    <div class="col-md-9">
                                                        <div class="radio-list">
                                                            <%=Utils.ShowRadioByConfigkey("Mod.WebUserType","Type",item.Type) %>
                                                        </div>
                                                    </div>
                                                </div>--%>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tên công ty:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="CompanyName" id="CompanyName" value="<%=item.CompanyName %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Địa chỉ công ty:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="CompanyAddress" id="CompanyAddress" value="<%=item.CompanyAddress %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">MST công ty:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="CompanyTax" id="CompanyTax" value="<%=item.CompanyTax %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Điểm thưởng:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" style="width: 150px;" name="Point" id="Point" value="<%=item.Point %>" />
                                                    </div>
                                                </div>
                                                <%--<div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ảnh xác minh:</label>
                                                    <div class="col-md-9">
                                                        <ul class="cmd-custom" id="list-file">
                                                            <%for (int i = 0; listFile != null && i < listFile.Count; i++)
                                                                {%>
                                                            <li data-src="<%=listFile[i].File.Replace("~/","/") %>">
                                                                <img src="<%=listFile[i].File.Replace("~/","/") %>" data-src="<%=listFile[i].File.Replace("~/","/") %>" />
                                                                <a href="javascript:void(0)" onclick="deleteFileUser('<%=listFile[i].File %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa"><i class="fa fa-ban"></i></a>
                                                                <a href="javascript:void(0)" onclick="upFileUser('<%=listFile[i].File %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển lên trên"><i class="fa fa-arrow-up"></i></a>
                                                                <a href="javascript:void(0)" onclick="downFileUser('<%=listFile[i].File %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển xuống dưới"><i class="fa fa-arrow-down"></i></a>
                                                            </li>
                                                            <%} %>
                                                        </ul>
                                                        <div class="form-inline">
                                                            <button type="button" class="btn btn-primary" onclick="ShowFileUser(); return false">Chọn ảnh</button>
                                                        </div>
                                                    </div>
                                                </div>--%>
                                                <%if (CPViewPage.UserPermissions.Approve)
                                                    {%>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Duyệt</label>
                                                    <div class="col-md-9">
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
                                                </div>
                                                <%} %>
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
        get_child2('<%=item.DistrictID%>', '<%=item.WardID%>', '.list-ward');
    });
</script>
<script type="text/javascript">
    $(document).ready(function () {
        $('#list-file').lightGallery();
    });
</script>
