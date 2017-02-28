'use strict';
angular.module('app',[
    'ngRoute',
    'SignalR'
])

.config(['$routeProvider','$locationProvider', function($routeProvider, $locationProvider){
    $locationProvider.hashPrefix('');
    $routeProvider
    .when("/" ,{
        templateUrl : "app/templates/lobbyTemplate.html",
        controller: "HomeController"
    })
    .when("/lobby" ,{
        templateUrl : "app/templates/lobbyTemplate.html",
        controller: "HomeController"
    })
    .when("/game", {
        templateUrl : "app/templates/gameTemplate.html",
        controller: "GameController"
    })

    
}]);