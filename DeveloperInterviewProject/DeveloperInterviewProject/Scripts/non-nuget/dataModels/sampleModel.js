var packSize = packSize || {};

$(document).ready(function () {
    // define the ViewModel
    packSize.sampleVm = function () {

        //observables
        inputText = ko.observable("");

        //functions
        getCoursesPath = function () {

           
        };

        return {
            inputText: inputText,
            getCoursesPath: getCoursesPath
        }
    }

    // apply ko bindings
    setTimeout(function () {
        ko.applyBindings(packSize.sampleVm(), document.getElementById("#sampleModelRange"));
    }, 500)
    

});