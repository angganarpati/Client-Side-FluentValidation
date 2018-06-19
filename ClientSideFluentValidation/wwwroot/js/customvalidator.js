'use strict';

$.validator.setDefaults({ ignore: '' });
var customval = function () { };
var conf = 'MM/DD/YYYY';
customval.is = function (value1, value2, operator, isnullable) {
    if (isnullable) {
        var isNullish = function (input) {
            return input === null || input === undefined || input === "";
        };

        var value1nullish = isNullish(value1);
        var value2nullish = isNullish(value2);

        if (value1nullish && !value2nullish || value2nullish && !value1nullish)
            return true;
    }

    var isNumeric = function (input) {
        return !isNaN(input);
    };

    var isDate = function (input) {
        return moment(input, conf, true).isValid();
    };

    var isBool = function (input) {
        return input === true || input === false || input === "true" || input === "false";
    };

    if (isDate(value1)) {
        value1 = moment(value1, conf).toDate();
        value2 = moment(value2, conf).toDate();

        if (isNaN(value1.getTime()) || isNaN(value2.getTime())) {
            return false;
        }
    }
    else if (isBool(value1)) {
        if (value1 === "false") value1 = false;
        if (value2 === "false") value2 = false;
        value1 = !!value1;
        value2 = !!value2;
    }
    else if (isNumeric(value1)) {
        value1 = parseFloat(value1);
        value2 = parseFloat(value2);

        if (isNaN(value1) || isNaN(value2)) {
            return false;
        }
    }

    switch (operator) {
        case "greaterthan": return value1 > value2;
        case "lessthan": return value1 < value2;
        case "greaterthanorequal": return value1 >= value2;
        case "lessthanorequal": return value1 <= value2;
    }
    return false;
};

customval.findnullable = function (element) {
    if (element.attributes.isnullable !== undefined) {
        return element.attributes.isnullable.value;
    }
    return undefined;
};

function getComparerElement(element) {
    return $("#" + element.attributes.field.value)[0];
}

(function ($) {
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, conf, true).isValid();
    };

    $.validator.unobtrusive.adapters.add('greaterthanorequal');

    $.validator.addMethod('greaterthanorequal',
        function (value, element, params) {
            var isnullable = false;
            var compareelement = getComparerElement(element);

            var val1nullable = customval.findnullable(element);
            var val2nullable = customval.findnullable(compareelement);

            if (val1nullable === "True" || val2nullable === "True") {
                isnullable = true;
            }
            $.validator.messages.greaterthanorequal = params;
            return customval.is(value, compareelement.value, "greaterthanorequal", isnullable);
        }
    );

    $.validator.unobtrusive.adapters.add('greaterthan');

    $.validator.addMethod('greaterthan',
        function (value, element, params) {
            var isnullable = false;
            var compareelement = getComparerElement(element);

            var val1nullable = customval.findnullable(element);
            var val2nullable = customval.findnullable(compareelement);

            if (val1nullable === "True" || val2nullable === "True") {
                isnullable = true;
            }
            $.validator.messages.greaterthan = params;
            return customval.is(value, compareelement.value, "greaterthan", isnullable);
        }
    );

    $.validator.unobtrusive.adapters.add('lessthan');
    $.validator.addMethod('lessthan',
        function (value, element, params) {
            var isnullable = false;
            var compareelement = getComparerElement(element);

            var val1nullable = customval.findnullable(element);
            var val2nullable = customval.findnullable(compareelement);

            if (val1nullable === "True" || val2nullable === "True") {
                isnullable = true;
            }
            $.validator.messages.lessthan = params;
            return customval.is(value, compareelement.value, "lessthan", isnullable);
        }
    );

    $.validator.unobtrusive.adapters.add('lessthanorequalto');
    $.validator.addMethod('lessthanorequalto',
        function (value, element, params) {
            var isnullable = false;
            var compareelement = getComparerElement(element);

            var val1nullable = customval.findnullable(element);
            var val2nullable = customval.findnullable(compareelement);

            if (val1nullable === "True" || val2nullable === "True") {
                isnullable = true;
            }
            $.validator.messages.lessthanorequalto = params;
            return customval.is(value, compareelement.value, "lessthanorequalto", isnullable);
        }
    );
})(jQuery);