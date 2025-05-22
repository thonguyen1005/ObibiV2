<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModUrlCheckModel;
    var listItem = ViewBag.Data as List<ModUrlCheckEntity> ?? new List<ModUrlCheckEntity>();
%>

<form id="vswForm" name="vswForm" method="post">

    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" id="boxchecked" name="boxchecked" value="0" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Kiểm tra Url</h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Kiểm tra Url</a>
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
                            <%=GetListCommand("export|Xuất Excel|btn-success")%>
                        </div>
                    </div>
                    <div class="portlet-title">
                        <div class="actions btn-set">
                            <input class="form-control input-inline input-sm" type="text" name="Excel" id="Excel" value="<%=model.Excel %>" style="width: 200px; display: inline-block;" maxlength="255" />
                            <input class="form-control input-inline input-sm" style="width: 100px; display: inline-block;" type="button" onclick="ShowFileForm('Excel'); return false;" value="Chọn File" />
                            <button type="button" style="padding: 4px 10px; font-size: 13px; line-height: 1.5; border-radius: 0; cursor: pointer; margin: 0 0 0 5px;" class="btn blue" onclick="vsw_exec_cmd('import')" data-toggle="tooltip" data-placement="bottom" data-original-title="Update Url From Excel"><i class="fa fa-plus-circle"></i>Tải dữ liệu</button>
                        </div>
                    </div>
                    <div class="portlet-body">
                        <div class="dataTables_wrapper">
                            <div class="row hidden-sm hidden-col">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                </div>
                            </div>

                            <div class="table-scrollable">
                                <table class="table table-striped table-hover table-bordered dataTable">
                                    <thead>
                                        <tr>
                                            <th class="sorting text-center w1p">#</th>
                                            <th class="sorting text-center">Address</th>
                                            <th class="sorting text-center w10p">Content</th>
                                            <th class="sorting text-center w8p">Status Code</th>
                                            <th class="sorting text-center w8p desktop">Status</th>
                                            <th class="sorting text-center w10p desktop">Indexability</th>
                                            <th class="sorting text-center w10p desktop">Indexability Status</th>
                                            <th class="sorting text-center w8p desktop">Hash</th>
                                            <th class="sorting text-center w8p desktop">Length</th>
                                            <th class="sorting text-center w10p desktop">Canonical Link Element 1</th>
                                            <th class="sorting text-center w10p desktop">URL Encoded Address</th>
                                            <th class="sorting text-center w10p desktop">Loại Url</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%for (var i = 0; listItem != null && i < listItem.Count; i++)
                                            { %>
                                        <tr>
                                            <td class="text-center"><%= i + 1%></td>
                                            <td>
                                                <%= listItem[i].Address%>
                                            </td>
                                            <td>
                                                <%= listItem[i].Content%>
                                            </td>
                                            <td>
                                                <%= listItem[i].Status_Code%>
                                            </td>
                                            <td>
                                                <%= listItem[i].Status%>
                                            </td>
                                            <td>
                                                <%= listItem[i].Indexability%>
                                            </td>
                                            <td>
                                                <%= listItem[i].Indexability_Status%>
                                            </td>
                                            <td>
                                                <%= listItem[i].Hash%>
                                            </td>
                                            <td>
                                                <%= listItem[i].Length%>
                                            </td>
                                            <td>
                                                <%= listItem[i].Canonical%>
                                            </td>
                                            <td>
                                                <%= listItem[i].URL_Encoded%>
                                            </td>
                                            <td>
                                                <%= listItem[i].Type%>
                                            </td>
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

    <script type="text/javascript">
        var VSWController = "ModUrlCheck";
    </script>

</form>
