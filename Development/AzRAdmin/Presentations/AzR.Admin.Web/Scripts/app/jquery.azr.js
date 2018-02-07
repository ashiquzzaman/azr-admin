/********************VARIABLE**************************/
var offset = 220;
var duration = 500;
var monthNames = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec");
var validImageExtensions = ["jpg", "jpeg", "bmp", "gif", "png"];
/***************IMAGE & FILE***********************/
function getExtention(name) {
    var extension = name.substring(name.lastIndexOf('.') + 1).toLowerCase();
    return extension;
}
function validedImageExtention(name) {
    var extension = name.substring(name.lastIndexOf('.') + 1).toLowerCase();
    return $.inArray(extension, validImageExtensions) !== -1;
}

/*************OOP JS****************************/
var Spiner = (function () {
    "use strict";
    var opts = {
        lines: 13,
        length: 28,
        width: 14,
        radius: 42,
        scale: 1,
        corners: 1,
        color: '#000',
        opacity: 0.25,
        rotate: 0,
        direction: 1,
        speed: 1,
        trail: 60,
        fps: 20,
        zIndex: 2e9,
        className: 'spinner',
        top: '50%',
        left: '50%',
        shadow: false,
        hwaccel: false,
        position: 'absolute'
    };
    var result = {};
    var spin = new Spinner(opts);
    result.show = function () {
        $('#splash-page').show();
        $('#splash-page').after(spin.spin().el);
    }
    result.hide = function () {
        setTimeout(function () {
            spin.stop();
            $('#splash-page').hide();
        }, 10);
    }
    return result;
}());
var MessageBox = (function () {
    "use strict";
    var elem,
        hideHandler,
        result = {};

    result.init = function (options) {
        elem = $(options.selector);
    };
    result.show = function (text) {
        clearTimeout(hideHandler);
        elem.find("span").html(text);
        elem.delay(200).fadeIn().delay(1000).fadeOut();
    }
    return result;
}());
var ToastBox = (function () {
    "use strict";
    var elem,
        hideHandler,
        that = {};

    that.init = function (options) {
        elem = $(options.selector);
    };
    that.show = function (text) {
        clearTimeout(hideHandler);
        elem.find("p").html(text);
        elem.delay(200).fadeIn().delay(1000).fadeOut();
    }
    return that;
}());
var Timer = (function () {
    "use strict";
    var interval = null;
    var targetId;
    var redirectUrl;
    var redirectHandle = null;
    var timeInrerval = 1;
    var sessionTime = 0;
    var redirect = function () {
        sessionTime = (timeInrerval) * 1000;
        function resetRedirect() {
            if (redirectHandle) clearTimeout(redirectHandle);
            redirectHandle = setTimeout(function () {
                window.location.href = redirectUrl;
            }, sessionTime);
        }
        $.ajaxSetup({ complete: function () { resetRedirect(); } });
        resetRedirect();
    }
    var timer = function () {
        redirect();
        interval = setInterval(function () {
            var count = parseInt($(targetId).html());
            if (count !== 0) {
                $(targetId).html(count - 1);
            } else {
                clearInterval(interval);
            }
        }, 1000);

    };
    var refresh = function () {
        $(targetId).html("0");
        clearInterval(interval);
        clearTimeout(redirectHandle);
        $(targetId).html(timeInrerval);
        timer();
    }
    var result = {};
    result.init = function (url, targetTimer) {
        redirectUrl = url;
        targetId = "#" + targetTimer;
        timeInrerval = parseInt($(targetId).html());
        timer();
    }
    result.refresh = function (inrervalTime) {
        timeInrerval = parseInt(inrervalTime);
        refresh();
    }
    return result;
}());

/*****************LOCAL STORAGE******************************/
function allStorageVlaue() {
    var values = [],
        keys = Object.keys(localStorage),
        i = keys.length;
    while (i--) {
        values.push(localStorage.getItem(keys[i]));
    }
    return values;
}
function allStorageObject() {

    var archive = {},
        keys = Object.keys(localStorage),
        i = keys.length;
    while (i--) {
        archive[keys[i]] = localStorage.getItem(keys[i]);
    }
    return archive;
}
function allStorageArray() {
    var archive = [],
        keys = Object.keys(localStorage),
        i = 0, key;
    for (; key = keys[i]; i++) {
        archive.push(key + '=' + localStorage.getItem(key));
    }
    return archive;
}

