
class Result {
    static FromJson(json) {
        return Object.assign(new Result(), json);
    }

    IsOk() {
        return this.Code === "00";
    }

    IsError() {
        return !this.IsOk();
    }
}

function get_child(ParentID, SelectedID, id, p_option_first) {
    /*$('#loading').show();*/

    $.ajax({
        type: 'GET',
        url: '/ajax/GetChild',
        data: 'ParentID=' + ParentID + '&SelectedID=' + SelectedID,
        dataType: 'json',
        success: function (response) {
            var data = Result.FromJson(response);
            if (data.IsOk()) {
                var content = data.Data;
                if (content != '')
                    $(id).html(p_option_first + content);
                else
                    $(id).html(p_option_first);

                $(id).select2({
                    language: "vi",
                    minimumResultsForSearch: 5,
                    selectOnClose: true
                });
            }
        },
        error: function () { $('#loading').hide(); }
    });
};

function addCart(ProductID, size, color) {
    $('#loading').show();
    var Quantity = 1;
    $.ajax({
        url: '/ajax/AddToCart',
        type: 'GET',
        data: 'ProductID=' + ProductID + '&Quantity=' + Quantity + '&SizeID=' + size + '&ColorID=' + color,
        success: function (response) {
            var data = Result.FromJson(response);
            if (data.IsOk()) {

                var content = data.Data.Html;
                var count = data.Data.CartCount;
                $(".cart_count").html(count);
                $(".dok-cart-list").html(content);
                Alert("Thông báo!", "Đã thêm sản phẩm vào giỏ hàng");
                $('#loading').hide();
            }
        },
        error: function () { }
    });
};

function deleteCart(productid, index) {
    showSwalQuestion("Bạn có muốn xóa sản phẩm này khỏi giỏ hàng?", "Thông báo!", (flag) => {
        if (flag) {
            var dataString = 'ProductID=' + productid + '&Index=' + index;
            $.ajax({
                type: "GET",
                url: "/ajax/DeleteCart",
                data: dataString,
                success: function (response) {
                    var data = Result.FromJson(response);
                    if (data.IsOk()) {
                        $("#item-cart-" + index).remove();
                        $(".cart_count").html(parseFloat($(".cart_count").html()) - 1);
                        Alert("Thông báo!", 'Đã xóa thành công');
                    } else {
                        Alert("Thông báo!", data.Message);
                    }

                },
                error: function () { }
            });
        }
    });
};

function load_cart(quantity, index) {
    //$('#loading').show();
    var dataString = 'Quantity=' + quantity + '&Index=' + index;
    $.ajax({
        type: "GET", url: "/ajax/UpdateCart",
        data: dataString,
        success: function (data) {
            var content = data.Html;
            var js = data.Js;
            $("#TotalMoney").html(js);
            //$('#loading').hide();
        }, error: function () {
            //$('#loading').hide();
        }
    })
};

function loadProductClassify(productid) {
    $('#loading').show();
    var Quantity = 1;
    var size = '';
    if ($('input[type=radio][name=Size]').length) {
        size = $("input[name='Size']:checked").val();
    }
    var color = '';
    if ($('input[type=radio][name=Color]').length) {
        color = $("input[name='Color']:checked").val();
    }
    $.ajax({
        url: '/ajax/LoadPropertyBuy.html',
        type: 'GET',
        data: 'ProductID=' + productid + '&SizeID=' + size + '&ColorID=' + color,
        success: function (data) {
            var header = data.Params;
            var body = data.Html;
            $("#boxHeaderProduct").html(header);
            $("#boxBodyProduct").html(body);
            $('#dok-buy-desktop').modal('show');
            $('#loading').hide();
        },
        error: function () { }
    });
}

function loadProductMore() {
    $('#loading').show();
    let page = parseFloat($('#page').val());
    page = page + 1;
    if (page < 1) page = 1;
    $('#page').val(page);
    loadProductAjax();
}

