'use strict';
angular.module('app').controller('HomeController',['$scope','$rootScope','$http','$location','$filter','LobbyService', 
function($scope, $rootScope, $http,$location,$filter, LobbyService){
    $scope.username = '';
    $scope.lobby = LobbyService;
    $scope.enteredLobby = false;
    
    init();
   

    $scope.enterLobby = function(username){

        var request = {};
        request.username = username;

        $http.post('/api/Lobby',request)
        .then(function(){                
                $scope.enteredLobby = true;
                $scope.lobby.currentUser = username;
            },function(error){
                alert('error occured');
                console.log(err);
            })

    }



    $scope.getClass = function(number,letter,table){
        
        var arrayToSearch = [];

        if(table == 'ONE')
        {            
            arrayToSearch = $scope.lobby.yourShips;           
        }else{            
            arrayToSearch = $scope.lobby.opponentShips;           
        } 

        var found = undefined;
        
        for(var i = 0;i<arrayToSearch.length;i++)
        {
            if(arrayToSearch[i].number === number && arrayToSearch[i].letter === letter)
            {
                found = arrayToSearch[i]
                break;
            }
        } 
        
        if(found === undefined || found === null)
            return 'emptyCell';
        
        if(found.isHit)
            return 'hitCell';
        
        if(found.isMiss)
            return 'missCell';
        
        return 'shipCell';
    }

    function init()
    {
        $http.get('/api/Lobby')
        .then(function(data){
            $scope.lobby.lobbyList = data.data;
        },function(err){
            alert('error loading users');
        });
    }
}]);