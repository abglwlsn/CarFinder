(function () {
    var car = angular.module("carDetails");
    car.controller("carCtrl", ['carSvc', function (carSvc) {
        var ref = this;
        ref.years = [];
        ref.makes = [];
        ref.models = [];
        ref.trims = [];
        //ref.transmission = [];
        //ref.drive = [];
        ref.selected = {
            year: '',
            make: '',
            model: '',
            trim: '',
          
        }
        ref.cars = [];

        ref.getCars = function (selected) {
            carSvc.getCars(selected).then(function(){
                ref.cars = data;
            })
        }


        // ------- Populate dropdowns with data from carSvc ----

        ref.getYears = function () {
            carSvc.getYears().then(function (data) {
                ref.years = data;

            })
        }

        ref.getMakes = function () {
            alert('inside getMakes');
            carSvc.getMakes(ref.selected).then(function (data) {
                //ref.selected.year as a parameter results in a Makes list that is dependent on a year selection (all makes for x year)
                ref.makes = data;
                ref.models = [];
                ref.trims = [];
                ref. make = '';
                ref.model = '';
                ref.trim = '';
                ref.getCars();
            })
        }

        ref.getModels = function () {
            carSvc.getModels(ref.selected).then(function (data) {
                ref.models = data;
                ref.trims = [];
                ref.model = '';
                ref.trim = '';
                ref.getCars();
            })
        }

        ref.getTrims = function () {
            carSvc.getTrims(ref.selected).then(function (data) {
                ref.trims = data;
                ref.getCars();
            })
        }

        //ref.getTransmission = function () {
        //    carSvc.getTransmission(ref.selected.transmission).then(function (data) {
        //        ref.transmission = data;
        //    })
        //}

        //ref.getDrive = function () {
        //    carSvc.getDrive(ref.selected.drive).then(function (data) {
        //        ref.drive = data;
        //    })
        //}



        ref.getYears();
    }]);
})();