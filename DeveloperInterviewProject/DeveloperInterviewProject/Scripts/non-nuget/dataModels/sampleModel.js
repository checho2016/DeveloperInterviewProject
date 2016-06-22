var packSize = packSize || {};

$(document).ready(function () {
    // define the ViewModel
    packSize.sampleVm = function () {

        //observables
        inputText = ko.observable("Introduction to Paper Airplanes: \
                                   Advanced Throwing Techniques: Introduction to Paper Airplanes \
                                   History of Cubicle Siege Engines: Rubber Band Catapults 101 \
                                   Advanced Office Warfare: History of Cubicle Siege Engines \
                                   Rubber Band Catapults 101: \
                                   Paper Jet Engines: Introduction to Paper Airplanes \
                                 ");

        //inputText = ko.observable("Intro to Arguing on the Internet: Godwin’s Law \
        //                           Understanding Circular Logic: Intro to Arguing on the Internet \
        //                           Godwin’s Law: Understanding Circular Logic \
        //                         ");

        coursesResult = ko.observable("");

        //functions
        getCoursesPath = function () {
            packSize.sampleDataService.sendCoursesProcess(inputText(), function(data) {
                coursesResult(data);
            });
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