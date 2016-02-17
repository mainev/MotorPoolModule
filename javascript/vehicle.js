var webService = "MotorPoolWebService.asmx";
var vTable;
var imageUrl;

//input fields
var input_name;
var input_descs;
var input_model_yr;
var input_color;
var input_plate_no;
var input_insu_po_no;
var input_seater_cap;
var input_fuel_type;
var input_location;
var input_distance_u;
var input_fuel_u;
var input_image;

if (window.File && window.FileReader && window.FileList && window.Blob) {
    document.getElementById('image').addEventListener('change', handleFileSelect, false);
} else {
    alert('The File APIs are not fully supported in this browser.');
}



$(document).ready(function () {
    initTable();
    initInputFields();

    disableInputFields();

    vTable.on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            vTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
        disableInputFields();
        var rowData = vTable.api().rows('tr.selected').data()[0];
        viewTableRowData(rowData)
    });

});

function initInputFields() {
    input_name = $('#input_name')
    input_descs = $('#input_descs')
    input_model_yr = $('#input_model_yr')
    input_color = $('#input_color')
    input_plate_no = $('#input_plate_no')
    input_insu_po_no = $('#input_insu_po_no')
    $('#input_seater_cap').priceFormat({ prefix: '', centsLimit: 0 });
    input_seater_cap = $('#input_seater_cap')
    input_fuel_type = $('#input_fuel_type')
    input_location = $('#input_location')
    input_distance_u = $('#input_distance_u')
    input_fuel_u = $('#input_fuel_u')
    // input_image = $('#input_fuel_u')

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

function validateInputFields() {
    input_name.validate({
        required: true
    })
    input_descs.validate()
    input_model_yr.validate()
    input_color.validate()
    input_plate_no.validate({ required: true })
    input_insu_po_no.validate()
    input_seater_cap.validate()
    input_fuel_type.validate()
    input_location.validate()
    input_distance_u.validate()
    input_fuel_u.validate()
}

/*
function GetVehicleImage(plateNo) {//returns the base64 encoded image of the vehicle
    var result;
   
    $.ajax({
        type: "GET",
        url: webService + "/GetVehicleImage",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: params = {
            plate_no: JSON.stringify(plateNo)
        },
       async: false,
        success: function (response) {
            var myData = response.d;
            result = JSON.parse(myData);
        },
        error: function (msg) { alert("ERROR " + msg); }
    });
    return result;
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
*/

function InsertVehicleToDatabase(vehicle) {
    console.log("calling save vehicle")
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
        'fuel_unit': vehicle.fuel_unit,
        'image': vehicle.image
    }

    $.ajax({
        type: "POST",
        url: webService + "/InsertVehicle",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(params),
        success: function (response) {
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
    enableInputFields()
})

$("#btn_Cancel").on("click", function () {
    disableInputFields()
})

$("#btn_Save").on("click", function () {

    if (vehicleEntryValid()) {
        var vehicle = {};
        vehicle["name"] = input_name.val();
        vehicle["descs"] = input_descs.val();
        vehicle["model_year"] = input_model_yr.val();
        vehicle["color"] = input_color.val();
        vehicle["plate_no"] = input_plate_no.val();
        vehicle["insurance_policy_no"] = input_insu_po_no.val();
        vehicle["seater_capacity"] = input_seater_cap.val();
        if (input_seater_cap.val() == "") { vehicle["seater_capacity"] = 0 };
        vehicle["fuel_type"] = input_fuel_type.val();
        vehicle["location"] = input_location.val();
        vehicle["distance_unit"] = input_distance_u.val();
        vehicle["fuel_unit"] = input_fuel_u.val();
        vehicle["image"] = imageUrl;
        if (typeof imageUrl == 'undefined') { vehicle["image"] = ""; }

        InsertVehicleToDatabase(vehicle);//ajax call
        disableInputFields()
    }
    else {
        $('#div_msg').html('');
        $('#div_msg').html('<span style="color:red">Please provide valid entry ...</span>');
        $("#div_msg").hide().fadeIn('slow');
        setTimeout(function () {
            $("#div_msg").fadeOut('slow');
        }, 5000);
    }
})

function vehicleEntryValid() {
   
    var isValid = true;
    isValid = isValid && input_name.data('isValid');
    isValid = isValid && input_descs.data("isValid");
    isValid = isValid && input_model_yr.data("isValid");
    isValid = isValid && input_color.data("isValid");
    isValid = isValid && input_plate_no.data("isValid");
    isValid = isValid && input_insu_po_no.data("isValid");
    isValid = isValid && input_seater_cap.data("isValid");
    isValid = isValid && input_fuel_type.data("isValid");
    isValid = isValid && input_location.data("isValid");
    isValid = isValid && input_distance_u.data("isValid");
    isValid = isValid && input_fuel_u.data("isValid");
    return isValid
}


function handleFileSelect(evt) {
    var files = evt.target.files;
    var file = files[0];

    //console.log(file)
    if (files && file) {
        var reader = new FileReader();

        reader.onload = function (readerEvt) {
            var binaryString = readerEvt.target.result;
            imageUrl = "data:" + file.type + ";base64," + btoa(binaryString)
            var imageid = document.getElementById("imageid")
            imageid.src = imageUrl
        };
        reader.readAsBinaryString(file);
    }

};



function enableInputFields() {
    $('#input_fields input').each(function (index, obj) {
        $(this).removeAttr("disabled")
        $(this).reset()
    });

    validateInputFields();

    var imageid = document.getElementById("imageid")
    imageid.src = ""
    $("#imageid").attr("src", "images/photo.png");

    $("#btn_Add").addClass("disabled")
    $("#btn_Cancel").removeClass("disabled")
    $("#btn_Save").removeClass("disabled")
    vTable.$('tr.selected').removeClass('selected');
}


function disableInputFields() {
    $('#input_fields input').each(function (index, obj) {
        $(this).attr("disabled", true)
        $(this).reset()
    });

    var imageid = document.getElementById("imageid")
    imageid.src = ""
    $("#imageid").attr("src", "images/photo.png");

    $("#btn_Cancel").addClass("disabled")
    $("#btn_Add").removeClass("disabled")
    $("#btn_Save").addClass("disabled")
}

function viewTableRowData(rowData) {

    if (rowData != null) {
        input_name.val(rowData.name)
        input_descs.val(rowData.descs)
        input_model_yr.val(rowData.model_year)
        input_color.val(rowData.color)
        input_plate_no.val(rowData.plate_no)
        input_insu_po_no.val(rowData.insurance_policy_no)
        input_seater_cap.val(rowData.seater_capacity)
        input_fuel_type.val(rowData.fuel_type)
        input_location.val(rowData.location)

        var imageid = document.getElementById("imageid")

        $.ajax({
            type: "GET",
            url: webService + "/GetVehicleImage",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: params = {
                plate_no: JSON.stringify(rowData.plate_no)
            },
            success: function (response) {
                var myData = response.d;
                var retrievedImg = JSON.parse(myData)[0].image
                if (!(retrievedImg == "" | retrievedImg == null)) {
                    imageid.src = retrievedImg
                } else {
                    $("#imageid").attr("src", "images/photo.png");
                }
            },
            error: function (msg) { alert("ERROR " + msg); }
        });
    }




}


