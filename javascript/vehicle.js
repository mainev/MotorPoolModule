var webService = "MotorPoolWebService.asmx";
var vTable;
/*
$.getScript("javascript/dataTables.ReloadAjax.js", function () {

   console.log("js loaded successfully")

});*/

$(document).ready(function () {
    initTable();
});


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
            { "mDataProp": "name", "sTitle": "Name"},
            { "mDataProp": "descs", "sTitle": "Description" },
            { "mDataProp": "model_year", "sTitle": "Model - Year" },
            { "mDataProp": "color", "sTitle": "Color" },
            { "mDataProp": "plate_no", "sTitle": "Plate No." },
            { "mDataProp": "insurance_policy_no", "sTitle": "Insurance Pol. No." },
            { "mDataProp": "seater_capacity", "sTitle": "Seater Capacity" },
            { "mDataProp": "fuel_type", "sTitle": "Fuel Type" },
            { "mDataProp": "location", "sTitle": "Location" },
              { "mDataProp": "audit_date", "sTitle": "Audit Date" , "visible": false},
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
    vehicle["name"] = $("#name_entry").val();
    vehicle["descs"] = $("#descs_entry").val();
    vehicle["model_year"] = $("#model_year_entry").val();
    vehicle["color"] = $("#color_entry").val();
    vehicle["plate_no"] = $("#plate_no_entry").val();
    vehicle["insurance_policy_no"] = $("#insurance_po_entry").val();
    vehicle["seater_capacity"] = $("#seater_cap_entry").val();
    vehicle["fuel_type"] = $("#fuel_type_entry").val();
    vehicle["location"] = $("#location_entry").val();
    vehicle["distance_unit"] = $("#distance_unit_entry").val();
    vehicle["fuel_unit"] = $("#fuel_unit_entry").val();

    InsertVehicle(vehicle);//ajax call
    $("#VehicleModal").modal('hide');
});

