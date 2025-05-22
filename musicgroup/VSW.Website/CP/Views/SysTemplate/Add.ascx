<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as SysTemplateModel;
    var item = ViewBag.Data as SysTemplateEntity;

    var path = Server.MapPath("~/Views/Shared");

    string[] arrFiles = null;
    if (System.IO.Directory.Exists(path))
        arrFiles = System.IO.Directory.GetFiles(path);
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <div class="page-content-wrapper">
        <h3 class="page-title">Mẫu giao diện <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Mẫu giao diện</a>
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
                            <div class="tabbable">
                                <ul class="nav nav-tabs justify-content-start">
                                    <li class="nav-link active" data-href="#general" data-toggle="tab">
                                        <a href="javascript:;">Thông tin chung</a>
                                    </li>
                                    <li class="nav-link" data-href="#design" data-toggle="tab">
                                        <a href="javascript:;">Thiết kế giao diện</a>
                                    </li>
                                </ul>
                                <div class="tab-content">
                                    <div class="tab-pane active" id="general">
                                        <div class="row">
                                            <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption">Thông tin chung</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-body">
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Tên mẫu:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control title" name="Name" value="<%=item.Name %>" maxlength="200" />
                                                                    <span class="help-block text-primary">Ký tự tối đa: 200</span>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">File:</label>
                                                                <div class="col-md-9">
                                                                    <select class="form-control" name="File">
                                                                        <%foreach (var file in arrFiles)
                                                                            {
                                                                                var fileName = System.IO.Path.GetFileName(file);
                                                                        %>
                                                                        <option <%if (item.File == fileName)
                                                                            {%>selected<%} %> value="<%=fileName %>"><%=fileName %></option>
                                                                        <%}%>
                                                                    </select>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Thiết bị:</label>
                                                                <div class="col-md-9">
                                                                    <div class="radio-list">
                                                                        <label class="radioPure radio-inline">
                                                                            <input type="radio" name="Device" <%= item.Device == 0 ? "checked": "" %> value="0" />
                                                                            <span class="outer"><span class="inner"></span></span><i>PC / Laptop</i>
                                                                        </label>
                                                                        <label class="radioPure radio-inline">
                                                                            <input type="radio" name="Device" <%= item.Device == 1 ? "checked": "" %> value="1" />
                                                                            <span class="outer"><span class="inner"></span></span><i>Mobile</i>
                                                                        </label>
                                                                        <label class="radioPure radio-inline">
                                                                            <input type="radio" name="Device" <%= item.Device == 2 ? "checked": "" %> value="2" />
                                                                            <span class="outer"><span class="inner"></span></span><i>Tablet</i>
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Mã thiết kế</label>
                                                                <div class="col-md-9">
                                                                    <textarea class="form-control custom" name="Custom" id="Custom" rows="20" cols=""><%=item.Custom %></textarea>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="design">
                                        <script type="text/javascript">

                                            var ListOnLoad = new Array();
                                            var has_update = false;

                                            function get_content_design() {
                                                var vswContainer = window.frames["frame_design"].document.getElementById("vsw_container");
                                                if (vswContainer)
                                                    document.getElementById("content_design").innerHTML = vswContainer.innerHTML;
                                                else
                                                    document.getElementById("content_design").innerHTML = window.frames["frame_design"].document.childNodes[0].innerHTML;

                                                if (has_update)
                                                    custom_update();

                                                for (var i = 0; i < ListOnLoad.length; i++) {
                                                    layout_change(ListOnLoad[i].pid, ListOnLoad[i].list_param, ListOnLoad[i].layout)
                                                }

                                                $('[data-toggle="tooltip"]').tooltip();
                                            }

                                            function custom_update() {
                                                var ranNum = Math.floor(Math.random() * 999999);
                                                var dataString = "";
                                                $.ajax({
                                                    url: "/{CPPath}/Ajax/TemplateGetCustom/<%= item.ID %>.aspx?rnd=" + ranNum,
                                                    type: "get",
                                                    data: dataString,
                                                    dataType: 'json',
                                                    success: function (data) {
                                                        var content = data.Html;
                                                        if (content !== "") $("#Custom").html(content);
                                                    },
                                                    error: function (status) { }
                                                });
                                            }

                                            function cp_update(value) {

                                                has_update = true;

                                                var input = document.createElement("input");
                                                input.type = "hidden";
                                                input.name = "vsw_submit";
                                                input.value = value;

                                                var cpForm = window.frames["frame_design"].document.forms[0];

                                                var arrTagInput = window.frames["frame_design"].document.getElementsByTagName("input");
                                                var i;
                                                for (i = 0; i < arrTagInput.length; i++) {
                                                    if (document.getElementById(arrTagInput[i].id))
                                                        arrTagInput[i].value = document.getElementById(arrTagInput[i].id).value;
                                                }

                                                var arrTagSelect = window.frames["frame_design"].document.getElementsByTagName("select");
                                                for (i = 0; i < arrTagSelect.length; i++) {
                                                    if (document.getElementById(arrTagSelect[i].id))
                                                        arrTagSelect[i].value = document.getElementById(arrTagSelect[i].id).value;
                                                }

                                                cpForm.appendChild(input);
                                                cpForm.submit();

                                            }

                                            function dragStart(ev) {
                                                ev.dataTransfer.effectAllowed = "move";
                                                ev.dataTransfer.setData("Text", ev.target.getAttribute("id"));
                                                ev.dataTransfer.setDragImage(ev.target, 0, 0);
                                                return true;
                                            }
                                            function dragEnter(ev) {
                                                ev.preventDefault();
                                                return true;
                                            }
                                            function dragOver() {
                                                return false;
                                            }
                                            function dragDrop(ev) {
                                                var src = ev.dataTransfer.getData("Text");

                                                var id = ev.target.id;

                                                if (id !== "to_vswid_" + src)
                                                    cp_update(id + "$" + src + "|move");

                                                //ev.target.appendChild(document.getElementById(src));

                                                ev.stopPropagation();
                                                return false;
                                            }
                                            function do_display(idTbl) {
                                                var o = document.getElementById(idTbl);
                                                if (o.style.display === "")
                                                    o.style.display = "none";
                                                else
                                                    o.style.display = "";
                                            }
                                        </script>

                                        <div id="content_design"></div>
                                        <iframe id="frame_design" name="frame_design" src="/{CPPath}/Design/EditTemplate.aspx?id=<%= model.RecordID %>" onload="get_content_design()" style="height: 0; width: 0; border-width: 0;"></iframe>
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
