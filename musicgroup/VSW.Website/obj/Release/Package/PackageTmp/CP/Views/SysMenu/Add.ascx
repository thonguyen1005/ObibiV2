<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as SysMenuModel;
    var item = ViewBag.Data as WebMenuEntity;

    var listParent = VSW.Lib.Global.ListItem.List.GetList(WebMenuService.Instance, model.LangID);

    if (model.RecordID > 0)
    {
        //loai bo danh muc con cua danh muc hien tai
        listParent = VSW.Lib.Global.ListItem.List.GetListForEdit(listParent, model.RecordID);
    }

    var listProperty = WebPropertyService.Instance.CreateQuery()
                                .Where(o => o.Activity == true && o.ParentID == 0)
                                .ToList_Cache();
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Chuyên mục <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Chuyên mục</a>
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
                                                    <label class="col-md-3 col-form-label text-right">Tên chuyên mục:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Name" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mã chuyên mục:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Code" value="<%=item.Code %>" placeholder="Nếu không nhập sẽ tự sinh theo Tên" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mô tả:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control description" rows="5" name="Summary" maxlength="400" placeholder="Nhập nội dung tóm tắt. Tối đa 400 ký tự"><%=item.Summary%></textarea>
                                                        <span class="help-block text-primary">Ký tự tối đa: 400</span>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mã thiết kế:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control" rows="5" name="Custom"><%=item.Custom%></textarea>
                                                    </div>
                                                </div>

                                                <%if (model.ParentID == 0){ %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Loại dữ liệu:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Type" value="<%=item.Type %>" />
                                                    </div>
                                                </div>
                                                <%} %>

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
                                                    <%if (!string.IsNullOrEmpty(item.File)){ %>
                                                    <p class="preview "><%= Utils.GetMedia(item.File, 80, 80)%></p>
                                                    <%}else{ %>
                                                    <p class="preview"><img src="" width="80" height="80" /></p>
                                                    <%} %>

                                                    <label class="portlet-title-sub">Hình minh họa:</label>
                                                    <div class="form-inline">
                                                        <input type="text" class="form-control" name="File" id="File" value="<%=item.File %>" />
                                                        <button type="button" class="btn btn-primary" onclick="ShowFileForm('File'); return false">Chọn ảnh</button>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="portlet-title-sub">Chuyên mục cha</label>
                                                    <select class="form-select select2" name="ParentID">
                                                        <option value="0">Root</option>
                                                        <%for (int i = 0; listParent != null && i < listParent.Count; i++){ %>
                                                        <option <%if (item.ParentID.ToString() == listParent[i].Value){%>selected<%} %> value='<%= listParent[i].Value%>'>&nbsp; <%= listParent[i].Name%></option>
                                                        <%} %>
                                                    </select>
                                                </div>

                                                <%if (model.ParentID > 0 && item.Type == "Product" ){%>
                                                <div class="form-group">
                                                    <label class="portlet-title-sub">Nhóm thuộc tính</label>
                                                    <select class="form-select select2" name="PropertyID">
                                                        <option value="0">Root</option>
                                                        <option value="0">&nbsp;Nhóm thuộc tính</option>
                                                        <%for (var i = 0; listProperty != null && i < listProperty.Count; i++){ %>
                                                        <option <%if (item.PropertyID == listProperty[i].ID){%>selected<%} %> value="<%= listProperty[i].ID%>">&nbsp;----&nbsp;<%= listProperty[i].Name%></option>
                                                        <%} %>
                                                    </select>
                                                </div>
                                                <%} %>
                                                 

                                                <%--<div class="form-group">
                                                    <label class="portlet-title-sub">Vị trí</label>
                                                    <div class="checkbox-list">
                                                        <%= Utils.ShowCheckBoxByConfigkey("Sys.MenuState", "ArrState", item.State)%>
                                                    </div>
                                                </div>--%>
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