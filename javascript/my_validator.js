//validator for input only
(function ($) {

    $.fn.validate = function (options) {
        var regex;
        var isValid = false;

        var settings = $.extend({
            regex: /^$|[A-Za-z0-9() _]*[A-Za-z0-9()][A-Za-z0-9() _]*$/, 
            minLength: 0,
            maxLength: 50,
            min: 0,
            max: 2147483647,
            required: false
        }, options);

        checkDefaultValues(this, settings)

        return this.on('keypress keydown keyup', function () {

            checkDefaultValues($(this), settings)
            /*
            if (!$(this).val().match(settings.regex)) {
                $(this).parent().addClass('has-error');
                isValid = false;
            }
            else {
                $(this).parent().removeClass('has-error');
                isValid = true;
            }

            $(this).attr("isValid", isValid)*/
        })

    }

    function checkDefaultValues(element, settings) {

        var is_valid = true;
        var value = element.val();

        var regex_match;
        var min_max_length_match;
        var required_match;

        //check for regex pattern
        if (!value.match(settings.regex)) {
            regex_match = false;
        }
        else {
            regex_match = true;
        }

      //  console.log("value is: " + value.length + " min: " + (value.length >= settings.minLength))
      //  console.log("value is: " + value.length + " max: " + (value.length <= settings.maxLength))
       
        //check for input length
        if ((value.length >= settings.minLength) && (value.length <= settings.maxLength)) {
            min_max_length_match = true;
        }
        else {
            min_max_length_match = false;
        }

        //check if input is required
        if ((value.length == 0) && settings.required == true) {
            required_match = false;
        }
        else { required_match = true; }

        is_valid = is_valid && regex_match;
        is_valid = is_valid && min_max_length_match;
        is_valid = is_valid && required_match;
       

        if (!is_valid) {
            element.parent().addClass('has-error');
        }
        else { element.parent().removeClass('has-error'); }

        //add tooltip message about error 

        return element.data('isValid', is_valid)

    }

    


}(jQuery))