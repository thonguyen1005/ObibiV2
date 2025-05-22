<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModAdvModel;
    var item = ViewBag.Data as ModAdvEntity;
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Quảng cáo - Liên kết <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Quảng cáo - Liên kết</a>
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
                                                    <label class="col-md-3 col-form-label text-right">Tên:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Name" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ảnh - Flash:</label>
                                                    <div class="col-md-9">
                                                        <%if (!string.IsNullOrEmpty(item.File)){ %>
                                                        <p class="preview "><%= Utils.GetMedia(item.File, 80, 80)%></p>
                                                        <%}else{ %>
                                                        <p class="preview"><img src="" width="80" height="80" /></p>
                                                        <%} %>
                                                        <div class="form-inline">
                                                            <input type="text" class="form-control" name="File" id="File" value="<%=item.File %>" />
                                                            <button type="button" class="btn btn-primary" onclick="ShowFileForm('File'); return false">Chọn ảnh</button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Chiều rộng:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Width" value="<%=item.Width %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Chiều cao:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Height" value="<%=item.Height %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Chèn thêm:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="AddInTag" value="<%=item.AddInTag %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">URL:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="URL" value="<%=item.URL %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Target:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-control" name="Target">
                                                            <option value=""></option>
                                                            <option <%if (item.Target == "_blank"){%>selected<%} %> value="_blank">_blank</option>
                                                            <option <%if (item.Target == "_parent"){%>selected<%} %> value="_parent">_parent</option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mã code:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control" rows="5" name="AdvCode"><%=item.AdvCode%></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Chuyên mục:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-select select2" name="MenuID" id="MenuID">
                                                            <option value="0">Root</option>
                                                            <%= Utils.ShowDdlMenuByType2("Adv", model.LangID, item.MenuID)%>
                                                        </select>
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