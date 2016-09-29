app.controller('FileController', ['$scope', '$q', '$http', function ($scope, $q, $http) {
    $scope.serviceFunc = getFiles = function (path) {
       
        $scope.path = path;
        $http.get('/api/files?path=' + path).then(function (response) {
            $scope.files = response.data;
        });

        $scope.getFilesBySize(path);
    };
    $scope.getFiles = function (path) {
        $scope.serviceFunc(path).then(
            function (response) {
                $scope.files = response.data;             
            },
            function (error) {
                $scope.title = error;
            });
    }

    $scope.serviseFuncDivideBySize = getFilesBySize = function (path) {
        $http.get('/api/files/getfilesbysize?path=' + path).then(function (response) {
            $scope.filesCount = response.data;
        });
    };
    $scope.getFilesBySize = function (path) {
        $scope.serviseFuncDivideBySize(path).then(
            function (responce) {
                $scope.filesCount = responce.data;
            },
            function (error) {
                $scope.title = error;
            });
    }
}]);

