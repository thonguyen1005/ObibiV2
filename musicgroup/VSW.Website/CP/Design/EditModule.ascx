<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="VSW.Lib.Global.ListItem" %>

<script runat="server">
    public VSW.Core.Global.Class CurrentObject { get; set; }
    public SysPageEntity CurrentPage { get; set; }
    public VSW.Lib.MVC.ModuleInfo CurrentModule { get; set; }
    public int LangID { get; set; }
</script>

<div class="form-group row">
    <label class="col-md-3 col-form-label text-right">Thuộc tính:</label>
    <div class="col-md-9" id="list-property">

        <%for (var i = 0; CurrentObject != null && i < CurrentObject.GetFields_Name().Length; i++)
            {
                var fieldName = CurrentObject.GetFields_Name()[i];

                var fieldInfo = CurrentObject.GetFieldInfo(fieldName);
                var attributes = fieldInfo.GetCustomAttributes(typeof(VSW.Core.MVC.PropertyInfo), true);
                if (attributes.GetLength(0) == 0)
                    continue;

                var propertyInfo = (VSW.Core.MVC.PropertyInfo)attributes[0];

                var currentId = CurrentModule.Code + "_" + fieldName;
                var currentValue = string.Empty;

                if (CurrentPage != null)
                    currentValue = CurrentPage.Items.GetValue(CurrentModule.Code + "." + fieldName).ToString();

                if (currentValue == string.Empty)
                    currentValue = (CurrentObject.GetField(fieldName) == null ? string.Empty : CurrentObject.GetField(fieldName).ToString());

                List<Item> list = null;

                if (propertyInfo.Value is string)
                    list = List.GetListByText(propertyInfo.Value.ToString());

                if (list != null && List.FindByName(list, "ConfigKey").Value != string.Empty)
                {
                    if (VSW.Lib.Global.ListItem.List.FindByName(list, "ConfigKey").Value == "Mod.ProductState")
                    {
                        list = new List<VSW.Lib.Global.ListItem.Item>();
                        var listState = ModProductStateService.Instance.CreateQuery().ToList();
                        for (int z = 0; listState != null && z < listState.Count; z++)
                        {
                            list.Add(new VSW.Lib.Global.ListItem.Item(listState[z].Name, listState[z].Value.ToString()));
                        }
                    }
                    else
                    {
                        list = List.GetListByText("," + VSW.Core.Global.Config.GetValue(List.FindByName(list, "ConfigKey").Value));
                    }
                }

                if (fieldName.IndexOf("ViewLayoutByUser", StringComparison.Ordinal) > -1)
                {
                    list = new List<Item>();
                    list.Insert(0, new Item("Dạng danh mục", "Cate"));
                }

                if (fieldName.IndexOf("PageID", StringComparison.Ordinal) > -1)
                {
                    list = List.GetList(SysPageService.Instance);
                    //list.Insert(0, new Item(string.Empty, string.Empty));
                }

                if (fieldName.IndexOf("MenuID", StringComparison.Ordinal) > -1)
                {
                    var hasContinue = false;
                    var menuType = string.Empty;
                    if (list != null)
                    {
                        menuType = List.FindByName(list, "Type").Value;
                        hasContinue = !VSW.Core.Global.Convert.ToBool(List.FindByName(list, "Show").Value);
                    }

                    if (hasContinue)
                        continue;

                    list = List.GetList(WebMenuService.Instance, LangID, menuType);
                    //list.Insert(0, new Item(string.Empty, string.Empty));
                }

                if (fieldName.IndexOf("BrandID", StringComparison.Ordinal) > -1)
                {
                    var hasContinue = false;
                    var menuType = string.Empty;
                    if (list != null)
                    {
                        menuType = List.FindByName(list, "Type").Value;
                        hasContinue = !VSW.Core.Global.Convert.ToBool(List.FindByName(list, "Show").Value);
                    }

                    if (hasContinue)
                        continue;

                    list = List.GetList(WebMenuService.Instance, LangID, menuType);
                    //list.Insert(0, new Item("-", string.Empty));
                }
        %>
        <div class="form-group row" id="tr_<%= currentId %>">
            <label class="col-md-3 col-form-label text-right"><%= propertyInfo.Key %>:</label>
            <div class="col-md-7">
                <div class="form-inline">

                    <%if (list != null)
                        {%>
                    <select class="form-control" id="<%= currentId %>" name="<%= currentId %>" onchange="UpdateCustom('<%= currentId %>', ''); return false">
                        <%for (var j = 0; j < list.Count; j++)
                            {
                                if (string.IsNullOrEmpty(list[j].Name)) continue;
                        %>
                        <option <%=(currentValue == list[j].Value ? "selected" : "") %> value="<%= list[j].Value%>"><%= list[j].Name %></option>
                        <%}%>
                    </select>
                    <%}
                        else
                        {%>
                    <input type="text" class="form-control" id="<%= currentId %>" name="<%= currentId %>" onkeyup="UpdateCustom('<%= currentId %>', ''); return false" value="<%= currentValue %>" />
                    <%}%>

                    <%if (fieldName.StartsWith("NewsID"))
                        {%>
                    <a href="javascript:void(0)" class="col-form-label" onclick="ShowNewsForm('<%= currentId %>', '<%= currentValue %>'); return false"><i class="fa fa-search fa-2x"></i></a>
                    <%}
                        else if (fieldName.StartsWith("Text"))
                        {
                            VSW.Lib.Global.Session.SetValue(currentId, currentValue);
                    %>
                    <a href="javascript:void(0)" class="col-form-label" onclick="ShowTextForm('<%= currentId %>', '<%= currentValue %>'); return false"><i class="fa fa-search fa-2x"></i></a>
                    <%}
                        else if (fieldName.StartsWith("File"))
                        {%>
                    <a href="javascript:void(0)" class="col-form-label" onclick="ShowFileForm('<%= currentId %>', '<%= currentValue %>'); return false"><i class="fa fa-search fa-2x"></i></a>
                    <%}%>
                </div>
            </div>
        </div>
        <%}%>
    </div>
</div>
