//app-todo.js
(function () {
    "use-strict";

    angular.module("app-todo", ["simpleControls", "ngRoute"])
        .config(function ($routeProvider) {
            $routeProvider.when("/", {
                controller: "todoController",
                controllerAs: "vm",
                templateUrl: "/views/tasksCollection.html"
            });

            $routeProvider.when("/editor/:taskId", {
                controller: "taskEditController",
                controllerAs: "vm",
                templateUrl: "/views/taskEditor.html"
            });

            $routeProvider.otherwise({ redirectTo: "/" });
        });
})();