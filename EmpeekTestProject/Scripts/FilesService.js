app.service('FileService', ["$http", function ($http) {
    this.getFiles = function (path) {
        var deferred = $q.defer();
        $http.get('/api/files?path=' + path, path, deferred);
        return deferred.promise;
    };
}]);