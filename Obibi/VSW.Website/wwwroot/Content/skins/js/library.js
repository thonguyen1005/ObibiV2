// CSRF (XSRF) security
function addAntiForgeryToken(data) {
    //if the object is undefined, create a new one.
    if (!data) {
        data = {};
    }
    //add token
    var tokenInput = $('input[name=__RequestVerificationToken]');
    if (tokenInput.length) {
        data.__RequestVerificationToken = tokenInput.val();
    }
    return data;
};

function doSearch(p_url, p_control) {
    var sURL = '';

    if (p_control.val().length < 2) {
        Alert('Thông báo !', 'Từ khóa phải nhiều hơn 1 ký tự.');
        return;
    }
    else sURL += (sURL == '' ? '?' : '&') + 'keyword=' + p_control.val();

    location.href = p_url + sURL;
}

$(document).on('click', '.addToCartAjax', function () {
    $('.btnAddToNow').hide();
    $('.btnAddToCart').show();
    $('#productIDFromProduct').val($(this).data('id'));
    var hasProperty = $(this).data('hasproperty');
    if (typeof hasProperty === 'undefined') {
        hasProperty = false;
    }
    if (hasProperty) {
        loadProductClassify($(this).data('id'));
    } else {
        var size = '';
        if ($('input[type=radio][name=Size]').length) {
            size = $("input[name='Size']:checked").val();
        }
        var color = '';
        if ($('input[type=radio][name=Color]').length) {
            color = $("input[name='Color']:checked").val();
        }
        addCart($(this).data('id'), size, color);
    }
});
$(document).on('click', '.btnAddToCart', function () {
    var size = '';
    if ($('input[type=radio][name=SizeFromProduct]').length) {
        size = $("input[name='SizeFromProduct']:checked").val();
    }
    var color = '';
    if ($('input[type=radio][name=ColorFromProduct]').length) {
        color = $("input[name='ColorFromProduct']:checked").val();
    }
    addCart($('#productIDFromProduct').val(), size, color);
});

$(document).on('click', '.btnAddToNow', function () {
    var size = '';
    if ($('input[type=radio][name=SizeFromProduct]').length) {
        size = $("input[name='SizeFromProduct']:checked").val();
    }
    var color = '';
    if ($('input[type=radio][name=ColorFromProduct]').length) {
        color = $("input[name='ColorFromProduct']:checked").val();
    }
    addCart($('#productIDFromProduct').val(), size, color, () => {
        location.href = '/gio-hang';
    });
});

$(document).ready(function () {
    _component_select2();
});

$(function () {
    $('.addcartnow').click(function () {
        $('.btnAddToNow').show();
        $('.btnAddToCart').hide();
        $('#productIDFromProduct').val($(this).data('id'));
        var hasProperty = $(this).data('hasproperty');
        if (typeof hasProperty === 'undefined') {
            hasProperty = false;
        }
        if (hasProperty) {
            loadProductClassify($(this).data('id'));
        } else {
            var size = '';
            if ($('input[type=radio][name=Size]').length) {
                size = $("input[name='Size']:checked").val();
            }
            var color = '';
            if ($('input[type=radio][name=Color]').length) {
                color = $("input[name='Color']:checked").val();
            }
            addCartUrl($('#ProductID').val(), $('#ReturnPath').val(), size, color);
        }
    });
    //$('.addcart').click(function () {
    //    var size = '';
    //    if ($('input[type=radio][name=Size]').length) {
    //        size = $("input[name='Size']:checked").val();
    //    }
    //    var color = '';
    //    if ($('input[type=radio][name=Color]').length) {
    //        color = $("input[name='Color']:checked").val();
    //    }
    //    addCart($(this).data('id'), size, color);
    //    //addCartUrl($('#ProductID').val(), $('#ReturnPath').val(), size, color);
    //})
});

function addCartUrl(ProductID, ReturnPath, size, color) {
    var Quantity = 1;
    location.href = '/gio-hang/Add.html?ProductID=' + ProductID + '&Quantity=' + Quantity + '&SizeID=' + size + '&ColorID=' + color + '&returnpath=' + ReturnPath;
}

$(function () {
    if (typeof hasProperty !== 'undefined') {
        activeProperty();
        loadProductAjax();
        loadBreadcrumbAjax();
    }
    if (typeof isDetailProduct !== 'undefined') {
        loadProductDetail();
    }
    if (typeof isCart !== "undefined") {
        if (CityID > 0) {
            get_child(CityID, DistrictID, '.list-district', '<option value=0>Chọn quận / huyện</option>');
        }
        if (DistrictID > 0) {
            get_child(DistrictID, WardID, '.list-ward', '<option value=0>Chọn phường / xã</option>');
        }
    }
})

