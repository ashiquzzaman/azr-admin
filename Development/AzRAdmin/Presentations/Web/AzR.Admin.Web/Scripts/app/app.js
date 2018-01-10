/**********************************************************************************/
var monthNames = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec");
var Spiner = (function () {
    "use strict";
    var opts = {
        lines: 15,
        length: 0,
        width: 21,
        radius: 43,
        scale: 0.75,
        corners: 1,
        color: '#004A89',
        opacity: 0.15,
        rotate: 0,
        direction: 1,
        speed: 0.7,
        trail: 80,
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
        spin.stop();
        $('#splash-page').hide();
        //Timer.refresh($("#session-time").val());
    }
    return result;
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
/**********************************************************************************/
function OnAjaxRequestSuccess(data) {
    $('#PageHeader').html(data);
}
function SubmitOnSuccess(result) {
    if (result.redirectTo) {
        closeModal();
        bootbox.alert({
            message: result.message
        });
        loadLink(result.redirectTo, result.position);
    } else {
        $('#modelContent').html(result);
        $('#myModal').modal('show');
    }
}
function handleOnSuccess(result) {
    if (result.redirectTo) {
        bootbox.alert({
            message: result.message
        });
        loadLink(result.redirectTo, result.position);
    } else {
        $('#mainContent').html(result);
    }
}
function redirectOnSuccess(result) {
    if (result.redirectTo) {
        bootbox.alert({
            message: result.message
        });
        window.location.href = result.redirectTo;
    } else {
        $('#modelContent').html(result);
        $('#myModal').modal('show');
    }
}
function loadLink(url, id) {
    if (typeof (id) === 'undefined') {
        loadPartialView(url, 'mainContent');
    } else {
        loadPartialView(url, id);
    }
}
function searchTable(result, tableId, trClass, url, contentPosition) {
    if (result) {
        result = result.trim();
        var reg = new RegExp(result, 'i');
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
        if (url) {
            if (contentPosition)
                loadPartialView(url, contentPosition);
            else
                loadPartialView(url, "mainContent");
        }
        else {
            $('#' + tableId + ' tbody').find('tr').show();
            $('#' + tableId).removeHighlight();
        }
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
function deleteModal(url, redirectTo) {
    bootbox.confirm("Are you sure want to Delete this Record?", function (result) {
        if (result) {
            $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    bootbox.alert({
                        message: "Record Deleted successfully!!!"
                    });
                    loadLink(redirectTo);
                },
                error: function (data) {
                    bootbox.alert('Error in getting result');
                }
            });
        }
    });

}
function loadMenu(url, header, id) {

    // toastr.remove();

    if (typeof (id) === 'undefined') {
        loadPartialView(url, 'mainContent');
    } else {
        loadPartialView(url, id);
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
    }).done(function (result) {
        $('#' + position).show();
        $('#' + position).html(result);
        Spiner.hide();
    }).fail(function (status) {
        alert(status);
        Spiner.hide();
    });
}
function addRowInFormTable(tableName) {
    var last = $('#' + tableName + '>tbody>tr:last');
    if (last.find('.select2-select').length > 0) {
        last.find('.select2-select').select2('destroy');
    }
    if (last.length > 0) {
        var id = last.children().find('input,select')[0].id;
        var prefix = (/^[^_]+(?=_)/).exec(id)[0];
        prefix = prefix == id.replace(/([A-Za-z_])/gi, '') ? "" : prefix;
        var index = Number(id.replace(/([A-Za-z_])/gi, '')) + 1;
        var tr = prefix != "" ? ('<tr>' + last.html().replace(/([A-Za-z])\w+[_]+[0-9]+/gi, prefix + '_' + index) + '</tr>')
               .replace(/([A-Za-z])\w+\[[0-9]+\]/gi, prefix + '[' + index + ']')
            : ('<tr>' + last.html().replace(/[0-9]+__/gi, index + '__') + '</tr>')
               .replace(/\[[0-9]+\]+[.]/gi, '[' + index + '].');

        if (last.find('.select2-select').length > 0) {
            last.find('.select2-select').select2({
                theme: "bootstrap",
                width: '100%'
            });
        }
        $('#' + tableName + ' tbody').append(tr);
        if (last.find('.select2-select').length > 0) {
            $('#' + tableName + ' tr:last')
                .find('.select2-select')
                .select2({
                    theme: "bootstrap",
                    width: '100%'
                });
        }
    }
}
function tableReIndixing(tableName) {
    var trs = "";
    $.each($('#' + tableName + '>tbody>tr'), function (index, last) {
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
    $('#' + tableName + ' tbody').html('');
    $('#' + tableName + ' tbody').append(trs);
    if ($('#' + tableName + ' tr').find('.select2-select').length > 0) {
        $('#' + tableName + ' tr').find('.select2-select').select2({
            theme: "bootstrap",
            width: '100%'
        });
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
function fnExcelReport(tableId) {
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
function onlyNumeric(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var exclusions = [8, 46];
    if (exclusions.indexOf(key) > -1) { return; }
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
    if (exclusions.indexOf(key) > -1) { return; }
    key = String.fromCharCode(key);
    var regex = /^[a-z0-9]+$/i;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}
function notNumeric(elem) {
    var numericExpression = /^((\d+(\.\d*)?)|((\d*\.)?\d+))$/;
    if (elem.value == '')
        return false;
    if (elem.value.match(numericExpression)) {
        return false;
    } else {
        elem.value = '';
        alert('Number only please');
        return true;
    }
};
function GetJsonDateToActualDate(jsonDate) {
    var date = new Date(parseInt(jsonDate.substr(6)));
    var month = date.getMonth();
    var day = date.getDate();
    var year = date.getFullYear();
    var finaldate = monthNames[month] + " " + day + ", " + year;
    return finaldate;
};
function GetJsonDateTolDateTime(jsonDate) {
    var newDate = new Date(parseInt(jsonDate.substr(6)));
    var sMonth = padValue(newDate.getMonth() + 1);
    var sDay = padValue(newDate.getDate());
    var sYear = newDate.getFullYear();
    var sHour = newDate.getHours();
    var sMinute = padValue(newDate.getMinutes());
    var sAmpm = "AM";
    var iHourCheck = parseInt(sHour);
    if (iHourCheck > 12) {
        sAmpm = "PM";
        sHour = iHourCheck - 12;
    }
    else if (iHourCheck === 0) {
        sHour = "12";
    }
    sHour = padValue(sHour);
    return monthNames[sMonth - 1] + " " + sDay + ", " + sYear + " " + sHour + ":" + sMinute + " " + sAmpm;
};
function getAge(dob) {

    var newDob = new Date(dob);
    var today = new Date();
    var age = Math.floor((today - newDob) / (365.25 * 24 * 60 * 60 * 1000));
    return age;
}

/****************************PAGE INIT****************************************/
