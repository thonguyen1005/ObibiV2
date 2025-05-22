<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as SysPageModel;
    var item = ViewBag.Data as SysPageEntity;

    var listTemplate = SysTemplateService.Instance.CreateQuery()
                                        .Where(o => o.LangID == model.LangID && o.Device == 0)
                                        .OrderByAsc(o => o.Order)
                                        .ToList();

    var listTemplateMobile = SysTemplateService.Instance.CreateQuery()
                                        .Where(o => o.LangID == model.LangID && o.Device == 1)
                                        .OrderByAsc(o => o.Order)
                                        .ToList();

    var listTemplateTablet = SysTemplateService.Instance.CreateQuery()
                                        .Where(o => o.LangID == model.LangID && o.Device == 2)
                                        .OrderByAsc(o => o.Order)
                                        .ToList();

    var listModule = VSW.Lib.Web.Application.Modules.Where(o => o.IsControl == false).OrderBy(o => o.Order).ToList();

    var listParent = VSW.Lib.Global.ListItem.List.GetList(SysPageService.Instance, model.LangID);

    if (model.RecordID > 0)
    {
        //loai bo danh muc con cua danh muc hien tai
        listParent = VSW.Lib.Global.ListItem.List.GetListForEdit(listParent, model.RecordID);
    }

    var parent = item.ID > 0 ? SysPageService.Instance.GetByID_Cache(item.ParentID) : null;
    //var listFile = item.GetFiles();
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Trang <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Trang</a>
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
                                                    <label class="col-md-3 col-form-label text-right">Tên trang:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="Name" id="Name" onchange="addValuSchema()" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tên viết tắt:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="AliasName" id="AliasName" onchange="addValuSchema()" value="<%=item.AliasName %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">URL trình duyệt:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Code" id="Code" value="<%=item.Code %>" onchange="addValuSchema()" placeholder="Nếu không nhập sẽ tự sinh theo Tên trang" />
                                                    </div>
                                                </div>
                                                <%--<div class="form-group row">
                                                    <label class="col-md-3 col-form-label">Mô tả:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control description" rows="5" name="Summary" placeholder="Nhập nội dung tóm tắt. Tối đa 400 ký tự"><%=item.Summary%></textarea>
                                                        <span class="help-block text-primary">Ký tự tối đa: 400</span>
                                                    </div>
                                                </div>--%>
                                                <%if (model.ParentID > 0)
                                                    {%>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mãu giao diện:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-select select2" name="TemplateID">
                                                            <option value="0"></option>
                                                            <%for (var i = 0; listTemplate != null && i < listTemplate.Count; i++)
                                                                { %>
                                                            <option <%if (item.TemplateID == listTemplate[i].ID)
                                                                {%>selected<%} %>
                                                                value="<%= listTemplate[i].ID%>">&nbsp; <%= listTemplate[i].Name%></option>
                                                            <%} %>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mãu giao diện Mobile:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-select select2" name="TemplateMobileID">
                                                            <option value="0"></option>
                                                            <%for (var i = 0; listTemplateMobile != null && i < listTemplateMobile.Count; i++)
                                                                { %>
                                                            <option <%if (item.TemplateMobileID == listTemplateMobile[i].ID)
                                                                {%>selected<%} %>
                                                                value="<%= listTemplateMobile[i].ID%>">&nbsp; <%= listTemplateMobile[i].Name%></option>
                                                            <%} %>
                                                        </select>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Chức năng:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-select select2" name="ModuleCode" id="ModuleCode" onchange="page_control_change(this.value)">
                                                            <option value="0"></option>
                                                            <%for (var i = 0; i < listModule.Count; i++)
                                                                { %>
                                                            <option <%if (item.ModuleCode == listModule[i].Code)
                                                                {%>selected<%} %>
                                                                value="<%= listModule[i].Code%>">&nbsp; <%= listModule[i].Name%></option>
                                                            <%} %>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div id="control_param"></div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Liên kết Chuyên mục:</label>
                                                    <div class="col-md-9">
                                                        <div id="list_menu"></div>
                                                    </div>
                                                </div>
                                                <div class="form-group row brand-block" style="display: none;">
                                                    <label class="col-md-3 col-form-label text-right">Liên kết Thương hiệu:</label>
                                                    <div class="col-md-9">
                                                        <div id="list_brand"></div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Trang cha:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-select select2" name="ParentID">
                                                            <option value="0">Root</option>
                                                            <%for (var i = 0; listParent != null && i < listParent.Count; i++)
                                                                { %>
                                                            <option <%if (item.ParentID.ToString() == listParent[i].Value)
                                                                {%>selected<%} %>
                                                                value="<%= listParent[i].Value%>">&nbsp; <%= listParent[i].Name%></option>
                                                            <%} %>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mã thiết kế:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control" rows="5" name="Custom" id="Custom" placeholder=""><%=item.Custom%></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-12 col-form-label text-right">TopContent</label>
                                                    <div class="col-md-12">
                                                        <textarea class="form-control ckeditor" name="TopContent" id="TopContent" rows="" cols=""><%=item.TopContent %></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-12 col-form-label text-right">Bottom Content</label>
                                                    <div class="col-md-12">
                                                        <textarea class="form-control ckeditor" name="Content" id="Content" rows="" cols=""><%=item.Content %></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-12 col-form-label text-right">Schema.org</label>
                                                    <div class="col-md-12">
                                                        <textarea class="form-control" rows="15" name="SchemaJson" id="SchemaJson"><%=item.SchemaJson%></textarea>
                                                        <div class="col-md-12">
                                                            <input class="btn btn-primary" type="button" onclick="loadSchema(0); return false;" value="Lấy dữ liệu trắng">
                                                            <input class="btn btn-primary" type="button" onclick="loadSchema(1); return false;" value="Lấy dữ liệu mặc định từ cấu hình tài nguyên">
                                                        </div>
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
                                                        <input type="text" class="form-control" name="File" id="File" onchange="addValuSchema()" value="<%=item.File %>" />
                                                        <button type="button" class="btn btn-primary" onclick="ShowFileForm('File'); return false">Chọn ảnh</button>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <%if (!string.IsNullOrEmpty(item.Icon))
                                                        { %>
                                                    <p class="preview "><%= Utils.GetMedia(item.Icon, 40, 40)%></p>
                                                    <%}
                                                        else
                                                        { %>
                                                    <p class="preview">
                                                        <img src="" width="40" height="40" />
                                                    </p>
                                                    <%} %>

                                                    <label class="portlet-title-sub">Icon:</label>
                                                    <div class="form-inline">
                                                        <input type="text" class="form-control" name="Icon" id="Icon" value="<%=item.Icon %>" />
                                                        <button type="button" class="btn btn-primary" onclick="ShowIconForm('Icon'); return false">Chọn icon</button>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="portlet-title-sub">Vị trí</label>
                                                    <div class="checkbox-list">
                                                        <%= Utils.ShowCheckBoxByConfigkey("Mod.PageState", "ArrState", item.State)%>
                                                    </div>
                                                </div>

                                                <%if (CPViewPage.UserPermissions.Approve)
                                                    {%>
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
                                                <%} %>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">SEO</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group">
                                                    <label class="col-form-label">PageTitle:</label>
                                                    <input type="text" class="form-control title" name="PageTitle" id="PageTitle" maxlength="200" onchange="addValuSchema()" value="<%=item.PageTitle %>" />
                                                    <span class="help-block text-primary">Ký tự tối đa: 200</span>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-form-label">Description:</label>
                                                    <textarea class="form-control description" rows="5" name="PageDescription" maxlength="400" id="PageDescription" onchange="addValuSchema()" placeholder="Nhập nội dung tóm tắt. Tối đa 400 ký tự"><%=item.PageDescription%></textarea>
                                                    <span class="help-block text-primary">Ký tự tối đa: 400</span>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-form-label">Keywords:</label>
                                                    <input type="text" class="form-control" name="PageKeywords" id="PageKeywords" onchange="addValuSchema()" value="<%=item.PageKeywords %>" />
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
        function menu_change(name) {
            var txtPageName = document.getElementById('Name');
            if (txtPageName.value === '') {
                var i = name.indexOf('---- ');
                if (i > -1)
                    txtPageName.value = name.substr(i + 5);
            }
        }

        function setup_editor() {
            if (document.getElementById('Content')) {
                var editorContent = CKEDITOR.instances['Content'];
                if (editorContent) { editorContent.destroy(true); }
                CKEDITOR.replace('Content');
            }
            if (document.getElementById('ContentBottom')) {
                var editorContent = CKEDITOR.instances['ContentBottom'];
                if (editorContent) { editorContent.destroy(true); }
                CKEDITOR.replace('ContentBottom');
            }
        }


        function page_control_change(controlID) {

            var ranNum = Math.floor(Math.random() * 999999);
            var dataString = 'LangID=<%= model.LangID%>&PageID=<%=item.ID %>&ModuleCode=' + controlID + '&rnd=' + ranNum;

            $.ajax({
                url: '/{CPPath}/Ajax/PageGetControl.aspx',
                type: 'get',
                data: dataString,
                dataType: 'json',
                //data chinh la response lay dc sau khi ajax xu ly xong. no la 1 object JsonModel. do do co the goi data.Html, data.Js, data.Extension,...
                success: function (data) {
                    var content = data.Html;
                    var listMenu = data.Params;
                    var listBrand = data.Js;

                    listMenu = '<select class="form-select select2" name="MenuID" onchange="menu_change(this.options[this.selectedIndex].text)"><option value="0">Root</option>' + listMenu + '</select>';
                    listBrand = '<select class="form-select select2" name="BrandID" style="width:100%;"><option value="0">Root</option>' + listBrand + '</select>';

                    $('#control_param').html(content);
                    $('#list_menu').html(listMenu);
                    $('#list_brand').html(listBrand);

                    if (controlID != 'MProduct')
                        $('.brand-block').hide();
                    else
                        $('.brand-block').show();

                    App.initSelect2();
                    window.setTimeout('CKEditorInstance()', 100);
                },
                error: function () { }
            });
        }

        if ('<%=item.ModuleCode%>' !== '') page_control_change('<%= item.ModuleCode %>');
        else if ($('#ModuleCode').val() !== '') page_control_change($('#ModuleCode').val());

        if ('<%=item.ModuleCode%>' !== 'MProduct') $('.brand-block').show();
    </script>

</form>
