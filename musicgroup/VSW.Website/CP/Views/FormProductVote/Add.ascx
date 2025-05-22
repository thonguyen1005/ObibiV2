<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModProductVoteModel;
    var item = ViewBag.Data as ModProductVoteEntity;
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Câu hỏi <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
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
                                                    <label class="col-md-3 col-form-label text-right">Chọn tiêu đề:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <select class="form-control select2" name="MenuID" id="MenuID" onchange="changeName(this);">
                                                            <option value="0">Root</option>
                                                            <%= Utils.ShowDdlMenuByTypeVote("ProductVote", model.LangID, item.MenuID)%>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tiêu đề:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="Name" id="Name" value="<%=item.Name %>" />
                                                        <input type="checkbox" name="IsSave" id="IsSave" value="1" />
                                                        Cập nhật vào danh sách
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tiêu đề có:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="NameYes" id="NameYes" value="<%=item.NameYes %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tiêu đề không:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control title" name="NameNo" id="NameNo" value="<%=item.NameNo %>" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Số lượng đồng ý:</label>
                                                    <div class="col-md-9">
                                                        <div class="form-inline">
                                                            <input type="number" class="form-control price" name="Yes" value="<%=item.Yes %>" />
                                                            <span class="help-block text-primary"><%=Utils.NumberToWord(item.Yes.ToString())%></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Số lượng không đồng ý:</label>
                                                    <div class="col-md-9">
                                                        <div class="form-inline">
                                                            <input type="number" class="form-control price" name="No" value="<%=item.No %>" />
                                                            <span class="help-block text-primary"><%=Utils.NumberToWord(item.No.ToString())%></span>
                                                        </div>
                                                    </div>
                                                </div>
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
                                                <%-- <div class="form-group">
                                                    <label class="portlet-title-sub">Hiển thị</label>
                                                    <div class="radio-list">
                                                        <label class="radioPure radio-inline">
                                                            <input type="radio" name="TypeView" <%= item.TypeView == 1 ? "checked": "" %> value="1" />
                                                            <span class="outer"><span class="inner"></span></span><i>Biểu đồ tròn</i>
                                                        </label>
                                                        <label class="radioPure radio-inline">
                                                            <input type="radio" name="TypeView" <%= item.TypeView != 1 ? "checked": "" %> value="0" />
                                                            <span class="outer"><span class="inner"></span></span><i>Biểu đồ đánh giá ngang</i>
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
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</form>
<script type="text/javascript">
    function changeName(p_select) {
        if ($(p_select).val() > 0) {
            var optionSelected = p_select.options[p_select.selectedIndex];
            var summary = $(optionSelected).data('summary');
            $('#Name').val(optionSelected.text);
            if (summary != '') {
                var arrSummary = summary.split(',');
                if (arrSummary.length > 0) {
                    $('#NameYes').val(arrSummary[0]);
                }
                if (arrSummary.length > 1) {
                    $('#NameNo').val(arrSummary[1]);
                }
            }
            $('#IsSave').prop('checked', false);
            $('#IsSave').removeAttr('checked');
        }
    }
</script>
