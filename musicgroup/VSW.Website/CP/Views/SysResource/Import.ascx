<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as SysResourceModel;
    var listLang = SysLangService.Instance.CreateQuery().ToList_Cache();
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Tài nguyên <small>Thêm nhiều</small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Tài nguyên</a>
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
                                <%= GetSortAddCommand()%>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="tabbable">
                                <div class="row">
                                    <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                        <div class="portlet box blue-steel">
                                            <div class="portlet-title">
                                                <div class="caption">Thông tin chung</div>
                                            </div>
                                            <div class="portlet-body">
                                                <div class="form-body">
                                                    <div class="form-group row">
                                                        <label class="col-md-3 col-form-label text-right">Tài nguyên</label>
                                                        <div class="col-md-9">
                                                            <textarea class="form-control custom" name="Resource" id="Resource" rows="10" cols="" placeholder="Key1=Value1&#10;Key2=Value2"><%=model.Resource %></textarea>
                                                        </div>
                                                    </div>
                                                    <div class="form-group row">
                                                        <label class="col-md-3 col-form-label text-right">&nbsp;</label>
                                                        <div class="col-md-9">
                                                            <p class="help-block text-primary">Nhập dạng:</p>
                                                            <p class="help-block text-primary">Key1=Value1</p>
                                                            <p class="help-block text-primary">Key2=Value2</p>
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