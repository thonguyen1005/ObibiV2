<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModVoteModel;
    var item = ViewBag.Data as ModVoteEntity;
    if (string.IsNullOrEmpty(item.Name)) item.Name = "Administrator";
    if (item.ID < 1) item.ChucVu = "QTV";
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
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
                                            <div class="caption">Thông tin trả lời</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Đối tượng:</label>
                                                    <div class="col-md-9">
                                                        <b><%=item.GetObject() %></b>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Họ và tên:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Name" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Nhiệm vụ:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="ChucVu" value="<%=item.ChucVu %>" />
                                                    </div>
                                                </div>
                                                <%--<div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Điện thoại:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Phone" value="<%=item.Phone %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Email:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Email" value="<%=item.Email %>" />
                                                    </div>
                                                </div>--%>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Nội dung:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control description" rows="5" name="Content" placeholder="Nhập nội dung tóm tắt. Tối đa 400 ký tự"><%=item.Content%></textarea>
                                                    </div>
                                                </div>
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
