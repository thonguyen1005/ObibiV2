<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<script runat="server">
    private List<EntityBase> AutoGetMap(SysMenuModel model)
    {
        List<EntityBase> list = new List<EntityBase>();

        int _id = model.ParentID;
        while (_id > 0)
        {
            var _item = WebMenuService.Instance.GetByID(_id);

            if (_item == null)
                break;

            _id = _item.ParentID;

            list.Insert(0, _item);
        }

        return list;
    }
</script>

<% 
    var model = ViewBag.Model as SysMenuModel;
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Chuyên mục <small>Thêm nhiều</small></h3>
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

                <div class="form-horizontal form-row-seperated">
                    <div class="portlet">
                        <div class="portlet-title">
                            <div class="caption"></div>
                            <div class="actions btn-set">
                                <%=GetSortAddCommand()%>
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
                                                    <label class="col-md-3 col-form-label text-right">Danh sách chuyên mục:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control custom" name="Value" rows="10" cols="" placeholder="Nhập vào danh sách các chuyên mục, Enter để phân cách"><%=model.Value %></textarea>
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