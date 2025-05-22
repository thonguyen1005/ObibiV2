
function vsw_exec_cmd(cmdName) {
    cmdName = cmdName.replace('-', '');

    if (cmdName) {
        var cmdParam = '';
        var listCid;
        var i;
        if (cmdName === "copy") {
            listCid = document.getElementsByName('cid');
            for (i = 0; i < listCid.length; i++) {
                if (listCid[i].checked) {
                    cmdParam = listCid[i].value;
                    break;
                }
            }
        }
        else if (cmdName === "publish" || cmdName === "unpublish" || cmdName === "publishp" || cmdName === "unpublishp" || cmdName === "publishh" || cmdName === "unpublishh" || cmdName === 'delete') {
            listCid = document.getElementsByName('cid');
            for (i = 0; i < listCid.length; i++) {
                if (listCid[i].checked) {
                    cmdParam += (cmdParam === '' ? '' : ',') + listCid[i].value;
                }
            }
        }
        else if (cmdName === "edit") {
            listCid = document.getElementsByName('cid');
            for (i = 0; i < listCid.length; i++) {
                if (listCid[i].checked) {
                    cmdParam = listCid[i].value;
                    break;
                }
            }
            VSWRedirect('Add', cmdParam, 'RecordID');
            return;
        }
        else if (cmdName === "saveorder") {
            listCid = document.getElementsByName('cid');
            for (i = 0; i < listCid.length; i++) {
                cmdParam += (cmdParam === '' ? '' : ',') + listCid[i].value;
                var order = document.getElementById('order[' + listCid[i].value + ']');
                cmdParam += (cmdParam === '' ? '' : ',') + order.value;
            }
        }

        if (cmdParam !== '')
            cmdName = '[' + cmdName + '][' + cmdParam + ']';

        document.getElementById('_vsw_action').value = cmdName;
    }

    if (typeof document.vswForm.onsubmit == "function") {
        document.vswForm.onsubmit();
    }

    document.vswForm.submit();
}

function isChecked(isitchecked) {
    if (isitchecked === true) {
        document.vswForm.boxchecked.value++;
    }
    else {
        document.vswForm.boxchecked.value--;
    }

    if ($('#boxActionHide').length) {
        if ($('#countChose').length) $('#countChose').html(parseFloat(document.vswForm.boxchecked.value));
        if (parseFloat(document.vswForm.boxchecked.value) > 0) {
            $('#boxActionHide').show();
        } else {
            $('#boxActionHide').hide();
        }
    }
}

function checkAll(n, fldName) {
    if (!fldName) {
        fldName = 'cb';
    }

    var f = document.vswForm;
    var c = f.toggle.checked;
    var n2 = 0;

    for (var i = 0; i < n; i++) {
        var cb = eval('f.' + fldName + '' + i);
        if (cb) {
            cb.checked = c;
            n2++;
        }
    }

    if (c) {
        document.vswForm.boxchecked.value = n2;
    } else {
        document.vswForm.boxchecked.value = 0;
    }

    if ($('#boxActionHide').length) {
        if ($('#countChose').length) $('#countChose').html(parseFloat(document.vswForm.boxchecked.value));
        if (parseFloat(document.vswForm.boxchecked.value) > 0) {
            $('#boxActionHide').show();
        } else {
            $('#boxActionHide').hide();
        }
    }
}

function gmobj(o) {
    if (document.getElementById) { m = document.getElementById(o); }
    else if (document.all) { m = document.all[o]; }
    else if (document.layers) { m = document[o]; }
    return m;
}

function getNodeValue(o) {
    try {
        return o.item(0).firstChild.nodeValue;
    }
    catch (err) {
        return '';
    }
}

function VSWCheckDefaultValue(value, name) {
    if (typeof (window.VSWArrDefault) != 'undefined') {
        for (var i = 0; i < window.VSWArrDefault.length; i++) {
            if (i === window.VSWArrDefault.length - 1) break;

            if (window.VSWArrDefault[i] == value && window.VSWArrDefault[i + 1] === name)
                return true;

            i++;
        }
    }

    return false;
}

function VSWRedirect(control, value, name) {
    var sUrl = '';

    if (value && value !== '' && value !== '0')
        sUrl += '/' + (name ? name : 'RecordID') + '/' + value;

    var i;
    var obj;
    var objValue;

    if (typeof (window.VSWArrVar) != 'undefined') {
        for (i = 0; i < window.VSWArrVar.length; i++) {
            if (i === window.VSWArrVar.length - 1) break;
            obj = document.getElementById(window.VSWArrVar[i]);
            if (obj != null) {
                objValue = obj.value;
                if (objValue !== '' && objValue !== '0') {
                    if (!VSWCheckDefaultValue(objValue, window.VSWArrVar[i + 1]))
                        sUrl += '/' + window.VSWArrVar[i + 1] + '/' + objValue;
                }
            }

            i++;
        }
    }

    if (typeof (window.VSWArrQT) != 'undefined') {
        for (i = 0; i < window.VSWArrQT.length; i++) {
            if (i === window.VSWArrQT.length - 1) break;

            if (name && name === window.VSWArrQT[i + 1]) {
                i++;
                continue;
            }

            if ((control ? control : 'Index') === 'Index' && 'PageIndex' === window.VSWArrQT[i + 1]) {
                i++;
                continue;
            }

            if (window.VSWArrQT[i] !== '' && window.VSWArrQT[i] !== '0')
                if (!VSWCheckDefaultValue(window.VSWArrQT[i], window.VSWArrQT[i + 1]))
                    sUrl += '/' + window.VSWArrQT[i + 1] + '/' + window.VSWArrQT[i];

            i++;
        }
    }

    var url;
    if (typeof (window.VSWArrVar_QS) != 'undefined') {
        url = '';
        for (i = 0; i < window.VSWArrVar_QS.length; i++) {
            if (i === window.VSWArrVar_QS.length - 1) break;
            obj = document.getElementById(window.VSWArrVar_QS[i]);
            if (obj != null) {
                objValue = obj.value;
                if (objValue !== '' && objValue !== '0') {
                    if (!VSWCheckDefaultValue(objValue, window.VSWArrVar_QS[i + 1]))
                        url += (url === '' ? '' : '&') + window.VSWArrVar_QS[i + 1] + '=' + objValue;
                }
            }

            i++;
        }
        if (url !== '')
            sUrl = sUrl + '?' + url;
    }

    if (typeof (window.VSWArrQT_QS) != 'undefined') {
        url = '';
        for (i = 0; i < window.VSWArrQT_QS.length; i++) {
            if (i === window.VSWArrQT_QS.length - 1) break;

            if (window.VSWArrQT_QS[i] !== '' && window.VSWArrQT_QS[i] !== '0')
                if (!VSWCheckDefaultValue(window.VSWArrQT_QS[i], window.VSWArrQT_QS[i + 1]))
                    url += (url === '' ? '' : '&') + window.VSWArrQT_QS[i + 1] + '=' + window.VSWArrQT_QS[i];

            i++;
        }
        if (url !== '')
            sUrl = sUrl + '?' + url;
    }

    if (control)
        sUrl = control + '.aspx' + sUrl;
    else
        sUrl = 'Index.aspx' + sUrl;

    window.location.href = '/' + window.CPPath + '/' + window.VSWController + '/' + sUrl;
}