function activeProperty() {
    mapParams();
    if ($('#b').length) {
        var lstbrand = $('#b').val().split('-');
        lstbrand.forEach(value => {
            $('.filterBrand a[data-id="' + value + '"]').each(function (idx, item) {
                $(item).addClass('active');
                var parent = $(item).closest('.filter-box');
                console.log(parent);
                if (typeof (parent) != undefined) {
                    let name = $(item).data('name');
                    parent.find('.filter-box-txt').html(name);
                    parent.find('.filter-box-txt').addClass('active');
                }
            });
        });
    }
    if ($('#c').length) {
        var lstatr = $('#c').val().split('-');
        lstatr.forEach(value => {
            $('.filterAtr a[data-id="' + value + '"]').each(function (idx, item) {
                $(item).addClass('active');
                var parent = $(item).closest('.filter-box');
                if (typeof (parent) != undefined) {
                    //let name = $(item).data('name');
                    //let parentName = parent.data('name');
                    //let parentTitle = parent.find('.filter-box-txt').html();
                    //if (parentName == parentTitle) {
                    //    parent.find('.filter-box-txt').html(name);
                    //} else {
                    //    parent.find('.filter-box-txt').html(parentTitle + ', ' + name);
                    //}
                    parent.find('.filter-box-txt').addClass('active');
                }
            });
        });
    }
}

function mapParams() {
    // Lấy phần hash sau dấu #
    let hash = window.location.hash.substring(1); // Loại bỏ dấu #

    // Tạo đối tượng chứa các tham số
    let params = {};

    // Tách các cặp key=value bằng dấu &
    let keyValuePairs = hash.split('&');

    keyValuePairs.forEach(pair => {
        // Tách key và value bằng dấu =
        const [key, value] = pair.split('=');

        // Lưu vào đối tượng params
        if (key) {
            if ($('#' + key).length) {
                $('#' + key).val(decodeURIComponent(value || ''));
            }
            params[key] = decodeURIComponent(value || ''); // Giải mã URI và gán giá trị mặc định là chuỗi rỗng nếu không có value
        }
    });

    //return params;
}

function GetURL(menuId, code) {
    let url = '/' + code + '.html';
    return url;
}

function GetUrlFile(file) {
    if (file == '')
        return "/Content/images/noimage.png";

    if (file.startsWith("http"))
        return file;

    return file.replace("~/", "/");
}

function showInputRating(n) {
    $(".read-assess").removeClass("hide").addClass("showR");
    $(".locationbox__overlay").show();
}
function hideInputRating() {
    $(".read-assess").removeClass("showR").addClass("hide");
    $(".locationbox__overlay").hide();
}
$(".ul-star li").click(function () {
    var n, t;
    for ($(".ul-star li i").removeClass("active"),
        $(".ul-star li p").removeClass("active-slt"),
        n = parseInt($(this).attr("data-val")),
        t = 0; t < n; t++)
        $(".ul-star li i").eq(t).addClass("active");
    $(".ul-star li p").eq(n - 1).addClass("active-slt");
    $("#StarVote").val(n);
});
$(".ul-orslt li .btn-assess").click(function () {
    var n = $(this).attr("data-id"),
        t = $(this).attr("data-val");
    $(".criteriaID" + n + " .btn-assess").removeClass("checkact");
    $(this).addClass("checkact");
    $("#criteriaID" + n).attr("value", t)
});

function showRatingCmtChild(n) {
    var t = n.replace("r-", "");
    $(".rp-" + t).removeClass("hide");
    $(".rr-" + t).removeClass("hide");
}
function showReplyConfirmPopup() { $(".rRepPopup").removeClass("hide").addClass("blockshow"); $(".locationbox__overlay").show(); }
function hideReplyConfirmPopup() { $(".rRepPopup").removeClass("blockshow").addClass("hide"); $(".locationbox__overlay").hide(); }

function load_nameCmt(cfmName) {
    if (cfmName != '') {
        $(".ifrl").find("span").html(cfmName);
        $(".ifrl").removeClass("hide");
        $(".box-input-comment").addClass("hide");
    }
}

function showReplyForm(commentId) {
    $('#ReplyIndex').val(commentId);

    $('#dok-tra-loi').modal('show');
}
function loadurl(url) {
    $('#loading').show();
    const baseUrl = url.split('#')[0]; // Lấy phần URL trước dấu #
    const newHash = url.split('#')[1]; // Lấy phần hash từ URL truyền vào

    // Kiểm tra nếu có hash mới để thêm vào
    if (newHash) {
        window.location.href = `${baseUrl}#${newHash}`; // Thay đổi hash mà không thay đổi URL chính
        location.reload(); // Tải lại trang
    } else {
        window.location.href = url; // Nếu không có hash thì chuyển hướng đến URL đầy đủ
    }

    window.location.href = url;
}

var _component_select2 = function (p_select) {
    if (!$().select2) {
        console.warn('Warning - Select2 Js is not loaded.');
        return;
    }

    var select = $('[select2]');
    if (p_select) select = p_select;

    if (select.length > 0) {
        $(select).select2({
            language: "vi",
            minimumResultsForSearch: 5,
            selectOnClose: true
        });
        //thonv edit
        $(select).on('select2:selecting', function (e) {
            var value = e.params.args.data.id;
            if (value != '') {
                var thisAlert = $(e.currentTarget).parent();
                $(thisAlert).attr("data-validate", "");
                $(thisAlert).removeClass('alert-validate');
                $(thisAlert).removeClass('active');
                $(thisAlert).addClass("has-success").removeClass("has-error");
                $(e.currentTarget)[0].setCustomValidity('');
            }
        });
    }
}