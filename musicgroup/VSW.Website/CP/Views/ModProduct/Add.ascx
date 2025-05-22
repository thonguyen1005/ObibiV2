<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModProductModel;
    var item = ViewBag.Data as ModProductEntity;

    var listFile = item.GetFile();
    //var listGift = item.GetGift2();
    var listProductClassify = item.GetProductClassify();
    var listProductClassifyDetail = item.GetProductClassifyDetail();
    var listProductClassifyDetailPrice = item.GetProductClassifyDetailPrice();
    //var listProduct = item.GetOther();
    //var listProductOld = item.GetOld();
    var listVideo = item.GetVideo();
    var listFAQ = item.GetFAQ();
    //var arrSummary = !string.IsNullOrEmpty(item.Summary) ? item.Summary.Split(new string[] { "<br/>" }, StringSplitOptions.None).ToList() : new List<string>() { "" };
    //var arrPromotion = !string.IsNullOrEmpty(item.Promotion) ? item.Promotion.Split(new string[] { "<br/>" }, StringSplitOptions.None).ToList() : new List<string>() { "" };
    //var arrSpecifications = !string.IsNullOrEmpty(item.Specifications) ? item.Specifications.Split(new string[] { "<br/>" }, StringSplitOptions.None).ToList() : new List<string>() { "" };