function trim(str, chars) {
    return ltrim(rtrim(str, chars), chars);
}

function ltrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}

function rtrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}

function GetIndex(custom, key, index) {
    var i = custom.indexOf(key + '=', index);
    if (i > -1) {
        var k = custom.indexOf('\n', i - 1);
        if (k === -1 || k === i - 1 || k === custom.length - 1) {
            return i;
        }
        else {
            if (k < i) {
                var s = custom.substr(k, i - k);
                s = trim(s, '');

                if (s === '')
                    return i;
                else
                    return GetIndex(custom, key, i + key.length + 1);
            }

            return i;
        }
    }

    return -1;
}

function getvalue(custom, key, value) {
    return getvalue(custom, key, value, 0);
}

function getvalue(custom, key, value, index) {
    var i = GetIndex(custom, key, 0);
    if (i > -1) {
        var j = custom.indexOf('\n', i);
        if (j === -1) j = custom.length;

        var oldvalue = custom.substr(i, j - i);

        custom = custom.replace(oldvalue, key + '=' + value);
    }
    else {
        if (custom === '') custom = key + '=' + value;
        else custom += '\n' + key + '=' + value;
    }

    return custom;
}

function GetCustom(key) {
    var txtCustom = document.getElementById("Custom");
    var txtSetCustom = document.getElementById("set_custom");

    var custom = txtCustom.value;
    txtSetCustom.value = '';

    var i = GetIndex(custom, key, 0);
    if (i > -1) {
        var j = custom.indexOf('\n', i);

        if (j === -1)
            j = custom.length;

        var value = custom.substr(i + key.length + 1, j - i - key.length - 1);

        txtSetCustom.value = value;
    }
}

function SetCustom() {
    var key = '';
    for (var i = 0; i < document.getElementsByName("rSetCustom").length; i++) {
        if (document.getElementsByName("rSetCustom").item(i).checked) {
            key = document.getElementsByName("rSetCustom").item(i).value;
            break;
        }
    }

    var txtCustom = document.getElementById("Custom");
    var txtSetCustom = document.getElementById("set_custom");
    var sCode = '';

    if (txtCustom.value !== '')
        sCode = txtCustom.value;

    sCode = getvalue(sCode, key, txtSetCustom.value);

    txtCustom.value = sCode;
}


//update custom - page
function UpdateCustom(cID, sType) {
    var key = cID.toString().replace("_", ".") + '';
    var value = document.getElementById(cID).value + '';

    var txtCustom = document.getElementById("Custom");
    var sCode = '';

    if (txtCustom.value !== '')
        sCode = txtCustom.value;

    sCode = getvalue(sCode, key, value);

    txtCustom.value = sCode;
}


function vsw_checkAll(form, field, value) {
    for (var i = 0; i < form.elements.length; i++) {
        if (form.elements[i].name === field) {
            form.elements[i].checked = value;
            if (form.elements[i].disabled)
                form.elements[i].checked = false;
        }
    }
}

function ShowNewsForm(cID, sValue) {
    name_control = cID;
    window.open("/" + window.CPPath + "/FormNews/Index.aspx?Value=" + sValue, "", "width=1024, height=800, top=80, left=200,scrollbars=yes");
    return false;
}

function ShowTextForm(cID, sValue) {
    name_control = cID;
    window.open("/" + window.CPPath + "/FormText/Index.aspx?TextID=" + cID, "", "width=1024, height=800, top=80, left=200,scrollbars=yes");
    return false;
}

function ShowFileForm(cID, sValue) {
    name_control = cID;

    var finder = new CKFinder();
    finder.basePath = '../';
    finder.selectActionFunction = refreshPage;
    finder.popup();

    return false;
}
function ShowIconForm(cID, sValue) {
    name_control = cID;

    var finder = new CKFinder();
    finder.basePath = '../';
    finder.selectActionFunction = refreshIcon;
    finder.popup();

    return false;
}
function ShowLandingForm(cID, sValue) {
    name_control = cID;

    var finder = new CKFinder();
    finder.basePath = '../';
    finder.selectActionFunction = refreshLanding;
    finder.popup();

    return false;
}
var name_control = '';
function refreshPage(arg) {
    var obj = document.getElementById(name_control);
    if (name_control.indexOf('File') > -1 || name_control.indexOf('Img') > -1 || name_control.indexOf('Logo') > -1)
        obj.value = '~' + arg;
    else
        obj.value = arg;

    //arg = '~' + arg;
    $('#' + name_control).val(arg);

    var info = $('#' + name_control).parent().parent();

    if (info.length) {
        info.find('img').attr('src', arg);
    }
}
function refreshIcon(arg) {
    var obj = document.getElementById(name_control);
    if (name_control.indexOf('Icon') > -1 || name_control.indexOf('Img') > -1 || name_control.indexOf('Logo') > -1)
        obj.value = '~' + arg;
    else
        obj.value = arg;

    //arg = '~' + arg;
    $('#' + name_control).val(arg);

    var info = $('#' + name_control).parent().parent();

    if (info.length) {
        info.find('img').attr('src', arg);
    }
}
function layout_change(pid, listParam, layout) {
    if (listParam === '') return;
    var listLayout = listParam.split(',')
    for (var i = 0; i < listLayout.length; i++) {
        var ib = listLayout[i].indexOf('[');
        //var ie = listLayout[i].indexOf(']');
        var layoutValue = listLayout[i].substring(0, ib);
        var listControlParam = listLayout[i].substring(ib + 1, listLayout[i].length - 1);

        if (layoutValue === 'Default' || layoutValue === layout)
            control_change(pid, listControlParam);
    }
}
function control_change(pid, listParam) {
    var listControl = listParam.split('|');
    for (var i = 0; i < listControl.length; i++) {
        var control = listControl[i].split('-')[0];
        var visible = listControl[i].split('-')[1];
        //document.getElementById(pid + '_' + control).disabled = (visible == 'false');
        document.getElementById('tr_' + pid + '_' + control).style.display = (visible === 'false' ? 'none' : '');
    }
}

