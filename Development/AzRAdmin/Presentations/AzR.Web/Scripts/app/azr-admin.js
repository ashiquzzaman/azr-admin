/********************VARIABLE**************************/
var offset = 220;
var duration = 500;
var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"];

var validImageExtensions = ["jpg", "jpeg", "bmp", "gif", "png"];

var units = ['', 'One ', 'Two ', 'Three ', 'Four ', 'Five ', 'Six ', 'Seven ', 'Eight ', 'Nine '];

var teens = ["Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"];

var tens = ['', '', "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"];

var scales = ["", "Thousand", "Lakh", "Million", "Crore", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion", "Septillion", "Octillion", "Nonillion"];

/***************IMAGE & FILE***********************/
function getExtention(name) {
    var extension = name.substring(name.lastIndexOf('.') + 1).toLowerCase();
    return extension;
}
function validedImageExtention(name) {
    var extension = name.substring(name.lastIndexOf('.') + 1).toLowerCase();
    return $.inArray(extension, validImageExtensions) !== -1;
}

/*****************OOP************************/
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
        that = {};

    that.init = function (options) {
        elem = $(options.selector);
    };
    that.show = function (text) {
        clearTimeout(hideHandler);
        elem.find("span").html(text);
        elem.delay(200).fadeIn().delay(1000).fadeOut();
    }
    return that;
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
/*********************COOKIES*************************************/
function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
    return null;
}

function createCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

