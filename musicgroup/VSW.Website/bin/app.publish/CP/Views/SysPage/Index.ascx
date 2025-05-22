<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<script runat="server">
    private List<EntityBase> AutoGetMap(SysPageModel model)
    {
        List<EntityBase> list = new List<EntityBase>();

        int _id = model.ParentID;
        while (_id > 0)
        {
            var _item = SysPageService.Instance.GetByID(_id);

            if (_item == null)
                break;

            _id = _item.ParentID;

            list.Insert(0, _item);
        }

        return list;
    }
</script>

<%
    var model = ViewBag.Model as SysPageModel;
    var listItem = ViewBag.Data as List<SysPageEntity>;

    var parent = listItem != null ? SysPageService.Instance.GetByID_Cache(listItem[0].ParentID) : null;
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Trang</h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <%= ShowMap(AutoGetMap(model))%>
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
                            <%=GetDefaultListCommandV2("upload|Thêm nhiều|btn-primary")%>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
                            <div class="row">
                                <div class="col-md-8 col-sm-12"></div>
                                <div class="col-md-4 col-sm-12">
                                    <div class="col-12 col-sm-12 col-md-8 col-lg-8 text-right pull-right">
                                        <div class="table-group-actions d-inline-block">
                                            <%= ShowDDLLang(model.LangID)%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="portlet portlet mt-2 hide" id="boxActionHide">
                                <div class="portlet-title">
                                    <div class="actions btn-set" style="float: left">
                                        <span class="mr-2">Đã chọn: <span id="countChose">0</span></span>
                                        <%=GetDefaultListHideCommandV2()%>
                                    </div>
                                </div>
                            </div>
                            <div class="table-scrollable">
                                <table class="table table-striped table-hover table-bordered dataTable">
                                    <thead>
                                        <tr>
                                            <th class="sorting text-center w1p">#</th>
                                            <th class="sorting_disabled text-center w1p">
                                                <label class="itemCheckBox itemCheckBox-sm">
                                                    <input type="checkbox" name="toggle" onclick="checkAll(<%= model.PageSize %>)">
                                                    <i class="check-box"></i>
                                                </label>
                                            </th>
                                            <th class="sorting_disabled text-center w1p">#</th>
                                            <th class="sorting"><%= GetSortLink("Tên trang", "Name")%></th>

                                            <%if (model.ParentID > 0)
                                                {%>
                                            <th class="sorting text-center desktop"><%= GetSortLink("Mã", "Code")%></th>
                                            <th class="sorting text-center desktop"><%= GetSortLink("Chức năng", "ModuleCode")%></th>
                                            <th class="sorting text-center desktop"><%= GetSortLink("Mẫu giao diện", "TemplateID")%></th>
                                            <th class="sorting text-center desktop"><%= GetSortLink("Mobile", "TemplateMobileID")%></th>
                                            <th class="sorting text-center w10p desktop"><%= GetSortLink("Vị trí", "State")%></th>
											<%} %>
                                            <th class="sorting text-center w1p"><%= GetSortLink("Duyệt", "Activity")%></th>
                                            <th class="sorting text-center w10p desktop">
                                                <%= GetSortLink("Sắp xếp", "Order")%>
                                                <a href="javascript:vsw_exec_cmd('saveorder')" class="saveorder" data-toggle="tooltip" data-placement="bottom" data-original-title="Lưu sắp xếp"><i class="fa fa-save"></i></a>
                                            </th>
                                            <th class="sorting text-center w1p"><%= GetSortLink("ID", "ID")%></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%for (var i = 0; listItem != null && i < listItem.Count; i++)
                                            {
                                                var moduleInfo = SysModuleService.Instance.VSW_Core_GetByCode(listItem[i].ModuleCode) as VSW.Lib.MVC.ModuleInfo;
                                        %>
                                        <tr>
                                            <td class="text-center"><%= i + 1%></td>
                                            <td class="text-center">
                                                <%= GetCheckbox(listItem[i].ID, i)%>
                                            </td>
                                            <td class="text-center">
                                                <a href="javascript:VSWRedirect('Add', <%= listItem[i].ID %>)"><i class="fa fa-pencil-square-o"></i></a>
                                            </td>
                                            <td>
                                                <a href="javascript:VSWRedirect('Index', <%= listItem[i].ID %>, 'ParentID')"><%= listItem[i].Name%></a>
												<%if (!string.IsNullOrEmpty(listItem[i].AliasName))
                                                {%>
												<br/>
												(<%= listItem[i].AliasName%>)
												<%}%>
                                            </td>

                                            <%if (model.ParentID > 0)
                                                {%>
                                            <td class="text-center">
                                                <%= listItem[i].Code %>
                                            </td>
                                            <td class="text-center">
                                                <%= moduleInfo == null ? string.Empty : moduleInfo.Name%>
                                            </td>
                                            <td class="text-center">
                                                <%= GetName(SysTemplateService.Instance.GetByID(listItem[i].TemplateID))%>
                                            </td>
                                            <td class="text-center">
                                                <%= GetName(SysTemplateService.Instance.GetByID(listItem[i].TemplateMobileID))%>
                                            </td>
                                            <td class="text-center"><%= Utils.ShowNameByConfigkey("Mod.PageState", listItem[i].State)%></td>
                                            <%} %>
                                            <td class="text-center">
                                                <%= GetPublish(listItem[i].ID, listItem[i].Activity)%>
                                            </td>
                                            <td class="text-center">
                                                <%= GetOrder(listItem[i].ID, listItem[i].Order)%>
                                            </td>
                                            <td class="text-center"><%= listItem[i].ID%></td>
                                        </tr>
                                        <%} %>
                                    </tbody>
                                </table>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-sm-12 justify-content-center d-flex ">
                                    <%= GetPagination(model.PageIndex, model.PageSize, model.TotalRecord)%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        var VSWController = "SysPage";

        var VSWArrVar = [
            "filter_lang", "LangID",
            "limit", "PageSize"
        ];

        var VSWArrQT = [
            "<%= model.PageIndex + 1 %>", "PageIndex",
            "<%= model.ParentID %>", "ParentID",
            "<%= model.Sort %>", "Sort"
        ];

        var VSWArrDefault = [
            "1", "PageIndex",
            "1", "LangID",
            "20", "PageSize"
        ];
    </script>

</form>
