<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModPromotionModel;
    var item = ViewBag.Data as ModPromotionEntity;
    var lstProduct = item.GetProduct();
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" name="RecordID" id="RecordID" value="<%=model.RecordID %>" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Khuyến mại <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Khuyến mại</a>
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
                                                    <label class="col-md-3 col-form-label text-right">Tiêu đề:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="Name" id="Name" value="<%=item.Name %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Chuyên mục:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-control select2" name="MenuID" id="MenuID">
                                                            <option value="0">Root</option>
                                                            <%= Utils.ShowDdlMenuByType("Product", model.LangID, item.MenuID)%>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Thương hiệu:</label>
                                                    <div class="col-md-9">
                                                        <select class="form-control select2" name="BrandID" id="BrandID">
                                                            <option value="0">Root</option>
                                                            <%= Utils.ShowDdlMenuByType("Brand", model.LangID, item.BrandID)%>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Nội dung</div>
                                        </div>
                                        <div class="portlet-body">
                                            <textarea class="form-control ckeditor" name="Content" id="Content" data-formatcheck="true" rows="" cols=""><%=item.Content %></textarea>
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
                                                 <div class="form-group">
                                                    <label class="portlet-title-sub">Từ ngày</label>
                                                    <div class="checkbox-list">
                                                        <input type="date" name="FromDate" id="FromDate" value="<%=item.FromDate > DateTime.MinValue ? item.FromDate.ToString("yyyy-MM-dd") : "" %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="portlet-title-sub">Đến ngày</label>
                                                    <div class="checkbox-list">
                                                        <input type="date" name="ToDate" id="ToDate" value="<%=item.ToDate > DateTime.MinValue ? item.ToDate.ToString("yyyy-MM-dd") : "" %>" />
                                                    </div>
                                                </div>
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
                            <div class="row">
                                <div class="col-12 col-sm-12 col-md-12 col-lg-12">
                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">Sản phẩm liên quan</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="dataTables_wrapper">
                                                <div class="table-scrollable">
                                                    <table class="table table-striped table-hover table-bordered dataTable">
                                                        <thead>
                                                            <tr>
                                                                <th class="sorting text-center w1p">#</th>
                                                                <th class="sorting">Tên sản phẩm</th>
                                                                <th class="sorting text-center w15p desktop">Ảnh</th>
                                                                <th class="sorting text-center w15p desktop">Giá bán</th>
                                                                <th class="sorting text-center w10p">Xóa</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="listproduct">
                                                            <%
                                                                for (var i = 0; lstProduct != null && i < lstProduct.Count; i++)
                                                                {
                                                            %>
                                                            <tr>
                                                                <td class="text-center"><%= i + 1%></td>
                                                                <td>
                                                                    <a href="/{CPPath}/ModProduct/Add.aspx/RecordID/<%= lstProduct[i].ID %>" target="_blank"><%= lstProduct[i].Name%></a>
                                                                </td>
                                                                <td class="text-center">
                                                                    <%= Utils.GetMedia(lstProduct[i].File, 40, 40)%>
                                                                </td>
                                                                <td class="text-center"><%= string.Format("{0:#,##0}", lstProduct[i].Price) %></td>
                                                                <td class="text-center">
                                                                    <a href="javascript:void(0)" onclick="deleteProductFromPromotion('<%=lstProduct[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa"><i class="fa fa-ban"></i></a>
                                                                </td>
                                                            </tr>
                                                            <%} %>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div class="form-group">
                                                    <div class="form-inline">
                                                        <button type="button" class="btn btn-primary" onclick="ShowProductForm('Promotion'); return false">Thêm sản phẩm</button>
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
