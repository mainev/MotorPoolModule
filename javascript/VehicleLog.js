﻿

$(document).ready(function () {
    initializeBindings();
    loadDropDownListVehicleItems('#ContentPlaceHolder1_DD_vehicle_plate_number'); 
    loadDropDownListVehicleItems('#ContentPlaceHolder1_DD_driver_username');
})

var initializeBindings = function () {

    //ADD=============================================================
    $('#btnAdd').click(function () {
        $('#btnAdd').disable(true);
        $('#btnSave').disable(false);
        $('#btnCancel').disable(false);
        $.fn.enable_Form(0);
        getCheckedRadio();
        loadVehicleLogTable($("#ContentPlaceHolder1_DD_vehicle_plate_number option:selected").val());
    });

    //SAVE============================================================
    $('#btnSave').click(function () {
        console.log('button save is clicked!');
        insertVehicleLog();
    });

    //CANCEL==========================================================
    $('#btnCancel').click(function () {
        $('#btnAdd').disable(false);
        $('#btnSave').disable(true);
        $('#btnCancel').disable(true);

        $('#ContentPlaceHolder1_odometer').attr('disabled', true);
        $('#ContentPlaceHolder1_last_fuel_price').attr('disabled', true);
        $('#ContentPlaceHolder1_date').attr('disabled', true);
        $('#ContentPlaceHolder1_total_gas').attr('disabled', true);
        $('#ContentPlaceHolder1_total_cost').attr('disabled', true);
        $('#ContentPlaceHolder1_RB_full_tank').find('*').each(function () {
            $(this).attr('disabled', true);
        });
        $("#ContentPlaceHolder1_DD_driver_username").attr('disabled', true);
        $("#ContentPlaceHolder1_DD_vehicle_plate_number").attr('disabled', true);

    });
    
    $("#ContentPlaceHolder1_DD_vehicle_plate_number").bind("change", function () {
        loadVehicleLogTable($("#ContentPlaceHolder1_DD_vehicle_plate_number option:selected").val());
    });

    $.fn.enable_Form = function (edit_mode) {

        $('#ContentPlaceHolder1_odometer').removeAttr('disabled');
        $('#ContentPlaceHolder1_last_fuel_price').removeAttr('disabled');
        $('#ContentPlaceHolder1_date').removeAttr('disabled');

        $('#ContentPlaceHolder1_total_gas').removeAttr('disabled');
        $('#ContentPlaceHolder1_total_cost').removeAttr('disabled');

        
        $('#ContentPlaceHolder1_RB_full_tank').find('*').each(function () {
            $(this).removeAttr("disabled");
        });
        $("#ContentPlaceHolder1_DD_driver_username").attr('disabled', false);
        $("#ContentPlaceHolder1_DD_vehicle_plate_number").attr('disabled', false);

        $("#btnAddItem").removeClass('disabled');
    };

    var today = new Date();
    $('#ContentPlaceHolder1_date').datepicker({
        maxDate: new Date(today),
        inline: true,
        showOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        changeMonth: true,
        changeYear: true,
        yearRange: '-5:+20'
    });

    $('#ContentPlaceHolder1_odometer').priceFormat({
        prefix: '',
        centsLimit: 0
    });
    
    $('#ContentPlaceHolder1_total_gas').priceFormat({
        prefix: '',
    });

    $('#ContentPlaceHolder1_total_cost').priceFormat({
        prefix: '₱',

    });
}

