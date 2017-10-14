//taskEditController.js
(function () {
    "use-strict";

    angular.module("app-todo")
        .controller("taskEditController", taskEditController);

    function taskEditController($routeParams, $http) {
        vm = this;

        vm.taskId = $routeParams.taskId;

        vm.task = {};
        vm.priorities = [
            { id: 0, name: "Lowest" },
            { id: 1, name: "Low" },
            { id: 2, name: "Medium" },
            { id: 3, name: "High" },
            { id: 4, name: "Critical" }
        ];

        vm.isBusy = false;
        vm.errorMessage = "";

        if (vm.taskId != 0) {
            vm.isBusy = true;
            $http.get("api/tasks/" + vm.taskId)
                .then(function (response) {
                    vm.task = response.data;
                    var d = new Date(response.data.dueDate);
                    vm.task.dueDate = d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
                }, function (error) {
                    vm.errorMessage = "Failed to load data: " + error;
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        }

        vm.submitForm = function () {
            vm.isBusy = true;
            vm.errorMessage = "";
            $http.post("api/tasks", vm.task)
                .then(function (response) {
                    vm.task = {};
                }, function (error) {
                    vm.errorMessage = "Failed to edit task: " + error;
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };
    }
})();