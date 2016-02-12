var webService = "MotorPoolWebService.asmx";
var vTable;

/*
$.getScript("Scripts/my_validator_plugin.js", function () {
   console.log("js loaded successfully")
});
*/
$(document).ready(function () {
    initTable();
    initInputFields();


});

var name_entry;
var descs_entry;
var model_year_entry;
var color_entry;
var plate_no_entry;
var insurance_po_entry;
var seater_cap_entry;
var fuel_type_entry;
var location_entry;
var distance_unit_entry;
var fuel_unit_entry;

function initInputFields() {
    name_entry = $('#name_entry').validate({
        minLength : 3,
        required : true
    })
    descs_entry = $('#descs_entry').validate({
        required: false
    })
    model_year_entry = $('#model_year_entry').validate({
        required: true
    })

    color_entry = $('#color_entry').validate({
        required: false
    })

    plate_no_entry = $('#plate_no_entry').validate({
        required: true
    })

    insurance_po_entry = $('#insurance_po_entry').validate({
        required: false
    })

    $('#seater_cap_entry').priceFormat({//add validation for seater capacity
        prefix: '',
        centsLimit: 0
    });

    seater_cap_entry = $('#seater_cap_entry').validate({
        required: false
    })

    fuel_type_entry = $('#fuel_type_entry').validate({
        required: false
    })

    location_entry = $('#location_entry').validate({
        required: false
    })

    distance_unit_entry = $('#distance_unit_entry').validate({
        required: false
    })

    fuel_unit_entry = $('#fuel_unit_entry').validate({
        required: false
    })

}

function initTable() {
    vTable = $('#tbl_vehicle').dataTable({
        "bLengthChange": false,
        "responsive": true,
        "filter": false,
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
        "sAjaxSource": webService + "/GetVehicleList",
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
                    fnCallback(json);
                }
            })
        },
        "aoColumns": [
            { "mDataProp": "name", "sTitle": "Name" },
            { "mDataProp": "descs", "sTitle": "Description" },
            { "mDataProp": "model_year", "sTitle": "Model - Year" },
            { "mDataProp": "color", "sTitle": "Color" },
            { "mDataProp": "plate_no", "sTitle": "Plate No." },
            { "mDataProp": "insurance_policy_no", "sTitle": "Insurance Pol. No." },
            { "mDataProp": "seater_capacity", "sTitle": "Seater Capacity" },
            { "mDataProp": "fuel_type", "sTitle": "Fuel Type" },
            { "mDataProp": "location", "sTitle": "Location" }

        ]
    });
}

function GetVehicleList() {//returns list of vehicles in json format
    var result;
    var params = {}
    $.ajax({
        type: "GET",
        url: webService + "/GetVehicleList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {},
        async: false,
        success: function (response) {
            var myData = response.d;
            result = JSON.parse(myData);
        },
        error: function (msg) { alert("ERROR " + msg); }
    });
    return result;
}

function InsertVehicle(vehicle) {
    var result;
    var params = {
        'name': vehicle.name,
        'descs': vehicle.descs,
        'model_year': vehicle.model_year,
        'color': vehicle.color,
        'plate_no': vehicle.plate_no,
        'insurance_policy_no': vehicle.insurance_policy_no,
        'seater_capacity': vehicle.seater_capacity,
        'fuel_type': vehicle.fuel_type,
        'location': vehicle.location,
        'distance_unit': vehicle.distance_unit,
        'fuel_unit': vehicle.fuel_unit
    }

    $.ajax({
        type: "POST",
        url: webService + "/InsertVehicle",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(params),
        success: function (response) {
            //   vTable.fnDraw();
            //   vTable.fnReloadAjax();
            vTable.api().ajax.reload();
            $('#div_msg').html('');
            $('#div_msg').html('<span style="color:green">Vehicle Added ...</span>');
            $("#div_msg").hide().fadeIn('slow');
            setTimeout(function () {
                $("#div_msg").fadeOut('slow');
            }, 5000);
        },
        error: {
            //add exception handler here
        }
    });
    return result;
}


$("#btn_Add").on("click", function () {
    $("#VehicleModal").modal('show')
})

$("#btn_AddRowItem").on("click", function (e) {
    
    var vehicle = {};
    var isValid = true;
    vehicle["name"] = name_entry.val();
    vehicle["descs"] = descs_entry.val();
    vehicle["model_year"] = model_year_entry.val();
    vehicle["color"] = color_entry.val();
    vehicle["plate_no"] = plate_no_entry.val();
    vehicle["insurance_policy_no"] = insurance_po_entry.val();
    vehicle["seater_capacity"] = seater_cap_entry.val();
    vehicle["fuel_type"] = fuel_type_entry.val();
    vehicle["location"] = location_entry.val();
    vehicle["distance_unit"] = distance_unit_entry.val();
    vehicle["fuel_unit"] = fuel_unit_entry.val();

    isValid = isValid && name_entry.data('isValid');
    isValid = isValid && descs_entry.data("isValid");
    isValid = isValid && model_year_entry.data("isValid");
    isValid = isValid && color_entry.data("isValid");
    isValid = isValid && plate_no_entry.data("isValid");
    isValid = isValid && insurance_po_entry.data("isValid");
    isValid = isValid && seater_cap_entry.data("isValid");
    isValid = isValid && fuel_type_entry.data("isValid");
    isValid = isValid && location_entry.data("isValid");
    isValid = isValid && distance_unit_entry.data("isValid");
    isValid = isValid && fuel_unit_entry.data("isValid");
  
   // console.log("OvERALL:" + isValid)
    if (isValid) {
        InsertVehicle(vehicle);//ajax call
        $("#VehicleModal").modal('hide');
    }
    else {
        $('#div_msg').html('');
        $('#div_msg').html('<span style="color:red">Please provide the following entry ...</span>');
        $("#div_msg").hide().fadeIn('slow');
        setTimeout(function () {
            $("#div_msg").fadeOut('slow');
        }, 5000);
    }
});