/*********************AJAX*************************************/

function OnAjaxRequestSuccess(arg, header) {
    if (typeof (arg) !== 'undefined') {
        var urlArr = arg.pathname.split('/');
        if (urlArr.length < 3) {
            $("#area-name").val("");
            $("#controller-name").val(urlArr[1]);
        } else {
            $("#area-name").val(urlArr[1]);
            $("#controller-name").val(urlArr[2]);
        }
    }
    if (typeof (header) !== 'undefined') {
        $('#PageHeader').html(header);
    } else {
        if (typeof (arg) !== 'undefined') {
            if ($(arg).children().length) {
                $('#PageHeader').html($($(arg).children().last()).html());
            } else {
                $('#PageHeader').html($(arg).html());

            }
        }

    }
    getFixedFoltingButton();
}
function OnRequestSuccess(header) {
    if (typeof (header) !== 'undefined') {
        $('#PageHeader').html(header);
    } else {
        $('#PageHeader').html($(this).html());
        
    }
}
function SubmitOnSuccess(result) {
    if (result.redirectTo) {
        closeModal();
        showMessage(result.message);
        $('#' + result.position).load(result.redirectTo);
    } else {
        $('#modelContent').html(result);
        $('#myModal').modal('show');
    }
}
function handleOnSuccess(result) {
    if (result.redirectTo) {
        showMessage(result.message);
        $('#' + result.position).load(result.redirectTo);
    } else {
        $('#mainContent').html(result);
    }
}
function redirectOnSuccess(result) {
    if (result.redirectTo) {
        closeModal();
        window.location.href =result.redirectTo;
    } else {
        $('#modelContent').html(result);
        $('#myModal').modal('show');
    }
}
function closeModal() {
    $('#myModal').modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
}
function createModal(url, page) {
    
    $.ajax({
        url: url,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#modelContent').html(data);
            $('#myModal').modal('show');
            if (typeof (page) !== 'undefined') {
                $('#page').val(page);
            } else {
                $('#page').val(1);
            }
        },
        error: function (data) {
            bootbox.alert('Error in creating records');
        }
    });
}
function deActivateModal(url, redirectTo, msg) {
    if (typeof (msg) === 'undefined') {
        msg = "Do you want to deactive this Record?";
    }
    bootbox.confirm(msg, function (result) {
        if (result) {
            $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    Spiner.show();
                },
                success: function (data) {
                    showMessage("Record DeActivated successfully!!!");
                    loadLink(redirectTo);
                    Spiner.hide();
                },
                error: function (data) {
                    MessageBox.show("Error in getting result");
                    Spiner.hide();

                }
            });
        }
    });

}
function deleteModal(url, redirectTo) {
    bootbox.confirm("Are you sure want to Delete this Record?", function (result) {
        if (result) {
            $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    Spiner.show();
                },
                success: function (data) {
                    showMessage("Record Deleted successfully!!!");
                    loadLink(redirectTo);
                    Spiner.hide();

                },
                error: function (data) {
                    MessageBox.show("Error in getting result");
                    Spiner.hide();

                }
            });
        }
    });

}
function loadLink(url, id) {
    if (typeof (id) === 'undefined') {
        $('#mainContent').load(url);
    } else {
        $('#mainContent').load(url);
    }
}
function loadMenu(url, header, id) {
    if (typeof (id) === 'undefined') {
        $('#mainContent').load(url);
    } else {
        $('#' + id).load(url);
    }
    if (typeof (header) !== 'undefined') {
        $('#PageHeader').html(header);
    }

}
function loadPartialView(link, position) {

    $('#' + position).html('');
    $.ajax({
        url: link,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        async: false,
        dataType: 'html',
        beforeSend: function () {
            Spiner.show();
        }
    }).success(function (result) {
            $('#' + position).html(result);
            Spiner.hide();
        }).error(function (xhr, status) {
            bootbox.alert(status);
            Spiner.show();

        });
}
function showMessage(msg) {
    //bootbox.alert({
    //    message: msg,
    //    callback: function () {
    //        MessageBox.show(msg);
    //    }
    //});
    ToastBox.show(msg);
}
/**************************UTILITY******************************************/
function priceValidation(e, mrpArg) {
    onlyNonNegativeNumeric(e);
    var mrpValue = $(mrpArg).val();
    if (mrpValue === '') {
        $(mrpArg).val('0.00');
    }
}
function quantityValidation(e, qyArg) {
    if ($(qyArg).val() === '') {
        $(qyArg).val('1');
    }
    onlyInteger(e);
}
function onlyNumeric(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var exclusions = [8, 46];
    if (exclusions.indexOf(key) > -1) {
        return;
    }
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}
function onlyInteger(e) {

    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        e.returnValue = false;
        if (e.preventDefault) e.preventDefault();
    };
    return true;
}
function onlyNonNegativeNumeric(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var exclusions = [8, 46];
    if (exclusions.indexOf(key) > -1) {
        return;
    }
    if (key === 45 || key === 189 || key === 109) theEvent.preventDefault();
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }

}
function onlyAlphaNumeric(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var exclusions = [8, 46];
    if (exclusions.indexOf(key) > -1) {
        return;
    }
    key = String.fromCharCode(key);
    var regex = /^[a-z0-9]+$/i;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}
