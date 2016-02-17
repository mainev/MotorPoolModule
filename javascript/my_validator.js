//validator for input only
(function ($) {

    $.fn.reset = function () {
        this.parent().removeClass('has-error');
        this.val(null)
        return this.data('isValid', true)
    },

    $.fn.validate = function (options) {
        var regex;
        var isValid = false;

        var settings = $.extend({
            regex: /^[A-Za-z0-9() _-]*$/, //regex to match alphabets,numbers, and other special characters only
            minLength: 0,
            maxLength: 50,
          //  min: 0,
           // max: 2147483647,
            num: false,
            required: false


        }, options);


        checkDefaultValues(this, settings)

        return this.on('keypress keydown keyup', function () {//revised this
            checkDefaultValues($(this), settings)
        })

        //private function
        function checkDefaultValues(element, settings) {
            var is_valid = true;
            var value = element.val();

            var regex_match;
            var min_max_length_match;
            var required_match;

            //check for regex pattern
            regex_match = (value.match(settings.regex))

            //check for input length
            min_max_length_match = ((value.length >= settings.minLength) && (value.length <= settings.maxLength));
           
            //check if input is required
            required_match =   !((value.length == 0) && settings.required == true);

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

    }



}(jQuery))