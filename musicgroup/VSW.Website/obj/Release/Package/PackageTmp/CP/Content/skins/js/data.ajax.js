//Sản phẩm
function ShowProductForm(sType) {
    window.open('/' + window.CPPath + '/FormProduct/Index.aspx?Type=' + sType, '', 'width=1280, height=800, top=80, left=20,scrollbars=yes');
    return false;
}
function CloseProduct(productID, sType) {
    if (window.opener) {
        window.opener.refreshProduct(productID, sType);
    }
    else {
        window.parent.refreshProduct(productID, sType);
    }

    window.close();
}
function refreshProduct(productID, sType) {
    addProduct(productID, sType)
}
function addProduct(ProductID, sType) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddProduct.aspx',
        data: {
            ID: $('#RecordID').val(),
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: ProductID,
            Type: sType
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo!', params);
                return;
            }

            $('#listproduct').html(content);
            if (js != '') {
                $('#RecordID').val(js);
                if (sType == 'News') {
                    window.history.pushState('', '', '/' + window.CPPath + '/ModNews/Add.aspx/RecordID/' + js);
                }
                else window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
function CloseProductMulti(sType) {
    var allVals = [];
    $('input[name="cid"]:checked').each(function () {
        allVals.push($(this).val());
    });

    if (window.opener)
        window.opener.refreshProductMulti(allVals, sType);
    else
        window.parent.refreshProductMulti(allVals, sType);

    window.close();
}
function refreshProductMulti(allVals, sType) {
    addProductMulti(allVals, sType);
}
function addProductMulti(allVals, sType) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddProductMulti.aspx',
        data: {
            ID: $('#RecordID').val(),
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ListProductID: allVals,
            Type: sType
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#listproduct').html(content);
            if (js != '') {
                $('#RecordID').val(js);
                if (sType == 'News') {
                    window.history.pushState('', '', '/' + window.CPPath + '/ModNews/Add.aspx/RecordID/' + js);
                }
                else window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
//gift
function ShowGiftForm(sValue) {
    var RecordID = $('#RecordID').val();
    window.open('/' + window.CPPath + '/FormGift/Index.aspx?Value=' + RecordID, '', 'width=1024, height=800, top=80, left=200,scrollbars=yes');
    return false;
}
function refreshGift(arg) {
    addGift(arg)
}
function CloseGift(arg) {
    if (window.opener)
        window.opener.refreshGift(arg);
    else
        window.parent.refreshGift(arg);

    window.close();
}
function CloseGiftMulti() {
    var allVals = [];
    $('input[name="cid"]:checked').each(function () {
        allVals.push($(this).val());
    });

    if (window.opener)
        window.opener.refreshGiftMulti(allVals);
    else
        window.parent.refreshGiftMulti(allVals);

    window.close();
}
function refreshGiftMulti(allVals) {
    addGiftMulti(allVals);
}
function addGift(giftID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddGift.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            GiftID: giftID
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-gift').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
function addGiftMulti(allVals) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddGiftMulti.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            ListGiftID: allVals
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-gift').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
function deleteGift(giftID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DeleteGift.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            GiftID: giftID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-gift').html(content);
        },
        error: function (status) { }
    });
}
function upGift(giftID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpGift.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            GiftID: giftID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-gift').html(content);
        },
        error: function (status) { }
    });
}
function downGift(giftID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DownGift.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            GiftID: giftID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-gift').html(content);
        },
        error: function (status) { }
    });
}
//file
function ShowFile() {
    var finder = new CKFinder();
    finder.basePath = '../';
    finder.selectActionFunction = refreshFile;
    finder.popup();

    return false;
}
function refreshFile(fileUrl, file, files) {
    for (var i = 0; i < files.length; i++) {
        addFile(files[i].url);
    }
}
function addFile(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddFile.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-file').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
function deleteFile(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DeleteFile.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-file').html(content);
        },
        error: function (status) { }
    });
}
function upFile(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpFile.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-file').html(content);
        },
        error: function (status) { }
    });
}
function downFile(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DownFile.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-file').html(content);
        },
        error: function (status) { }
    });
}
function ChoseFile(fileID, productID, productSizeID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/ChoseFile.aspx',
        data: {
            ProductID: productID,
            FileID: fileID,
            ProductSizeID: productSizeID
        },
        dataType: 'json',
        success: function (data) {
            var params = data.Params;
            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#productfilesize' + fileID).attr("onclick", "UnChoseFile('" + fileID + "','" + productID + "','" + productSizeID + "')");
            $('#productfilesize' + fileID).attr("style", "color: #22aa13; font-size: 20px;");
            $('#productfilesize' + fileID).html('<i class="fa fa-check-square-o"></i>');
        },
        error: function (status) { }
    });
}
function UnChoseFile(fileID, productID, productSizeID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UnChoseFile.aspx',
        data: {
            ProductID: productID,
            FileID: fileID,
            ProductSizeID: productSizeID
        },
        dataType: 'json',
        success: function (data) {
            var params = data.Params;
            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#productfilesize' + fileID).attr("onclick", "ChoseFile('" + fileID + "','" + productID + "','" + productSizeID + "')");
            $('#productfilesize' + fileID).attr("style", "color: #ccc; font-size: 20px;");
            $('#productfilesize' + fileID).html('<i class="fa fa fa-square-o"></i>');
        },
        error: function (status) { }
    });
}
function ChoseFileAvatar(fileID, productID, productSizeID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/ChoseFileAvatar.aspx',
        data: {
            ProductID: productID,
            FileID: fileID,
            ProductSizeID: productSizeID
        },
        dataType: 'json',
        success: function (data) {
            var params = data.Params;
            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#productfilesizeavatar' + fileID).attr("onclick", "UnChoseFileAvatar('" + fileID + "','" + productID + "','" + productSizeID + "')");
            $('#productfilesizeavatar' + fileID).attr("style", "color: #22aa13; font-size: 20px;");
            $('#productfilesizeavatar' + fileID).html('<i class="fa fa-check-square-o"></i>');
        },
        error: function (status) { }
    });
}
function UnChoseFileAvatar(fileID, productID, productSizeID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UnChoseFileAvatar.aspx',
        data: {
            ProductID: productID,
            FileID: fileID,
            ProductSizeID: productSizeID
        },
        dataType: 'json',
        success: function (data) {
            var params = data.Params;
            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#productfilesizeavatar' + fileID).attr("onclick", "ChoseFileAvatar('" + fileID + "','" + productID + "','" + productSizeID + "')");
            $('#productfilesizeavatar' + fileID).attr("style", "color: #ccc; font-size: 20px;");
            $('#productfilesizeavatar' + fileID).html('<i class="fa fa fa-square-o"></i>');
        },
        error: function (status) { }
    });
}
//file User
function ShowFileUser() {
    var finder = new CKFinder();
    finder.basePath = '../';
    finder.selectActionFunction = refreshFileUser;
    finder.popup();

    return false;
}
function refreshFileUser(arg) {
    addFileUser(arg)
}
function addFileUser(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddFileUser.aspx',
        data: {
            Name: $('#Name').val(),
            UserName: $('#UserName').val(),
            WebUserID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }

            $('#list-file').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModWebUser/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
function deleteFileUser(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DeleteFileUser.aspx',
        data: {
            WebUserID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-file').html(content);
        },
        error: function (status) { }
    });
}
function upFileUser(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpFileUser.aspx',
        data: {
            WebUserID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-file').html(content);
        },
        error: function (status) { }
    });
}
function downFileUser(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DownFileUser.aspx',
        data: {
            WebUserID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-file').html(content);
        },
        error: function (status) { }
    });
}
//get child
function get_child(ParentID, SelectedID, id) {
    $('.loading').show();

    $.ajax({
        type: 'GET',
        url: '/' + window.CPPath + '/Ajax/GetChild.aspx',
        data: 'ParentID=' + ParentID + '&SelectedID=' + SelectedID,
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            if (content != '')
                $(id).html(content);
            else
                $(id).html('');

            $('.loading').hide();
        },
        error: function () { }
    });
}
function get_child2(ParentID, SelectedID, id) {
    $('.loading').show();

    $.ajax({
        type: 'GET',
        url: '/' + window.CPPath + '/Ajax/GetChild2.aspx',
        data: 'ParentID=' + ParentID + '&SelectedID=' + SelectedID,
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            if (content != '')
                $(id).html(content);
            else
                $(id).html('');

            $('.loading').hide();
        },
        error: function () { }
    });
}
//Sản phẩm
function ShowProductSizeImageForm(ID, ProductID) {
    window.open('/' + window.CPPath + '/FormProductSizeImage/Index.aspx?ID=' + ID + '&ProductID=' + ProductID, '', 'width=1024, height=800, top=80, left=200,scrollbars=yes');
    return false;
}
function CloseProductSizeImage(arg) {
    window.close();
}
//product other
function ShowProductOtherForm(cID, sValue) {
    var RecordID = $('#RecordID').val();
    name_control = cID;
    window.open("/" + window.CPPath + "/FormProductOther/Index.aspx?Value=" + RecordID, "", "width=1024, height=800, top=80, left=200,scrollbars=yes");
    return false;
}
function CloseProductOther(arg) {
    if (window.opener)
        window.opener.refreshProductOther(arg);
    else
        window.parent.refreshProductOther(arg);
    window.close();
}
function refreshProductOther(arg) {
    addProductOther(arg);
}
function addProductOther(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddProductOther.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            ProductOtherID: productID
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-product').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
function deleteProductOther(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DeleteProductOther.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            ProductOtherID: productID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-product').html(content);
        },
        error: function (status) { }
    });
}
function upProductOther(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpProductOther.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            ProductOtherID: productID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-product').html(content);
        },
        error: function (status) { }
    });
}
function downProductOther(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DownProductOther.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            ProductOtherID: productID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-product').html(content);
        },
        error: function (status) { }
    });
}
function CloseProductOtherMulti() {
    var allVals = [];
    $('input[name="cid"]:checked').each(function () {
        allVals.push($(this).val());
    });

    if (window.opener)
        window.opener.refreshProductOtherMulti(allVals);
    else
        window.parent.refreshProductOtherMulti(allVals);

    window.close();
}
function refreshProductOtherMulti(allVals) {
    addProductOtherMulti(allVals);
}
function addProductOtherMulti(allVals) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddProductOtherMulti.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            ListProductOtherID: allVals
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-product').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
//product video
function ShowProductVideoForm(cID, sValue) {
    var RecordID = $('#RecordID').val();
    name_control = cID;
    window.open("/" + window.CPPath + "/FormProductVideo/Index.aspx?Value=" + RecordID, "", "width=1024, height=800, top=80, left=200,scrollbars=yes");
    return false;
}
function CloseProductVideo(arg) {
    if (window.opener)
        window.opener.refreshProductVideo(arg);
    else
        window.parent.refreshProductVideo(arg);
    window.close();
}
function refreshProductVideo(arg) {
    addProductVideo(arg);
}
function addProductVideo(videoID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddProductVideo.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            VideoID: videoID
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-video').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
function deleteProductVideo(videoID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DeleteProductVideo.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            VideoID: videoID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-video').html(content);
        },
        error: function (status) { }
    });
}
function upProductVideo(videoID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpProductVideo.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            VideoID: videoID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-video').html(content);
        },
        error: function (status) { }
    });
}
function downProductVideo(videoID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DownProductVideo.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            VideoID: videoID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-video').html(content);
        },
        error: function (status) { }
    });
}
function CloseProductVideoMulti() {
    var allVals = [];
    $('input[name="cid"]:checked').each(function () {
        allVals.push($(this).val());
    });

    if (window.opener)
        window.opener.refreshProductVideoMulti(allVals);
    else
        window.parent.refreshProductVideoMulti(allVals);

    window.close();
}
function refreshProductVideoMulti(allVals) {
    addProductVideoMulti(allVals);
}
function addProductVideoMulti(allVals) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddProductVideoMulti.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            ListVideoID: allVals
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-video').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
//product FAQ
function ShowProductFAQForm(cID, sValue) {
    var RecordID = $('#RecordID').val();
    name_control = cID;
    window.open("/" + window.CPPath + "/FormProductFAQ/Index.aspx?Value=" + RecordID, "", "width=1024, height=800, top=80, left=200,scrollbars=yes");
    return false;
}
function CloseProductFAQ(arg) {
    if (window.opener)
        window.opener.refreshProductFAQ(arg);
    else
        window.parent.refreshProductFAQ(arg);
    window.close();
}
function refreshProductFAQ(arg) {
    addProductFAQ(arg);
}
function addProductFAQ(FaqID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddProductFAQ.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            FAQID: FaqID
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-faq').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
function deleteProductFAQ(FaqID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DeleteProductFAQ.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            FAQID: FaqID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-faq').html(content);
        },
        error: function (status) { }
    });
}
function upProductFAQ(FaqID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpProductFAQ.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            FAQID: FaqID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-faq').html(content);
        },
        error: function (status) { }
    });
}
function downProductFAQ(FaqID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DownProductFAQ.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            FAQID: FaqID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-faq').html(content);
        },
        error: function (status) { }
    });
}
function CloseProductFAQMulti() {
    var allVals = [];
    $('input[name="cid"]:checked').each(function () {
        allVals.push($(this).val());
    });

    if (window.opener)
        window.opener.refreshProductFAQMulti(allVals);
    else
        window.parent.refreshProductFAQMulti(allVals);

    window.close();
}
function refreshProductFAQMulti(allVals) {
    addProductFAQMulti(allVals);
}
function addProductFAQMulti(allVals) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddProductFAQMulti.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            ListFAQID: allVals
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-faq').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
//add mẫu
function add_mau(filename) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/LoadTemp.aspx',
        data: {
            File: filename,
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;
            //console.log(content);
            if (content != '') {
                CKEDITOR.instances['Content'].insertHtml(content);
            }
        },
        error: function (status) { }
    });
}
function add_mauthongso() {
    var content = '<div id="thongso">';
    content += '<table>';
    content += '<tr>';
    content += '<td>Type:</td>';
    content += '<td>Digital Mixer</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>Channels:</td>';
    content += '<td>48 Stereo (40 x channel, 8 x aux)</td>';
    content += '</tr>';
    content += '</table>';
    content += '</div>';
    //content += '<ul class="table">';
    //content += '<li class="table__row">';
    //content += '<strong class="table__header">Type:</strong><span class="table__cell">Digital Mixer </span>';
    //content += '</li>';
    //content += '<li class="table__row"><strong class="table__header">Channels:';
    //content += '</strong><span class="table__cell">48 Stereo (40 x channel, 8 x aux)                    </span>';
    //content += '</li>';
    //content += '</ul>';
    CKEDITOR.instances['Specifications'].insertHtml(content);
}
function add_maupromotion() {
    var content = '<div id="khuyenmai">';
    content += '<strong>Khuyến mãi trị giá <span><span class="time-end">Giá và khuyến mãi dự kiến áp dụng đến </span><span>23:00 14/03</span> </span></strong>';
    content += '<table>';
    content += '<tr>';
    content += '<td>fa-check-circle</td>';
    content += '<td>Tặng 10m dây loa trị giá 150.000đ</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-check-circle</td>';
    content += '<td>Miễn phí giao hàng trong bán kính 30km</a></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-check-circle</td>';
    content += '<td>Miễn phí set up, lắp đặt với đội ngũ kỹ thuật chuyên sâu</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-check-circle</td>';
    content += '<td>Giao hàng toàn quốc, thanh toán COD</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-check-circle</td>';
    content += '<td>Trả góp chỉ từ 1%, thủ tục đơn giản <a href="" target="_blank">(click xem chi tiết)</a></td>';
    content += '</tr>';
    content += '</table>';
    content += '</div>';

    //content += '<span class="promo"><i class="numeric"></i>Tặng 10m dây loa trị giá 150.000đ </span>';
    //content += '<span class="promo"><i class="numeric"></i>Miễn phí giao hàng trong bán kính 30km </span>';
    //content += '<span class="promo"><i class="numeric"></i>Miễn phí set up, lắp đặt với đội ngũ kỹ thuật chuyên sâu</span>';
    //content += '<span class="promo"><i class="numeric"></i>Giao hàng toàn quốc, thanh toán COD</span>';
    //content += '<span class="promo"><i class="numeric"></i>Trả góp chỉ từ 1%, thủ tục đơn giản <a href="#" target="_blank">(click xem chi tiết)</a> </span>';
    //content += '';
    CKEDITOR.instances['Promotion'].insertHtml(content);
}
function add_mauinfo() {
    var content = '<div id="info">';
    content += '<table>';
    content += '<tr>';
    content += '<td>fa-dropbox</td>';
    content += '<td>Bộ sản phẩm gồm: <a rel="nofollow" href="" target="_blank">Hộp, Sách hướng dẫn, Cáp</a></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-bell-o</td>';
    content += '<td>Bảo hành chính hãng 12 tháng <a rel="nofollow" href="" target="_blank">Tìm hiểu</a></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-refresh</td>';
    content += '<td>Hư gì đổi nấy 12 tháng <a rel="nofollow" href="" target="_blank">Tìm hiểu</a></td>';
    content += '</tr>';
    content += '</table>';
    content += '</div>';

    //content += '<ul class="listService listtt">';
    //content += '<li>';
    //content += '<div class="icon"><i class="fa fa-dropbox"></i></div>';
    //content += '<span>Bộ sản phẩm gồm: <a rel="nofollow" href="" target="_blank">Hộp, Sách hướng dẫn, Cáp</a></span>';
    //content += '</li>';
    //content += '<li>';
    //content += '<div class="icon"><i class="fa fa-bell-o"></i></div>';
    //content += '<span>Bảo hành chính hãng 12 tháng <a rel="nofollow" href="" target="_blank">Tìm hiểu</a></span>';
    //content += '</li>';
    //content += '<li>';
    //content += '<div class="icon"><i class="fa fa-refresh"></i></div>';
    //content += '<span>Hư gì đổi nấy 12 tháng <a rel="nofollow" href="" target="_blank">Tìm hiểu</a></span>';
    //content += '</li>';
    //content += '</ul>';
    CKEDITOR.instances['Info'].insertHtml(content);
}
function add_maupromotion2() {
    var content = '<div id="uudaithem">';
    content += '<strong>Ưu đãi thêm <span><span class="time-end">Dự kiến áp dụng dự kiến đến</span> <span>31/2/2021</span> </span></strong>';
    content += '<table>';
    content += '<tr>';
    content += '<td>fa-check-circle</td>';
    content += '<td>Tặng cho khách lần đầu mua hàng online tại web obibi.vn';
    content += '<ul>';
    content += '<li><b>100.000đ</b> để mua đơn hàng obibi.vn từ <b>300.000đ</b></li>';
    content += '<li>5 lần <b>FREEship</b></li>';
    content += '<ul>';
    content += '</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-check-circle</td>';
    content += '<td>Áp dụng tại Tp.HCM và 1 số khu vực, <b>1 SĐT nhận 1 lần</b></td>';
    content += '</tr>';
    content += '</table>';
    content += '</div>';
    //content += '<span class="promo">';
    //content += '<i class="numeric"></i>Tặng cho khách lần đầu mua hàng online tại web obibi.vn';
    //content += '<div class="content">';
    //content += '<p class="first-pap"><b>100.000đ</b> để mua đơn hàng obibi.vn từ <b>300.000đ</b></p>';
    //content += '<p>5 lần <b>FREEship</b></p>';
    //content += '</div>';
    //content += '</span>';
    //content += '<span class="promo">Áp dụng tại Tp.HCM và 1 số khu vực, <b>1 SĐT nhận 1 lần</b>';
    //content += '</span>';
    //content += '<span class="promo"><i class="numeric"></i>Mua mic hát giảm đến 40% (không kèm KM khác)</span>';
    CKEDITOR.instances['Promotion2'].insertHtml(content);
}
function add_taisaochon() {
    var content = '';
    //content += '<div class="policy_avl">';
    //content += '<h4>Vì Sao Bạn Nên Chọn';
    //content += '<br>';
    content += '<div id="taisaochon">';
    content += '<strong>Vì Sao Bạn Nên Chọn</strong>';
    content += '<table>';
    content += '<tr>';
    content += '<td>fa-building</td>';
    content += '<td><span>Hệ thống <strong>10 cửa hàng</strong> lớn nhất Việt Nam</span></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-money</td>';
    content += '<td><span>Hàng <strong>chính hãng</strong>, giá Rẻ Nhất Việt Nam</span></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-handshake-o</td>';
    content += '<td><span>Nhà phân phối <strong>đại lý cấp 1</strong> được ủy quyền từ các hãng</span></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-wrench</td>';
    content += '<td><span>Dịch vụ bảo hành, bảo trì sản phẩm nhanh <strong>số 1 Việt Nam</span></td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>fa-car</td>';
    content += '<td><span>Giải pháp đồng bộ, trải nghiệm thực tế, giao hàng <strong>miễn phí 30km</strong></span></td>';
    content += '</tr>';
    content += '</table>';
    content += '</div>';
    //content += 'AVL Group ?</h4>';
    //content += '<ul class="listService">';
    //content += '<li>';
    //content += '<div class="icon"><i class="fa fa-building"></i></div>';
    //content += '<span>Hệ thống <strong>10 cửa hàng</strong> lớn nhất Việt Nam</span>';
    //content += '</li>';
    //content += '<li>';
    //content += '<div class="icon"><i class="fa fa-money"></i></div>';
    //content += '<span>Hàng <strong>chính hãng</strong>, giá Rẻ Nhất Việt Nam</span>';
    //content += '</li>';
    //content += '<li>';
    //content += '<div class="icon"><i class="fa fa-handshake-o"></i></div>';
    //content += '<span>Nhà phân phối <strong>đại lý cấp 1</strong> được ủy quyền từ các hãng</span>';
    //content += '</li>';
    //content += '<li>';
    //content += '<div class="icon"><i class="fa fa-wrench"></i></div>';
    //content += '<span>Dịch vụ bảo hành, bảo trì sản phẩm nhanh <strong>số 1 Việt Nam</strong></span>';
    //content += '</li>';
    //content += '<li>';
    //content += '<div class="icon"><i class="fa fa-car"></i></div>';
    //content += '<span>Giải pháp đồng bộ, trải nghiệm thực tế, giao hàng <strong>miễn phí 30km</strong></span>';
    //content += '</li>';
    //content += '</ul>';
    //content += '</div>';
    CKEDITOR.instances['TaiSaoChon'].insertHtml(content);
}
function add_infoadd() {
    var content = '';
    content += '<div id="infoadd">';
    content += '<table>';
    content += '<tr>';
    content += '<td>Đơn vị</td>';
    content += '<td>Chiếc</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>Bảo hành</td>';
    content += '<td>36 tháng</td>';
    content += '</tr>';
    content += '<tr>';
    content += '<td>Nguồn gốc/xuất xứ</td>';
    content += '<td></td>';
    content += '</tr>';
    content += '</table>';
    content += '</div>';
    CKEDITOR.instances['InfoAdd'].insertHtml(content);
}
//product old
function ShowProductOldForm(cID, sValue) {
    var RecordID = $('#RecordID').val();
    name_control = cID;
    window.open("/" + window.CPPath + "/FormProductOld/Index.aspx?Value=" + RecordID, "", "width=1024, height=800, top=80, left=200,scrollbars=yes");
    return false;
}
function CloseProductOld(arg) {
    if (window.opener)
        window.opener.refreshProductOld(arg);
    else
        window.parent.refreshProductOld(arg);
    window.close();
}
function refreshProductOld(arg) {
    addProductOld(arg);
}
function addProductOld(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddProductOld.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            ProductOldID: productID
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-product-old').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}
function deleteProductOld(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DeleteProductOld.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            ProductOldID: productID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-product-old').html(content);
        },
        error: function (status) { }
    });
}
function upProductOld(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpProductOld.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            ProductOldID: productID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-product-old').html(content);
        },
        error: function (status) { }
    });
}
function downProductOld(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DownProductOld.aspx',
        data: {
            ProductID: $('#RecordID').val(),
            ProductOldID: productID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-product-old').html(content);
        },
        error: function (status) { }
    });
}
function CloseProductOldMulti() {
    var allVals = [];
    $('input[name="cid"]:checked').each(function () {
        allVals.push($(this).val());
    });

    if (window.opener)
        window.opener.refreshProductOldMulti(allVals);
    else
        window.parent.refreshProductOldMulti(allVals);

    window.close();
}
function refreshProductOldMulti(allVals) {
    addProductOldMulti(allVals);
}
function addProductOldMulti(allVals) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/AddProductOldMulti.aspx',
        data: {
            Name: $('#Name').val(),
            MenuID: $('#MenuID').val(),
            ProductID: $('#RecordID').val(),
            ListProductOldID: allVals
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#list-product-old').html(content);
            if (js != '') {
                $('#RecordID').val(js)
                window.history.pushState('', '', '/' + window.CPPath + '/ModProduct/Add.aspx/RecordID/' + js);
                return;
            }
        },
        error: function (status) { }
    });
}