function specialCharacterAlphaNumeric(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var exclusions = [8, 46];
    if (exclusions.indexOf(key) > -1) {
        return;
    }
    key = String.fromCharCode(key);
    var regex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}$/;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}
function getJsonDateToActualDate(jsonDate, serparator) {
    if (typeof (serparator) === "undefined") {
        serparator = '/';
    }
    if (!jsonDate) {
        return ''
    };
    var date = new Date(parseInt(jsonDate.substr(6)));
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var year = date.getFullYear();
    var finaldate = day + serparator + month + serparator + year;
    return finaldate;
}
function updateClock() {
    var currentTime = new Date();
    var currentHours = currentTime.getHours();
    var currentMinutes = currentTime.getMinutes();
    var currentSeconds = currentTime.getSeconds();

    // Pad the minutes and seconds with leading zeros, if required
    currentMinutes = (currentMinutes < 10 ? "0" : "") + currentMinutes;
    currentSeconds = (currentSeconds < 10 ? "0" : "") + currentSeconds;

    // Choose either "AM" or "PM" as appropriate
    var timeOfDay = (currentHours < 12) ? "AM" : "PM";

    // Convert the hours component to 12-hour format if needed
    currentHours = (currentHours > 12) ? currentHours - 12 : currentHours;

    // Convert an hours component of "0" to "12"
    currentHours = (currentHours == 0) ? 12 : currentHours;

    // Compose the string for display
    var currentTimeString = currentHours + ":" + currentMinutes + ":" + currentSeconds + " " + timeOfDay;
    $("#clock").html(currentTimeString);
}
/*********************TABLE*********************************/
function searchTable(result, tableId, trClass) {

    if (result) {
        var reg = new RegExp(result, 'i'); // case-insesitive
        $('#' + tableId + ' tbody').find('tr.' + trClass).each(function () {
            var $me = $(this);
            if ($me.children('td').text().match(reg)) {
                $me.show();
            } else {
                $me.hide();
            }
        });
        $('#' + tableId).removeHighlight();
        $('#' + tableId).highlight(result);
    } else {
        $('#' + tableId + ' tbody').find('tr').show();
        $('#' + tableId).removeHighlight();
    }
}
function addRowInFormTable(tableName, tag, index) {
    var tds = '<tr>' +
        $('#' + tableName + ' tr:last').html()
        .replace(/([A-Za-z])\w+_[0-9]/gi, tag + '_' + index)
        + '</tr>';
    var tr = tds.replace(/([A-Za-z])\w+\[[0-9]\]/gi, tag + '[' + index + ']');
    $('#' + tableName + ' tbody').append(tr);
    var ings = $('#' + tableName + ' tr:last').find('input');
    $.each(ings, function (index, item) {
        if (item.type === 'hidden') {
            item.value = '0';
        } else if (item.type === 'text') {
            item.value = '';
        }
    });
}
function addRowInFormTableWithSelect2(tableName, tag, index) {
    var lastTr = $('#' + tableName + ' tr:last');
    lastTr.find('.select2-select').select2('destroy');
    var tds = '<tr>' + lastTr.html().replace(/([A-Za-z])\w+_[0-9]/gi, tag + '_' + index) + '</tr>';
    var newTr = tds.replace(/([A-Za-z])\w+\[[0-9]\]/gi, tag + '[' + index + ']');
    $('#' + tableName + ' tbody').append(newTr);
    lastTr.find('.select2-select').select2({
        theme: "bootstrap",
        width: '100%'
    });
    $('#' + tableName + ' tr:last').find('.select2-select').select2({
        theme: "bootstrap",
        width: '100%'
    });
}
function addRowInModalFormTableWithSelect2(tableName, tag, index) {
    var lastTr = $('#' + tableName + ' tr:last');
    lastTr.find('.select2-select').select2('destroy');
    var tds = '<tr>' + lastTr.html().replace(/([A-Za-z])\w+_[0-9]/gi, tag + '_' + index) + '</tr>';
    var newTr = tds.replace(/([A-Za-z])\w+\[[0-9]\]/gi, tag + '[' + index + ']');
    $('#' + tableName + ' tbody').append(newTr);
    lastTr.find('.select2-select').select2({
        theme: "bootstrap",
        width: '100%',
        dropdownParent: $("#myModal")
    });
    $('#' + tableName + ' tr:last').find('.select2-select').select2({
        theme: "bootstrap",
        width: '100%',
        dropdownParent: $("#myModal")
    });
}
function tableLastIndex(tableId) {
    var last = $('#' + tableId + '>tbody>tr:last');
    var id = last.children().find('input,select')[0].id;
    var index = Number(id.replace(/([A-Za-z_])/gi, ''));
    return index;
}
function tableReIndixing(tableId) {
    var trs = "";
    $.each($('#' + tableId + '>tbody>tr'), function (index, last) {
        if ($(this).find('.select2-select').length > 0) {
            $(this).find('.select2-select').select2('destroy');
        }
        var prefix = (/^[^_]+(?=_)/).exec($(this).children().find('input,select')[0].id)[0];
        prefix = prefix == index ? "" : prefix;
        var tr = prefix != "" ? ('<tr>' + last.innerHTML.replace(/([A-Za-z])\w+[_]+[0-9]+/gi, prefix + '_' + index) + '</tr>')
                .replace(/([A-Za-z])\w+\[[0-9]+\]/gi, prefix + '[' + index + ']')
             : ('<tr>' + last.html().replace(/[0-9]+__/gi, index + '__') + '</tr>')
                .replace(/\[[0-9]+\]+[.]/gi, '[' + index + '].');
        trs += tr;
    });
    $('#' + tableId + ' tbody').html('');
    $('#' + tableId + ' tbody').append(trs);
    if ($('#' + tableId + ' tr').find('.select2-select').length > 0) {
        $('#' + tableId + ' tr').find('.select2-select').select2({
            theme: "bootstrap",
            width: '100%'
        });
    }
}
function exportTableToExcel(tableId) {
    var tabText = "<table border='2px'><tr>";
    var tab = document.getElementById(tableId);

    for (var j = 1 ; j < tab.rows.length ; j++) {
        tabText = tabText + tab.rows[j].innerHTML + "</tr>";
    }

    tabText = tabText + "</table>";
    tabText = tabText.replace(/<A[^>]*>|<\/A>/g, "");//remove links
    tabText = tabText.replace(/<img[^>]*>/gi, ""); // remove images
    tabText = tabText.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");
    var sa;
    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))  // If Internet Explorer
    {
        tab.append('<iframe id="txtArea" style="display:none"></iframe>');
        var txtArea = document.getElementById('txtArea');
        txtArea.document.open("txt/html", "replace");
        txtArea.document.write(tabText);
        txtArea.document.close();
        txtArea.focus();
        sa = txtArea.document.execCommand("SaveAs", true, "download.xls");
    }
    else
        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tabText));

    return (sa);
}
function sortingTable(tableId, columnIdList) {
    var table = $(tableId);
    $(columnIdList)
        .append('<i class="fa fa-fw fa-sort"></i>')
        .each(function () {
            var th = $(this),
                thIndex = th.index(),
                inverse = false;
            var desc = true;
            th.click(function () {
                table.find('td').filter(function () {
                    return $(this).index() === thIndex;
                }).sortElements(function (a, b) {
                    return $.text([a]) > $.text([b]) ?
                        inverse ? -1 : 1
                        : inverse ? 1 : -1;
                }, function () {
                    return this.parentNode;
                });
                $(columnIdList).children('i').remove();
                $(columnIdList).append('<i class="fa fa-fw fa-sort"></i>');
                $(this).children('i').remove();
                if (desc) {
                    $(this).append('<i class="fa fa-fw fa-sort-desc"></i>');
                } else {
                    $(this).append('<i class="fa fa-fw fa-sort-asc"></i>');
                }
                desc = !desc;
                inverse = !inverse;
            });
        });
}
function sortingTableWithRowspan(tableId, columnIdList) {
    var inverse = false;
    var desc = true;
    $(columnIdList).append('<i class="fa fa-fw fa-sort"></i>');
    function sortColumn(index) {
        var trs = $(tableId + ' > tbody > tr'),
            nbRowspans = trs.first().children('[rowspan]').length,
            offset = trs.first().children('[rowspan]').last().offset().left;

        var tds = trs.children('[rowspan]').each(function () {
            $(this).data('row', $(this).parent().index());
            $(this).data('column', $(this).index());
            $(this).data('offset', $(this).offset().left);
        }).each(function () {
            if ($(this).data('offset') != offset)
                return;
            var rowMin = $(this).data('row'),
                rowMax = rowMin + parseInt($(this).attr('rowspan'));
            trs.slice(rowMin, rowMax).children().filter(function () {
                return $(this).index() == index + $(this).parent().children('[rowspan]').length - nbRowspans;
            }).sortElements(function (a, b) {
                a = convertToNum($(a).text());
                b = convertToNum($(b).text());
                return (
                    isNaN(a) || isNaN(b) ?
                    a > b : +a > +b
                    ) ?
                inverse ? -1 : 1 :
                inverse ? 1 : -1;
            }, function () {
                return this.parentNode;
            });
        });
        var trs = $(tableId + ' > tbody > tr');
        tds.each(function () {
            if ($(this).parent().index() != $(this).data('row'))
                $(this).insertBefore(trs.eq($(this).data('row')).children().eq($(this).data('column')));
        });
        inverse = !inverse;
    }
    function convertToNum(str) {
        if (isNaN(str)) {
            var holder = "";
            for (i = 0; i < str.length; i++) {
                if (!isNaN(str.charAt(i))) {
                    holder += str.charAt(i);
                }
            }
            return holder;
        } else {
            return str;
        }
    }
    $(columnIdList).click(function () {
        sortColumn($(this).index());
        $(columnIdList).children('i').remove();
        $(columnIdList).append('<i class="fa fa-fw fa-sort"></i>');
        $(this).children('i').remove();
        if (desc) {
            $(this).append('<i class="fa fa-fw fa-sort-desc"></i>');
        } else {
            $(this).append('<i class="fa fa-fw fa-sort-asc"></i>');
        }
        desc = !desc;
    });
}
/*********************HIGLITE*********************************/
jQuery.fn.highlight = function (c) {
    function e(b, c) {
        var d = 0;
        if (3 == b.nodeType) {
            var a = b.data.toUpperCase().indexOf(c),
                a = a - (b.data.substr(0, a).toUpperCase().length - b.data.substr(0, a).length);
            if (0 <= a) {
                d = document.createElement("span");
                d.className = "highlight";
                a = b.splitText(a);
                a.splitText(c.length);
                var f = a.cloneNode(!0);
                d.appendChild(f);
                a.parentNode.replaceChild(d, a);
                d = 1;
            }
        } else if (1 == b.nodeType && b.childNodes && !/(script|style)/i.test(b.tagName)
        ) for (a = 0; a < b.childNodes.length; ++a) a += e(b.childNodes[a], c);
        return d;
    }

    return this.length && c && c.length ? this.each(function () { e(this, c.toUpperCase()) }) : this;
};
jQuery.fn.removeHighlight = function () {
    return this.find("span.highlight")
        .each(function () {
            this.parentNode.firstChild.nodeName;
            with (this.parentNode) replaceChild(this.firstChild, this), normalize();
        })
        .end();
};

