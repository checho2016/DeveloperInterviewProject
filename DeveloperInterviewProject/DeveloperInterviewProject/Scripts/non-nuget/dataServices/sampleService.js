var packSize = packSize || {};

packSize.sampleDataService = new function (vva) {

    sendCoursesProcess = function (inputObject, callback) {
        $.ajax({
            type: "POST",
            url: "../API/SampleAPI/ProcessOrder",
            contentType: "application/json",
            data: JSON.stringify(inputObject),
            async: true,
            cache: false,
            success: function (data) {
                callback(data);
            }
        });
    };

    return {
        sendCoursesProcess: sendCoursesProcess
    };

}();