function control_set_value(id, value) {
    var obj = document.getElementById(id);
    if (obj) {
        obj.value = value;
    } else {
        if (value === 'True') value = 1;
        if (value === 'False') value = 0;
        var arr = document.getElementsByName(id);
        if (arr != null) {
            for (var j = 0; j < arr.length; j++) {
                if (arr[j].value === value) {
                    arr[j].checked = true;
                    break;
                }
            }
        }
    }
}

function Close(arg) {
    if (window.opener)
        window.opener.refreshPage(arg);
    else
        window.parent.refreshPage(arg);

    window.close();
}

function Cancel() {
    window.close();
}

Array.prototype.swap = function (a, b) {
    var temp = this[a];
    this[a] = this[b];
    this[b] = temp;
};

this.imagePreview = function () {
    /* CONFIG */

    xOffset = 10;
    yOffset = 30;

    // these 2 variable determine popup's distance from the cursor
    // you might want to adjust to get the right result

    /* END CONFIG */
    $('a.preview').hover(function (e) {
        this.t = this.title;
        this.title = '';
        var c = (this.t != '') ? '<br/>' + this.t : '';
        $('body').append('<p id="preview"><img src="' + $(this).data('src') + '" width="350" alt="' + this.title + '" />' + c + '</p>');
        $('#preview')
            .css('top', (e.pageY - xOffset) + 'px')
            .css('left', (e.pageX + yOffset) + 'px')
            .fadeIn('fast');
    },
        function () {
            this.title = this.t;
            $('#preview').remove();
        });
    $('a.preview').mousemove(function (e) {
        $('#preview')
            .css('top', (e.pageY - xOffset) + 'px')
            .css('left', (e.pageX + yOffset) + 'px');
    });
};

function formatDollar(value) {
    return value.split("").reverse().reduce(function (acc, value, i, orig) {
        return value + (i && !(i % 3) ? "." : "") + acc;
    }, "");
}

function copyToClipboard(e) {
    var $temp = $('<textarea>');
    $('body').append($temp);
    $temp.val($(e).text()).select();
    document.execCommand('copy');
    $temp.remove();

    zebra_alert('Thông báo !', 'Đã copy thành công');
}