/* filter product */
function addMenu(p_this, menuId, menuCode) {
    $('.filterMenu').find('a.active').each(function () {
        $(this).removeClass('active');
        $(this).removeClass('check');
    });
    $('.filterMenu').find('a[data-id="' + menuId + '"]').each(function () {
        $(this).addClass('active');
    });
    $(p_this).addClass('active');
    $('#menuID').val(menuId);
    $('#pageCode').val(menuCode);
    loadPropertyAjax(0);
}

function removeAllMenu(menuId, menuCode) {
    $('.filterMenu').find('a.active').each(function () {
        $(this).removeClass('active');
        $(this).removeClass('check');
    });
    var menuParentID = $('#menuParentID').val();
    var pageParentCode = $('#pageParentCode').val();
    $('#menuID').val(menuParentID);
    $('#pageCode').val(pageParentCode);
    loadPropertyAjax(1);
}

function addMenuReload(menuId, menuCode) {
    $('.filterMenu').find('a.active').each(function () {
        $(this).removeClass('active');
        $(this).removeClass('check');
    });
    $('.filterMenu').find('a[data-id="' + menuId + '"]').each(function () {
        $(this).addClass('active');
    });
    $('#menuID').val(menuId);
    $('#pageCode').val(menuCode);
    loadPropertyAjax(1);
}

function addBrand(p_this, brandId) {
    //load 1 brand
    $('.filterBrand').find('a.active').each(function () {
        $(this).removeClass('active');
        $(this).removeClass('check');
    });
    $('.filterBrand').find('a[data-id="' + brandId + '"]').each(function () {
        $(this).addClass('active');
    });
    $(p_this).addClass('active');
    $('#b').val(brandId);
    loadPropertyAjax(0);
}

function removeAllBrand() {
    $('.filterBrand').find('a.active').each(function () {
        $(this).removeClass('active');
        $(this).removeClass('check');
    });
    $('#b').val('');
    loadPropertyAjax(1);
}

function addBrandReload(p_this, brandId) {
    $('.filterBrand').find('a.active').each(function () {
        $(this).removeClass('active');
        $(this).removeClass('check');
    });
    $('.filterBrand').find('a[data-id="' + brandId + '"]').each(function () {
        $(this).addClass('active');
    });
    $('#b').val(brandId);
    loadPropertyAjax(1);
}

function addAtr(p_this, atrId, atrCode) {
    removeAllAtr2(atrCode);
    $('.filter' + atrCode).find('a[data-id="' + atrId + '"]').each(function () {
        $(this).addClass('active');
        $(this).removeClass('check');
    });
    $(p_this).addClass('active');
    let dataArray = [];
    $('.filterProperty').find('.c-btnbox.active').each(function (index, item) {
        let value = $(item).data('id');
        if (!dataArray.includes(value)) {
            dataArray.push(value);
        }
    });
    let atr = '';
    console.log(dataArray);
    if (dataArray && dataArray.length > 0) {
        atr = dataArray.join('-');
    }
    $('#c').val(atr);
    loadPropertyAjax(0);
    //let atr = $('#c').val();
    //$('.filter' + atrCode).find('a[data-id="' + atrId + '"]').each(function () {
    //    $(this).addClass('active');
    //    $(this).removeClass('check');
    //});
    //$(p_this).addClass('active');
    //atr += (atr != '' ? '-' : '') + atrId;
    //$('#c').val(atr);
    //loadPropertyAjax(0);
}

function removeAllAtr(atrCode) {
    let atr = $('#c').val();
    var arrAtr = atr.split('-');
    $('.filter' + atrCode).find('a.active').each(function () {
        $(this).removeClass('active');
        $(this).removeClass('check');
        let index = arrAtr.indexOf($(this).data('id').toString());
        if (index > -1) {
            arrAtr.splice(index, 1);
        }
    });
    atr = arrAtr.join("-");
    $('#c').val(atr);
    loadPropertyAjax(1);
}