function loadDataFromExcel(p_this) {
    if (p_this.files) {
        $('#loading').show();

        var dataToPost = new FormData();
        dataToPost.append('File', p_this.files[0]);
        $.ajax({
            url: '/' + window.CPPath + '/Ajax/LoadThongSo.aspx',
            data: dataToPost,
            type: "POST",
            processData: false,
            contentType: false,
            success: function (data) {
                var js = data.Js;
                var params = data.Params;
                var content = data.Html;
                if (params.trim() != '') {
                    Alert('Thông báo !', params.trim());
                    return;
                }
                CKEDITOR.instances['Specifications'].insertHtml(content);
            },
            error: function (xhr, status, error) {
            }
        });
    }
    return;
}
function loadSchema(loaiData) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/LoadSchema.aspx',
        data: {
            LoaiData: loaiData,
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;
            //console.log(content);
            $('#SchemaJson').val(content);
            addValuSchema();
        },
        error: function (status) { }
    });
}
function addValuSchema() {
    try {
        var url = 'https://obibi.vn';
        var Name = $('#Name').val();
        var AliasName = $('#AliasName').val();
        var Code = $('#Code').val();
        var File = $('#File').val();
        var PageTitle = $('#PageTitle').val();
        var PageDescription = $('#PageDescription').val();
        var PageKeywords = $('#PageKeywords').val();
        var SchemaJson = $('#SchemaJson').val();
        if (SchemaJson != '') {
            var json = JSON.parse(SchemaJson);
            if (Code != '') {
                json.url = url + '/' + Code;
            } else if (Name != '') {
                Code = removeVietnameseTones(Name);
                Code = Code.trim().replace(" ", "-")
                    .replace("'", "")
                    .replace("/", "-")
                    .replace("*", "-")
                    .replace("\\", "-")
                    .replace("--", "-")
                    .replace("--", "-")
                    .toLowerCase();
                json.url = url + '/' + Code;
            }
            if (File != '') {
                json.image = url + File.replace('~/', '/');
            }
            json.description = PageDescription;
            json.name = (PageTitle != '' ? PageTitle : Name);
            json.legalName = (PageTitle != '' ? PageTitle : Name);
            //console.log(json);
            var content = JSON.stringify(json, null, 2);
            //console.log(myJSON);
            $('#SchemaJson').val(content);
        }
    }
    catch (err) {
    }
}
function removeVietnameseTones(str) {
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
    str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
    str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
    str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
    str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
    str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
    str = str.replace(/Đ/g, "D");
    // Some system encode vietnamese combining accent as individual utf-8 characters
    // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
    str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
    // Remove extra spaces
    // Bỏ các khoảng trắng liền nhau
    str = str.replace(/ + /g, " ");
    str = str.trim();
    // Remove punctuations
    // Bỏ dấu câu, kí tự đặc biệt
    str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, " ");
    return str;
}
function deleteFileVote(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DeleteFileVote.aspx',
        data: {
            CommentID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-file').html(content);
        },
        error: function (status) { }
    });
}
function upFileVote(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpFileVote.aspx',
        data: {
            CommentID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-file').html(content);
        },
        error: function (status) { }
    });
}
function downFileVote(file) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DownFileVote.aspx',
        data: {
            CommentID: $('#RecordID').val(),
            File: file
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#list-file').html(content);
        },
        error: function (status) { }
    });
}
function updateFileVote(file, p_checkbox) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpdateFileVote.aspx',
        data: {
            CommentID: $('#RecordID').val(),
            File: file,
            Status: (p_checkbox.checked ? 1 : 0)
        },
        dataType: 'json',
        success: function (data) {
            //var content = data.Html;
            //$('#list-file').html(content);
        },
        error: function (status) { }
    });
}
function UnChoseFile(fileID, productID, productSizeID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UnChoseFile.aspx',
        data: {
            ProductID: productID,
            FileID: fileID,
            ProductSizeID: productSizeID
        },
        dataType: 'json',
        success: function (data) {
            var params = data.Params;
            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            $('#productfilesize' + fileID).attr("onclick", "ChoseFile('" + fileID + "','" + productID + "','" + productSizeID + "')");
            $('#productfilesize' + fileID).attr("style", "color: #ccc; font-size: 20px;");
            $('#productfilesize' + fileID).html('<i class="fa fa fa-square-o"></i>');
        },
        error: function (status) { }
    });
}

