(function () {
    angular.module('carDetails').factory('carSvc', ['$http', function ($http) {
        var service = {};


        service.getCarDetails = function (id) {
            return $http.post('/api/Car/GetCarDetails', { id: id }).then(function (response) {
                return response.data;
            })
        }

        //Get Cars
        service.getCars = function (selected) {
            return $http.post('/api/Car/GetCars', selected).then(function (response) {
                return response.data;
            })
        }


        //Get Makes
        service.getMakes = function () {

            return $http.post('/api/car/GetUniqueMakes').then(function (response) {
                return response.data;
            })

            //  Use this code if not making an http request, but working from hard code
            //var deferred = $q.defer();
            //switch (year) {
            //    case 1999:
            //        deferred.resolve(['Ford', 'Hyundai', 'Dodge', 'Acura'])
            //        break;
            //    case 2000:
            //    case 2002:
            //    case 2003:
            //    case 2004:
            //    case 2005:
            //    case 2006:
            //    case 2007:
            //}
            //return deferred.promise;
        };

        //Get Models
        service.getModels = function (selected) {

            return $http.post('/api/car/getUniqueModelsByMake', selected).then(function (response) {
                return response.data;
            })

        };

        // Get Years
        service.getYears = function (selected) {

            return $http.post('/api/Car/GetUniqueYearsByMakeModel', selected).then(function (response) {  //  .post does not include caching (.get does)
                return response.data; //.data removes headers, post information, etc and leaves just data
            })

        }

        //Get Trims
        service.getTrims = function (selected) {

            return $http.post('/api/car/getUniqueTrimsByMakeModelYear', selected).then(function (response) {
                return response.data;
            })

            //var deferred = $q.defer();
            //switch (model) {
            //    case 'Focus':
            //        deferred.resolve(['XS', 'SL', 'XM', 'XL'])
            //        break;
            //    case 'Fiesta':
            //    case 'Mustang':
            //    case 'Expedition':
            //    case 'F - 150':
            //}
        };

        //service.getTransmission = function () {
        //    var deferred = $q.defer();
        //    deferred.resolve(['manual', 'automatic'])
        //    return deferred.promise;
        //}

        //service.getDrive = function () {
        //    var deferred = $q.defer();
        //    deferred.resolve(['4WD', 'front', 'back', 'AWD'])
        //    return deferred.promise;
        //}

        return service;
    }]);
})();