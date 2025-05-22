<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Core.Design.ViewControlDesign" %>
<%@ Import Namespace="VSW.Core.Design" %>

<%var template = SysTemplateService.Instance.GetByID(ViewPageDesign.CurrentTemplate.ID); %>

<div id="to_vswid_<%= ViewControlName%>$<%=CphName %>" ondragenter="return dragEnter(event)" ondrop="return dragDrop(event)" ondragover="return dragOver(event)"></div>

<div class="border-control">
    <div id="<%= ViewControlName%>$<%=CphName %>" <%if (!AloneMode)
        { %>style="cursor: move;"
        draggable="true" ondragstart="return dragStart(event)" <%} %>>
        <a href="javascript:void(0)" onclick="do_display('tbl_<%= ViewControlName%>');">
            <%= ViewControlID%> <%if (CurrentModule != null)
                                    { %> - <%= ((VSW.Lib.MVC.ModuleInfo)CurrentModule).Name %><%}
                                                                                                  else if (ErrorMode)
                                                                                                  { %> - Không tồn tại <%} %>
        </a>
    </div>

    <div class="form-body mt10" id="tbl_<%= ViewControlName%>" style="display: none;">
        <%
            var layoutDefine = string.Empty;
            var layoutValue = string.Empty;

            for (var i = 0; CurrentObject != null && i < CurrentObject.GetFields_Name().Length; i++)
            {
                var fieldName = CurrentObject.GetFields_Name()[i];

                List<VSW.Lib.Global.ListItem.Item> list = null;

                var fieldInfo = CurrentObject.GetFieldInfo(fieldName);

                var attributes = fieldInfo.GetCustomAttributes(typeof(VSW.Core.MVC.PropertyInfo), true);
                if (attributes.GetLength(0) == 0)
                    continue;

                var propertyInfo = (VSW.Core.MVC.PropertyInfo)attributes[0];

                var currentTitle = propertyInfo.Key;

                if (fieldName == "LayoutDefine")
                {
                    layoutDefine = currentTitle;
                    continue;
                }

                if (propertyInfo.Value is string)
                    list = VSW.Lib.Global.ListItem.List.GetListByText(propertyInfo.Value.ToString());

                if (list != null && VSW.Lib.Global.ListItem.List.FindByName(list, "ConfigKey").Value != "")
                {
                    list = VSW.Lib.Global.ListItem.List.GetListByText("," + VSW.Core.Global.Config.GetValue(VSW.Lib.Global.ListItem.List.FindByName(list, "ConfigKey").Value));
                }

                if (fieldName.IndexOf("PageID", StringComparison.Ordinal) > -1)
                {
                    if (ViewPageDesign.PageViewState["EditControl_PageID_" + LangID] != null)
                    {
                        list = ViewPageDesign.PageViewState["EditControl_PageID_" + LangID] as List<VSW.Lib.Global.ListItem.Item>;
                    }
                    else
                    {
                        list = VSW.Lib.Global.ListItem.List.GetList(SysPageService.Instance, LangID);

                        ViewPageDesign.PageViewState["EditControl_PageID_" + LangID] = list;
                    }
                }

                if (fieldName.IndexOf("MenuID", StringComparison.Ordinal) > -1)
                {
                    var typeMenu = string.Empty;
                    if (list != null)
                        typeMenu = VSW.Lib.Global.ListItem.List.FindByName(list, "Type").Value;

                    var keyViewState = "EditControl_MenuID_" + typeMenu + "_" + LangID;

                    if (ViewPageDesign.PageViewState[keyViewState] != null)
                    {
                        list = ViewPageDesign.PageViewState[keyViewState] as List<VSW.Lib.Global.ListItem.Item>;
                    }
                    else
                    {
                        list = VSW.Lib.Global.ListItem.List.GetList(WebMenuService.Instance, LangID, typeMenu);

                        ViewPageDesign.PageViewState[keyViewState] = list;
                    }
                }

                if (fieldName.IndexOf("Theme", StringComparison.Ordinal) > -1)
                {
                    var keyViewState = "EditControl_Theme_" + LangID;

                    if (ViewPageDesign.PageViewState[keyViewState] != null)
                    {
                        list = ViewPageDesign.PageViewState[keyViewState] as List<VSW.Lib.Global.ListItem.Item>;
                    }
                    else
                    {
                        list = new List<VSW.Lib.Global.ListItem.Item>();
                        list.Insert(0, new VSW.Lib.Global.ListItem.Item("Kiểu 4", "theme4"));
                        list.Insert(0, new VSW.Lib.Global.ListItem.Item("Kiểu 3", "theme3"));
                        list.Insert(0, new VSW.Lib.Global.ListItem.Item("Kiểu 2", "theme2"));
                        list.Insert(0, new VSW.Lib.Global.ListItem.Item("Kiểu 1", "theme_default"));

                        ViewPageDesign.PageViewState[keyViewState] = list;
                    }
                }

                if (fieldName.IndexOf("MultiRecord", StringComparison.Ordinal) > -1)
                {
                    var keyViewState = "EditControl_Data_" + LangID;

                    if (ViewPageDesign.PageViewState[keyViewState] != null)
                    {
                        list = ViewPageDesign.PageViewState[keyViewState] as List<VSW.Lib.Global.ListItem.Item>;
                    }
                    else
                    {
                        list = new List<VSW.Lib.Global.ListItem.Item>();
                        list.Insert(0, new VSW.Lib.Global.ListItem.Item("Lấy nhiều bản ghi", "1"));
                        list.Insert(0, new VSW.Lib.Global.ListItem.Item("Lấy 1 bản ghi", "0"));

                        ViewPageDesign.PageViewState[keyViewState] = list;
                    }
                }

                var currentId = ViewControlID + "_" + fieldName;
                var currentValue = GetValueCustom(fieldName);

                if (currentValue == string.Empty)
                    currentValue = (CurrentObject.GetField(fieldName) == null ? string.Empty : CurrentObject.GetField(fieldName).ToString());

        %>
        <div class="form-group row" id="tr_<%= currentId%>">
            <label class="col-md-3 col-form-label text-right"><%= currentTitle%>:</label>
            <div class="col-md-9">
                <%if (list != null)
                    { %>
                <select class="form-control" id="<%= currentId%>" name="<%= currentId%>">
                    <option selected="selected" value="">-</option>
                    <%for (var j = 0; j < list.Count; j++)
                        {
                            if (string.IsNullOrEmpty(list[j].Name)) continue;
                    %>
                    <option <%if (currentValue == list[j].Value)
                        {%>selected<%} %>
                        value="<%= list[j].Value%>"><%= list[j].Name%></option>
                    <%} %>
                </select>
                <%}
                    else
                    { %>

                <%if (fieldName.IndexOf("NewsID", System.StringComparison.Ordinal) > -1)
                    { %>
                <input type="text" class="form-control" id="<%= currentId%>" name="<%= currentId%>" onclick="ShowNewsForm('<%= currentId%>',<%=currentValue %>); return false;" value="<%=currentValue %>" placeholder="Click chọn <%= currentTitle%>" data-toggle="tooltip" data-placement="bottom" data-original-title="Click chọn <%= currentTitle%>" />
                <%}
                    else if (fieldName.IndexOf("Text", System.StringComparison.Ordinal) > -1)
                    {
                        VSW.Lib.Global.Session.SetValue(currentId, currentValue);
                %>
                <input type="text" class="form-control" id="<%= currentId%>" name="<%= currentId%>" onclick="ShowTextForm('<%= currentId%>','');return false;" value="<%=currentValue %>" placeholder="Click chọn  <%= currentTitle%>" data-toggle="tooltip" data-placement="bottom" data-original-title="Click chọn <%= currentTitle%>" />
                <%}
                    else if (fieldName.IndexOf("File", StringComparison.Ordinal) > -1)
                    {%>
                <input type="text" class="form-control" id="<%= currentId%>" name="<%= currentId%>" onclick="ShowFileForm('<%= currentId%>','<%=currentValue %>');return false;" value="<%=currentValue %>" placeholder="Click chọn <%= currentTitle%>" data-toggle="tooltip" data-placement="bottom" data-original-title="Click chọn <%= currentTitle%>" />
                <%}
                    else if (fieldName.IndexOf("Date", StringComparison.Ordinal) > -1)
                    {%>
                <input type="text" class="form-control date" id="<%= currentId%>" name="<%= currentId%>" value="<%=currentValue %>" placeholder="Chọn <%= currentTitle%>" data-toggle="tooltip" data-placement="bottom" data-original-title="Chọn <%= currentTitle%>" />
                <%}
                    else if (fieldName.IndexOf("PageSize", StringComparison.Ordinal) > -1)
                    {%>
                <input type="number" min="0" class="form-control" id="<%= currentId%>" name="<%= currentId%>" value="<%=currentValue %>" placeholder="Nhập <%= currentTitle%>" data-toggle="tooltip" data-placement="bottom" data-original-title="Nhập <%= currentTitle%>" />
                <%}
                    else
                    {%>
                <input type="text" class="form-control" id="<%= currentId%>" name="<%= currentId%>" value="<%=currentValue %>" placeholder="Nhập <%= currentTitle%>" data-toggle="tooltip" data-placement="bottom" data-original-title="Nhập <%= currentTitle%>" />
                <%} %>

                <%} %>
            </div>
        </div>
        <%} %>

        <%if (CurrentModule != null)
            {
                var path = Server.MapPath("~/Views/" + CurrentModule.Code);
                if (System.IO.Directory.Exists(path))
                {
                    var arrFiles = System.IO.Directory.GetFiles(path);

                    var list = new List<VSW.Lib.Global.ListItem.Item>();

                    foreach (var file in arrFiles)
                    {
                        var s = System.IO.Path.GetFileNameWithoutExtension(file);
                        if (string.IsNullOrEmpty(s)) continue;

                        switch (template.Device)
                        {
                            case 1:
                                if (s.EndsWith("Mobile"))
                                    list.Add(new VSW.Lib.Global.ListItem.Item(s, s));
                                break;
                            case 2:
                                if (s.EndsWith("Tablet"))
                                    list.Add(new VSW.Lib.Global.ListItem.Item(s, s));
                                break;
                            default:
                                if (!s.EndsWith("Mobile") && !s.EndsWith("Tablet"))
                                    list.Add(new VSW.Lib.Global.ListItem.Item(s, s));
                                break;
                        }
                    }

                    var currentValue = GetValueCustom("ViewLayout");

                    if (currentValue == string.Empty)
                        currentValue = (CurrentObject.GetPropertyInfo("ViewLayout") == null ? string.Empty : CurrentObject.GetPropertyInfo("ViewLayout").ToString());

                    layoutValue = currentValue;
        %>
        <div class="form-group row" id="tr_<%= ViewControlID%>_ViewLayout">
            <label class="col-md-3 col-form-label text-right">Hiển thị:</label>
            <div class="col-md-9">
                <select class="form-control" id="<%= ViewControlID%>_ViewLayout" name="<%= ViewControlID%>_ViewLayout" <%if (layoutDefine != "")
                    { %>onchange="layout_change('<%= ViewControlID%>', '<%=layoutDefine %>', this.value)"
                    <%} %>>
                    <option selected value="">-</option>
                    <%for (var j = 0; j < list.Count; j++)
                        {
                            if (string.IsNullOrEmpty(list[j].Name)) continue;
                    %>
                    <option <%if (currentValue == list[j].Value)
                        {%>selected<%} %>
                        value="<%= list[j].Value%>"><%= list[j].Name%></option>
                    <%} %>
                </select>
            </div>
        </div>
        <%}
            } %>

        <div class="cmd-control justify-content-end">

            <%if (!ReadOnlyMode)
                { %>
            <a href="javascript:cp_update('<%= ViewControlID%>|save');" data-toggle="tooltip" data-placement="bottom" data-original-title="Lưu thay đổi"><i class="icon-check"></i></a>
            <%} %>

            <%if (!AloneMode)
                { %>

            <%if ((PosID & 1) != 1)
                { %>
            <a href="javascript:cp_update('<%= ViewControlID%>|up')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển lên trên"><i class="fa fa-toggle-up"></i></a>
            <%} %>
            <%if ((PosID & 4) != 4)
                { %>
            <a href="javascript:cp_update('<%= ViewControlID%>|down')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển xuống dưới"><i class="fa fa-toggle-down"></i></a>
            <%} %>

            <%} %>

            <%if (!AloneMode)
                { %>
            <a href="javascript:void(0)" onclick="showSwalQuestion('Bạn chắc là mình muốn xóa chứ !', 'Thông báo !', (flag) => {if(flag){cp_update('<%= ViewControlID%>|delete');}})" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa điều khiển"><i class="fa fa-times-circle"></i></a>
            <%} %>
        </div>
    </div>
</div>

<%if (layoutDefine != string.Empty)
    { %>
<script type="text/javascript">
    window.parent.ListOnLoad.push({ pid: "<%= ViewControlID%>", list_param: "<%=layoutDefine %>", layout: "<%=layoutValue %>" });
</script>
<%} %>