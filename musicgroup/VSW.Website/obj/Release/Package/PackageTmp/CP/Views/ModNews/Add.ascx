<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.CPViewControl" %>

<%
    var model = ViewBag.Model as ModNewsModel;
    var item = ViewBag.Data as ModNewsEntity;
    var lstProduct = item.GetProduct();
    var lstAuthor = ModAuthorService.Instance.CreateQuery().ToList_Cache();
%>

<form id="vswForm" name="vswForm" method="post">
    <input type="hidden" id="_vsw_action" name="_vsw_action" />
    <input type="hidden" name="RecordID" id="RecordID" value="<%=model.RecordID %>" />

    <div class="page-content-wrapper">
        <h3 class="page-title">Bài viết <small><%= model.RecordID > 0 ? "Chỉnh sửa": "Thêm mới"%></small></h3>
        <div class="page-bar justify-content-between">
            <ul class="breadcrumb">
                <li class="breadcrumb-item">
                    <i class="fa fa-home"></i>
                    <a href="/{CPPath}/">Home</a>
                </li>
                <li class="breadcrumb-item">
                    <a href="/{CPPath}/<%=CPViewPage.CurrentModule.Code%>/Index.aspx">Bài viết</a>
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
                                                <%if (!string.IsNullOrEmpty(item.Code))
                                                    { %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Xem trên web site</label>
                                                    <div class="col-md-9">
                                                        <a href="/<%=item.Code + Setting.Sys_PageExt%>" style="color: blue;" target="_blank">/<%=item.Code%>.html</a>
                                                    </div>
                                                </div>
                                                <%} %>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">URL trình duyệt:</label>
                                                    <div class="col-md-9">
                                                        <input type="text" class="form-control" name="Code" value="<%=item.Code %>" placeholder="Nếu không nhập sẽ tự sinh theo Tiêu đề" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Mô tả:</label>
                                                    <div class="col-md-9">
                                                        <textarea class="form-control description" rows="5" name="Summary"><%=item.Summary%></textarea>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Chuyên mục:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <select class="form-control select2" name="MenuID" id="MenuID">
                                                            <option value="0">Root</option>
                                                            <%= Utils.ShowDdlMenuByType("News", model.LangID, item.MenuID)%>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-md-3 col-form-label text-right">Tác giả:<span class="requied">*</span></label>
                                                    <div class="col-md-9">
                                                        <select class="form-control select2" name="AuthorID" id="AuthorID">
                                                            <option value="0">Root</option>
                                                            <%for (int i = 0; lstAuthor != null && i < lstAuthor.Count; i++)
                                                                { %>
                                                            <option value="<%=lstAuthor[i].ID %>" <%=lstAuthor[i].ID == item.AuthorID ? "selected" : "" %>><%=lstAuthor[i].Name %></option>
                                                            <% } %>
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
                                            <textarea class="form-control ckeditor" name="Content" id="Content" rows="" cols=""><%=item.Content %></textarea>
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
                                            <div class="caption">Thuộc tính</div>
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

                                                <div class="form-group">
                                                    <label class="portlet-title-sub">Vị trí</label>
                                                    <div class="checkbox-list">
                                                        <%= Utils.ShowCheckBoxByConfigkey("Mod.NewsState", "ArrState", item.State)%>
                                                    </div>
                                                </div>
                                                <%--<div class="form-group">
                                                    <label class="portlet-title-sub">Hiển thị liên hệ</label>
                                                    <div class="radio-list">
                                                        <label class="radioPure radio-inline">
                                                            <input type="radio" name="HasFeedback" <%= item.HasFeedback ? "checked": "" %> value="1" />
                                                            <span class="outer"><span class="inner"></span></span><i>Có</i>
                                                        </label>
                                                        <label class="radioPure radio-inline">
                                                            <input type="radio" name="HasFeedback" <%= !item.HasFeedback ? "checked": "" %> value="0" />
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

                                    <div class="portlet box blue-steel">
                                        <div class="portlet-title">
                                            <div class="caption">SEO</div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="form-body">
                                                <div class="form-group ">
                                                    <label class="col-form-label">PageTitle:</label>
                                                    <input type="text" class="form-control title" name="PageTitle" maxlength="200" value="<%=item.PageTitle %>" />
                                                    <span class="help-block text-primary">Ký tự tối đa: 200</span>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-form-label">Description:</label>
                                                    <textarea class="form-control description" rows="5" name="PageDescription" maxlength="400" placeholder="Nhập nội dung tóm tắt. Tối đa 400 ký tự"><%=item.PageDescription%></textarea>
                                                    <span class="help-block text-primary">Ký tự tối đa: 400</span>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-form-label">Keywords:</label>
                                                    <input type="text" class="form-control" name="PageKeywords" value="<%=item.PageKeywords %>" />
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
                                                    {
                                                        if (string.IsNullOrEmpty(item.Tags.Split(',')[i].Trim())) continue;
                                                        %>
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

                                                                    <a href="javascript:void(0)" onclick="deleteProductFromNews('<%=lstProduct[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Xóa"><i class="fa fa-ban"></i></a>
                                                                    <a href="javascript:void(0)" onclick="upProductFromNews('<%=lstProduct[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển lên trên"><i class="fa fa-arrow-up"></i></a>
                                                                    <a href="javascript:void(0)" onclick="downProductFromNews('<%=lstProduct[i].ID %>')" data-toggle="tooltip" data-placement="bottom" data-original-title="Chuyển xuống dưới"><i class="fa fa-arrow-down"></i></a>
                                                                </td>
                                                            </tr>
                                                            <%} %>
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
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        //setTimeout(function () { vsw_exec_cmd('[autosave][<%=model.RecordID%>]') }, 5000);
    </script>

</form>
