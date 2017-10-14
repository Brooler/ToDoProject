//todoController.js

(function () {
    "use-strict";

    angular.module("app-todo")
        .controller("todoController", todoController);

    function todoController ($http) {
        var vm = this;

        vm.tasks = [];
        vm.isBusy = false;
        vm.errorMessage = "";

        vm.isBusy = true;
        $http.get("api/tasks/all")
            .then(function (response) {
                angular.copy(response.data, vm.tasks);
            }, function (error) {
                vm.errorMessage = "Failed to load data: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.completeTask = function (taskId) {
            var result = confirm("Complete task?");
            if (result) {
                vm.errorMessage = "";
                vm.isBusy = true;
                $http.put("api/tasks/complete/" + taskId)
                    .then(function (response) {
                        removeTaskById(taskId);
                    }, function (error) {
                        vm.errorMessage = "Failed to complete task: " + error;
                    })
                    .finally(function () {
                        vm.isBusy = false;
                    });
            }
        }

        vm.deleteTask = function (taskId) {
            var result = confirm("Delete task?");
            if (result) {
                vm.errorMessage = "";
                vm.isBusy = true;
                $http.delete("api/tasks/" + taskId)
                    .then(function (response) {
                        removeTaskById(taskId);
                    }, function (error) {
                        vm.errorMessage = "Failed to complete task: " + error;
                    })
                    .finally(function () {
                        vm.isBusy = false;
                    });
            }
        }

        vm.getClassForItem = function (item) {
            var cssClass = "";
            if (item.isOverdue) {
                cssClass = "danger";
            }

            return cssClass;
        };

        function removeTaskById(taskId) {
            var index = vm.tasks.map(function (item) { return item.id; })
                .indexOf(taskId);

            vm.tasks.splice(index, 1);
        };
    }
})();