%>
<form id="vswForm" name="vswForm" method="post" enctype="multipart/form-data">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" name="RecordID" id="RecordID" value="<%=model.RecordID %>" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Sản phẩm <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Sản phẩm</a>
                </li>
            </ul>
            <div class="page-toolbar">
                <div class="btn-group">
                    <a href="/" class="btn green" target="_blank"><i class="icon-screen-desktop"></i>Xem Website</a>
                </div>
            </div>
        </div>
        <div class="tabbable">
            <%-- <%if (model.RecordID > 0)
                { %>
            <ul class="nav nav-tabs justify-content-start">
                <li class="nav-link active" data-href="#tab-1" data-toggle="tab">
                    <a href="javascript:;">Thông tin sản phẩm</a>
                </li>
                <li class="nav-link" data-href="#tab-2" data-toggle="tab">
                    <a href="javascript:;">Đánh giá</a>
                </li>
            </ul>
            <%} %>--%>
            <div class="tab-content" style="padding: 0px;">
                <div class="tab-pane active" id="tab-1">
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
                                                                <label class="col-md-3 col-form-label text-right">Tên sản phẩm:<span class="requied">*</span></label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control title" name="Name" id="Name" value="<%=item.Name %>" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Số sao:</label>
                                                                <div class="col-md-3">
                                                                    <input type="text" class="form-control" name="Star" id="Star" value="<%=item.Star %>" />
                                                                </div>
                                                                <label class="col-md-3 col-form-label text-right">Kho hàng:</label>
                                                                <div class="col-md-3">
                                                                    <input type="text" class="form-control" name="SoLuong" id="SoLuong" value="<%=item.SoLuong %>" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">URL trình duyệt:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control" name="Code" value="<%=item.Code %>" placeholder="Nếu không nhập sẽ tự sinh theo Tiêu đề" />
                                                                </div>
                                                            </div>
                                                            <%if (!string.IsNullOrEmpty(item.Code))
                                                                { %>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Xem trên web site</label>
                                                                <div class="col-md-9">
                                                                    <a href="/<%=item.Code + Setting.Sys_PageExt%>" style="color: blue;" target="_blank">Xem website</a>
                                                                </div>
                                                            </div>
                                                            <%} %>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Mã SP:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control title" name="Model" id="Model" value="<%=item.Model %>" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Cân nặng(gram):</label>
                                                                <div class="col-md-9">
                                                                    <input type="number" style="width: 150px;" class="form-control price" name="Weight" value="<%=item.Weight %>" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Giá bán:</label>
                                                                <div class="col-md-9">
                                                                    <div class="form-inline">
                                                                        <input type="number" style="width: 150px !important;" class="form-control price mr-1" name="Price" value="<%=item.Price %>" />
                                                                        <span class="help-block text-primary"><%=Utils.FormatMoney(item.Price)%></span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Giá thị trường:</label>
                                                                <div class="col-md-9">
                                                                    <div class="form-inline">
                                                                        <input type="number" style="width: 150px !important;" class="form-control price mr-1" name="Price2" value="<%=item.Price2 %>" />
                                                                        <span class="help-block text-primary"><%=Utils.FormatMoney(item.Price2)%></span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Tình trạng sản phẩm:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control" name="TinhTrang" id="TinhTrang" value="<%=item.TinhTrang %>" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Nguồn gốc/xuất xứ:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control" name="NguonGoc" id="NguonGoc" value="<%=item.NguonGoc %>" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Bảo hành:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control" name="BaoHanh" id="BaoHanh" value="<%=item.BaoHanh %>" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Đơn vị tính:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control" name="DonVi" id="DonVi" value="<%=item.DonVi %>" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Thương hiệu:</label>
                                                                <div class="col-md-7">
                                                                    <select class="form-control select2" style="width: 100%" name="BrandID" id="BrandID">
                                                                        <option value="0">Root</option>
                                                                        <%= Utils.ShowDdlMenuByType("Brand", model.LangID, item.BrandID)%>
                                                                    </select>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <a href="javascript:ShowWebMenu('Brand', $('#BrandID').val(),'BrandID')" class="text-primary"><i class="fa fa-plus"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Chuyên mục:<span class="requied">*</span></label>
                                                                <div class="col-md-7">
                                                                    <select class="form-control select2" name="MenuID" id="MenuID" style="width: 100%" onchange="GetProperties(this.value)">
                                                                        <option value="0">Root</option>
                                                                        <%= Utils.ShowDdlMenuByType("Product", model.LangID, item.MenuID)%>
                                                                    </select>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <a href="javascript:ShowWebMenu('Product', $('#MenuID').val(),'MenuID')" class="text-primary"><i class="fa fa-plus"></i></a>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Thuộc tính lọc:</label>
                                                                <div class="col-md-9" id="list-property"></div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption d-flex">Thông tin nhóm</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-group row">
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Nhóm sản phẩm:</label>
                                                                <div class="col-md-9">
                                                                    <select class="form-control select2" style="width: 100%" name="GroupMenuID" id="GroupMenuID" title="Chọn nhóm sản phẩm">
                                                                        <option value="0">Root</option>
                                                                        <%= Utils.ShowDdlMenuByType("GroupProduct", model.LangID, item.GroupMenuID)%>
                                                                    </select>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <label class="col-md-3 col-form-label text-right">Phiên bản:</label>
                                                                <div class="col-md-9">
                                                                    <input type="text" class="form-control title" name="PhienBan" id="PhienBan" value="<%=item.PhienBan %>" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption d-flex">Thông tin bán hàng</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-group row">
                                                            <label class="col-md-3 col-form-label text-right">Phân loại hàng:</label>
                                                            <div class="col-md-9">
                                                                <div id="lstPLH">
                                                                    <%for (int i = 0; listProductClassify != null && i < listProductClassify.Count; i++)
                                                                        {
                                                                            var lstDetail = listProductClassifyDetail != null ? listProductClassifyDetail.Where(o => o.ClassifyID == listProductClassify[i].ID).ToList() : null;
                                                                    %>
                                                                    <div class="box" id="box_ProductClassify<%=i %>">
                                                                        <div class="box-header box-header-edit" style="display: none;">
                                                                            <div class="col-md-6">
                                                                                <input type="text" class="form-control" onchange="ProductClassifyOnchange(this.value, 'ProductClassify<%=i %>')" name="ProductClassify" id="ProductClassify<%=i %>" value="<%=listProductClassify[i].Name %>" maxlength="20">
                                                                            </div>
                                                                            <div class="col-md-6 text-right">
                                                                                <a href="javascript:AddProductClassifyValue('ProductClassify<%=i %>',<%=i %>)" class="mr-1"><i class="fa fa-plus"></i></a>
                                                                                <a href="javascript:DelProductClassify('ProductClassify<%=i %>')"><i class="fa fa-close"></i></a>
                                                                            </div>
                                                                        </div>
                                                                        <div class="box-header box-header-view" style="">
                                                                            <div class="col-md-6"><span class="title" id="title_ProductClassify<%=i %>"><%=listProductClassify[i].Name %></span></div>
                                                                            <div class="col-md-6 text-right">
                                                                                <a href="javascript:AddProductClassifyValue('ProductClassify<%=i %>',<%=i %>)" class="mr-1"><i class="fa fa-plus"></i></a>
                                                                                <a href="javascript:EditProductClassify('ProductClassify<%=i %>')" class="mr-1"><i class="fa fa-edit ml-1"></i></a>
                                                                                <a href="javascript:DelProductClassify('ProductClassify<%=i %>')"><i class="fa fa-trash"></i></a>
                                                                            </div>
                                                                        </div>
                                                                        <hr>
                                                                        <div class="box-body row">
                                                                            <%for (int j = 0; lstDetail != null && j < lstDetail.Count; j++)
                                                                                {%>
                                                                            <div class="form-group col-md-6 item-ProductClassify">
                                                                                <div class="input-group">
                                                                                    <div class="img">
                                                                                        <%if (!string.IsNullOrEmpty(lstDetail[j].File))
                                                                                            { %>
                                                                                        <img src="<%=Utils.GetWebPFile(lstDetail[j].File, 4, 40, 40) %>">
                                                                                        <%}
                                                                                            else
                                                                                            { %>
                                                                                        <i class="fa fa-image" onclick="ShowFileClassify(this); return false"></i>
                                                                                        <%} %>
                                                                                        <input type="hidden" name="ProductClassifyFile" value="<%=lstDetail[j].File %>">
                                                                                    </div>
                                                                                    <input type="hidden" name="ProductClassifyParrent" value="<%=i %>" maxlength="20">
                                                                                    <input type="text" class="form-control" name="ProductClassifyValue" value="<%=lstDetail[j].Name %>" maxlength="20">
                                                                                    <a href="javascript:;" onclick="DelProductClassifyValue(this,'ProductClassify<%=i %>');"><i class="fa fa-trash"></i></a>
                                                                                </div>
                                                                            </div>
                                                                            <% } %>
                                                                        </div>
                                                                    </div>
                                                                    <%} %>
                                                                </div>
                                                                <a href="javascript:AddProductClassify();" class="btn btn-primary" id="btnAddProductClassify"><i class="fa fa-plus mr-1"></i>Thêm phân loại</a>
                                                            </div>
                                                        </div>
                                                        <div class="form-group row phanloai" style="<%=listProductClassify != null && listProductClassify.Count > 0 ? "": "display:none;" %>">
                                                            <label class="col-md-3 col-form-label text-right">Danh sách phân loại:</label>
                                                            <div class="col-md-9">
                                                                <div class="row">
                                                                    <div class="col-md-9 d-flex">
                                                                        <input type="text" class="form-control" name="PriceProductClassify" id="PriceProductClassify" value="" placeholder="Giá" />
                                                                        <input type="text" class="form-control" name="SoLuongProductClassify" id="SoLuongProductClassify" value="" placeholder="Kho hàng" />
                                                                        <input type="text" class="form-control" name="SkuProductClassify" id="SkuProductClassify" value="" placeholder="Sku" />
                                                                    </div>
                                                                    <div class="col-md-3">
                                                                        <a href="javascript:;" class="btn btn-primary" onclick="ApplyToProductClassify()">Áp dụng</a>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group row phanloai" style="<%=listProductClassify != null && listProductClassify.Count > 0 ? "": "display:none;" %>">
                                                            <div class="col-md-12">
                                                                <div class="dataTables_wrapper">
                                                                    <div class="table-scrollable">
                                                                        <table class="table table-hover table-bordered" id="tblCaculator">
                                                                            <thead>
                                                                                <tr>
                                                                                    <%for (int i = 0; listProductClassify != null && i < listProductClassify.Count; i++)
                                                                                        { %>
                                                                                    <th class="sorting text-center <%=i > 0 ? "w25p" : "" %>"><%=listProductClassify[i].Name %></th>
                                                                                    <%} %>
                                                                                    <th class="sorting text-center w20p">Giá</th>
                                                                                    <th class="sorting text-center w15p">Kho hàng</th>
                                                                                    <th class="sorting text-center w15p">SKU</th>
                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <%if (listProductClassifyDetail != null && listProductClassifyDetailPrice.Count > 0)
                                                                                    {
                                                                                        var lstItem1 = listProductClassifyDetail.Where(o => o.ClassifyID == listProductClassify[0].ID).ToList();
                                                                                %>
                                                                                <%for (int i = 0; i < lstItem1.Count; i++)
                                                                                    {
                                                                                        var lstItemBy = listProductClassifyDetailPrice.Where(o => o.ClassifyDetailID1 == lstItem1[i].ID).ToList();
                                                                                %>
                                                                                <%for (int j = 0; j < lstItemBy.Count; j++)
                                                                                    {
                                                                                        string name2 = "";
                                                                                        if (lstItemBy[j].ClassifyDetailID2 > 0)
                                                                                        {
                                                                                            name2 = listProductClassifyDetail.Where(o => o.ID == lstItemBy[j].ClassifyDetailID2).Select(o => o.Name).FirstOrDefault();
                                                                                        }
                                                                                %>
                                                                                <tr data-value1="<%=lstItem1[i].Name %>" data-value2="<%=name2 %>">
                                                                                    <%if (j == 0)
                                                                                        { %>
                                                                                    <td class="text-center name" rowspan="<%=lstItemBy.Count %>" data-value="<%=lstItem1[i].Name %>">
                                                                                        <%=lstItem1[i].Name %>
                                                                                    </td>
                                                                                    <% } %>
                                                                                    <%if (!string.IsNullOrEmpty(name2))
                                                                                        {
                                                                                    %>
                                                                                    <td class="text-center name2">
                                                                                        <%=name2 %>
                                                                                    </td>
                                                                                    <%} %>
                                                                                    <td class="text-center">
                                                                                        <input type="number" class="form-control price mr-1" name="PriceProductClassifyDetail" value="<%=lstItemBy[j].Price %>" />
                                                                                        <span class="help-block text-primary"><%=Utils.FormatMoney(lstItemBy[j].Price)%></span>
                                                                                    </td>
                                                                                    <td class="text-center">
                                                                                        <input type="number" class="form-control" name="SoLuongProductClassifyDetail" value="<%= lstItemBy[j].SoLuong %>" />
                                                                                    </td>
                                                                                    <td class="text-center">
                                                                                        <input type="text" class="form-control" name="SKUProductClassifyDetail" value="<%= lstItemBy[j].Sku %>" />
                                                                                        <input type="hidden" name="ClassifyDetailIndex1ProductClassifyDetail" value="<%=i %>" />
                                                                                        <input type="hidden" name="ClassifyDetailIndex2ProductClassifyDetail" value="<%=j %>" />
                                                                                    </td>
                                                                                </tr>
                                                                                <% } %>
                                                                                <% } %>
                                                                                <% } %>
                                                                            </tbody>
                                                                        </table>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <div class="form-inline">
                                                                            <button type="button" class="btn btn-primary" onclick="ShowProductForm('News'); return false">Thêm sản phẩm</button>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption d-flex">Đặc điểm nổi bật</div>
                                                        <%--<div class="caption d-flex justify-content-end" style="float: none;">
                                                            <a href="javascript:AddControlInput('tblSummary','Summary');" onclick=""><i class="fa fa-plus mr-1"></i></a>
                                                        </div>--%>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <textarea class="form-control ckeditor" name="Summary" id="Summary" data-formatcheck="true" rows="" cols=""><%=item.Summary %></textarea>
                                                        <%--<div class="form-group row">
                                                            <div class="col-md-12">
                                                                <table class="w100p">
                                                                    <tbody id="tblSummary">
                                                                        <%for (int i = 0; i < arrSummary.Count; i++)
                                                                            {%>
                                                                        <tr>
                                                                            <td class="w90p">
                                                                                <input type="text" class="form-control" name="Summary" value="<%=arrSummary[i] %>" />
                                                                            </td>
                                                                            <td class="text-center"><a href="javascript:;" onclick="removeRow(event)"><i class="fa fa-close"></i></a></td>
                                                                        </tr>
                                                                        <% } %>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                                </div>

                                                <%--                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption d-flex">Khuyến mại</div>
                                                        <div class="caption d-flex justify-content-end" style="float: none;">
                                                            <a href="javascript:AddControlInput('tblPromotion','Promotion');" onclick=""><i class="fa fa-plus mr-1"></i></a>
                                                        </div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-group row">
                                                            <div class="col-md-12">
                                                                <table class="w100p">
                                                                    <tbody id="tblPromotion">
                                                                        <%for (int i = 0; i < arrPromotion.Count; i++)
                                                                            {%>
                                                                        <tr>
                                                                            <td class="w90p">
                                                                                <input type="text" class="form-control" name="Promotion" value="<%=arrPromotion[i] %>" />
                                                                            </td>
                                                                            <td class="text-center"><a href="javascript:;" onclick="removeRow(event)"><i class="fa fa-close"></i></a></td>
                                                                        </tr>
                                                                        <% } %>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>--%>

                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption">Mô tả sản phẩm</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <textarea class="form-control ckeditor" name="Content" id="Content" rows="" cols=""><%=item.Content %></textarea>
                                                    </div>
                                                </div>
                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption d-flex">Thông số kỹ thuật</div>
                                                        <%--<div class="caption d-flex justify-content-end" style="float: none;">
                                                            <a href="javascript:AddControlTextarea('tblSpecifications','Specifications');" onclick=""><i class="fa fa-plus mr-1"></i></a>
                                                        </div>--%>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <textarea class="form-control ckeditor" name="Specifications" id="Specifications" rows="" cols=""><%=item.Specifications %></textarea>
                                                        <%--<div class="form-group row">
                                                            <div class="col-md-12">
                                                                <table class="w100p">
                                                                    <tbody id="tblSpecifications">
                                                                        <%for (int i = 0; i < arrSpecifications.Count; i++)
                                                                            {%>
                                                                        <tr>
                                                                            <td class="w90p">
                                                                                <textarea class="form-control ckeditorRow" name="Specifications" id="Specifications<%=i %>"><%=arrSpecifications[i] %></textarea>
                                                                            </td>
                                                                            <td class="text-center"><a href="javascript:;" onclick="removeRow(event)"><i class="fa fa-close"></i></a></td>
                                                                        </tr>
                                                                        <% } %>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>--%>
                                                    </div>
                                                </div>
                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption">Schema.org</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <textarea class="form-control" rows="15" name="SchemaJson" id="SchemaJson"><%=item.SchemaJson%></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-12 col-md-12 col-lg-4">
                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption">HÌNH ẢNH</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-body">
                                                            <div class="form-group">
                                                                <%if (!string.IsNullOrEmpty(item.File))
                                                                    { %>
                                                                <p class="preview "><%= Utils.GetMedia(item.File, 80, 80)%></p>
                                                                <%}
                                                                    else
                                                                    { %>
                                                                <p class="preview">
                                                                    <img src="" width="80" height="80" />
                                                                </p>
                                                                <%} %>

                                                                <label class="portlet-title-sub">Hình minh họa:</label>
                                                                <div class="form-inline">
                                                                    <input type="text" class="form-control" name="File" id="File" value="<%=item.File %>" />
                                                                    <button type="button" class="btn btn-primary" onclick="ShowFileForm('File'); return false">Chọn ảnh</button>
                                                                </div>
                                                            </div>
                                                            <%--<div class="form-group">
                                                            <label class="itemCheckBox itemCheckBox-sm">
                                                                <input type="checkbox" name="IsChangeFile" value="1">
                                                                <i class="check-box"></i>
                                                                <span>Xóa ảnh cũ</span>
                                                            </label>
                                                        </div>--%>
                                                            <div class="form-group">
                                                                <label class="portlet-title-sub">Ảnh phụ:</label>
                                                                <ul class="cmd-custom" id="list-file">
                                                                    <%for (int i = 0; i < listFile.Count; i++)
                                                                        {%>
                                                                    <li>
                                                                        <%= Utils.GetMedia(listFile[i].File, 80, 80)%>
                                                                        <a href="javascript:void(0)" onclick="deleteFile('<%=listFile[i].File %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa"><i class="fa fa-ban"></i></a>
                                                                        <a href="javascript:void(0)" onclick="upFile('<%=listFile[i].File %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển lên trên"><i class="fa fa-arrow-up"></i></a>
                                                                        <a href="javascript:void(0)" onclick="downFile('<%=listFile[i].File %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển xuống dưới"><i class="fa fa-arrow-down"></i></a>
                                                                    </li>
                                                                    <%} %>
                                                                </ul>
                                                                <div class="form-inline">
                                                                    <button type="button" class="btn btn-primary" onclick="ShowFile(); return false">Chọn ảnh</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%--<div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption">QUÀ TẶNG</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-body">

                                                            <div class="form-group">
                                                                <label class="portlet-title-sub">Quà tặng:</label>
                                                                <ul class="cmd-custom" id="list-gift">
                                                                    <%for (int i = 0; i < listGift.Count; i++)
                                                                        {%>
                                                                    <li>
                                                                        <%= Utils.GetMedia(listGift[i].File, 80, 80)%>
                                                                        <b><%=listGift[i].Name %></b>
                                                                        <a href="javascript:void(0)" onclick="deleteGift('<%=listGift[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa"><i class="fa fa-ban"></i></a>
                                                                        <a href="javascript:void(0)" onclick="upGift('<%=listGift[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển lên trên"><i class="fa fa-arrow-up"></i></a>
                                                                        <a href="javascript:void(0)" onclick="downGift('<%=listGift[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển xuống dưới"><i class="fa fa-arrow-down"></i></a>
                                                                    </li>
                                                                    <%} %>
                                                                </ul>
                                                                <div class="form-inline">
                                                                    <button type="button" class="btn btn-primary" onclick="ShowGiftForm('Gift'); return false">Chọn quà tặng</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>--%>
                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption">THUỘC TÍNH</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-body">
                                                            <div class="form-group">
                                                                <label class="portlet-title-sub">Vị trí</label>
                                                                <div class="checkbox-list">
                                                                    <%= Utils.ShowCheckBoxByConfigkey("Mod.ProductState", "ArrState", item.State)%>
                                                                </div>
                                                            </div>
                                                            <%-- <div class="form-group">
                                                                <label class="portlet-title-sub">Sản phẩm là quà tặng</label>
                                                                <div class="radio-list">
                                                                    <label class="radioPure radio-inline">
                                                                        <input type="radio" name="IsGift" <%= item.IsGift ? "checked": "" %> value="1" />
                                                                        <span class="outer"><span class="inner"></span></span><i>Có</i>
                                                                    </label>
                                                                    <label class="radioPure radio-inline">
                                                                        <input type="radio" name="IsGift" <%= !item.IsGift ? "checked": "" %> value="0" />
                                                                        <span class="outer"><span class="inner"></span></span><i>Không</i>
                                                                    </label>
                                                                </div>
                                                            </div>--%>
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
                                                <%--<div class="portlet box blue-steel">
                                                <div class="portlet-title">
                                                    <div class="caption">Sản phẩm mua kèm</div>
                                                </div>
                                                <div class="portlet-body">
                                                    <div class="form-body">
                                                        <div class="form-group">
                                                            <label class="portlet-title-sub">Sản phẩm:</label>
                                                            <ul class="cmd-custom" id="list-product">
                                                                <%for (int i = 0; listProduct != null && i < listProduct.Count; i++)
                                                                    {%>
                                                                <li>
                                                                    <%= Utils.GetMedia(listProduct[i].File, 80, 80)%>
                                                                    <b><%=listProduct[i].Name %></b>
                                                                    <a href="javascript:void(0)" onclick="deleteProductOther('<%=listProduct[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa"><i class="fa fa-ban"></i></a>
                                                                    <a href="javascript:void(0)" onclick="upProductOther('<%=listProduct[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển lên trên"><i class="fa fa-arrow-up"></i></a>
                                                                    <a href="javascript:void(0)" onclick="downProductOther('<%=listProduct[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển xuống dưới"><i class="fa fa-arrow-down"></i></a>
                                                                </li>
                                                                <%} %>
                                                            </ul>
                                                            <div>
                                                                <button type="button" class="btn btn-primary" onclick="ShowProductOtherForm('ProductOther'); return false">Chọn sản phẩm</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>--%>

                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption">Video</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-body">
                                                            <div class="form-group">
                                                                <label class="portlet-title-sub">Video:</label>
                                                                <ul class="cmd-custom" id="list-video">
                                                                    <%for (int i = 0; listVideo != null && i < listVideo.Count; i++)
                                                                        {%>
                                                                    <li>
                                                                        <%= Utils.GetMedia(listVideo[i].Image, 80, 80)%>
                                                                        <b><%=listVideo[i].Name %></b>
                                                                        <a href="javascript:void(0)" onclick="deleteProductVideo('<%=listVideo[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa"><i class="fa fa-ban"></i></a>
                                                                        <a href="javascript:void(0)" onclick="upProductVideo('<%=listVideo[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển lên trên"><i class="fa fa-arrow-up"></i></a>
                                                                        <a href="javascript:void(0)" onclick="downProductVideo('<%=listVideo[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển xuống dưới"><i class="fa fa-arrow-down"></i></a>
                                                                    </li>
                                                                    <%} %>
                                                                </ul>
                                                                <div>
                                                                    <button type="button" class="btn btn-primary" onclick="ShowProductVideoForm('ProductVideo'); return false">Chọn video</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption">Hỏi đáp</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-body">
                                                            <div class="form-group">
                                                                <label class="portlet-title-sub">Hỏi đáp:</label>
                                                                <ul class="cmd-custom" id="list-faq">
                                                                    <%for (int i = 0; listFAQ != null && i < listFAQ.Count; i++)
                                                                        {%>
                                                                    <li>
                                                                        <img src="/Content/images/hoi-dap.png" />
                                                                        <b><%=listFAQ[i].Name %></b>
                                                                        <a href="javascript:void(0)" onclick="deleteProductFAQ('<%=listFAQ[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa"><i class="fa fa-ban"></i></a>
                                                                        <a href="javascript:void(0)" onclick="upProductFAQ('<%=listFAQ[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển lên trên"><i class="fa fa-arrow-up"></i></a>
                                                                        <a href="javascript:void(0)" onclick="downProductFAQ('<%=listFAQ[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển xuống dưới"><i class="fa fa-arrow-down"></i></a>
                                                                    </li>
                                                                    <%} %>
                                                                </ul>
                                                                <div>
                                                                    <button type="button" class="btn btn-primary" onclick="ShowProductFAQForm('ProductFAQ'); return false">Chọn hỏi đáp</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption">SEO</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-body">
                                                            <div class="form-group ">
                                                                <label class="col-form-label">PageTitle:</label>
                                                                <input type="text" class="form-control title" name="PageTitle" value="<%=item.PageTitle %>" maxlength="70" />
                                                                <span class="help-block text-primary">Ký tự tối đa: 70</span>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-form-label">Description:</label>
                                                                <textarea class="form-control description" rows="8" style="height: 120px;" name="PageDescription" placeholder="Nhập nội dung tóm tắt. Tối đa 400 ký tự" maxlength="300"><%=item.PageDescription%></textarea>
                                                                <span class="help-block text-primary">Ký tự tối đa: 300</span>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-form-label">Keywords:</label>
                                                                <input type="text" class="form-control" name="PageKeywords" id="PageKeywords" value="<%=item.PageKeywords %>" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="portlet box blue-steel">
                                                    <div class="portlet-title">
                                                        <div class="caption">TAGS</div>
                                                    </div>
                                                    <div class="portlet-body">
                                                        <div class="form-body">
                                                            <input type="hidden" name="Tags" id="Tags" value="<%=item.Tags %>" />
                                                            <table width="100%">
                                                                <tr>
                                                                    <td width="100%">
                                                                        <input class="form-control" type="text" name="add_tag" id="add_tag" style="width: 60%; float: left" value="" />
                                                                        <input class="btn btn-primary" style="margin-left: 5px;" type="button" onclick="tag_add(); return false;" value="Thêm Tag" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100%">
                                                                        <div id="list_tag">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100%">
                                                                        <b>Chọn tag gần đây</b> :
                                                                     <% 
                                                                         var listTag = ModTagService.Instance.CreateQuery().Take(20).ToList();
                                                                         for (int i = 0; listTag != null && i < listTag.Count; i++)
                                                                         {
                                                                     %>
                                                                        <a href="javascript:tag_add_v2('<%=listTag[i].Name %>');"><%=listTag[i].Name %></a>
                                                                        <%if (i != listTag.Count - 1)
                                                                            { %>,<%} %>
                                                                        <%} %>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <script type="text/javascript">

                                                                var arrTags = new Array();

                                                            <% for (int i = 0; item.Tags != null && i < item.Tags.Split(',').Length; i++)
                                                                { %>
                                                                arrTags.push('<%=item.Tags.Split(',')[i] %>');
                                                            <%} %>

                                                                tag_display();

                                                                function tag_add() {
                                                                    var tag = document.getElementById('add_tag');

                                                                    for (var i = 0; i < arrTags.length; i++) {
                                                                        if (arrTags[i] == tag.value) {
                                                                            alert('Tag đã tồn tại!');
                                                                            return;
                                                                        }

                                                                    }
                                                                    if (tag.value == '') {
                                                                        alert('Nhập mã Tag!');
                                                                        return;
                                                                    }
                                                                    arrTags.push(tag.value);
                                                                    tag_display();

                                                                    tag.value = '';
                                                                }

                                                                function tag_add_v2(tagName) {

                                                                    for (var i = 0; i < arrTags.length; i++) {
                                                                        if (arrTags[i] == tagName) {
                                                                            alert('Tag đã tồn tại!');
                                                                            return;
                                                                        }

                                                                    }
                                                                    if (tagName == '') {
                                                                        alert('Nhập mã Tag!');
                                                                        return;
                                                                    }
                                                                    arrTags.push(tagName);
                                                                    tag_display();
                                                                }

                                                                function tag_display() {

                                                                    var list_tag = document.getElementById('list_tag');
                                                                    var s = '';
                                                                    var v = '';
                                                                    for (var i = 0; i < arrTags.length; i++) {
                                                                        v += (v == '' ? '' : ',') + arrTags[i];
                                                                        s += '<a href="javascript:tag_delete(' + i + ');"><i class="fa fa-times-circle"></i></a> ' + arrTags[i] + '<br />';
                                                                    }
                                                                    list_tag.innerHTML = s;

                                                                    document.getElementById('Tags').value = v;
                                                                }

                                                                function tag_delete(index) {
                                                                    if (confirm('Bạn chắc muốn xóa không ?')) {
                                                                        arrTags.splice(index, 1);
                                                                        tag_display();
                                                                    }
                                                                }

                                                            </script>
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
                <%-- <%if (model.RecordID > 0)
                    {%>
                <div class="tab-pane" id="tab-2">
                    <iframe src="/CP/FormProductVote/Index.aspx/ProductID/<%=model.RecordID %>" style='position: static; top: 240px; left: 0px; width: 101%; height: 1100px; z-index: 999; overflow: auto;' frameborder='no'></iframe>
                </div>
                <%} %>--%>
            </div>

        </div>
    </div>
    <script type="text/javascript" src="/{CPPath}/Content/ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        CKFinder.setupCKEditor(null, { basePath: "/{CPPath}/Content/ckfinder/", rememberLastFolder: true });
    </script>
    <script type="text/javascript">
        if ($('#Summary').length) {
            var ckEditor = CKEDITOR.instances["Summary"];
            if (ckEditor) { ckEditor.destroy(true); }
            var editor = CKEDITOR.replace('Summary', {
                toolbar: 'Summary'
            });
            if ($('#Summary').attr('data-formatcheck') != undefined) {
                editor.on('key', function (event) {
                    if (event.data.keyCode === 13) {
                        // Lấy con trỏ hiện tại trong CKEditor
                        var editor = CKEDITOR.instances['Summary'];
                        var selection = editor.getSelection();
                        var range = selection.getRanges()[0];
                        var element = range.startContainer;

                        // Kiểm tra nếu phần tử hiện tại là thẻ <p>
                        if (element.getAscendant('p', true)) {
                            var pElement = element.getAscendant('p', true);
                            var iElement = pElement.findOne('i');
                            if (!iElement) {
                                var currentHtml = pElement.getHtml();
                                pElement.setHtml('<i class="fas fa-check-circle text-success me-2">&nbsp;</i>' + currentHtml);
                                var italicElement = pElement.getFirst();
                                // Đặt con trỏ sau thẻ <i>
                                range.moveToPosition(pElement, CKEDITOR.POSITION_BEFORE_END);
                                selection.selectRanges([range]);
                            }
                        }
                    }
                });
            }
        }
        //thuoc tinh
        function GetProperties(MenuID) {
            var ranNum = Math.floor(Math.random() * 999999);
            var dataString = "MenuID=" + MenuID + "&LangID=<%=model.LangID %>&ProductID=<%=item.ID%>&rnd=" + ranNum;

            $.ajax({
                url: "/{CPPath}/Ajax/GetProperties.aspx",
                type: "get",
                data: dataString,
                dataType: 'json',
                success: function (data) {
                    var content = data.Html;
                    var params = data.Params;
                    if (content == '' && params == 0) {
                        $("#list-property").html('<a class="text-primary" href="javascript:ChoseProperty(' + MenuID + ');">Chọn nhóm thuộc tính</a>');
                    } else {
                        $("#list-property").html(content);
                        App.initSelect2();
                        //$("#list-property").find('select').select2();
                    }
                },
                error: function (status) { }
            });
        }
        if (<%=item.MenuID%> > 0) GetProperties('<%=item.MenuID%>');
        else GetProperties($('#MenuID').val());
        <%--//kich co
        function GetSizes(SizeID) {
            var sizes = $('#Sizes').val();
            var ranNum = Math.floor(Math.random() * 999999);
            var dataString = "SizeID=" + SizeID + "&Sizes=" + sizes + "&LangID=<%=model.LangID %>&ProductID=<%=item.ID%>&rnd=" + ranNum;

            $.ajax({
                url: "/{CPPath}/Ajax/GetSizes.aspx",
                type: "get",
                data: dataString,
                dataType: 'json',
                success: function (data) {
                    var content = data.Html;
                    $("#list-size").html(content);
                },
                error: function (status) { }
            });
        }
        if (<%=item.SizeID%> > 0) GetSizes('<%=item.SizeID%>');
        else GetSizes($('#SizeID').val());
        //mau sac
        function GetColors(ColorID) {
            var colors = $('#Colors').val();
            var ranNum = Math.floor(Math.random() * 999999);
            var dataString = "ColorID=" + ColorID + "&Colors=" + colors + "&LangID=<%=model.LangID %>&ProductID=<%=item.ID%>&rnd=" + ranNum;

            $.ajax({
                url: "/{CPPath}/Ajax/GetColors.aspx",
                type: "get",
                data: dataString,
                dataType: 'json',
                success: function (data) {
                    var content = data.Html;
                    $("#list-color").html(content);
                },
                error: function (status) { }
            });
        }
        if (<%=item.ColorID%> > 0) GetColors('<%=item.ColorID%>');
        else GetColors($('#ColorID').val());

        function addsize() {
            var value = '';
            $("input:checkbox[name=cbSizes]:checked").each(function () {
                value += (value != '' ? '|' : '') + $(this).val();
            });
            $('#Sizes').val(value);
        }
        function addcolor() {
            var value = '';
            $("input:checkbox[name=cbColors]:checked").each(function () {
                value += (value != '' ? '|' : '') + $(this).val();
            });
            $('#Colors').val(value);
        }--%>
        // Material Select Initialization
    </script>
</form>
