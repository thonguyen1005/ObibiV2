<%@ Control Language="C#" AutoEventWireup="true" %>

<script runat="server">

    private string _cphName = string.Empty;
    public string CphName
    {
        get { return _cphName; }
        set { _cphName = value; }
    }

    List<VSW.Lib.MVC.ModuleInfo> _listModule;
    protected void Page_Load(object sender, EventArgs e)
    {
        _listModule = VSW.Lib.Web.Application.Modules.Where(o => o.IsControl == true).OrderBy(o => o.Order).ToList();
    }
</script>

    <div id="to_cph_<%= CphName%>" ondragenter="return dragEnter(event)" ondrop="return dragDrop(event)" ondragover="return dragOver(event)"></div>

    <div class="border-control">
        <a href="javascript:void(0)" onclick="do_display('tbl_<%= CphName%>')">Thêm mới điều khiển</a>
    </div>
    <div class="border-control" id="tbl_<%= CphName%>" style="display: none;">
        <div class="form-body mt10">
            <div class="form-group row">
                <label class="col-md-4 col-form-label text-right">Điều khiển:</label>
                <div class="col-md-8">
                    <select class="form-control" name="vsw_cph_<%= CphName %>_control_code" id="vsw_cph_<%= CphName %>_control_code">
                        <%for (var i = 0; _listModule != null && i < _listModule.Count; i++){ %>
                        <option value="<%= _listModule[i].Code%>"><%= _listModule[i].Name%></option>
                        <%} %>
                        <option value="VSWMODULE">VSWMODULE</option>
                    </select>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-md-4 col-form-label text-right">Tên điều khiển</label>
                <div class="col-md-8">
                    <input type="text" class="form-control" id="vsw_cph_<%= CphName %>_VSWID" name="vsw_cph_<%= CphName %>_VSWID" value="" placeholder="Tên điều khiển. Ví dụ: cMenuTop" />
                </div>
            </div>
            <div class="cmd-control justify-content-end">
                <a href="javascript:cp_update('vsw_cph_<%= CphName %>|add')" data-toggle="tooltip" data-placement="bottom" data-original-title="Thêm điều khiển"><i class="fa fa-plus-circle"></i></a>
            </div>
        </div>
    </div>
    <div style="padding-top: 10px"></div>
</div>