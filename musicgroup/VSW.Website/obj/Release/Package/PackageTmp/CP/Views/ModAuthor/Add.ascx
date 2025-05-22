<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModAuthorModel;
    var item = ViewBag.Data as ModAuthorEntity;
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" name="RecordID" id="RecordID" value="<%=model.RecordID %>" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Tác giả <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Tác giả</a>
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
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tên tác giả:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="Name" id="Name" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">URL trình duyệt:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Code" value="<%=item.Code %>" placeholder="Nếu không nhập sẽ tự sinh theo Tiêu đề" />
                                                    </div>
                                                </div>                                                       
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Chức vụ:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Position" value="<%=item.Position %>"  />
                                                    </div>
                                                </div>  
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Facebook:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Facebook" value="<%=item.Facebook %>"  />
                                                    </div>
                                                </div>     
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Zalo:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Zalo" value="<%=item.Zalo %>"  />
                                                    </div>
                                                </div>       
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tiktok:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Tiktok" value="<%=item.Tiktok %>"  />
                                                    </div>
                                                </div>        
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Youtube:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Youtube" value="<%=item.Youtube %>"  />
                                                    </div>
                                                </div>     
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Thông tin cá nhân</div>
                                        </div>
                                        <div class="portlet-body">
                                            <textarea class="form-control ckeditor" name="Story" id="Story" rows="" cols=""><%=item.Story %></textarea>
                                        </div>
                                    </div>
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Tiểu sử và hoạt động</div>
                                        </div>
                                        <div class="portlet-body">
                                            <textarea class="form-control ckeditor" name="Content" id="Content" rows="" cols=""><%=item.Content %></textarea>
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
                                                <div class="form-group">
                                                    <%if (!string.IsNullOrEmpty(item.Banner))
                                                        { %>
                                                    <p class="preview "><%= Utils.GetMedia(item.Banner, 80, 80)%></p>
                                                    <%}
                                                        else
                                                        { %>
                                                    <p class="preview">
                                                        <img src="" width="80" height="80" />
                                                    </p>
                                                    <%} %>

                                                    <label class="portlet-title-sub">Banner:</label>
                                                    <div class="form-inline">
                                                        <input type="text" class="form-control" name="Banner" id="Banner" value="<%=item.Banner %>" />
                                                        <button type="button" class="btn btn-primary" onclick="ShowFileForm('Banner'); return false">Chọn ảnh</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">SEO</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group ">
                                                    <label class="col-form-label">PageTitle:</label>
                                                    <input type="text" class="form-control title" name="PageTitle" maxlength="200" value="<%=item.PageTitle %>" />
                                                    <span class="help-block text-primary">Ký tự tối đa: 200</span>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-form-label">Description:</label>
                                                    <textarea class="form-control description" rows="5" name="PageDescription" maxlength="400" placeholder="Nhập nội dung tóm tắt. Tối đa 400 ký tự"><%=item.PageDescription%></textarea>
                                                    <span class="help-block text-primary">Ký tự tối đa: 400</span>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-form-label">Keywords:</label>
                                                    <input type="text" class="form-control" name="PageKeywords" value="<%=item.PageKeywords %>" />
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
