<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as WebPropertyConfigModel;
    var listItem = ViewBag.Data as Dictionary<WebPropertyEntity, List<WebPropertyEntity>>;
    var ListBrand = ViewBag.ListBrand as List<WebMenuEntity>;
    int indexSub = -1;
    var listProperty = WebPropertyConfigService.Instance.CreateQuery()
                        .Where(o => o.MenuID == model.MenuID)
                        .ToList_Cache();
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Thông số thuộc tính</h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Thông số thuộc tính</a>
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
                <div class="portlet portlet">
                    <div class="portlet-title">
                        <div class="actions btn-set">
                            <%=GetListCommand("apply|Lưu|btn-primary,config|Xóa cache|purple")%>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
                            <div class="row hidden-sm hidden-col">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12 text-right pull-right">
                                    <div class="table-group-actions d-inline-block">
                                        <label>Chuyên mục</label>
                                        <select id="filter_menu" class="form-select select2 input-inline input-sm" onchange="VSWRedirect()" size="1">
                                            <option value="0">(Tất cả)</option>
                                            <%= Utils.ShowDdlMenuByType("Product", model.LangID, model.MenuID)%>
                                        </select>
                                    </div>
                                    <div class="table-group-actions d-inline-block">
                                        <%= ShowDDLLang(model.LangID)%>
                                    </div>
                                </div>
                            </div>

                            <div class="table-scrollable">
                                <table class="table table-striped table-hover table-bordered dataTable">
                                    <thead>
                                        <tr>
                                            <th class="sorting text-center w1p">#</th>
                                            <th class="sorting">Tên Thuộc tính</th>
                                            <th class="sorting text-center w20p desktop">Hiển thị ở menu</th>
                                            <th class="sorting text-center w20p desktop">Hiển thị lọc nhanh</th>
                                            <th class="sorting text-center w20p desktop">Hiển thị breadcrumb</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%if (listItem != null)
                                            { %>
                                        <tr style="font-weight: bold;">
                                            <td class="text-center">1</td>
                                            <td colspan="5">Hãng</td>
                                        </tr>
                                        <%for (var i = 0; ListBrand != null && i < ListBrand.Count; i++)
                                            {
                                                indexSub++;
                                                bool IsShowMenu = false;
                                                bool ShowFilterFast = false;
                                                bool ShowBreadCrumb = false;
                                                int ID = 0;
                                                if (listProperty != null)
                                                {
                                                    var config = listProperty.Where(o => o.BrandID == ListBrand[i].ID).FirstOrDefault();
                                                    if (config != null)
                                                    {
                                                        IsShowMenu = config.IsShowMenu;
                                                        ShowFilterFast = config.ShowFilterFast;
                                                        ShowBreadCrumb = config.ShowBreadCrumb;
                                                        ID = config.ID;
                                                    }
                                                }
                                        %>
                                        <tr>
                                            <td class="text-center"><%=i+1 %>
                                                <input type="hidden" name="ID<%=indexSub %>" id="ID<%=indexSub %>" value="<%=ID %>" />
                                                <input type="hidden" name="BrandID<%=indexSub %>" id="BrandID<%=indexSub %>" value="<%=ListBrand[i].ID %>" />
                                                <input type="hidden" name="PropertyID<%=indexSub %>" id="PropertyID<%=indexSub %>" value="0" />
                                            </td>
                                            <td><%=ListBrand[i].Name %></td>
                                            <td class="text-center">
                                                <input type="checkbox" id="ShowMenu<%=indexSub %>" name="ShowMenu<%=indexSub %>" <%=IsShowMenu ? "checked" : "" %> value="1" /></td>
                                            <td class="text-center">
                                                <input type="checkbox" id="ShowFilterFast<%=indexSub %>" name="ShowFilterFast<%=indexSub %>" <%=ShowFilterFast ? "checked" : "" %> value="1" /></td>
                                            <td class="text-center">
                                                <input type="checkbox" id="ShowBreadCrumb<%=indexSub %>" name="ShowBreadCrumb<%=indexSub %>" <%=ShowBreadCrumb ? "checked" : "" %> value="1" /></td>
                                        </tr>
                                        <%} %>
                                        <%  int index = 1;
                                            foreach (var item in listItem.Keys)
                                            {
                                                index++;
                                                var listChildItem = listItem[item];
                                                if (listChildItem == null) continue;
                                                if (listChildItem.Count < 1) continue;
                                        %>
                                        <tr style="font-weight: bold;">
                                            <td class="text-center"><%=index %></td>
                                            <td colspan="5"><%=item.Name %></td>
                                        </tr>
                                        <%for (var i = 0; listChildItem != null && i < listChildItem.Count; i++)
                                            {
                                                indexSub++;
                                                bool IsShowMenu = false;
                                                bool ShowFilterFast = false;
                                                bool ShowBreadCrumb = false;
                                                int ID = 0;

                                                if (listProperty != null)
                                                {
                                                    var config = listProperty.Where(o => o.PropertyID == listChildItem[i].ID).FirstOrDefault();
                                                    if (config != null)
                                                    {
                                                        IsShowMenu = config.IsShowMenu;
                                                        ShowFilterFast = config.ShowFilterFast;
                                                        ShowBreadCrumb = config.ShowBreadCrumb;
                                                        ID = config.ID;
                                                    }
                                                }
                                        %>
                                        <tr>
                                            <td class="text-center"><%=i+1 %>
                                                <input type="hidden" name="ID<%=indexSub %>" id="ID<%=indexSub %>" value="<%=ID %>" />
                                                <input type="hidden" name="BrandID<%=indexSub %>" id="BrandID<%=indexSub %>" value="0" />
                                                <input type="hidden" name="PropertyID<%=indexSub %>" id="PropertyID<%=indexSub %>" value="<%=listChildItem[i].ID %>" />
                                            </td>
                                            <td><%=listChildItem[i].Name %></td>
                                            <td class="text-center">
                                                <input type="checkbox" id="ShowMenu<%=indexSub %>" name="ShowMenu<%=indexSub %>" <%=IsShowMenu ? "checked" : "" %> value="1" /></td>
                                            <td class="text-center">
                                                <input type="checkbox" id="ShowFilterFast<%=indexSub %>" name="ShowFilterFast<%=indexSub %>" <%=ShowFilterFast ? "checked" : "" %> value="1" /></td>
                                            <td class="text-center">
                                                <input type="checkbox" id="ShowBreadCrumb<%=indexSub %>" name="ShowBreadCrumb<%=indexSub %>" <%=ShowBreadCrumb ? "checked" : "" %> value="1" /></td>
                                        </tr>
                                        <%} %>
                                        <%} %>
                                        <%} %>
                                    </tbody>
                                </table>
                                <input type="hidden" name="CountProperty" id="CountProperty" value="<%=indexSub %>" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        var VSWController = "WebPropertyConfig";

        var VSWArrVar = [
            "filter_menu", "MenuID",
            "filter_lang", "LangID",
            "limit", "PageSize"
        ];

        var VSWArrVar_QS = [
        ];

        var VSWArrQT = [
            "<%= model.PageIndex + 1 %>", "PageIndex",
            "<%= model.Sort %>", "Sort"
        ];

        var VSWArrDefault = [
            "1", "PageIndex",
            "1", "LangID",
            "20", "PageSize"
        ];
    </script>

</form>