/***********************RANDOM VALUE(captcha entry)****************************************** */
function drawCaptcha(id) {
    var a = Math.ceil(Math.random() * 10) + '';
    var b = Math.ceil(Math.random() * 10) + '';
    var c = Math.ceil(Math.random() * 10) + '';
    var d = Math.ceil(Math.random() * 10) + '';
    var e = Math.ceil(Math.random() * 10) + '';
    var f = Math.ceil(Math.random() * 10) + '';
    var g = Math.ceil(Math.random() * 10) + '';
    //  var code = a + ' ' + b + ' ' + ' ' + c + ' ' + d + ' ' + e + ' ' + f + ' ' + g;
    var code = a + '' + b + '' + '' + c + '' + d + '' + e + '' + f + '' + g;
    $("#" + id).val(code);
}
function validCaptcha(capture, input) {
    var str1 = ($("#" + id).val(capture)).split(' ').join('');
    var str2 = ($("#" + id).val(input)).split(' ').join('');
    if (str1 === str2) return true;
    return false;

}

/*************************INIT APP**********************************/
$(function () {
    $('#myModal').on('show.bs.modal', function () {
        $(this).find('.modal-dialog').removeAttr("style");
        if ($("#modal-type").val() === "large") {
            $(this).find('.modal-dialog').css({
                width: '80%',
                height: 'auto',
                'overflow-y': 'auto',
                'max-height': '90%'
            });
        }

    });
    $('#myModal').on('hidden.bs.modal', function () {
        $(this).find('.modal-dialog').removeAttr("style");
    });

    function stripTrailingSlash(str) {
        if (str.substr(-1) === '/') {
            return str.substr(0, str.length - 1);
        }
        return str;
    }
    var url = window.location.pathname;
    var activePage = stripTrailingSlash(url);
    $('.nav li a').each(function () {
        var currentPage = stripTrailingSlash($(this).attr('href'));
        if (activePage === currentPage) {
            $(this).parent().addClass('active');
        }
    });
    MessageBox.init({
        "selector": "#side-alert"
    });
    ToastBox.init({
        "selector": "#toast-alert"
    });
    getFixedFoltingButton();
});

