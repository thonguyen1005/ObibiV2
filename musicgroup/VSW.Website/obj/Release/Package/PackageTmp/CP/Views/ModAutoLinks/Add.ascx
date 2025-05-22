<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModAutoLinksModel;
    var item = ViewBag.Data as ModAutoLinksEntity;
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Tạo link tự động <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Tạo link tự động</a>
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
                                                    <label class="col-md-3 col-form-label text-right">Từ khóa:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Name" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Link:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Link" value="<%=item.Link %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tiêu đề link:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Title" value="<%=item.Title %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Số lượng replace:</label>
                                                    <div class="col-md-9">
                                                        <input type="number" class="form-control" name="Quantity" value="<%=item.Quantity %>" />
                                                        <span class="help-block text-primary">Số lượng bằng 0 sẽ replace toàn bộ</span>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Duyệt:</label>
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