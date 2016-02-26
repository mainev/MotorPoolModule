

//input fields
var input_vehicle_cd,
    input_accessory_cd,
    input_accessory_group_cd,
    input_audit_user,
    input_search,
    
    buttonAdd,
    buttonSave,
    buttonCancel

$(document).ready(function () {
    initializeButtons();
    initializeBindings();
    initializeInput();
    loadDropDownList('#ContentPlaceHolder1_DD_vehicle_plate_number');    
    loadDropDownList('#ContentPlaceHolder1_DD_accessory');
})

var initializeBindings = function () {

    //ADD=============================================================
    buttonAdd.click(function () {
        buttonAdd.disable(true);
        buttonSave.disable(false);
        buttonCancel.disable(false);
        enable_Form();
        input_is_new_entry = true;
        //clear_form();
        //loadVehicleLogTable($("#ContentPlaceHolder1_DD_vehicle_plate_number option:selected").val());
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
    });
   
    $("#search").on("keyup", function () {
        delay(function () {
            loadAccessories();
        }, 500);
    })  

    $('#btnSelect').click(function () {
        multipleInsert();
    })

    $('#btnUnSelect').click(function () {
        multipleRemove();
    })

    $('th input:checkbox').click(function (e) {
        var table = $(e.target).closest('table');
        $('td input:checkbox', table).prop('checked', e.target.checked);
    });
}