/*********************AJAX*************************************/
function ValidateForm() {
    return $.validator.unobtrusive.parse($('form'));
}
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
            var child = $(arg).children();
            if (child.length === 0) {
                $('#PageHeader').html($(arg).html());
            } else {
                $('#PageHeader').html($(child[1]).html());
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
        window.location.href = result.redirectTo;
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
            $('#page').val(page);
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
                success: function (data) {
                    showMessage("Record DeActivated successfully!!!");
                    loadLink(redirectTo);
                },
                error: function (data) {
                    bootbox.alert('Error in getting result');
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
                success: function (data) {
                    showMessage("Record Deleted successfully!!!");
                    loadLink(redirectTo);
                },
                error: function (data) {
                    bootbox.alert('Error in getting result');
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
        dataType: 'html'
    })
        .success(function (result) {
            Spiner.show();
            $('#' + position).html(result);
            Spiner.hide();
        })
        .error(function (xhr, status) {
            alert(status);
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
function removeDuplicate(arr, prop) {
    var newArr = [];
    var lookup = {};

    var i, j;
    for (i in arr) {
        if (arr.hasOwnProperty(i)) {
            lookup[arr[i][prop]] = arr[i];
        }
    }

    for (j in lookup) {
        if (lookup.hasOwnProperty(j)) {
            newArr.push(lookup[j]);
        }
    }

    return newArr;
};
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
    totalCalculate('#' + $(qyArg)[0].id);
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
    if (key === 45 || key === 189) theEvent.preventDefault();
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
/*********************NUMBER TO WORD*********************************/
function InWord(num) {
    if ((num = num.toString()).length > 9) return 'overflow';
    n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
    if (!n) return; var str = '';
    str += (n[1] != 0) ? (units[Number(n[1])] || tens[n[1][0]] + ' ' + units[n[1][1]]) + 'Crore ' : '';
    str += (n[2] != 0) ? (units[Number(n[2])] || tens[n[2][0]] + ' ' + units[n[2][1]]) + 'Lakh ' : '';
    str += (n[3] != 0) ? (units[Number(n[3])] || tens[n[3][0]] + ' ' + units[n[3][1]]) + 'Thousand ' : '';
    str += (n[4] != 0) ? (units[Number(n[4])] || tens[n[4][0]] + ' ' + units[n[4][1]]) + 'Hundred ' : '';
    str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (units[Number(n[5])] || tens[n[5][0]] + ' ' + units[n[5][1]]) + 'only ' : '';
    return str;
}

function numberToEnglish(n) {

    var string = n.toString(), start, end, chunks, chunksLen, chunk, ints, i, word, words, and = 'and';

    /* Is number zero? */
    if (parseInt(string) === 0) {
        return 'zero';
    }

    /* Split user arguemnt into 3 digit chunks from right to left */
    start = string.length;
    chunks = [];
    while (start > 0) {
        end = start;
        chunks.push(string.slice((start = Math.max(0, start - 3)), end));
    }

    /* Check if function has enough scale words to be able to stringify the user argument */
    chunksLen = chunks.length;
    if (chunksLen > scales.length) {
        return '';
    }

    /* Stringify each integer in each chunk */
    words = [];
    for (i = 0; i < chunksLen; i++) {

        chunk = parseInt(chunks[i]);

        if (chunk) {

            /* Split chunk into array of individual integers */
            ints = chunks[i].split('').reverse().map(parseFloat);

            /* If tens integer is 1, i.e. 10, then add 10 to units integer */
            if (ints[1] === 1) {
                ints[0] += 10;
            }

            /* Add scale word if chunk is not zero and array item exists */
            if ((word = scales[i])) {
                words.push(word);
            }

            /* Add unit word if array item exists */
            if ((word = units[ints[0]])) {
                words.push(word);
            }

            /* Add tens word if array item exists */
            if ((word = tens[ints[1]])) {
                words.push(word);
            }

            /* Add 'and' string after units or tens integer if: */
            if (ints[0] || ints[1]) {

                /* Chunk has a hundreds integer or chunk is the first of multiple chunks */
                if (ints[2] || !i && chunksLen) {
                    words.push(and);
                }

            }

            /* Add hundreds word if array item exists */
            if ((word = units[ints[2]])) {
                words.push(word + ' hundred');
            }

        }

    }
    return words.reverse().join(' ');

}

function convertMillion(num) {
    if (num >= 1000000) {
        return convertMillion(Math.floor(num / 1000000)) + " million " + convertThousand(num % 1000000);
    }
    else {
        return convertThousand(num);
    }
}

function convertThousand(num) {
    if (num >= 1000) {
        return convertHundred(Math.floor(num / 1000)) + " thousand " + convertHundred(num % 1000);
    }
    else {
        return convertHundred(num);
    }
}

function convertHundred(num) {
    if (num > 99) {
        return units[Math.floor(num / 100)] + " hundred " + convertTen(num % 100);
    }
    else {
        return convertTen(num);
    }
}

function convertTen(num) {
    if (num < 10) return units[num];
    else if (num >= 10 && num < 20) return teens[num - 10];
    else {
        return tens[Math.floor(num / 10)] + " " + units[num % 10];
    }
}

function convert(num) {
    if (num == 0) return "zero";
    else return convertMillion(num);
}

/*********************DIV******************************************/
function addRowInDiv(dynamicGroupId) {
    if (typeof (dynamicGroupId) === 'undefined') {
        dynamicGroupId = "#dynamicRowGroup";
    } else {
        dynamicGroupId = '#' + dynamicGroupId;
    }

    var $last = $(dynamicGroupId + ">div.dynamicRow:last");

    if ($last.length > 0) {
        if ($(dynamicGroupId).find('.select2-select').length > 0) {
            $('.select2-select').select2('destroy');
        }
        var name = $last.children().find('input,select')[0].name;
        var prePost = name.match(/([a-zA-Z]+?)(s\b|\b)/gi);
        var index = Number(name.replace(/[^0-9]/gi, '')) + 1;
        var prefix = prePost.length === 1 ? "" : prePost[0];

        var div = '<div class="row no-gap dynamicRow">' +
            ($last.html()
                .replace(/([A-Za-z])\w+[_]+[0-9]+/gi, prefix + '_' + index))
                .replace(/([A-Za-z])\w+\[[0-9]+\]/gi, prefix + '[' + index + ']') +
            '</div>';

        $last.after(div);
        if ($(dynamicGroupId).find('.select2-select').length > 0) {
            $('.select2-select').select2({
                theme: "bootstrap",
                width: '100%',
                dropdownParent: $("#myModal")
            });
        }
        $.each($(dynamicGroupId + ">div.dynamicRow:last").find('input,select'), function (i, item) {
            if (item.type === 'hidden' && item.id.match(/__Id$/)) {
                item.value = '0';
            }
            if (item.type === 'text') {
                item.value = '';
                item.defaultValue = '';
            }
            if (item.type === 'select') {
                $('#' + item.id + ' option').removeAttr('selected');
            }
        });
    }
}
function divReIndixing(dynamicGroupId) {
    if (typeof (dynamicGroupId) === 'undefined') {
        dynamicGroupId = "#dynamicRowGroup";
    } else {
        dynamicGroupId = '#' + dynamicGroupId;
    }
    if ($(dynamicGroupId).find('.select2-select').length > 0) {
        $('.select2-select').select2('destroy');
    }
    var divs = "";
    $.each($(dynamicGroupId + ">div.dynamicRow"), function (index, last) {
        var name = $(this).children().find('input,select')[0].name;
        var prePost = name.match(/([a-zA-Z]+?)(s\b|\b)/gi);
        var prefix = prePost.length === 1 ? "" : prePost[0];
        var div = '<div class="row no-gap dynamicRow">' +
            ($(last).html()
                .replace(/([A-Za-z])\w+[_]+[0-9]+/gi, prefix + '_' + index))
                .replace(/([A-Za-z])\w+\[[0-9]+\]/gi, prefix + '[' + index + ']') +
            '</div>';

        divs += div;
    });
    $("div.dynamicRow").remove();
    $(dynamicGroupId + ">div:first").after(divs);
    if ($(dynamicGroupId).find('.select2-select').length > 0) {
        $('.select2-select').select2({
            theme: "bootstrap",
            width: '100%',
            dropdownParent: $("#myModal")
        });
    }
}

/*********************TABLE******************************************/
$(document).on('click', 'table>tbody>tr>td>.removeItem', function (e) {
    removeRowInTable(e.target.parentNode);
});

function addRowInTable(tableId) {
    if (typeof (tableId) === 'undefined') {
        tableId = '#dataTable';
    } else {
        tableId = '#' + tableId;
    }
    var $last = $(tableId + '>tbody>tr:last');

    if ($last.length > 0) {

        if ($(tableId).find('.select2-select').length > 0) {
            $('.select2-select').select2('destroy');
        }
        var name = $last.children().find('input,select')[0].name;
        var prePost = name.match(/([a-zA-Z]+?)(s\b|\b)/gi);
        var index = Number(name.replace(/[^0-9]/gi, '')) + 1;
        var prefix = prePost.length === 1 ? "" : prePost[0];

        var tr = '<tr>' +
            $last.html()
                .replace(/([A-Za-z])\w+[_]+[0-9]+/gi, prefix + '_' + index)
                .replace(/([A-Za-z])\w+\[[0-9]+\]/gi, prefix + '[' + index + ']') +
            '</tr>';

        $(tableId + ' tbody').append(tr);

        if ($(tableId).find('.select2-select').length > 0) {
            $('.select2-select').select2({
                theme: "bootstrap",
                width: '100%',
                dropdownParent: $("#myModal")
            });
        }
        $.each($(tableId + '>tbody>tr:last').find('input,select'), function (index, item) {
            if (item.type === 'hidden' && item.id.match(/__Id$/)) {
                item.value = '0';
            }
            if (item.type === 'text') {
                item.value = '';
                item.defaultValue = '';
            }
            if (item.type === 'select') {
                $('#' + item.id + ' option').removeAttr('selected');
            }
        });
    }
}

function removeRowInTable(arg) {
    var table = arg.parentNode.parentNode.parentNode.parentNode;
    var rowCount = $('#' + table.id + '>tbody>tr').length;
    if (rowCount > 1) {
        $(arg).parent().parent().remove();
        tableReIndixing(table.id);
    }
}

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

function tableLastIndex(tableId) {
    var last = $('#' + tableId + '>tbody>tr:last');
    var id = last.children().find('input,select')[0].id;
    var index = Number(id.replace(/([A-Za-z_])/gi, ''));
    return index;
}

function tableReIndixing(tableId) {
    if (typeof (tableId) === 'undefined') {
        tableId = '#dataTable';
    } else {
        tableId = '#' + tableId;
    }
    var trs = "";
    if ($(tableId).find('.select2-select').length > 0) {
        $('.select2-select').select2('destroy');
    }
    $.each($(tableId + '>tbody>tr'), function (index, last) {
        var name = $(this).children().find('input,select')[0].name;
        var prePost = name.match(/([a-zA-Z]+?)(s\b|\b)/gi);
        var prefix = prePost.length === 1 ? "" : prePost[0];

        var tr = '<tr>' +
            $(last).html()
                .replace(/([A-Za-z])\w+[_]+[0-9]+/gi, prefix + '_' + index)
                .replace(/([A-Za-z])\w+\[[0-9]+\]/gi, prefix + '[' + index + ']') +
            '</tr>';
        trs += tr;
    });
    $(tableId + ' tbody').html('');
    $(tableId + ' tbody').append(trs);
    if ($(tableId).find('.select2-select').length > 0) {
        $('.select2-select').select2({
            theme: "bootstrap",
            width: '100%',
            dropdownParent: $("#myModal")
        });
    }
}

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
function exportTableToExcel(tableId) {
    var tabText = "<table border='2px'><tr>";
    var tab = document.getElementById(tableId);

    for (var j = 1; j < tab.rows.length; j++) {
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
/*********************MODAL*********************************/
function modalResize() {
    $('#myModal').on('show.bs.modal', function () {
        var modalType = $($(this).find("[modal-type]")[0]).attr('modal-type');
        $(this).find('.modal-dialog').removeAttr("style");

        if (modalType === "auth") {
            $("#myModal").css("margin-top", "100px");
        } else {
            $("#myModal").css("margin-top", "");
        }
        if (modalType === "large") {
            $(this).find('.modal-dialog').css({
                width: '80%',
                height: 'auto',
                'overflow-y': 'auto',
                'max-height': '90%'
            });
        }
        if ($(this).find('.select2-select').length > 0) {
            $('.select2-select').select2({
                theme: "bootstrap",
                width: '100%',
                dropdownParent: $("#myModal")
            });
        }
    });
    $('#myModal').on('hidden.bs.modal', function () {
        $(this).find('.modal-dialog').removeAttr("style");
        if ($(this).find('.select2-select').length > 0) {
            $(this).find('.select2-select').select2('destroy');
        }
    });
}
/*********************NAV*********************************/
function stripTrailingSlash(str) {
    if (str.substr(-1) === '/') {
        return str.substr(0, str.length - 1);
    }
    return str;
}
function addActiveNav() {
    var url = window.location.pathname;
    var activePage = stripTrailingSlash(url);
    $('.nav li a').each(function () {
        var currentPage = stripTrailingSlash($(this).attr('href'));
        if (activePage === currentPage) {
            $(this).parent().addClass('active');
        }
    });
}
function openWithSubmit(action, valueForm) {
    if (typeof (valueForm) === 'undefined') {
        valueForm = "ReportForm";
    }
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", action);
    form.setAttribute("target", "formresult");

    var otherData = $('#' + valueForm).serializeArray();

    $.each(otherData, function (key, input) {
        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("name", input.name);
        hiddenField.setAttribute("value", input.value);
        form.appendChild(hiddenField);
    });

    document.body.appendChild(form);
    window.open(action, 'formresult');
    form.submit();

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

/*************************CSS*********************************/
function colorReplace(findHexColor, replaceWith) {
    // Convert rgb color strings to hex
    function rgb2hex(rgb) {
        if (/^#[0-9A-F]{6}$/i.test(rgb)) return rgb;
        rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
        function hex(x) {
            return ("0" + parseInt(x).toString(16)).slice(-2);
        }
        return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
    }

    // Select and run a map function on every tag
    $('*').map(function (i, el) {
        // Get the computed styles of each tag
        var styles = window.getComputedStyle(el);

        // Go through each computed style and search for "color"
        Object.keys(styles).reduce(function (acc, k) {
            var name = styles[k];
            var value = styles.getPropertyValue(name);
            if (value !== null && name.indexOf("color") >= 0) {
                // Convert the rgb color to hex and compare with the target color
                if (value.indexOf("rgb(") >= 0 && rgb2hex(value) === findHexColor) {
                    // Replace the color on this found color attribute
                    $(el).css(name, replaceWith);
                }
            }
        });
    });
}

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

/*****************************INIT APP*****************************************************************/
$(function () {
    modalResize();
    addActiveNav();
    MessageBox.init({
        "selector": "#side-alert"
    });
    ToastBox.init({
        "selector": "#toast-alert"
    });
    //setInterval('updateClock()', 1000);
    getFixedFoltingButton();
});

$(document).on("click", '.azr-nav li>a', function () {
    $('.azr-nav li> a.active').removeClass('active');
    $(this).addClass('active');
});
$('.azr-nav li>a').on('click', function () {
    $('.azr-nav li.active').removeClass('active');
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

/*******************************CRUD BUTTON***********************************************************************************/
$(document).on('click', "table#dataTable>tbody>tr>td>span", function (e) {
    var arg = $(e.target);
    var page = $("#data-box").attr("current-page") == undefined ? 1 : $("#data-box").attr("current-page");
    var area = $("#area-name").val();
    var ctrl = $("#controller-name").val();
    var pram = arg.attr("data-id");
    var action = arg.attr('data-action');
    if (action != undefined) {
        var url = area
            ? '/' + area + "/" + ctrl + "/" + action + "/" + pram + "/"
            : "/" + ctrl + "/" + action + "/" + pram + "/";
        if (action === 'delete') {
            var currentPage = area
                ? '/' + area + "/" + ctrl + "/Index/?page=" + page
                : "/" + ctrl + "/" + action + "/Index/?page=" + page;
            var msg = "Do you want to deactive this " + $('#PageHeader').html() + "?";
            deActivateModal(url, currentPage, msg);
        }
        if (action === 'save') {
            createModal(url, page);
        }
        if (action === 'details') {
            createModal(url, page);
        }
        if (action === 'print') {
            window.open(url, '_blank').focus();
        }
    }

});
function getFixedFoltingButton() {

    var create = $("#data-box").attr("data-action");
    if (create) {
        $("#fixed-button").show();
    } else {
        $("#fixed-button").hide();
    }
}
$(document).on("click", "#fixed-button", function () {
    var page = $("#data-box").attr("total-object") == undefined ? 1 : $("#data-box").attr("total-object");
    var area = $("#area-name").val();
    var ctrl = $("#controller-name").val();
    var action = $("#data-box").attr("data-action");
    var url = area ? '/' + area + "/" + ctrl + "/" + action + "/" : "/" + ctrl + "/" + action + "/";
    createModal(url, page);

});
function loadPagination(link, divId, arg) {
    var url = link.replace(/pageSize=\d+/, "pageSize=" + arg.value);
    loadLink(url, divId);
}

$("#search-button ").click(function (e) {
    e.preventDefault();
});
/*******************************AzR-Admin***********************************************************************************/
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
$(document).on("click", ".language-list>.list-group-item", function () {
    var val = this.getAttribute("culture-name");
    window.location.href = "/dashboard/SetCulture/?culture=" + val;
});
$(document).on("click", ".theme-list>.list-group-item", function () {
    var val = this.getAttribute("theme-name");
    window.location.href = "/dashboard/SetTheme/?id=" + val;
});
/*******************************END***********************************************************************************/