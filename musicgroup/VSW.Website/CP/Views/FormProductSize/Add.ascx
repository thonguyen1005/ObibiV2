<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModProductSizeModel;
    var item = ViewBag.Data as ModProductSizeEntity;

    var listSize = ModSizeService.Instance.CreateQuery()
                            .Where(o => o.Activity == true)
                            .OrderByAsc(o => new { o.Order, o.ID })
                            .ToList_Cache();

    var listColor = ModColorService.Instance.CreateQuery()
                            .Where(o => o.Activity == true)
                            .OrderByAsc(o => new { o.Order, o.ID })
                            .ToList_Cache();
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Kích cỡ và màu sắc <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Kích cỡ và màu sắc</a>
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
                                <div class="col-12 col-sm-12 col-md-12 col-lg-8">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Thông tin chung</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Kích cỡ:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-control" name="SizeID" id="SizeID">
                                                            <option value="0">Root</option>
                                                            <%for (int i = 0; listSize != null && i < listSize.Count; i++)
                                                                {%>
                                                            <option value="<%=listSize[i].ID %>" <%if (item.SizeID == listSize[i].ID)
                                                                {%>selected="selected"
                                                                <%} %>><%=listSize[i].Name %></option>
                                                            <%} %>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Màu sắc:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-control" name="ColorID" id="ColorID">
                                                            <option value="0">Root</option>
                                                            <%for (int i = 0; listColor != null && i < listColor.Count; i++)
                                                                {%>
                                                            <option value="<%=listColor[i].ID %>" <%if (item.ColorID == listColor[i].ID)
                                                                {%>selected="selected"
                                                                <%} %>><%=listColor[i].Name %></option>
                                                            <%} %>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Cân nặng(gram):</label>
                                                    <div class="col-md-9">
                                                        <div class="form-inline">
                                                            <input type="number" class="form-control price" name="Weight" value="<%=item.Weight %>" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Giá bán:</label>
                                                    <div class="col-md-9">
                                                        <div class="form-inline">
                                                            <input type="number" class="form-control price" name="Price" value="<%=item.Price %>" />
                                                            <span class="help-block text-primary"><%=Utils.NumberToWord(item.Price.ToString())%></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Giá thị trường:</label>
                                                    <div class="col-md-9">
                                                        <div class="form-inline">
                                                            <input type="number" class="form-control price" name="Price2" value="<%=item.Price2 %>" />
                                                            <span class="help-block text-primary"><%=Utils.NumberToWord(item.Price2.ToString())%></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Giá khuyến mại:</label>
                                                    <div class="col-md-9">
                                                        <div class="form-inline">
                                                            <input type="number" class="form-control price" name="PricePromotion" value="<%=item.PricePromotion %>" />
                                                            <span class="help-block text-primary"><%=Utils.NumberToWord(item.PricePromotion.ToString())%></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%if (item.ID > 0)
                                                    {
                                                        var listFile = ModProductFileService.Instance.CreateQuery()
                                                        .Where(o => o.ProductID == model.ProductID)
                                                        .OrderByAsc(o => o.Order)
                                                        .ToList_Cache();
                                                %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Ảnh phụ:</label>
                                                    <div class="col-md-9">
                                                        <ul class="cmd-custom" id="list-file">
                                                            <%for (int i = 0; i < listFile.Count; i++)
                                                                {%>
                                                            <li>
                                                                <img src="<%=listFile[i].File %>" />
                                                                <%if (listFile[i].ProductSizeID.Contains(item.ID.ToString()))
                                                                    { %>
                                                                <a href="javascript:void(0)" onclick="UnChoseFile('<%=listFile[i].ID %>','<%=item.ProductID %>','<%=item.ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chọn" style="color: #22aa13; font-size: 20px;" id="productfilesize<%=listFile[i].ID %>">
                                                                    <i class="fa fa-check-square-o"></i>
                                                                </a>
                                                                <%}
                                                                    else
                                                                    { %>
                                                                <a href="javascript:void(0)" onclick="ChoseFile('<%=listFile[i].ID %>','<%=item.ProductID %>','<%=item.ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chọn" style="color: #ccc; font-size: 20px;" id="productfilesize<%=listFile[i].ID %>">
                                                                    <i class="fa fa-square-o"></i>
                                                                </a>
                                                                <%} %>
                                                            </li>
                                                            <%} %>
                                                        </ul>
                                                    </div>
                                                </div>
                                                <%} %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12 col-sm-12 col-md-12 col-lg-4">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Thuộc tính</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <%if (CPViewPage.UserPermissions.Approve)
                                                    {%>
                                                <div class="form-group">
                                                    <label class="portlet-title-sub">Duyệt</label>
                                                    <div class="radio-list">
                                                        <label class="radioPure radio-inline">
                                                            <input type="radio" name="Activity" <%= item.Activity ? "checked": "" %> value="1" />
                                                            <span class="outer"><span class="inner"></span></span><i>Có</i>
                                                        </label>
                                                        <label class="radioPure radio-inline">
                                                            <input type="radio" name="Activity" <%= !item.Activity ? "checked": "" %> value="0" />
                                                            <span class="outer"><span class="inner"></span></span><i>Không</i>
                                                        </label>
                                                    </div>
                                                </div>
                                                <%} %>
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
