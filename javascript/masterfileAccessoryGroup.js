
var input_code,
    input_descs,
    input_audit_user,
    input_is_new_entry,
    buttonAdd,
    buttonSave,
    buttonCancel

$(document).ready(function () {
    initializeButtons();
    initializeBindings();
    initializeInput();
    //loadDropDownList('#ContentPlaceHolder1_DD_accessory_group');
})

var initializeBindings = function () {

    //initialize datatable
    initTable();
    //initTableOnClick();

    //ADD=============================================================
    buttonAdd.click(function () {
        buttonAdd.disable(true);
        buttonSave.disable(false);
        buttonCancel.disable(false);
        enable_Form();
        input_is_new_entry = true;

        clear_form();
        //loadVehicleLogTable($("#ContentPlaceHolder1_DD_vehicle_plate_number option:selected").val());
    });

    //SAVE============================================================
    buttonSave.click(function () {
        console.log('button save is clicked!');
        saveAccessoryGroup();
        disable_form();
        //insertVehicleLog();
    });

    //CANCEL==========================================================
    buttonCancel.click(function () {
        disable_form();
    });

    //initialize datatable
    initTable();
    initTableOnClick();

}

var initializeInput = function () {
    input_code = $("#txt_code");
    input_descs = $("#txt_description");
    input_audit_user = $('#ContentPlaceHolder1_hidden_username');
    input_is_new_entry = true;
}

var initializeButtons = function () {
    buttonAdd = $('#btnAdd');
    buttonSave = $('#btnSave');
    buttonCancel = $('#btnCancel');
}


var initTable = function () {

    vTable = $('#T_Accessory_Group').dataTable({

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
        "bServerSide": false,
        "sAjaxSource": "masterfile.asmx/getAccessoryGroups",
        // "aaData": tblData,
        "fnServerData": function (sSource, aoData, fnCallback) {
            $.ajax({
                "dataType": 'json',
                "contentType": "application/json; charset=utf-8",
                "type": "GET",
                "url": sSource,
                "data": {},
                "success": function (msg) {
                    var json = jQuery.parseJSON(msg.d);
                    console.log("the data that comes bck from datatable is : " + JSON.stringify(msg));
                    fnCallback(json);
                }
            })
        },

        "aoColumns": [
            { "mDataProp": "code", "sTitle": "Code", "bSearchable": true },
            { "mDataProp": "descs", "sTitle": "Description" }
        ]
    });
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
        //disableInputFields();
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

var saveAccessoryGroup = function () {
    var code = input_code.val();
    var descs = input_descs.val();
    var audit_user = input_audit_user.val();
    var is_new_entry = input_is_new_entry;
    var params = {
        code: code,
        descs: descs,
        audit_user: audit_user,
        is_new_entry: is_new_entry
    }

    console.log('the parameters of saveAccessoryGroup are : ' + JSON.stringify(params));
    ajaxHelper('masterfile.asmx/saveAccessoryGroup', 'POST', JSON.stringify(params)).done(function (data) {
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
        //loadVehicleLogTable(vehicle_plate_no);
    });
    //clearFields();


}


function viewTableRowData(rowData) {
    if (rowData) {
        input_code.val(rowData.code)
        input_descs.val(rowData.descs)
    }
}

var enable_Form = function () {
    input_code.removeAttr('disabled');
    input_descs.removeAttr('disabled');
    buttonAdd.removeClass('disabled');
    buttonAdd.disable(true);
};

var disable_form = function () {
    buttonAdd.disable(false);
    buttonSave.disable(true);
    buttonCancel.disable(true);

    input_code.attr('disabled', true);
    input_descs.attr('disabled', true);
}

var update_form = function () {
    buttonAdd.disable(true);
    buttonSave.disable(false);
    buttonCancel.disable(false);

    input_code.attr('disabled', true);
    input_descs.removeAttr('disabled');
}

var clear_form = function () {
    input_code.val('');
    input_descs.val('');
}