function removeAllAtr2(atrCode) {
    let atr = $('#c').val();
    var arrAtr = atr.split('-');
    $('.filter' + atrCode).find('a.active').each(function () {
        $(this).removeClass('active');
        $(this).removeClass('check');
        let index = arrAtr.indexOf($(this).data('id').toString());
        if (index > -1) {
            arrAtr.splice(index, 1);
        }
    });
    atr = arrAtr.join("-");
    $('#c').val(atr);
}

function loadPropertyAjax(p_load = 0) {
    //console.log($("#productIndex").serialize());
    //$('#loading').show();
    $('.btn-filter-readmore').attr('href', 'javascript:;');
    $('.btn-filter-readmore').removeAttr('onclick');
    $('<div class="stage-two"><div class="load"><\/div><\/div>').insertBefore('.total-reloading');
    $.ajax({
        url: '/ajax/LoadProperty.html',
        type: 'POST',
        data: $("#productIndex").serialize(),
        success: function (data) {
            var count = parseInt(data.Html);
            if (isNaN(count)) count = 0;
            var url = data.Params;
            if (p_load == 1) {
                location.href = url;
            }
            if (count > 0) {
                $('.btn-filter-readmore').attr('href', 'javascript:;');
                $('.btn-filter-readmore').attr('onclick', "loadurl('" + url + "');");

                $('.btn-filter-readmore').removeClass('disabled').removeClass('prevent');
                $('.btn-filter-readmore').html('Xem <b class="total-reloading">' + count + '</b> kết quả');

            } else {
                $('.btn-filter-readmore').attr('href', 'javascript:;');
                $('.btn-filter-readmore').removeAttr('onclick');
                $('.btn-filter-readmore').addClass('disabled').addClass('prevent');
                $('.total-reloading').html('0');
            }
            $(".stage-two").each(function () {
                $(this).remove();
            });
            //$('#loading').hide();
        },
        error: function () { }
    });
};

function loadProductAjax() {
    $.ajax({
        url: '/ajax/LoadProduct.html',
        type: 'POST',
        data: $("#productIndex").serialize(),
        success: function (data) {
            var count = parseInt(data.Params);
            if (isNaN(count)) count = 0;
            var html = data.Html;
            var js = data.Js;

            $('.countProduct').html(count);
            if (html != '') {
                $('#boxProduct').append(html);
            }

            if (js == "0") {
                $('#page_ajax').hide();
            }
            $('#loading').hide();
        },
        error: function () { }
    });
}

function loadBreadcrumbAjax() {
    let rawUrl = window.location.href;
    const lastSegment = rawUrl.split('/').pop().split('?')[0];

    $.ajax({
        url: '/ajax/LoadBreadcrumb.html?Url=' + lastSegment,
        type: 'POST',
        data: $("#productIndex").serialize(),
        success: function (data) {
            var html = data.Html;
            var titleAdd = data.Params;
            if (html != '') {
                $('#breadcrumb').html(html);
            }
            if (titleAdd != '') {
                $('.pageProductName').append(titleAdd);
                let currentContent = $('meta[itemprop="name"]').attr("content");
                $('meta[itemprop="name"]').attr("content", currentContent + " " + titleAdd);
                $('meta[property="og:title"]').attr("content", $('meta[property="og:title"]').attr("content") + " " + titleAdd);
                $('meta[name="title"]').attr("content", currentContent + " " + + titleAdd);
                $('title').text(currentContent + " " + titleAdd);
                document.title = currentContent + " " + titleAdd;

            }
        },
        error: function () { }
    });
}

/* filter product end*/