//product news 
function deleteProductFromNews(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DeleteProductFromNews.aspx',
        data: {
            NewsID: $('#RecordID').val(),
            ProductID: productID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#listproduct').html(content);
        },
        error: function (status) { }
    });
}
function upProductFromNews(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpProductFromNews.aspx',
        data: {
            NewsID: $('#RecordID').val(),
            ProductID: productID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#listproduct').html(content);
        },
        error: function (status) { }
    });
}
function downProductFromNews(productID) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/DownProductFromNews.aspx',
        data: {
            NewsID: $('#RecordID').val(),
            ProductID: productID
        },
        dataType: 'json',
        success: function (data) {
            var content = data.Html;

            $('#listproduct').html(content);
        },
        error: function (status) { }
    });
}

function ShowWebMenu(type, value, itemControl) {
    window.open('/' + window.CPPath + '/FormWebMenu/Index.aspx?Type=' + type + '&Selected=' + value + '&ItemControl=' + itemControl, '', 'width=1280, height=800, top=80, left=20,scrollbars=yes');
    return false;
}
function CloseWebMenu(itemControl, value, type) {
    if (window.opener) {
        window.opener.refreshWebMenu(itemControl, value, type);
    }
    else {
        window.parent.refreshWebMenu(itemControl, value, type);
    }

    window.close();
}

