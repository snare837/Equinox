jQuery.validator.addMethod("agerange", function (value, element, params) {
    if (value === '') return false;

    var dateToCheck = new Date(value);
    if (dateToCheck === "Invalid Date") return false;

    var today = new Date();
    var age = today.getFullYear() - dateToCheck.getFullYear();
    var m = today.getMonth() - dateToCheck.getMonth();

    if (m < 0 || (m === 0 && today.getDate() < dateToCheck.getDate())) {
        age--;
    }

    return age >= params.min && age <= params.max;
});

jQuery.validator.unobtrusive.adapters.add("agerange", ["min", "max"], function (options) {
    options.rules["agerange"] = {
        min: parseInt(options.params.min),
        max: parseInt(options.params.max)
    };
    options.messages["agerange"] = options.message;
});