/* detail product */
function loadProductDetail() {
    var productid = $('#ProductID').val();
    $('#loading').show();
    let dataString = 'ProductID=' + productid;
    $.ajax({
        type: "GET",
        url: "/ajax/LoadProductDetail.html",
        data: dataString,
        success: function (data) {
            let content = data.Html;
            if (content != '') {
                let arr = JSON.parse(content);
                for (let i = 0; arr != null && i < arr.length; i++) {
                    let key = arr[i].Key;
                    let data = arr[i].Data;
                    if (key == 'video') {
                        loadVideo(data);
                    }
                    else if (key == 'faq') {
                        loadFAQ(data);
                    }
                    else if (key == 'like') {
                        loadLike(data);
                    }
                    else if (key == 'customer') {
                        loadCustomer(data);
                    }
                }
            }
            //changePrice();
            $('#loading').hide();
        },
        error: function () { }
    });
};
function loadVideo(listVideo) {
    if (loadVideo != null) {
        if (listVideo.length > 0) {
            let htmlContent = '<div class="tab-mobile d-lg-none d-block">';
            for (let i = 0; listVideo != null && i < listVideo.length; i++) {
                htmlContent += '<div class="position-relative mb-3">';
                htmlContent += '<a href="' + listVideo[i].File + '" data-fancybox="video-gallery-mobile">';
                htmlContent += '<i class="icon-play fab fa-youtube"></i>';
                htmlContent += '<img class="img-fluid" src="' + GetUrlFile(listVideo[i].Image) + '" alt="' + listVideo[i].Name + '" />';
                htmlContent += '</a>';
                htmlContent += '</div>';
            }
            htmlContent += '</div>';

            htmlContent += '<div class="tab-desktop d-lg-block d-none">';
            htmlContent += '<div class="tab-content">';
            for (let i = 0; listVideo != null && i < listVideo.length; i++) {
                htmlContent += '<div class="tab-pane fade ' + (i == 0 ? ' show active' : '') + '" id="video-' + (i + 1) + '" role="tabpanel" aria-labelledby="video-tab-' + (i + 1) + '">';
                htmlContent += '    <a href="' + listVideo[i].File + '" data-fancybox="video-gallery">';
                htmlContent += '        <i class="icon-play fab fa-youtube"></i>';
                htmlContent += '        <img class="img-fluid" src="' + GetUrlFile(listVideo[i].Image) + '" alt="' + listVideo[i].Name + '" />';
                htmlContent += '    </a>';
                htmlContent += '</div>';
            }
            htmlContent += '</div>';
            htmlContent += '<ul class="nav nav-tabs" role="tablist">';
            for (let i = 0; listVideo != null && i < listVideo.length; i++) {
                htmlContent += '<li class="' + (i == 0 ? ' active' : '') + '" id="video-tab-' + (i + 1) + '" data-bs-toggle="tab"';
                htmlContent += '    data-bs-target="#video-' + (i + 1) + '" type="button" role="tab"';
                htmlContent += '    aria-controls="video-' + (i + 1) + '" aria-selected="true">';
                htmlContent += '    <img class="img-fluid" src="' + GetUrlFile(listVideo[i].Image) + '" />';
                htmlContent += '</li>';
            }
            htmlContent += '</ul>';
            htmlContent += '</div>';
            $('#tab-video').append(htmlContent);
            if ($('[data-fancybox="video-gallery"]').length > 0) {
                Fancybox.bind('[data-fancybox="video-gallery"]', {
                    Thumbs: {
                        type: "classic",
                    },
                    parentEl: document.getElementById("tab-video")
                });
            }


            if ($('[data-fancybox="video-gallery-mobile"]').length > 0) {
                Fancybox.bind('[data-fancybox="video-gallery-mobile"]', {
                    Thumbs: {
                        type: "classic",
                    },
                    parentEl: document.getElementById("tab-video")
                });
            }
        }
    }
}
function loadFAQ(listFAQ) {
    if (listFAQ != null) {
        if (listFAQ.length > 0) {
            let htmlContent = '';
            for (let i = 0; listFAQ != null && i < listFAQ.length; i++) {
                htmlContent += '<div class="mb-1">';
                htmlContent += '<button class="b-button collapsed" type="button" data-bs-toggle="collapse"';
                htmlContent += 'data-bs-target="#fq-' + (i) + '">';
                htmlContent += '<h3>' + listFAQ[i].Name + '</h3>';
                htmlContent += '<div class="icon">';
                htmlContent += '<i class="fas fa-angle-right"></i>';
                htmlContent += '</div>';
                htmlContent += '</button>';
                htmlContent += '<div id="fq-' + (i) + '" class="accordion__content collapse">';
                htmlContent += '<div class="content-wrapper">';
                htmlContent += '<div>';
                htmlContent += '' + listFAQ[i].Content + '';
                htmlContent += '</div>';
                htmlContent += '</div>';
                htmlContent += '</div>';
                htmlContent += '</div>';
            }
            $('#accordion').append(htmlContent);
            $('#boxFaq').removeClass('hide');
            //$('#accordion .collapse').collapse();
        }
    }
}
function loadLike(listLike) {
    if (listLike != null) {
        for (let i = 0; listLike != null && i < listLike.length; i++) {
            //like vote
            if (!listLike[i].IsVote) {
                loadHtmlVote(listLike[i].CommentID, listLike[i].Count, listLike[i].IsLike);
            } else {
                //like comment
                loadHtmlComment(listLike[i].CommentID, listLike[i].Count, listLike[i].IsLike);
                loadHtmlCommentUnLike(listLike[i].CommentID, listLike[i].CountUnLike, listLike[i].IsUnLike);
            }
        }
    }
}
function loadCustomer(data) {
    if (data != null) {
        $('#cfmName').val(data.Name);
        $('#cfmEmail').val(data.Email);
        $('#cfmPhone').val(data.Phone);
        var gender = data.AnhChi;
        $('input:radio[name=cfmGender]').prop('checked', false);
        if (gender) $('#cfmGender1').prop('checked', true);
        else $('#cfmGender0').prop('checked', true);

        load_nameCmt(data.Name);
    }
}
function loadHtmlVote(CommentID, Count, IsLike) {
    let html = '';
    if (IsLike) {
        html = '<i class="icondetail-like"></i> Hữu ích (' + Count + ')';
        $('#CommentLike_' + CommentID).attr('data-like', '0');
        $('#CommentLike_' + CommentID).attr('href', 'javascript:;');
    } else {
        html = '<i class="icondetail-likewhite"></i> Hữu ích (' + Count + ')';
        $('#CommentLike_' + CommentID).attr('data-like', '1');
        $('#CommentLike_' + CommentID).attr('href', 'javascript:likeRating(' + CommentID + ',' + Count + ')');
    }
    $('#CommentLike_' + CommentID).html(html);
}
function loadHtmlVoteUnLike(CommentID, Count, IsLike) {
    let html = '';
    if (IsLike) {
        html = '<i class="fa fa-thumbs-down active"></i> Không hữu ích (' + Count + ')';
        $('#CommentUnLike_' + CommentID).attr('data-like', '0');
        $('#CommentUnLike_' + CommentID).attr('href', 'javascript:;');
    } else {
        html = '<i class="fa fa-thumbs-down"></i> Không hữu ích (' + Count + ')';
        $('#CommentUnLike_' + CommentID).attr('data-like', '1');
        $('#CommentUnLike_' + CommentID).attr('href', 'javascript:unLikeRating(' + CommentID + ',' + Count + ')');
    }
    $('#CommentUnLike_' + CommentID).html(html);
}
function loadHtmlComment(CommentID, Count, IsLike) {
    let html = '';
    if (IsLike) {
        html = '<i class="fa fa-thumbs-up active"></i> Hài lòng (' + Count + ')';
        $('#CommentLike_' + CommentID).attr('data-like', '0');
        $('#CommentLike_' + CommentID).attr('href', 'javascript:;');
    } else {
        html = '<i class="fa fa-thumbs-up"></i> Hài lòng (' + Count + ')';
        $('#CommentLike_' + CommentID).attr('data-like', '1');
        $('#CommentLike_' + CommentID).attr('href', 'javascript:likeRating(' + CommentID + ',' + Count + ', true)');
    }
    $('#CommentLike_' + CommentID).html(html);
}
function loadHtmlCommentUnLike(CommentID, Count, IsLike) {
    let html = '';
    if (IsLike) {
        html = '<i class="fa fa-thumbs-down active"></i> Không hài lòng (' + Count + ')';
        $('#CommentUnLike_' + CommentID).attr('data-like', '0');
        $('#CommentUnLike_' + CommentID).attr('href', 'javascript:;');
    } else {
        html = '<i class="fa fa-thumbs-down"></i> Không hài lòng (' + Count + ')';
        $('#CommentUnLike_' + CommentID).attr('data-like', '1');
        $('#CommentUnLike_' + CommentID).attr('href', 'javascript:unLikeRating(' + CommentID + ',' + Count + ', true)');
    }
    $('#CommentUnLike_' + CommentID).html(html);
}
function likeRating(CommentID, Count, isComment = false) {
    var ProductID = $('#ProductID').val();
    if (isComment) {
        loadHtmlComment(CommentID, (Count + 1), true);
    } else {
        loadHtmlVote(CommentID, (Count + 1), true);
    }
    $('#loading').show();
    let dataString = 'ProductID=' + ProductID + '&CommentID=' + CommentID;
    $.ajax({
        type: "GET",
        url: "/ajax/LikeRating.html",
        data: dataString,
        success: function (data) {
            let content = data.Html;
            $('#loading').hide();
        },
        error: function () { }
    });
}
function unLikeRating(CommentID, Count, isComment = false) {
    var ProductID = $('#ProductID').val();
    if (isComment) {
        loadHtmlCommentUnLike(CommentID, (Count + 1), true);
    } else {
        loadHtmlVoteUnLike(CommentID, (Count + 1), true);
    }
    $('#loading').show();
    let dataString = 'ProductID=' + ProductID + '&CommentID=' + CommentID;
    $.ajax({
        type: "GET",
        url: "/ajax/UnLikeRating.html",
        data: dataString,
        success: function (data) {
            let content = data.Html;
            $('#loading').hide();
        },
        error: function () { }
    });
}
function loadFile(input, p_control) {
    if (input.files) {
        for (var i = 0; i < (input.files.length > 3 ? 3 : input.files.length); i++) {

            var reader = new FileReader();
            reader.onload = function (e) {
                let html = '<li><img src="' + e.target.result + '"></li>';
                $(p_control).append(html.trim());
            }
            reader.readAsDataURL(input.files[i]);
        }
        $(p_control).removeClass('hide');
    }
}
function submitCommentRating() {
    $('#loading').show();
    $.ajax({
        url: '/ajax/CommentRating.html',
        type: 'POST',
        data: $("#ratingform").serialize(),
        success: function (data) {
            var content = data.Html;
            var param = data.Params;
            if (param != '') {
                Alert('Thông bao!', param);
            } else {
                Success('Thông bao!', 'Đã gửi đánh giá thành công.', '');
                hideInputRating();
                $(".ul-star li i").removeClass("active");
                $(".ul-star li p").removeClass("active-slt");
                $("#ratingform")[0].reset();
            }
            $('#loading').hide();
        },
        error: function () { }
    });
}
/* detail product end */
function rCmtConfirmUser() {
    $('#loading').show();

    $.ajax({
        url: '/ajax/ConfirmUser.html',
        type: 'POST',
        data: $("#cfmForm").serialize(),
        success: function (data) {

            var content = data.Html;
            var param = data.Params;
            if (param != '') {
                Alert('Thông báo!', param);
            }
            if (content != '') {
                Alert('Thông báo!', content);
            }
            load_nameCmt($('#cfmName').val());
            hideReplyConfirmPopup();
            $('#loading').hide();
        },
        error: function () { }
    });
};
function ratingRelply(productid, commentid, type) {
    var name = $('#cfmName').val();
    if (name == '') {
        showReplyConfirmPopup();
    }
    else {
        $('#loading').show();
        Addvote(productid, commentid, commentid, type, false);
    }
}