var insertVehicleLog = function () {
    var vehicle_plate_no = $("#ContentPlaceHolder1_DD_vehicle_plate_number option:selected").val();
    var odometer = $('#ContentPlaceHolder1_odometer').val();
    if ($("#ContentPlaceHolder1_last_fuel_price").val())
        var last_fuel_price = $("#ContentPlaceHolder1_last_fuel_price").val();
    var total_gas = $("#ContentPlaceHolder1_total_gas").val();
    var total_cost1 = $("#ContentPlaceHolder1_total_cost").val();
    
    var total_cost = Number(total_cost1.replace(/[^0-9\.]+/g, ""));
    var date = $("#ContentPlaceHolder1_date").val();      
    var full_tank = $('#ContentPlaceHolder1_RB_full_tank input:checked').val();
    var result = $("#ContentPlaceHolder1_last_fuel_price").val();
    var driver_username = $("#ContentPlaceHolder1_DD_driver_username option:selected").val();
    var audit_user = $('#ContentPlaceHolder1_audit_user').val();
    var audit_date = $('#ContentPlaceHolder1_audit_date').val();

    var vehicleLogDTO = {
        vehicle_plate_no : vehicle_plate_no,
        odometer : odometer,
        last_fuel_price: last_fuel_price,
        total_gas : total_gas,
        total_cost: total_cost,
        date: date,
        full_tank: full_tank,
        //result: result,
        driver_username: driver_username,
        audit_user: audit_user,
        audit_date: audit_date
    }
    var param = { vehicleLogDTO: vehicleLogDTO };
  
    console.log('the params are : ' + result + " : " + JSON.stringify(param));
    ajaxHelper('masterfile.asmx/insertVehicleLog', 'POST', JSON.stringify(param)).done(function (data) {
        console.log('the received data is : ' + JSON.stringify(data));
        datad = data.d;
        var fullTank = datad.full_tank;
   
        var total_cost = datad.total_cost;
        var total_gas = datad.total_gas;
        console.log('The last fuel price is ' + total_cost + ' : ' + total_gas);
        $('#ContentPlaceHolder1_last_fuel_price').val(total_cost / total_gas);
     
        loadVehicleLogTable($("#ContentPlaceHolder1_DD_vehicle_plate_number option:selected").val());
        //$('#T_VehicleLog').prepend("<tr><td> " + datad.vehicle_plate_no +"</td> <td>"
        //                                      + datad.odometer + "</td><td>"
        //                                      + datad.last_fuel_price + "</td><td>"
        //                                      + datad.total_gas + "</td><td>"
        //                                      + datad.total_cost + "</td><td>"
        //                                      + datad.date + "</td><td>"
        //                                      + fullTank + "</td><td>"
        //                                      + datad.result + "</td><td>"                                              
        //                                      + datad.driver_username + "</td></tr>");
    });
    clearFields();
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

jQuery.fn.extend({
    disable: function (state) {
        return this.each(function () {
            var $this = $(this);
            $this.toggleClass('disabled', state);
        });
    }
});

function getCheckedRadio() {
    var radioButtons = document.getElementsByName("#ContentPlaceHolder1_RB_full_tank");
    for (var x = 0; x < radioButtons.length; x++) {
        if (radioButtons[x].checked) {
            alert("You checked " + radioButtons[x].id + " which has the value " + radioButtons[x].value);
        }
    }
}

var loadDropDownListVehicleItems = function (obj) {
    var parameter = '{ Obj: "' + obj + '"}';
    ajaxHelper('masterfile.asmx/getDropDownListItems', 'POST', parameter).done(function (data) {
        var datad = data.d;
        $.each(datad, function (key, value) {
            $(obj).append($("<option></option>").val(value.code).html(value.descs));
        });   
    });
}

var loadVehicleLogTable = function(vehicle_plate_no){
    var parameter = '{ vehicle_plate_no: "' + vehicle_plate_no + '"}';

    console.log('the vehicle_plate_no is : ' + vehicle_plate_no );
    ajaxHelper('masterfile.asmx/getVehicleLogs', 'POST', parameter).done(function (data) {
        var datad = data.d;
        console.log("the datad from getVehicleLogs are : " + JSON.stringify(datad));
        //$.each(datad, function (key, value) {
        //    $(obj).append($("<option></option>").val(value.code).html(value.descs));
        //}); 
        $('#ContentPlaceHolder1_last_fuel_price').val('');
        if (datad.length >0) {
            var total_cost = datad[0].total_cost;
            var total_gas = datad[0].total_gas;
            console.log('The last fuel price is ' + total_cost + ' : ' + total_gas );
            $('#ContentPlaceHolder1_last_fuel_price').val(total_cost/total_gas);
        }

        $('#T_VehicleLog tbody').remove();
        $.each(datad, function (key, value) {
            $('#T_VehicleLog').append("<tr><td> " + value.vehicle_plate_no + "</td> <td>"
                                                  + value.odometer + "</td><td>"
                                                  + value.last_fuel_price + "</td><td>"
                                                  + value.total_gas + "</td><td>"
                                                  + value.total_cost + "</td><td>"
                                                  + value.date + "</td><td>"
                                                  + value.full_tank + "</td><td>"
                                                  + value.result + "</td><td>"
                                                  + value.driver_username + "</td></tr>");
        });
    });
}

var clearFields = function () {
   $('#ContentPlaceHolder1_odometer').val('');
   $("#ContentPlaceHolder1_total_gas").val('');
   $("#ContentPlaceHolder1_total_cost").val('');
   $("#ContentPlaceHolder1_date").val('');      
}