<%@ Control Language="C#" AutoEventWireup="true" Inherits="VSW.Lib.MVC.ViewControl" %>
<%
    var listvote = ModVoteService.Instance.CreateQuery().Where(o => o.ProductID == ViewPage.CurrentPage.ID && o.Type == "Page" && o.Activity == true).ToList();
    var listCommnet = (listvote != null ? listvote.Where(o => o.ParentID == 0 && o.Vote <= 0).OrderByDescending(o => o.Created).ToList() : new List<ModVoteEntity>());
%>
<hr class="mobile-line" />
<div id="dok-to-hoi-dap">
    <div id="block-comment-cps" class="dok-comments dok-box-s comment-container">
        <div class="d-flex align-items-center flex-wrap mb-3">
            <p class="dok-box-s-title mb-0">
                Hỏi đáp về sản phẩm                                   
            </p>
            <a href="javascript:void(0);" class="btn btn-secondary btn-sub2 ms-auto"
                data-bs-toggle="modal" data-bs-target="#dok-hoi-dap">Viết câu hỏi
                       </a>
        </div>

        <div class="modal fade" id="dok-hoi-dap" tabindex="-1" aria-labelledby=""
            aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title fz-18 fw-bold text-center" id="">Đặt câu hỏi cho OBIBI</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"
                            aria-label="Close">
                        </button>
                    </div>
                    <div class="modal-body">
                        <form name="comment_form" id="comment_form" method="post">
                            <div class="mb-3">
                                <textarea class="form-control" rows="4" cols="50"
                                    placeholder="Xin mời bạn để lại câu hỏi, OBIBI.VN sẽ giải đáp nhanh..."
                                    name="CommentContent0" id="CommentContent0"></textarea>
                            </div>
                            <div class="row box-input-comment">
                                <div class="col-md-6 mb-3">
                                    <input type="text" class="form-control"
                                        placeholder="Họ tên (Bắt buộc)" name="CommentName0" id="CommentName0">
                                </div>
                                <div class="col-md-6 mb-3">
                                    <input type="text" class="form-control"
                                        placeholder="Số điện thoại" name="CommentPhone0" id="CommentPhone0">
                                </div>
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-bs-dismiss="modal">Huỷ</button>
                        <button type="button" onclick="AddComment(<%=ViewPage.CurrentPage.ID %>, 0, 0, 'Page'); return false;" class="btn btn-secondary btn-sub2">
                            Gửi câu hỏi</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="dok-tra-loi" tabindex="-1" aria-labelledby=""
            aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title fz-18 fw-bold text-center" id="">Trả lời câu hỏi</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"
                            aria-label="Close">
                        </button>
                    </div>
                    <div class="modal-body">
                        <form name="reply_form" id="reply_form" method="post">
                            <input type="hidden" name="ReplyIndex" id="ReplyIndex" value="0" />
                            <div class="mb-3">
                                <textarea class="form-control" rows="4" cols="50"
                                    placeholder="Nhập nội dung trả lời"
                                    name="ReplyContent" id="ReplyContent"></textarea>
                            </div>
                            <div class="row box-input-comment">
                                <div class="col-md-6 mb-3">
                                    <input type="text" class="form-control"
                                        placeholder="Họ tên (Bắt buộc)" name="ReplyName" id="ReplyName">
                                </div>
                                <div class="col-md-6 mb-3">
                                    <input type="text" class="form-control"
                                        placeholder="Số điện thoại" name="ReplyPhone" id="ReplyPhone">
                                </div>
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-bs-dismiss="modal">Huỷ</button>
                        <button type="button" onclick="AddCommentReply(<%=ViewPage.CurrentPage.ID %>, 'Page'); return false;" class="btn btn-secondary btn-sub2">Gửi trả lời</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="box-border">
            <div class="comment comment--all ratingLst">

                <%for (int i = 0; listCommnet != null && i < listCommnet.Count; i++)
                    {
                        var sunComment = listvote.Where(o => o.ParentID == listCommnet[i].ID).OrderByDescending(o => o.Created).ToList();
                %>
                <div class="comment__item par">
                    <div class="item-top">
                        <p class="txtname"><%=listCommnet[i].Name %></p>
                    </div>
                    <div class="comment-content">
                        <div class="cmt-txt">
                            <%=listCommnet[i].Content %>
                        </div>
                    </div>
                    <div class="d-flex align-items-center mt-4">
                        <div class="text-secondary">
                            <%= string.Format("{0:dd-MM-yyyy HH:mm}", listCommnet[i].Created) %>
                        </div>
                        <button type="button" class="btn-rep-cmt" onclick="showReplyForm(<%=listCommnet[i].ID %>);">
                            <div><i class="fas fa-messages"></i></div>
                            &nbsp;Trả lời
                        </button>
                    </div>
                    <%if (sunComment != null && sunComment.Count > 0)
                        { %>
                    <ul class="listrep-at">
                        <%for (int z = 0; sunComment != null && z < sunComment.Count; z++)
                            {
                        %>
                        <li>
                            <div class="comava-at qtv-at"><%=sunComment[z].Name.Substring(0,1) %></div>
                            <div class="combody-at">
                                <strong><%=sunComment[z].Name %> </strong>
                                <p><%=sunComment[z].Content %> </p>
                            </div>
                        </li>
                        <%} %>
                    </ul>
                    <%} %>
                </div>
                <%} %>
            </div>
        </div>
    </div>
</div>