function refreshWebMenu(itemControl, value, type) {
    if ($("#" + itemControl + " option[value='" + value + "']").length) {
        $("#" + itemControl).val(value);
        $("#" + itemControl).select2();
        $("#" + itemControl).trigger('change');
    } else {
        $.ajax({
            type: 'post',
            url: '/' + window.CPPath + '/Ajax/LoadOptionMenu.aspx',
            data: {
                Value: value,
                Type: type
            },
            dataType: 'json',
            success: function (data) {
                var js = data.Js;
                var params = data.Params;
                var content = data.Html;

                $("#" + itemControl).html(content);
                $("#" + itemControl).val(value);
                $("#" + itemControl).select2();
                $("#" + itemControl).trigger('change');
            },
            error: function (status) { }
        });
    }
}

function ChoseProperty(menuId) {
    window.open('/' + window.CPPath + '/FormProperty/Index.aspx?MenuID=' + menuId, '', 'width=1280, height=800, top=80, left=20,scrollbars=yes');
    return false;
}

function CloseProperty(menuId, value) {
    if (window.opener) {
        window.opener.refreshProperty(menuId, value);
    }
    else {
        window.parent.refreshProperty(menuId, value);
    }

    window.close();
}

function refreshProperty(menuId, value) {
    $.ajax({
        type: 'post',
        url: '/' + window.CPPath + '/Ajax/UpdateProperty.aspx',
        data: {
            Value: value,
            MenuID: menuId
        },
        dataType: 'json',
        success: function (data) {
            var js = data.Js;
            var params = data.Params;
            var content = data.Html;

            if (params != '') {
                Alert('Thông báo !', params);
                return;
            }
            GetProperties(menuId);
        },
        error: function (status) { }
    });
}

