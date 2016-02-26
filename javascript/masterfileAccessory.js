
//input fields
var input_code,
    input_descs,
    input_audit_user,
    input_is_new_entry,
    input_dd_group_cd,
    input_txtsearch,
    buttonAdd,
    buttonSave,
    buttonCancel


$(document).ready(function () {
    initializeButtons();
    initializeBindings();
    loadDropDownList('#ContentPlaceHolder1_va_DD_accessory_group');    
})

var initializeBindings = function () {

    //ADD=============================================================
    buttonAdd.click(function () {
        buttonAdd.disable(true);
        buttonSave.disable(false);
        buttonCancel.disable(false);
        enable_Form();
        input_is_new_entry = true;

        clear_form();        
    });

    //SAVE============================================================
    buttonSave.click(function () {
        console.log('button save is clicked!');
        saveAccessory();
        disable_form();        
    });

    //CANCEL==========================================================
    buttonCancel.click(function () {
        disable_form();
        //var pathname = window.location.pathname;

        //window.location = pathname + "?ID='" + pathname+ "'&NAME=" ;
        //console.log('the pathname is ' + pathname);
    });

    $("#txtsearch").on("keyup", function () {
        delay(function () {
            initTable();
        }, 500);
    })

    initializeInput();
    //initialize datatable
    initTable();
    initTableOnClick();
}

var initializeInput = function () {
    input_code = $("#va_code");
    input_descs = $("#va_description");    
    input_audit_user = $('#ContentPlaceHolder1_hidden_username');
    input_dd_group_cd = $("#ContentPlaceHolder1_va_DD_accessory_group");
    input_txtsearch = $("#txtsearch");
    input_is_new_entry = true;
}

var initializeButtons = function () {
    buttonAdd = $('#btnAdd');
    buttonSave = $('#btnSave');
    buttonCancel = $('#btnCancel');
}

var initTableOnClick = function () {
    vTable.on('click', 'tr', function () {
        input_is_new_entry = false;
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            vTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }       
        var rowData = vTable.api().rows('tr.selected').data()[0];
        viewTableRowData(rowData)
        update_form();
    });
}

jQuery.fn.extend({
    disable: function (state) {
        return this.each(function () {
            var $this = $(this);
            $this.toggleClass('disabled', state);
        });
    }
});


var loadDropDownList = function (obj) {
    var parameter = '{ Obj: "' + obj + '"}';
    ajaxHelper('masterfile.asmx/getDropDownListItems', 'POST', parameter).done(function (data) {
        var datad = data.d;
        $.each(datad, function (key, value) {
            $(obj).append($("<option></option>").val(value.code).html(value.descs));
        });
        initializeInput();
    });
}

function ajaxHelper(uri, method, data) {
    return $.ajax({
        type: method,
        url: uri,
        dataType: 'json',
        contentType: 'application/json',
        data: data ? data : null
    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert("the error is : " + jqXHR.responseText);
    });
}

var saveAccessory = function () {
    var code = input_code.val();
    var descs = input_descs.val();
    var group_cd = input_dd_group_cd.val();
    var audit_user = input_audit_user.val();
    var is_new_entry = input_is_new_entry;
    var params = {
        code: code,
        descs: descs,
        group_cd: group_cd,
        audit_user: audit_user,
        is_new_entry: is_new_entry
    }

    console.log('the parameters of saveAccessory are : ' + JSON.stringify(params));
    ajaxHelper('masterfile.asmx/saveAccessory', 'POST', JSON.stringify(params)).done(function (data) {
        console.log('the received from insertVehicleLog data is : ' + JSON.stringify(data));
        if (data.d != '') {
            $('#div_msg').html('');
            $('#div_msg').html(data.d);

            $("#div_msg").hide().fadeIn('slow'); //.slideDown();
            setTimeout(function () {
                $("#div_msg").fadeOut('slow');
            }, 10000);
        }
        vTable.api().ajax.reload();        
    });    
}

var initTable = function () {

    var search = $('#txtsearch').val();
    console.log('the txtsearch is : ' + search );
    vTable = $('#T_Accessory').dataTable({
        "bLengthChange": false,
        "filter": false,
        "responsive": true,
        "pagingType": "simple_numbers",
        "orderClasses": false,
        "info": false,
        "scrollY": "400px",
        "scrollX": true,
        "scrollCollapse": true,
        "bProcessing": true,
        "bPaginate": true,
        "bDestroy": true,
        "bServerSide": true,
        "sAjaxSource": "masterfile.asmx/getAccessoriesDT",
        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "roleId", "value": "admin" });
            aoData.push({ "name": "searchValue", "value":  search});
            $.ajax({
                "dataType": 'json',
                "contentType": "application/json; charset=utf-8",
                "type": "GET",
                "url": sSource,
                "data": aoData,
                "success": function (msg) {
                   
                    var json = jQuery.parseJSON(msg.d);
                    fnCallback(json);
                    console.log("the value of msg is : " + JSON.stringify(msg) );
                    
                },
                error: function (xhr, textStatus, error) {
                    if (typeof console == "object") {
                        console.log(xhr.status + "," + xhr.responseText + "," + textStatus + "," + error);
                    }
                }
            })
        },

        "aoColumns": [
            { "mDataProp": "code" ,"sTitle": "Code", "bSearchable": true },
            { "mDataProp": "descs", "sTitle": "Description" },
            { "mDataProp": "group_cd", "sTitle": "Group Code" }
        ]
        //,
        //"aoColumns": [
        //    { "mDataProp": "code" },
        //    { "mDataProp": "descs" },
        //    { "mDataProp": "group_cd" }
        //]

    //    "aoColumns": [
    //        { "mDataProp": 'Id' },
    //        { "mDataProp": 'FirstName' },
    //        { "mDataProp": 'NickName' },
    //        { "mDataProp": 'MiddleName' },
    //        { "mDataProp": "LastName" },
    //        { "mDataProp": "PhoneNumber" },
    //        { "mDataProp": "Gender" },
    //        { "mDataProp": "Email" },
    //        { "mDataProp": "Birthdate" },
    //        {
    //            "mDataProp": null,
    //            "sClass": "center",
    //            mRender: function (o) {
    //                return '<a href="" data-target="#editModal" ' +
    //                        'data-bind="click: editLink" data-toggle="modal" ' +
    //                        'class="">Edit</a> /' +
    //                        '<a href="" data-target="#removeModal"' +
    //                        'data-bind="click: editLink" data-toggle="modal" ' +
    //                        'class="">Delete</a>'
    //            }
    //        }
    //]
    });
   
}

function viewTableRowData(rowData) {
    if (rowData) {
        input_code.val(rowData.code)
        input_descs.val(rowData.descs)
        $('#ContentPlaceHolder1_va_DD_accessory_group').val(rowData.group_cd)       
    }
}

var enable_Form = function () {
    input_code.removeAttr('disabled');
    input_descs.removeAttr('disabled');
    input_dd_group_cd.attr('disabled', false);
    buttonAdd.disable(true);
};

var disable_form = function () {
    buttonAdd.disable(false);
    buttonSave.disable(true);
    buttonCancel.disable(true);

    input_code.attr('disabled', true);
    input_descs.attr('disabled', true);
    input_dd_group_cd.attr('disabled', true);
}

var update_form = function () {
    buttonAdd.disable(true);
    buttonSave.disable(false);
    buttonCancel.disable(false);

    input_code.attr('disabled', true);
    input_descs.removeAttr('disabled');
    input_dd_group_cd.attr('disabled', false);
}

var clear_form = function () {
    input_code.val('');
    input_descs.val('');
}

var delay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();
