<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<script runat="server">

    List<CPAccessEntity> listUserAccess = null;
    SysRoleModel model;
    CPRoleEntity item;
    protected void Page_Load(object sender, EventArgs e)
    {
        model = ViewBag.Model as SysRoleModel;
        item = ViewBag.Data as CPRoleEntity;

        if (model.RecordID > 0)
        {
            listUserAccess = CPAccessService.Instance.CreateQuery()
                                 .Where(o => o.Type == "CP.MODULE" && o.RoleID == model.RecordID)
                                 .ToList();
        }
    }

    int GetAccess(string refCode)
    {
        if (listUserAccess == null)
            return 0;

        var obj = listUserAccess.Find(o => o.RefCode == refCode);

        return obj == null ? 0 : obj.Value;
    }
</script>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Nhóm người sử dụng <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Nhóm người sử dụng</a>
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
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Thông tin chung</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tên nhóm:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Name" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Quyền:</label>
                                                    <div class="col-md-9">
                                                        <div class="table-scrollable">
                                                            <table class="table table-striped table-hover table-bordered dataTable">
                                                                <thead>
                                                                    <tr>
                                                                        <th class="sorting_disabled w1p">#</th>
                                                                        <th class="sorting_disabled w10p text-center">
                                                                            <p>Duyệt</p>
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" onclick="javascript:vsw_checkAll(document.forms[0], 'ArrApprove', this.checked)" />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </th>
                                                                        <th class="sorting_disabled w10p text-center">
                                                                            <p>Xóa</p>
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" onclick="javascript: vsw_checkAll(document.forms[0], 'ArrDelete', this.checked)" />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </th>
                                                                        <th class="sorting_disabled w10p text-center">
                                                                            <p>Sửa</p>
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" onclick="javascript: vsw_checkAll(document.forms[0], 'ArrEdit', this.checked)" />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </th>
                                                                        <th class="sorting_disabled w10p text-center">
                                                                            <p>Thêm</p>
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" onclick="javascript: vsw_checkAll(document.forms[0], 'ArrAdd', this.checked)" />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </th>
                                                                        <th class="sorting_disabled w10p text-center">
                                                                            <p>Truy cập</p>
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" onclick="javascript: vsw_checkAll(document.forms[0], 'ArrView', this.checked)" />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </th>
                                                                        <th class="sorting_disabled">Tên Chức năng</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>1</td>
                                                                        <td class="text-center">
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" name="ArrApprove" value="SysAdministrator" disabled="disabled" />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </td>
                                                                        <td class="text-center">
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" name="ArrApprove" value="SysAdministrator" disabled="disabled" />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </td>
                                                                        <td class="text-center">
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" name="ArrApprove" value="SysAdministrator" disabled="disabled" />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </td>
                                                                        <td class="text-center">
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" name="ArrApprove" value="SysAdministrator" disabled="disabled" />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </td>
                                                                        <td class="text-center">
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" name="ArrView" value="SysAdministrator" <%if ((GetAccess("SysAdministrator") & 1) == 1){ %>checked="checked" <%} %> />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </td>
                                                                        <td>Quản trị hệ thống</td>
                                                                    </tr>

                                                                    <%for (var i = 0; i < VSW.Lib.Web.Application.CPModules.OrderBy(o => o.Order).ToList().Count; i++){ %>
                                                                    <tr>
                                                                        <td><%=i + 2 %></td>
                                                                        <td class="text-center">
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" name="ArrApprove" value="<%= VSW.Lib.Web.Application.CPModules[i].Code%>" <%if ((GetAccess(VSW.Lib.Web.Application.CPModules[i].Code) & 16) == 16){ %>checked="checked" <%} %> <%if ((VSW.Lib.Web.Application.CPModules[i].Access & 16) != 16){ %>disabled="disabled" <%} %> />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </td>
                                                                        <td class="text-center">
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" name="ArrDelete" value="<%= VSW.Lib.Web.Application.CPModules[i].Code%>" <%if ((GetAccess(VSW.Lib.Web.Application.CPModules[i].Code) & 8) == 8){ %>checked="checked" <%} %> <%if ((VSW.Lib.Web.Application.CPModules[i].Access & 8) != 8){ %>disabled="disabled" <%} %> />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </td>
                                                                        <td class="text-center">
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" name="ArrEdit" value="<%= VSW.Lib.Web.Application.CPModules[i].Code%>" <%if ((GetAccess(VSW.Lib.Web.Application.CPModules[i].Code) & 4) == 4){ %>checked="checked" <%} %> <%if ((VSW.Lib.Web.Application.CPModules[i].Access & 4) != 4){ %>disabled="disabled" <%} %> />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </td>
                                                                        <td class="text-center">
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" name="ArrAdd" value="<%= VSW.Lib.Web.Application.CPModules[i].Code%>" <%if ((GetAccess(VSW.Lib.Web.Application.CPModules[i].Code) & 2) == 2){ %>checked="checked" <%} %> <%if ((VSW.Lib.Web.Application.CPModules[i].Access & 2) != 2){ %>disabled="disabled" <%} %> />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </td>
                                                                        <td class="text-center">
                                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                                <input type="checkbox" name="ArrView" value="<%= VSW.Lib.Web.Application.CPModules[i].Code%>" <%if ((GetAccess(VSW.Lib.Web.Application.CPModules[i].Code) & 1) == 1){ %>checked="checked" <%} %> <%if ((VSW.Lib.Web.Application.CPModules[i].Access & 1) != 1){ %>disabled="disabled" <%} %> />
                                                                                <i class="check-box"></i>
                                                                            </label>
                                                                        </td>
                                                                        <td><%= VSW.Lib.Web.Application.CPModules[i].Description%></td>
                                                                    </tr>
                                                                    <%} %>
                                                                </tbody>
                                                            </table>
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
    </div>
</form>