var initializeInput = function () {
    input_vehicle_cd = $("#ContentPlaceHolder1_DD_vehicle_plate_number");
    input_accessory_group_cd = $("#ContentPlaceHolder1_DD_accessory_group");
    //input_accessory_cd = $("#ContentPlaceHolder1_DD_accessory");    
    input_audit_user = $('#ContentPlaceHolder1_hidden_username');
    input_search = $("#search");
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
        //disableInputFields();
        var rowData = vTable.api().rows('tr.selected').data()[0];       
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
    ajaxHelper('masterfile.asmx/getDropDownListItems', 'POST',
                parameter).done(function (data) {
        var datad = data.d;
        $.each(datad, function (key, value) {
            $(obj).append($("<option></option>").val(value.code).html(value.descs));
        });
        if (obj == '#ContentPlaceHolder1_DD_vehicle_plate_number') {
            input_vehicle_cd.bind("change", function () {
                loadVehicleAccessories();
                loadAccessories();
            });
            loadVehicleAccessories();
            loadDropDownList('#ContentPlaceHolder1_DD_accessory_group');
        } 
        else if (obj == '#ContentPlaceHolder1_DD_accessory_group') {
            input_accessory_group_cd.bind("change", function () {
                loadAccessories();
            });
            loadAccessories();
        }
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

   
    ajaxHelper('masterfile.asmx/saveAccessory', 'POST',
                JSON.stringify(params)).done(function (data) {       
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

var loadVehicleAccessories = function () {
    $('#T_Vehicle_Accessory tbody').remove();
   
    var vehicle_plate_no = input_vehicle_cd.val();

    var parameters = { vehicle_plate_no: vehicle_plate_no };    
    
    ajaxHelper('masterfile.asmx/getVehicleAccessories', 'POST',
                JSON.stringify(parameters)).done(function (data) {
        var datad = data.d;        
        $.each(datad, function (key, value) {
            $('#T_Vehicle_Accessory').append("<tr><td> <input type='checkbox' id='CBx' name='xx'  >"
                + "</td><td>" + value.accessory_group + "</td><td>" + value.accessory + "</td></tr>");
        });
    });
}

var loadAccessories = function () {
    $('#T_Accessories tbody').remove();

    var group_cd = input_accessory_group_cd.val();
    var vehicle_plate_no = input_vehicle_cd.val();
    var searchString = input_search.val();
    var parameters = {
        group_cd: group_cd,
        vehicle_plate_no: vehicle_plate_no,
        searchString: searchString
    };

    console.log('the value of parameters in loadAccessories are : ' + JSON.stringify(parameters));
    ajaxHelper('masterfile.asmx/getAccessories', 'POST',
                JSON.stringify(parameters)).done(function (data) {
                    var datad = data.d;
                    $.each(datad, function (key, value) {
                        $('#T_Accessories').append("<tr><td> <input type='checkbox' id='CBx' name='xx' > </td><td>"
                             + value.code + "</td><td>" + value.descs + "</td></tr>");
                    });
                });
}

var multipleInsert = function () {
    var items = [];
    $('#T_Accessories').find('tr').each(function () {
        var row = $(this);
        var codeColumn = row.find("td:nth-child(2)");
        var descriptionColumn = row.find("td:nth-child(3)");

        if (row.find('input[type="checkbox"]').is(':checked')) {
            $.each(codeColumn, function () {                    // Visits every single <td> element
                console.log("code : " + $(this).text());        // Prints out the text within the <td>
                code = $(this).text();
            });
            $.each(descriptionColumn, function () {             // Visits every single <td> element
                console.log("description : " + $(this).text());     // Prints out the text within the <td>
                description = $(this).text();
            });
            items.push(new item(code, description));
        }
    });
    var parameters = {
        items: items,
        vehicle_plate_no: input_vehicle_cd.val(),
        group_cd: input_accessory_group_cd.val(),
        audit_user: input_audit_user.val()
    }
    console.log("the parameters of multiple insert is : " + JSON.stringify(parameters));

    if (items.length > 0) {
        ajaxHelper('masterfile.asmx/batchInsertToVehicleAccessory', 'POST', JSON.stringify(parameters)).done(function (data) {
            loadVehicleAccessories();
            loadAccessories();
            $('#div_msg').html('');
            $('#div_msg').html('<span style="color:green">Accessory Successfully Added..</span>');
            $("#div_msg").hide().fadeIn('slow');
            setTimeout(function () {
                $("#div_msg").fadeOut('slow');
            }, 5000);
        });
    } else {
        $('#div_msg').html('');
        $('#div_msg').html('<span style="color:red">Please Select Accessory to add ..</span>');
        $("#div_msg").hide().fadeIn('slow');
        setTimeout(function () {
            $("#div_msg").fadeOut('slow');
        }, 5000);
    }
}

var multipleRemove = function () {
    var items = [];
    $('#T_Vehicle_Accessory tbody').find('tr').each(function () {
        var row = $(this);
        var codeColumn = row.find("td:nth-child(2)");
        var descriptionColumn = row.find("td:nth-child(3)");

        if (row.find('input[type="checkbox"]').is(':checked')) {
            $.each(codeColumn, function () {                    // Visits every single <td> element
                console.log("code : " + $(this).text());        // Prints out the text within the <td>
                code = $(this).text();
            });
            $.each(descriptionColumn, function () {             // Visits every single <td> element
                console.log("description : " + $(this).text());     // Prints out the text within the <td>
                description = $(this).text();
            });
            items.push(new item(code, description));
        }
    });
    var parameters = {
        items: items,
        vehicle_plate_no: input_vehicle_cd.val(),
        audit_user: input_audit_user.val()
    }
    console.log("the parameters of multiple remove is : " + JSON.stringify(parameters));

    if (items.length > 0) {
        ajaxHelper('masterfile.asmx/batchRemoveFromVehicleAccessory', 'POST', JSON.stringify(parameters)).done(function (data) {
            loadVehicleAccessories();
            loadAccessories();
            $('#div_msg').html('');
            $('#div_msg').html('<span style="color:red">Accessory Successfully Removed..</span>');
            $("#div_msg").hide().fadeIn('slow');
            setTimeout(function () {
                $("#div_msg").fadeOut('slow');
            }, 5000);
        });
    } else {
        $('#div_msg').html('');
        $('#div_msg').html('<span style="color:red">Please Select Accessory to remove ..</span>');
        $("#div_msg").hide().fadeIn('slow');
        setTimeout(function () {
            $("#div_msg").fadeOut('slow');
        }, 5000);
    }
}

var enable_Form = function () {  
    input_vehicle_cd.attr('disabled', false);
    input_accessory_cd.attr('disabled', false);
    input_accessory_group_cd.attr('disabled', false);
    buttonAdd.removeClass('disabled');
};

var disable_form = function () {
    buttonAdd.disable(false);
    buttonSave.disable(true);
    buttonCancel.disable(true);

    input_vehicle_cd.attr('disabled', true);
    input_accessory_cd.attr('disabled', true);
    input_accessory_group_cd.attr('disabled', true);
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

var item = function (code, descs) {
    this.code = code;
    this.descs = descs;   
}
