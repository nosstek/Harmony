angular.module('MyApp')
    .controller('DropDownList', function($scope, LocationService) {

        $scope.DepartmentId = null;
        $scope.FieldOfStudyId = null;
        $scope.DepartmentList = null;
        $scope.FieldOfStudyList = null;

        $scope.StateTextToShow = "Wybierz kierunek";
        $scope.Result = "";

        LocationService.GetDepartment().then(function(d) {
            $scope.DepartmentList = d.data;
        }, function(error) {
            alert('Błąd!');
        });

        //Funkcja wywoływana po wybraniu wydziału

        //Wczytanie kierunków z bazy danych
        $scope.GetFieldOfStudy()
        {
            $scope.FieldOfStudyId = null;
            $scope.FieldOfStudyList = null;
            $scope.FieldOfStudyTextToShow = "Proszę czekać...";

            LocationService.GetFieldOfStudy($scope.DepartmentId).then(function(d) {
                $scope.FieldOfStudyList = d.data;
                $scope.FieldOfStudyTextToShow = "Wybierz kierunek";
            }, function(error) {
                alert('Błąd!');
            });
        }

        //Wyświetlenie wyników
        $scope.ShowResult = function() {
            $scope.Result = "Id wydziału: " + $scope.DepartmentId + "Id kierunku: " + $scope.FieldOfStudyId;
        }

    })
    .factory('LocationService', function($http) {
        var fac = {};
        fac.GetDepartment = function() {
            return $http.get('/Data/GetDepartments')
        }
        fac.GetFieldOfStudy = function(departmentId) {
            return $http.get('/Data/GetFieldsOfStudy?departmentId=' + departmentId)
        }
        return fac;
    })