function AddProperty(isMultiple, itemControl, value) {
    window.open('/' + window.CPPath + '/FormPropertyAdd/Index.aspx?IsMultiple=' + isMultiple + '&ItemControl=' + itemControl + '&ParentID=' + value, '', 'width=1280, height=800, top=80, left=20,scrollbars=yes');
    return false;
}

function ClosePropertyAdd(itemControl, value, isMultiple) {
    if (window.opener) {
        window.opener.refreshPropertyAdd(itemControl, value, isMultiple);
    }
    else {
        window.parent.refreshPropertyAdd(itemControl, value, isMultiple);
    }

    window.close();
}

function refreshPropertyAdd(itemControl, value, isMultiple) {
    if (isMultiple) {
        console.log(itemControl);
        console.log(value);
        console.log($("#" + itemControl + " input[type=checkbox][value='" + value + "']").length);
        if ($("#" + itemControl + " input[type=checkbox][value='" + value + "']").length) {
            $("#" + itemControl + " input[type=checkbox][value='" + value + "']").each(function () {
                $(this).attr('checked', 'checked');
                $(this).prop('checked', true);
            });
        }
    } else if ($("#" + itemControl + " option[value='" + value + "']").length) {
        $("#" + itemControl).val(value);
        $("#" + itemControl).select2();
        return;
    }

    //$.ajax({
    //    type: 'post',
    //    url: '/' + window.CPPath + '/Ajax/LoadOptionMenu.aspx',
    //    data: {
    //        Value: value,
    //        Type: type
    //    },
    //    dataType: 'json',
    //    success: function (data) {
    //        var js = data.Js;
    //        var params = data.Params;
    //        var content = data.Html;

    //        $("#" + itemControl).html(content);
    //        $("#" + itemControl).val(value);
    //        $("#" + itemControl).select2();
    //        $("#" + itemControl).trigger('change');
    //    },
    //    error: function (status) { }
    //});
}
