(function () {
    var app = angular.module("carDetails");

    // ----------------------------------------------- CAR INFORMATION CONTROLLER --------------------------------------------------------

    app.controller("carCtrl", ['carSvc', "$uibModal", function (carSvc, $uibModal) {
        var ref = this;
        ref.years = [];
        ref.makes = [];
        ref.models = [];
        ref.years = [];
        ref.trims = [];
        ref.selected = {
            make: '',
            model: '',
            year: '',
            trim: '',
          
        }
        ref.cars = [];

        ref.getCars = function () {
            carSvc.getCars(ref.selected).then(function(data){
                ref.cars = data;
            })
        }


        // ------- Populate dropdowns with data from carSvc ----


        ref.getMakes = function () {
            carSvc.getMakes().then(function (data) {
                ref.makes = data;
            })
        }

        ref.getModels = function () {
            carSvc.getModels(ref.selected).then(function (data) {
                //ref.selected as a parameter results in a Models list that is dependent on a make selection (all models for x make)
                ref.models = data;
                ref.years = [];
                ref.trims = [];
                ref.model = '';
                ref.year = '';
                ref.trim = '';
                ref.getCars();
            })
        }

        ref.getYears = function () {
            carSvc.getYears(ref.selected).then(function (data) {
                ref.years = data;
                ref.trims = [];
                ref.year = '';
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

        // ---------- Open Modal ---------

        ref.open = function (id) {
            console.log("Id in open " + id)
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'carModal.html',
                controller: 'carModalCtrl as cm',
                resolve: {
                    cc: function () {
                        return carSvc.getCarDetails(id);
                    }
                },
                size: 'lg',
            });
        };

        ref.getMakes();
    }]);

    // ---------------------------------------- CAR MODAL CONTROLLER ------------------------------------------------------------------------

    app.controller('carModalCtrl', ['$uibModalInstance', 'cc', function ($uibModalInstance, cc) {

        var ref = this;
        ref.n = 0;
        ref.car = cc;
        console.log("The car is : "+ref.car.car);
        ref.ok = function () {
            $uibModalInstance.close();
        };

        ref.cancel = function () {
            $uibModalInstance.dismiss();
        };
    }]);
})();