function CKEditorInstance() {
    if ($('#TopContent').length) {
        var ckEditor = CKEDITOR.instances["TopContent"];
        if (ckEditor) { ckEditor.destroy(true); }
        CKEDITOR.replace('TopContent', {
            toolbar: 'Basic'
        });
    }
    if ($('#Content').length) {
        var ckEditor = CKEDITOR.instances["Content"];
        if (ckEditor) { ckEditor.destroy(true); }
        var editor = CKEDITOR.replace('Content', {
            toolbar: 'Basic'
        });
        if ($('#Content').attr('data-formatcheck') != undefined) {
            editor.on('key', function (event) {
                if (event.data.keyCode === 13) {
                    // Lấy con trỏ hiện tại trong CKEditor
                    var editor = CKEDITOR.instances['Content'];
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
    if ($('#Specifications').length) {
        var ckEditor = CKEDITOR.instances["Specifications"];
        if (ckEditor) { ckEditor.destroy(true); }
        CKEDITOR.replace('Specifications', {
            toolbar: 'Basic'
        });
    }
}

//$('a[data-toggle="tab"]').click(function (e) {
//    e.preventDefault();
//    $(this).tab('show');
//});

//$('a[data-toggle="tab"]').on("shown.bs.tab", function (e) {
//    var id = $(e.target).attr("href");
//    localStorage.setItem('selectedTab', id)
//});

//var selectedTab = localStorage.getItem('selectedTab');
//if (selectedTab != null) {
//    $('a[data-toggle="tab"][href="' + selectedTab + '"]').tab('show');
//}

$(function () {
    $('.price').on('keyup', function (e) {
        $(this).parent().find('span').html(formatDollar($(this).val()));
    });

    $('.price').val()

    CKFinder.setupCKEditor(null, { basePath: "/CP/Content/ckfinder/", rememberLastFolder: true });
    CKEditorInstance();

    /*$('.box-content').height($('.box-logs').height());*/

    $html = $('.nav-desktop').html();

    $('.nav-mobie').html($html);


    var overlay = $('.sidebar-overlay');
    $('.sidebar-toggle-btn').on('click', function () {
        var sidebar = $('#sidebar');
        sidebar.toggleClass('open');
        overlay.addClass('active');
    });
    overlay.on('click', function () {
        $(this).removeClass('active');
        $('#sidebar').removeClass('open');
    });

    $('.nav-mobie li .a-open-down').on('click', function () {
        $(this).removeAttr('href');
        var element = $(this).parent('li');
        if (element.hasClass('open')) {
            element.removeClass('open');
            element.find('li').removeClass('open');
            element.find('ul').slideUp();
        } else {
            element.addClass('open');
            element.children('ul').slideDown();
            element.siblings('li').children('ul').slideUp();
            element.siblings('li').removeClass('open');
            element.siblings('li').find('li').removeClass('open');
            element.siblings('li').find('ul').slideUp();
        }
    });

    $('[data-toggle="tooltip"]').tooltip();

    $(".back-to-top a").click(function (n) {
        n.preventDefault();
        $("html, body").animate({
            scrollTop: 0
        }, 500)
    });
    $(window).scroll(function () {
        $(document).scrollTop() > 1e3 ? $(".back-to-top").addClass("display") : $(".back-to-top").removeClass("display")
    });

    imagePreview();

    $('textarea.description').keyup(function () {
        var max = 400;
        if ($(this).attr('maxlength') != '') {
            max = parseFloat($(this).attr('maxlength'));
        }
        var len = $(this).val().length;
        if (len >= max) {
            $(this).parent().find('.help-block').text('Ký tự tối đa: 0');
            $(this).val($(this).val().substring(0, 399));
        } else {
            var char = max - len;
            $(this).parent().find('.help-block').text('Ký tự tối đa: ' + char);
        }
    });
    $('input.title').keyup(function () {
        var max = 200;
        if ($(this).attr('maxlength') != '') {
            max = parseFloat($(this).attr('maxlength'));
        }
        var len = $(this).val().length;
        if (len >= max) {
            $(this).parent().find('.help-block').text('Ký tự tối đa: 0');
            $(this).val($(this).val().substring(0, 199));
        } else {
            var char = max - len;
            $(this).parent().find('.help-block').text('Ký tự tối đa: ' + char);
        }
    });

    $('textarea.description').each(function () {
        var max = 400;
        if ($(this).attr('maxlength') != '') {
            max = parseFloat($(this).attr('maxlength'));
        }
        var len = $(this).val().length;
        if (len >= max) {
            $(this).parent().find('.help-block').text('Ký tự tối đa: 0');
            $(this).val($(this).val().substring(0, 399));
        } else {
            var char = max - len;
            $(this).parent().find('.help-block').text('Ký tự tối đa: ' + char);
        }
    });

    $('input.title').each(function () {
        var max = 200;
        if ($(this).attr('maxlength') != '') {
            max = parseFloat($(this).attr('maxlength'));
        }
        var len = $(this).val().length;
        if (len >= max) {
            $(this).parent().find('.help-block').text('Ký tự tối đa: 0');
            $(this).val($(this).val().substring(0, 199));
        } else {
            var char = max - len;
            $(this).parent().find('.help-block').text('Ký tự tối đa: ' + char);
        }
    });

    $('.nav-tabs li').click(function (e) {
        $(this).parent().closest('.nav-tabs').find('li').removeClass('active');
        $(this).addClass('active');
        $(this).parent().parent().find('.tab-content .tab-pane').removeClass('active');
        $($(this).data('href')).addClass('active');
    });
});

$(function () {
    try {
        $('.date').datepicker({
            format: 'dd-mm-yyyy',
            todayHighlight: true,
            autoclose: true,
            todayBtn: true
        });
    } catch (e) {

    }
});

//Chỉ cho phép nhập số
function InputNumberOnly(t) {
    var e = t.which ? t.which : event.keyCode;
    return e >= 48 && e <= 57 || 46 == e
}
/*
** Trả về vị trí con trỏ của trường văn bản đã chỉ định.
** Phạm vi giá trị trả về là 0-oField.value.length.
*/
function doGetCaretPosition(oField) {

    // Initialize
    var iCaretPos = 0;

    // IE Support
    if (document.selection) {

        // Set focus on the element
        oField.focus();

        // To get cursor position, get empty selection range
        var oSel = document.selection.createRange();

        // Move selection start to 0 position
        oSel.moveStart('character', -oField.value.length);

        // The caret position is selection length
        iCaretPos = oSel.text.length;
    }

    // Firefox support
    else if (oField.selectionStart || oField.selectionStart == '0')
        iCaretPos = oField.selectionStart;

    // Return results
    return iCaretPos;
}
/*
* Dat vi tri con tro chuot tai vi tri nhap lieu.
*/
function setCaretPosition(oField, caretPos) {
    if (oField != null) {
        if (oField.createTextRange) {
            var range = oField.createTextRange();
            range.move('character', caretPos);
            range.select();
        }
        else {
            if (oField.selectionStart) {
                oField.focus();
                oField.setSelectionRange(caretPos, caretPos);
            }
            else
                oField.focus();
        }
    }
}
//	Ham FormatMoney tu dong them dau "," vao text box khi nhap gia tri co kien la "Tien"
//	Khi do TextBox co dang : "123,456,789"
//	Khi goi : onkeyup="JavaScript:FormatMoney(this)"
function format_money(Obj, event) {
    _DECIMAL_DELIMITOR = ",";
    var theKey;
    //var keyCode = (document.layers) ? keyStroke.which : event.keyCode;
    if (typeof (event) == "undefined") {
        theKey = window.event.keyCode;
    }
    else {
        theKey = (window.event) ? event.keyCode : event.which;
    }
    //var theKey = event.keyCode;
    // lay vi tri con tro 
    var v_vi_tri_con_tro = doGetCaretPosition(Obj);
    var v_is_vi_tri_con_tro_o_cuoi = (v_vi_tri_con_tro == Obj.value.length) ? 1 : 0;
    //phuonghv add 20/08/2016  tim va thay the cac ky tu khong phai la kieu so ve rong.
    Obj.value = Obj.value.replace(/[^0-9,.-]/g, '');
    var theStringNum = Obj.value;
    theSecondStringNum = "";
    // Neu ki tu dau tien la "." thi bo qua
    if (theStringNum == ".") {
        Obj.value = "";
        return;
    }
    var the_first_char = theStringNum.substr(0, 1);
    if (the_first_char == "-") {
        theStringNum = theStringNum.substr(1, theStringNum.length - 1);
    } else {
        the_first_char = "";
    }
    var theLen = theStringNum.length;

    pos = theStringNum.indexOf(".", 0)
    if (pos > 0) {
        arr_numstr = theStringNum.split(".");
        theFirstStringNum = theStringNum.substr(0, pos);
        theSecondStringNum = theStringNum.substr(pos + 1, theStringNum.length - pos);
        if (theSecondStringNum.substr(theSecondStringNum.length - 1, 1) == ".") {
            Obj.value = the_first_char + theStringNum.substr(0, theStringNum.length - 1);
            return;
        }
        theStringNum = theFirstStringNum;
    }
    //Chi nhan cac ky tu la so
    if ((theKey >= 48 && theKey <= 57) || (theKey >= 96 && theKey <= 105) || (theKey == 8)) {
        var theNewString;
        var theSubString;
        var LastIndex;
        LastIndex = 0;
        theSubString = ""
        // Thay the ky tu ","
        for (var i = 0; i < theStringNum.length; i++) {
            if (theStringNum.substring(i, i + 1) == _DECIMAL_DELIMITOR)		// Tim ky tu ","
            {
                theSubString = theSubString + theStringNum.substring(LastIndex, i)
                LastIndex = i + 1;
            }
        }
        theSubString = theSubString + theStringNum.substring(LastIndex, theStringNum.length) // Lay mot doan cuoi cung (vi doan cuoi cung khong co ky tu ",")
        theStringNum = theSubString;

        theNewString = ""
        if (theStringNum.length > 3)
            while (theStringNum.length > 3) {
                theSubString = theStringNum.substring(theStringNum.length - 3, theStringNum.length);
                theStringNum = theStringNum.substring(0, theStringNum.length - 3);
                theNewString = _DECIMAL_DELIMITOR + theSubString + theNewString;
                //phuonghv add 20/08/2016 neu vi tri o cuoi thi tu dong cong them 1 vi them phan tach phan nghin boi dau ,
                if (v_is_vi_tri_con_tro_o_cuoi == 1) {
                    v_vi_tri_con_tro++;
                }

            }
        if (pos > 0)
            theNewString = theStringNum + theNewString + "." + theSecondStringNum;
        else
            theNewString = theStringNum + theNewString;

        if (theLen > 3)
            Obj.value = the_first_char + theNewString;
        try {
            Obj.onchange();
        } catch (e) { ; }
    }
    // //phuonghv add 20/08/2016 dat lai vi tri con tro chuot vao vi tri nhap lieu
    setCaretPosition(Obj, v_vi_tri_con_tro);
}
Number.prototype.f_formatMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};
function format_date(txt_obj, event) {
    var separator_char = "/";
    //Lay gia tri ma ASCII cua phim an
    var theKey;
    //var keyCode = (document.layers) ? keyStroke.which : event.keyCode;
    if (typeof (event) == "undefined") {
        theKey = window.event.keyCode;
    }
    else {
        theKey = (window.event) ? event.keyCode : event.which;
    }
    //var theKey = event.keyCode;
    var theLen = txt_obj.value.length;
    //Neu an phim BackSpace, Up, Down, Left, Right, Home, End thi khong xu ly
    if (theKey == 8 || theKey == 37 || theKey == 39 || theKey == 40) { return 1; }
    //Loai bo cac ki tu khong phai ky tu so (ke ca dau phan cach ngay thang nam)
    theStr = "";
    for (var i = 0; i < theLen; i++) {
        theChar = txt_obj.value.charCodeAt(i);
        if (theChar >= 48 & theChar <= 57) {
            theStr = theStr + txt_obj.value.substring(i, i + 1);
        }
    }
    theLen = theStr.length;
    // Xu ly tao format theo dang thoi gian dd/mm/yyyy
    if (theLen >= 4) {
        theDate = theStr.substring(0, 2);
        theMonth = theStr.substring(2, 4);
        theYear = theStr.substring(4);
        txt_obj.value = theDate + separator_char + theMonth + separator_char + theYear;
    } else {
        if (theLen >= 2) {
            theDate = theStr.substring(0, 2);
            theMonth = theStr.substring(2);
            txt_obj.value = theDate + separator_char + theMonth;
        } else {
            txt_obj.value = theStr;
        }
    }
    return 1;
}

function AddControlInput(tblname, name) {
    var s = '<tr>';
    s += '<td class="w90p">';
    s += '<input type="text" class="form-control" name="' + name + '" value="" />';
    s += '</td>';
    s += '<td class="text-center"><a href="javascript:;" onclick="removeRow(event)"><i class="fa fa-close"></i></a></td>';
    s += '</tr>';
    $('#' + tblname).append(s);
}

function AddControlTextarea(tblname, name) {
    let textareas = document.querySelectorAll('textarea[name="' + name + '"]');
    let id = '';
    // Kiểm tra nếu có ít nhất một phần tử
    if (textareas.length > 0) {
        // Chọn phần tử cuối cùng
        let lastTextarea = textareas[textareas.length - 1];
        let index = parseFloat(lastTextarea.id.replace(name, ''));
        id = name + (index + 1);
    } else {
        id = name + '0';
    }

    var s = '<tr>';
    s += '<td class="w90p">';
    s += '<textarea class="form-control ckeditorRow" name="' + name + '" id="' + id + '"></textarea>';
    s += '</td>';
    s += '<td class="text-center"><a href="javascript:;" onclick="removeRow(event)"><i class="fa fa-close"></i></a></td>';
    s += '</tr>';
    $('#' + tblname).append(s);

    var ckEditor = CKEDITOR.instances[id];
    if (ckEditor) { ckEditor.destroy(true); }

    CKEDITOR.replace(id, {
        // Cấu hình CKEditor nếu cần thiết
        toolbar: [
            { name: 'all', items: ['Source', '-', 'Link', 'Unlink', '-', 'Bold', 'Italic', 'Underline'] }
        ],
        removePlugins: 'elementspath,save,font,justify,image,about,stylescombo,format,colorbutton,indent,block',
        height: 50
    });
}

function removeRow(event) {
    event.preventDefault(); // Prevent the default link behavior

    // Get the closest tr element
    let row = event.target.closest('tr');

    // Kiểm tra xem phần tử <tr> có tồn tại không và sau đó xóa nó
    if (row) {
        row.remove();
    }
}

$(document).ready(function () {
    document.querySelectorAll('textarea.ckeditorRow').forEach(function (textarea) {
        CKEDITOR.replace(textarea.id, {
            // Cấu hình CKEditor nếu cần thiết
            toolbar: [
                { name: 'all', items: ['Source', '-', 'Link', 'Unlink', '-', 'Bold', 'Italic', 'Underline'] }
            ],
            removePlugins: 'elementspath,save,font,justify,image,about,stylescombo,format,colorbutton,indent,block',
            height: 50
        });
    });
});

function AddProductClassify() {
    let arrItem = document.querySelectorAll('input[name="ProductClassify"]');
    if (arrItem.length == 1) $('#btnAddProductClassify').hide();
    let index = 0;
    // Kiểm tra nếu có ít nhất một phần tử
    if (arrItem.length > 0) {
        // Chọn phần tử cuối cùng
        let last = arrItem[arrItem.length - 1];
        index = parseFloat(last.id.replace('ProductClassify', '')) + 1;
    }
    let id = 'ProductClassify' + index;

    var s = '<div class="box" id="box_' + id + '">';
    s += '      <div class="box-header box-header-edit">'
    s += '          <div class="col-md-6">';
    s += "              <input type=\"text\" class=\"form-control\" onchange=\"ProductClassifyOnchange(this.value, '" + id + "')\" name=\"ProductClassify\" id=\"" + id + "\" value=\"\" maxlength=\"20\" />";
    s += '          </div>';
    s += '          <div class="col-md-6 text-right">';
    s += "              <a href=\"javascript:AddProductClassifyValue('" + id + "'," + index + ")\" class=\"mr-1\"><i class=\"fa fa-plus\"></i></a>";
    s += "              <a href=\"javascript:DelProductClassify('" + id + "')\"><i class=\"fa fa-close\"></i></a>";
    s += '          </div>';
    s += '      </div>';
    s += '      <div class="box-header box-header-view" style="display:none;">';
    s += '          <div class="col-md-6">';
    s += '              <span class="title" id="title_' + id + '"></span>';
    s += '           </div>';
    s += '           <div class="col-md-6 text-right">';
    s += "              <a href=\"javascript:AddProductClassifyValue('" + id + "'," + index + ")\" class=\"mr-1\"><i class=\"fa fa-plus\"></i></a>";
    s += "              <a href=\"javascript:EditProductClassify('" + id + "')\" class=\"mr-1\"><i class=\"fa fa-edit ml-1\"></i></a>";
    s += "              <a href=\"javascript:DelProductClassify('" + id + "')\"><i class=\"fa fa-trash\"></i></a>";
    s += '           </div>';
    s += '      </div>';
    s += '      <hr />';
    s += '      <div class="box-body row">';
    s += '          <div class="form-group col-md-6 item-ProductClassify">';
    s += '              <div class="input-group">';
    //s += '                  <i class="fa fa-photo"></i>';
    s += '                  <div class="img">';
    s += '                      <i class="fa fa-image" onclick="ShowFileClassify(this); return false"></i>';
    s += '                      <input type="hidden" name="ProductClassifyFile" value="">';
    s += '                  </div>';
    s += '                  <input type="hidden" name="ProductClassifyParrent" value="' + index + '" maxlength="20" />';
    s += "                  <input type=\"text\" class=\"form-control\" name=\"ProductClassifyValue\" onchange=\"ProductClassifyValueOnchange(this,'" + id + "')\" value=\"\" maxlength=\"20\" />";
    s += "                  <a href=\"javascript:;\" onclick=\"DelProductClassifyValue(this,'" + id + "');\"><i class=\"fa fa-trash\"></i></a>";
    s += '              </div>';
    s += '          </div>';
    s += '      </div>'
    s += '  </div> ';
    $('#lstPLH').append(s);
    $('.phanloai').show();
    LoadProductCaulator(true);
}

function DelProductClassify(id) {
    showSwalQuestion('Bạn có muộn xóa đối tượng này.', 'Thông báo', (flag) => {
        if (flag) {
            $('#box_' + id).remove();
            let arrItem = document.querySelectorAll('input[name="ProductClassify"]');
            if (arrItem.length < 2) $('#btnAddProductClassify').show();
            if (arrItem.length <= 0) $('.phanloai').hide();
            LoadProductCaulator(true);
        }
    });
}

function EditProductClassify(id) {
    $('#box_' + id + ' .box-header').hide();
    $('#box_' + id + ' .box-header-edit').show();
    $('#box_' + id + ' input[name="ProductClassify"]').focus();
}

function ProductClassifyOnchange(value, id) {
    $('#title_' + id).html(value);
    $('#box_' + id + ' .box-header').hide();
    $('#box_' + id + ' .box-header-view').show();
    let i = parseFloat(id.replace('ProductClassify', ''));
    $('#tblCaculator thead tr th').each(function (index, item) {
        if (i == index) {
            $(item).html(value);
        }
    });
}

function DelProductClassifyValue(p_obj, id) {
    showSwalQuestion('Bạn có muộn xóa đối tượng này.', 'Thông báo', (flag) => {
        if (flag) {
            var parent = $(p_obj).closest('.item-ProductClassify');
            let index = parent.index();
            let arrItem = document.querySelectorAll('input[name="ProductClassify"]');
            if (arrItem.length == 1) {
                $('#tblCaculator tbody tr')[index].remove();
            } else {
                let isAddBox1 = arrItem[0].id == id;
                let isAddBox2 = arrItem[1].id == id;
                if (isAddBox1) {
                    $('#tblCaculator tbody input[name="ClassifyDetailIndex1ProductClassifyDetail"][value="' + index + '"]').each(function (index, item) {
                        $(item).parent().parent().remove();
                    });
                }
                else if (isAddBox2) {
                    $('#tblCaculator tbody input[name="ClassifyDetailIndex2ProductClassifyDetail"][value="' + index + '"]').each(function (index, item) {
                        $(item).parent().parent().remove();
                    });

                    $('#tblCaculator tbody td.name').each(function (index, item) {
                        $(item).attr('rowspan', parseFloat($(item).attr('rowspan')) - 1);
                    });
                }

                parent.remove();
            }
            //LoadProductCaulator(true);
        }
    });
}

function AddProductClassifyValue(id, parentIndex) {
    let s = '';
    s += '          <div class="form-group col-md-6 item-ProductClassify">';
    s += '              <div class="input-group">';
    //s += '                  <i class="fa fa-photo"></i>';
    s += '                  <div class="img">';
    s += '                      <i class="fa fa-image" onclick="ShowFileClassify(this); return false"></i>';
    s += '                      <input type="hidden" name="ProductClassifyFile" value="">';
    s += '                  </div>';
    s += '                  <input type="hidden" name="ProductClassifyParrent" value="' + parentIndex + '" maxlength="20" />';
    s += "                  <input type=\"text\" class=\"form-control\" name=\"ProductClassifyValue\" onchange=\"ProductClassifyValueOnchange(this,'" + id + "')\" value=\"\" maxlength=\"20\" />";
    s += "                  <a href=\"javascript:;\" onclick=\"DelProductClassifyValue(this,'" + id + "');\"><i class=\"fa fa-trash\"></i></a>";
    s += '              </div>';
    s += '          </div>';
    $('#box_' + id + ' .box-body').append(s);

    let arrItem = document.querySelectorAll('input[name="ProductClassify"]');
    if (arrItem.length == 1) {
        let count = document.querySelectorAll('input[name="ProductClassifyValue"]').length;
        s = '';
        s += '<tr data-value1="" data-value2="">';
        s += '<td class="text-center name" data-value="">';
        s += '</td>';

        s += '<td class="text-center">';
        s += "<input type=\"number\" class=\"form-control\" name=\"PriceProductClassifyDetail\" value=\"\" onkeyup=\"$(this).parent().find('span').html(formatDollar($(this).val()));\" />";
        s += '<span class="help-block text-primary"></span>';
        s += '</td>';
        s += '<td class="text-center">';
        s += '<input type="number" class="form-control" name="SoLuongProductClassifyDetail" value="" />';
        s += '</td>';
        s += '<td class="text-center">';
        s += '<input type="text" class="form-control" name="SKUProductClassifyDetail" value="" />';
        s += '<input type="hidden" name="ClassifyDetailIndex1ProductClassifyDetail" value="' + i + '" />';
        s += '<input type="hidden" name="ClassifyDetailIndex2ProductClassifyDetail" value="' + (count - 1) + '" />';
        s += '</td>';
        s += '</tr>';

        $('#tblCaculator tbody').append(s);
    } else if (arrItem.length == 2) {
        let isAddBox1 = arrItem[0].id == id;
        let isAddBox2 = arrItem[1].id == id;
        if (isAddBox1) {
            let arrItemDetail2 = document.querySelectorAll('#box_' + arrItem[1].id + ' input[name="ProductClassifyValue"]');
            s = '';
            for (var j = 0; j < arrItemDetail2.length; j++) {
                s += '<tr data-value1="" data-value2="' + arrItemDetail2[j].value + '">';
                if (j == 0) {
                    s += '<td class="text-center name" rowspan="' + arrItemDetail2.length + '" data-value="">';
                    s += '</td>';
                }
                s += '<td class="text-center name2">';
                s += arrItemDetail2[j].value;
                s += '</td>';

                s += '<td class="text-center">';
                s += "<input type=\"number\" class=\"form-control\" name=\"PriceProductClassifyDetail\" value=\"\" onkeyup=\"$(this).parent().find('span').html(formatDollar($(this).val()));\" />";
                s += '<span class="help-block text-primary"></span>';
                s += '</td>';
                s += '<td class="text-center">';
                s += '<input type="number" class="form-control" name="SoLuongProductClassifyDetail" value="" />';
                s += '</td>';
                s += '<td class="text-center">';
                s += '<input type="text" class="form-control" name="SKUProductClassifyDetail" value="" />';
                s += '<input type="hidden" name="ClassifyDetailIndex1ProductClassifyDetail" value="' + (document.querySelectorAll('#box_' + arrItem[0].id + ' input[name="ProductClassifyValue"]').length - 1) + '" />';
                s += '<input type="hidden" name="ClassifyDetailIndex2ProductClassifyDetail" value="' + j + '" />';
                s += '</td>';
                s += '</tr>';
            }
            $('#tblCaculator tbody').append(s);
        } else if (isAddBox2) {
            $('#tblCaculator tbody td.name').attr('rowspan', (parseFloat($('#tblCaculator tbody td.name').attr('rowspan')) + 1));
            let arrItemDetail = document.querySelectorAll('#box_' + arrItem[0].id + ' input[name="ProductClassifyValue"]');
            let arrItemDetail2 = document.querySelectorAll('#box_' + arrItem[1].id + ' input[name="ProductClassifyValue"]');

            for (var i = 0; i < arrItemDetail.length; i++) {
                s = '';
                s += '<tr data-value1="' + arrItemDetail[i].value + '" data-value2="">';
                s += '<td class="text-center name2">';
                s += '</td>';

                s += '<td class="text-center">';
                s += "<input type=\"number\" class=\"form-control\" name=\"PriceProductClassifyDetail\" value=\"\" onkeyup=\"$(this).parent().find('span').html(formatDollar($(this).val()));\" />";
                s += '<span class="help-block text-primary"></span>';
                s += '</td>';
                s += '<td class="text-center">';
                s += '<input type="number" class="form-control" name="SoLuongProductClassifyDetail" value="" />';
                s += '</td>';
                s += '<td class="text-center">';
                s += '<input type="text" class="form-control" name="SKUProductClassifyDetail" value="" />';
                s += '<input type="hidden" name="ClassifyDetailIndex1ProductClassifyDetail" value="' + i + '" />';
                s += '<input type="hidden" name="ClassifyDetailIndex2ProductClassifyDetail" value="' + (arrItemDetail2.length - 1) + '" />';
                s += '</td>';
                s += '</tr>';

                var itemEnd = document.querySelectorAll('input[name="ClassifyDetailIndex1ProductClassifyDetail"][value="' + i + '"]');

                for (var z = 0; z < itemEnd.length; z++) {
                    var item = $(itemEnd[z]).parent().find('input[name="ClassifyDetailIndex2ProductClassifyDetail"][value="' + (arrItemDetail2.length - 2) + '"]');
                    item.parent().parent().after(s);
                }
            }
        }
    }
}

function ProductClassifyValueOnchange(p_obj, id) {
    var parent = $(p_obj).closest('.item-ProductClassify');
    let index = parent.index();
    let arrItem = document.querySelectorAll('input[name="ProductClassify"]');
    let isAddBox1 = arrItem[0].id == id;
    let isAddBox2 = arrItem[1].id == id;
    if (isAddBox1) {
        $('#tblCaculator tbody td.name').each(function (i, it) {
            if (index == i) {
                $(it).attr('data-value', $(p_obj).val());
                $(it).html($(p_obj).val());
                $(it).closest('tr').attr('data-value1', $(p_obj).val());
            }
        });
    } else {
        $('#tblCaculator tbody td.name2').each(function (i, it) {
            $(it).parent().find('[name="ClassifyDetailIndex2ProductClassifyDetail"][value="' + index + '"]').each(function () {
                $(this).parent().parent().find('.name2').html($(p_obj).val());
                $(this).parent().parent().attr('data-value2', $(p_obj).val());
            });
        });
    }
    //$('#box_' + id + ' input[name="ProductClassifyValue"]').each(function (index, item) {
    //    if (p_obj == item) {
    //        $('#tblCaculator [name="ClassifyDetailIndex1ProductClassifyDetail"][value="' + index + '"]').each(function (i, it) {

    //        });
    //    }
    //});
    //LoadProductCaulator(true);
}

function ApplyToProductClassify() {
    if ($('#PriceProductClassify').val() != '') {
        $('input[name="PriceProductClassifyDetail"]').each(function () {
            $(this).val($('#PriceProductClassify').val());
            $(this).trigger('change');
            //$('#PriceProductClassify').val('');
        });
    }
    if ($('#SoLuongProductClassify').val() != '') {
        $('input[name="SoLuongProductClassifyDetail"]').each(function () {
            $(this).val($('#SoLuongProductClassify').val());
            //$('#SoLuongProductClassify').val('');
        });
    }
    if ($('#SkuProductClassify').val() != '') {
        $('input[name="SKUProductClassifyDetail"]').each(function () {
            $(this).val($('#SkuProductClassify').val());
            //$('#SkuProductClassify').val('');
        });
    }
}

function LoadProductCaulator(isReload = false) {
    if (isReload) {
        let arrItem = document.querySelectorAll('input[name="ProductClassify"]');
        $('#tblCaculator thead tr').html('');
        for (var i = 0; i < arrItem.length; i++) {
            $('#tblCaculator thead tr').append('<th class="sorting text-center ' + (i > 0 ? "w25p" : "") + '">' + arrItem[i].value + '</th>');
        }
        $('#tblCaculator thead tr').append('<th class="sorting text-center w20p">Giá</th>');
        $('#tblCaculator thead tr').append('<th class="sorting text-center w15p">Kho hàng</th>');
        $('#tblCaculator thead tr').append('<th class="sorting text-center w15p">SKU</th>');

        let index = parseFloat(arrItem[0].id.replace('ProductClassify', ''));
        let arrItemDetail = document.querySelectorAll('#box_ProductClassify' + index + ' input[name="ProductClassifyValue"]');

        let s = '';
        for (var i = 0; i < arrItemDetail.length; i++) {
            if (arrItem.length == 2) {
                let index2 = parseFloat(arrItem[1].id.replace('ProductClassify', ''));
                let arrItemDetail2 = document.querySelectorAll('#box_ProductClassify' + index2 + ' input[name="ProductClassifyValue"]');
                for (var j = 0; j < arrItemDetail2.length; j++) {
                    s += '<tr data-value1="' + arrItemDetail[i].value + '" data-value2="' + arrItemDetail2[j].value + '">';
                    if (j == 0) {
                        s += '<td class="text-center name" rowspan="' + arrItemDetail2.length + '" data-value="' + arrItemDetail[i].value + '">';
                        s += arrItemDetail[i].value;
                        s += '</td>';
                    }
                    s += '<td class="text-center name2">';
                    s += arrItemDetail2[j].value;
                    s += '</td>';

                    s += '<td class="text-center">';
                    s += "<input type=\"number\" class=\"form-control\" name=\"PriceProductClassifyDetail\" value=\"\" onkeyup=\"$(this).parent().find('span').html(formatDollar($(this).val()));\" />";
                    s += '<span class="help-block text-primary"></span>';
                    s += '</td>';
                    s += '<td class="text-center">';
                    s += '<input type="number" class="form-control" name="SoLuongProductClassifyDetail" value="" />';
                    s += '</td>';
                    s += '<td class="text-center">';
                    s += '<input type="text" class="form-control" name="SKUProductClassifyDetail" value="" />';
                    s += '<input type="hidden" name="ClassifyDetailIndex1ProductClassifyDetail" value="' + i + '" />';
                    s += '<input type="hidden" name="ClassifyDetailIndex2ProductClassifyDetail" value="' + j + '" />';
                    s += '</td>';
                    s += '</tr>';
                }
            } else {
                s += '<tr data-value1="' + arrItemDetail[i].value + '" data-value2="">';
                s += '<td class="text-center name" data-value="' + arrItemDetail[i].value + '">';
                s += arrItemDetail[i].value;
                s += '</td>';

                s += '<td class="text-center">';
                s += "<input type=\"number\" class=\"form-control\" name=\"PriceProductClassifyDetail\" value=\"\" onkeyup=\"$(this).parent().find('span').html(formatDollar($(this).val()));\" />";
                s += '<span class="help-block text-primary"></span>';
                s += '</td>';
                s += '<td class="text-center">';
                s += '<input type="number" class="form-control" name="SoLuongProductClassifyDetail" value="" />';
                s += '</td>';
                s += '<td class="text-center">';
                s += '<input type="text" class="form-control" name="SKUProductClassifyDetail" value="" />';
                s += '<input type="hidden" name="ClassifyDetailIndex1ProductClassifyDetail" value="' + i + '" />';
                s += '<input type="hidden" name="ClassifyDetailIndex2ProductClassifyDetail" value="0" />';
                s += '</td>';
                s += '</tr>';
            }
        }
        $('#tblCaculator tbody').html(s);
    }
}


function ShowFileClassify(p_this) {
    obj_control = p_this;

    var finder = new CKFinder();
    finder.basePath = '../';
    finder.selectActionFunction = refreshPageClassify;
    finder.popup();

    return false;
}

var obj_control;
function refreshPageClassify(arg) {
    //$(obj_control).parent().find('input').attr('value', );

    $(obj_control).parent().html('<img src="' + arg + '"><input type="hidden" name="ProductClassifyFile" value="' + arg + '">');
}