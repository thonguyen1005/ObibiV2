<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModVoteModel;
    var item = ViewBag.Data as ModVoteEntity;
    var listFile = item.GetFile();
%>
<link href="/Content/utils/lightGallery/css/lightgallery.css" rel="stylesheet" type="text/css">
<script type="text/javascript" src="/Content/utils/lightGallery/js/lightgallery-all.min.js"></script>
<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" name="RecordID" id="RecordID" value="<%=model.RecordID %>" />
    <div class="page-content-wrapper">
        <h3 class="page-title">Đánh giá <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Đánh giá</a>
                </li>
            </ul>
            <div class="page-toolbar">
                <div class="btn-group">
                    <a href="/" class="btn green" target="_blank"><i class="icon-screen-desktop"></i>Xem Website</a>
                </div>
            </div>
        </div>
        <div class="tabbable">
            <%if (model.RecordID > 0)
                { %>
            <ul class="nav nav-tabs nav nav-tabs justify-content-start">
                <li class="nav-link active" data-href="#tab-1" data-toggle="tab">
                    <a href="javascript:;">Đánh giá</a>
                </li>
                <li class="nav-link" data-href="#tab-2" data-toggle="tab">
                    <a href="javascript:;">Danh sách trả lời</a>
                </li>
            </ul>
            <%} %>
            <div class="tab-content">
                <div class="tab-pane active" id="tab-1">
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
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Số sao:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control price" name="Vote" value="<%=item.Vote %>" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Nội dung:</label>
                                                                <div class="col-md-9">
                                                                    <textarea class="form-control description" rows="5" name="Content" placeholder="Nhập nội dung tóm tắt. Tối đa 400 ký tự"><%=item.Content%></textarea>
                                                                </div>
                                                            </div>
                                                            <hr />
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Admin Note:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control" name="AdminNote" value="<%=item.AdminNote %>" />
                                                                </div>
                                                            </div>
                                                            <%if (item.ID > 0 && item.Vote > 0)
                                                                {
                                                                    var listCommnet = ModVoteDetailService.Instance.CreateQuery().Where(o => o.CommentID == item.ID).ToList_Cache();
                                                            %>
                                                            <%if (listCommnet != null)
                                                                { %>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Câu hỏi sản phẩm:</label>
                                                                <%for (int i = 0; listCommnet != null && i < listCommnet.Count; i++)
                                                                    {%>
                                                                <div class="col-md-9">
                                                                    <%=listCommnet[i].GetProductVote().Name %>: <b><%=listCommnet[i].Vote ? "Có" : "Không" %></b>
                                                                </div>
                                                                <%} %>
                                                            </div>
                                                            <%} %>
                                                            <%} %>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Ảnh</label>
                                                                <div class="col-md-9">
                                                                    <ul class="cmd-custom" id="list-file">
                                                                        <%for (int i = 0; listFile != null && i < listFile.Count; i++)
                                                                            {%>
                                                                        <li data-src="<%=Utils.GetUrlFile(listFile[i].File) %>">
                                                                            <img src="<%=Utils.GetUrlFile(listFile[i].File) %>" data-src="<%=Utils.GetUrlFile(listFile[i].File) %>" />
                                                                            <a href="javascript:void(0)" onclick="deleteFileVote('<%=listFile[i].File %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa"><i class="fa fa-ban"></i></a>
                                                                            <a href="javascript:void(0)" onclick="upFileVote('<%=listFile[i].File %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển lên trên"><i class="fa fa-arrow-up"></i></a>
                                                                            <a href="javascript:void(0)" onclick="downFileVote('<%=listFile[i].File %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển xuống dưới"><i class="fa fa-arrow-down"></i></a>
                                                                            <label style="display: inline-block; text-align: center; width: 40px;" class="itemCheckBox itemCheckBox-sm" data-toggle="tooltip" data-placement="bottom" data-original-title="Chọn hiển thị">
                                                                                <input type="checkbox" onclick="updateFileVote('<%=listFile[i].File %>', this)" <%=listFile[i].Show ? "checked" : "" %> value="1">
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </li>
                                                                        <%} %>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                           <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Đơn hàng đã mua:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control price" name="OrderCode" value="<%=item.OrderCode %>" />
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
                <%if (model.RecordID > 0)
                    {%>
                <div class="tab-pane" id="tab-2">
                    <iframe src="/CP/FormVote/Index.aspx/ParentID/<%=model.RecordID %>/ProductID/<%=item.ProductID %>" style='position: static; top: 240px; left: 0px; width: 101%; height: 1100px; z-index: 999; overflow: auto;' frameborder='no'></iframe>
                </div>
                <%} %>
            </div>
        </div>
    </div>
</form>