function AddComment(productid, commentid, parentid, type, isVote = true) {
    $('#loading').show();
    var vote = 0;
    if ($("input[name='ValueVote']").length && isVote) {
        vote = $("input[name='ValueVote']:checked").val();
    }
    $.ajax({
        url: '/ajax/AddComment.html',
        type: 'POST',
        data: {
            ProductID: productid,
            ParentID: parentid,
            Type: type,
            Name: (typeof $('#CommentName' + commentid) !== "undefined") ? $('#CommentName' + commentid).val() : '',
            Phone: (typeof $('#CommentPhone' + commentid) !== "undefined") ? $('#CommentPhone' + commentid).val() : '',
            Email: (typeof $('#CommentEmail' + commentid) !== "undefined") ? $('#CommentEmail' + commentid).val() : '',
            Content: (typeof $('#CommentContent' + commentid) !== "undefined") ? $('#CommentContent' + commentid).val() : '',
            Vote: vote
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;
            var param = data.Params;
            if (param != '') {
                Alert('Thông báo!', param);
            }
            else if (content != '') {
                load_nameCmt($('#CommentName' + commentid).val());
                Success('Thông báo!', content, '');
                $('#CommentName' + commentid).val('');
                $('#CommentPhone' + commentid).val('');
                $('#CommentContent' + commentid).val('');
                $('#dok-hoi-dap').modal('hide');
            }
            $('#loading').hide();
        },
        error: function () { }
    });
}


