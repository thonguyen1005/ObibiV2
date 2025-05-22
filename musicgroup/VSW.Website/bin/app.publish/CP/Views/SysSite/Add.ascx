<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var listLang = SysLangService.Instance.CreateQuery().ToList();
    var model = ViewBag.Model as SysSiteModel;
    var item = ViewBag.Data as SysSiteEntity;
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <div class="page-content-wrapper">
        <h3 class="page-title">Site <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Site</a>
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
                            <div class="portlet box box-boder ">
                                <div class="portlet-body">
                                    <div class="form-body">
                                        <div class="form-group row">
                                            <label class="col-md-3 col-form-label text-right">Tên site:</label>
                                            <div class="col-md-9">
                                                <input type="text" class="form-control" name="Name" value="<%=item.Name %>" />
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-md-3 col-form-label text-right">Mã site:</label>
                                            <div class="col-md-9">
                                                <input type="text" class="form-control" name="Code" value="<%=item.Code %>" />
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-md-3 col-form-label text-right">Trang mặc định :</label>
                                            <div class="col-md-9">
                                                <div id="list_page"></div>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-md-3 col-form-label text-right">Ngôn ngữ:</label>
                                            <div class="col-md-9">
                                                <select class="form-select select2" onchange="lang_change(this.value)" name="LangID" id="LangID">
                                                    <%for (var i = 0; listLang != null && i < listLang.Count; i++)
                                                        { %>
                                                    <option <%if (item.LangID == listLang[i].ID)
                                                        {%>selected<%} %>
                                                        value="<%= listLang[i].ID%>">&nbsp; <%= listLang[i].Name%></option>
                                                    <%} %>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-md-3 col-form-label text-right">Mã thiết kế:</label>
                                            <div class="col-md-9">
                                                <textarea class="form-control" rows="5" name="Custom"><%=item.Custom%></textarea>
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
        var ddl_lang = document.getElementById("LangID");

        function lang_change(langID) {
            var ranNum = Math.floor(Math.random() * 999999);
            var dataString = "LangID=" + langID + "&PageID=<%=item.PageID %>&rnd=" + ranNum;
            $.ajax({
                url: "/{CPPath}/Ajax/SiteGetPage.aspx",
                type: "get",
                data: dataString,
                dataType: 'json',
                success: function (data) {
                    var content = data.Params;

                    content = "<select class=\"form-select select2\" name=\"PageID\" id=\"PageID\"><option value=\"0\">Root</option>" + content + "</select>";
                    $("#list_page").html(content);
                    App.initSelect2();
                },
                error: function (status) { }
            });
        }

        if (<%=item.LangID%> > 0) lang_change(<%=item.LangID %>);
        else if ($("#LangID").val() > 0) lang_change($("#LangID").val());
    </script>
</form>
