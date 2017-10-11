//todoController.js

(function () {
    "use-strict";

    angular.module("app-todo")
        .controller("todoController", todoController)

    function todoController ($http) {
        var vm = this;

        vm.tasks = [];
        vm.isBusy = false;
        vm.errorMessage = "";

        vm.isBusy = true;
        debugger
        $http.get("api/tasks/all")
            .then(function (response) {
                // Success
                angular.copy(response.data, vm.tasks);
            }, function (error) {
                // Failure
                vm.errorMessage = "Failed to load data: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });
    }
})();