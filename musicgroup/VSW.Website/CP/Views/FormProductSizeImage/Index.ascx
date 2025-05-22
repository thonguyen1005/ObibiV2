<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as FormProductSizeImageModel;
    var listFile = ViewBag.Data as List<ModProductFileEntity>;
    if (listFile == null) return;
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <div class="row">
            <div class="col-md-12">
                <%= ShowMessage()%>

                <div class="form-horizontal form-row-seperated">
                    <div class="portlet">
                        <div class="portlet-body">
                            <div class="row">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Danh sách hình ảnh</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ảnh phụ:</label>
                                                    <div class="col-md-9">
                                                        <ul class="cmd-custom" id="list-file">
                                                            <%for (int i = 0; i < listFile.Count; i++)
                                                                {
                                                                    if (string.IsNullOrEmpty(listFile[i].ProductSizeID))
                                                                    {
                                                                        listFile[i].ProductSizeID = "";
                                                                    }
                                                            %>
                                                            <li>
                                                                <img src="<%=listFile[i].File %>" />
                                                                <%if (listFile[i].ProductSizeID.Contains(model.ID.ToString()))
                                                                    { %>
                                                                <a href="javascript:void(0)" onclick="UnChoseFile('<%=listFile[i].ID %>','<%=model.ProductID %>','<%=model.ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chọn" style="color: #22aa13; font-size: 20px;" id="productfilesize<%=listFile[i].ID %>">
                                                                    <i class="fa fa-check-square-o"></i>
                                                                </a>
                                                                <%}
                                                                    else
                                                                    { %>
                                                                <a href="javascript:void(0)" onclick="ChoseFile('<%=listFile[i].ID %>','<%=model.ProductID %>','<%=model.ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chọn" style="color: #ccc; font-size: 20px;" id="productfilesize<%=listFile[i].ID %>">
                                                                    <i class="fa fa-square-o"></i>
                                                                </a>
                                                                <%} %>
                                                                <%if (listFile[i].ProductSizeID.Contains(model.ID.ToString()) && listFile[i].IsAvatar)
                                                                    { %>
                                                                <a href="javascript:void(0)" onclick="UnChoseFileAvatar('<%=listFile[i].ID %>','<%=model.ProductID %>','<%=model.ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chọn" style="color: #22aa13; font-size: 20px;" id="productfilesizeavatar<%=listFile[i].ID %>">
                                                                    <i class="fa fa-check-square-o"></i>
                                                                </a>Avatar
                                                                <%}
                                                                    else
                                                                    { %>
                                                                <a href="javascript:void(0)" onclick="ChoseFileAvatar('<%=listFile[i].ID %>','<%=model.ProductID %>','<%=model.ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chọn" style="color: #ccc; font-size: 20px;" id="productfilesizeavatar<%=listFile[i].ID %>">
                                                                    <i class="fa fa-square-o"></i>
                                                                </a>Avatar
                                                                <%} %>
                                                            </li>
                                                            <%} %>
                                                        </ul>
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