function AddCommentReply(productid, type) {
    $('#loading').show();
    var vote = 0;
    $.ajax({
        url: '/ajax/AddComment.html',
        type: 'POST',
        data: {
            ProductID: productid,
            ParentID: $('#ReplyIndex').val(),
            Type: type,
            Name: (typeof $('#ReplyName') !== "undefined") ? $('#ReplyName').val() : '',
            Phone: (typeof $('#ReplyPhone') !== "undefined") ? $('#ReplyPhone').val() : '',
            Email: (typeof $('#ReplyEmail') !== "undefined") ? $('#ReplyPhone').val() : '',
            Content: (typeof $('#ReplyContent') !== "undefined") ? $('#ReplyContent').val() : '',
            Vote: vote
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;
            var param = data.Params;
            if (param != '') {
                Alert('Thông báo!', param);
            }
            else if (content != '') {
                Success('Thông báo!', content, '');
                $('#ReplyName').val('');
                $('#ReplyPhone').val('');
                $('#ReplyContent').val('');
                $('#dok-tra-loi').modal('hide');
            }
            $('#loading').hide();
        },
        error: function () { }
    });
}

function ChangePrice() {
    var size = '';
    if ($('input[type=radio][name=Size]').length) {
        size = $("input[name='Size']:checked").val();
    }
    var color = '';
    if ($('input[type=radio][name=Color]').length) {
        color = $("input[name='Color']:checked").val();
    }
    var ProductID = $('#ProductID').val();
    $('#loading').show();
    $.ajax({
        url: '/ajax/ChangePrice.html',
        type: 'GET',
        data: 'ProductID=' + ProductID + '&SizeID=' + size + '&ColorID=' + color,
        success: function (data) {
            //console.log(data);
            var html = data.Html;
            $('.productPrice').html(html);
            $('#loading').hide();
        },
        error: function () { }
    });
}