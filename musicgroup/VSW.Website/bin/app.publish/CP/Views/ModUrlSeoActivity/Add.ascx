<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModUrlSeoActivityModel;
    var item = ViewBag.Data as ModUrlSeoActivityEntity;
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">SEO đường dẫn <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">SEO đường dẫn đã chuẩn hóa</a>
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
                                <div class="col-12 col-sm-12 col-md-12 col-lg-8">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Thông tin chung</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <%-- <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Số lượng xem</label>
                                                    <div class="col-md-9">
                                                        <%=Utils.FormatMoney(item.CountSearch) %>
                                                    </div>
                                                </div>--%>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Đường dẫn:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="Url" value="<%=item.Url %>" />
                                                    </div>
                                                </div>
                                                <%-- <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Đường dẫn chuẩn SEO:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="UrlRedirect" value="<%=item.UrlRedirect %>" />
                                                    </div>
                                                </div>--%>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">PageTitle:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="PageTitle" value="<%=item.PageTitle %>" placeholder="Nhập nội dung tóm tắt. Tối đa 70 ký tự" maxlength="70" />
                                                        <span class="help-block text-primary">Ký tự tối đa: 70</span>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Description:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control description title" rows="5" name="PageDescription" placeholder="Nhập nội dung tóm tắt. Tối đa 300 ký tự" maxlength="300"><%=item.PageDescription%></textarea>
                                                        <span class="help-block text-primary">Ký tự tối đa: 300</span>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Keywords:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="PageKeywords" value="<%=item.PageKeywords %>" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Nội dung</div>
                                        </div>
                                        <div class="portlet-body">
                                            <textarea class="form-control ckeditor" name="Content" id="Content" rows="" cols=""><%=item.Content %></textarea>
                                        </div>
                                    </div>
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Schema.org</div>
                                        </div>
                                        <div class="portlet-body">
                                            <textarea class="form-control" rows="15" name="SchemaJson" id="SchemaJson"><%=item.SchemaJson%></textarea>
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

                                                    <label class="portlet-title-sub">Hình minh họa:</label>
                                                    <div class="form-inline">
                                                        <input type="text" class="form-control" name="File" id="File" value="<%=item.File %>" />
                                                        <button type="button" class="btn btn-primary" onclick="ShowFileForm('File'); return false">Chọn ảnh</button>
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

    <script type="text/javascript">
        //setTimeout(function () { vsw_exec_cmd('[autosave][<%=model.RecordID%>]') }, 5000);
    </script>

</form>