$(document).on('click',"table#dataTable>tbody>tr>td>span",function() {
            var arg = $(this);
            var page = $("#current-page").val();
            var area = $("#area-name").val();
            var ctrl = $("#controller-name").val();
            var pram = arg.attr("data-id");
            var action = arg.attr('data-action');
            var url = area
                ? '/' + area + "/" + ctrl + "/" + action + "/" + pram + "/"
                : "/" + ctrl + "/" + action + "/" + pram + "/";
            if (arg.hasClass("btnDelete")) {
                var currentPage = area
                    ? '/' + area + "/" + ctrl + "/Index/?page=" + page
                    : "/" + ctrl + "/" + action + "/Index/?page=" + page;
                var msg = "Do you want to deactive this " + $('#PageHeader').html() + "?";
                deActivateModal(url, currentPage, msg);
            } else {
                createModal(url, page);
            }
        });

function getFixedFoltingButton() {
    if ($("#create-url").val()) {
        $("#fixed-button").show();
    } else {
        $("#fixed-button").hide();
    }
}

$("#fixed-button").click(function() {
    var url = $("#create-url").val();
    var page = $("#total-object").val();
    createModal(url, page);

});
$("#search-button ").click(function (e) {
    e.preventDefault();
});

$(function () {
    $(".navbar-expand-toggle").click(function () {
            $(".azr-container").toggleClass("expanded");
            return $(".navbar-expand-toggle").toggleClass("fa-rotate-90");
        });
    return $(".navbar-right-expand-toggle").click(function () {
            $(".navbar-right").toggleClass("expanded");
            return $(".navbar-right-expand-toggle").toggleClass("fa-rotate-90");
        });
});
$(function () {
    return $(".side-menu .nav .dropdown").on('show.bs.collapse',
            function () {
                return $(".side-menu .nav .dropdown .collapse").collapse('hide');
            });
});
$('.main-nav li>a').on('click', function () {
    $('.main-nav li.active').removeClass('active');
    $(this.parentNode).addClass('active');
});
$('#btn-back-to-top').click(function (e) {
    e.preventDefault();
});
$(window).scroll(function () {
    if ($(this).scrollTop() > offset) {
        $('.crunchify-top').fadeIn(duration);
    } else {
        $('.crunchify-top').fadeOut(duration);
    }
});
$('.crunchify-top').click(function (event) {
    event.preventDefault();
    $('html, body').animate({ scrollTop: 0 }, duration);
    return false;
});
