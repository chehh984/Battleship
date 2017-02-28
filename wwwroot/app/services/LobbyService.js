'use strict';

angular.module('app').factory('LobbyService', ['$rootScope','$http','Hub', function ($rootScope,$http, Hub) {

    var lobbyData = this;
    
    lobbyData.lobbyList = [];
    lobbyData.readyPlayers = [];
    lobbyData.currentUser = '';
    lobbyData.opponent = '';
    lobbyData.gameStarted = false;
    lobbyData.yourShips = [{}];
    lobbyData.opponentShips = [{}];
    
    var hub = Hub('broadcaster',{
       listeners:{
           'AddToLobby': function(list){
                lobbyData.lobbyList = list;
                $rootScope.$apply();
           },
           'UserReady': function(player){
                lobbyData.lobbyList.splice(lobbyData.lobbyList.indexOf(player), 1);
                lobbyData.readyPlayers.push(player);
                $rootScope.$apply();
           },
           'StartGame' : function(player){
                lobbyData.gameStarted = true;
                lobbyData.opponent = player;              
                $rootScope.$apply();
           },
           'Shoot' : function(matchModel){
                
                var opponentShips = [];
               if(lobbyData.currentUser === lobbyData.opponent)
               {
                   lobbyData.yourShips = matchModel.PlayerOneShips;
                   opponentShips = matchModel.PlayerTwoShips;
               }else{
                   opponentShips = matchModel.PlayerOneShips;
                   lobbyData.yourShips = matchModel.PlayerTwoShips;
               } 

               for(var j = 0;j<opponentShips.length;j++)
               {
                   var foundShip = null;
                   

                   for(var i = 0;i<lobbyData.opponentShips.length;i++)
                   {
                       if(lobbyData.opponentShips === undefined)
                            continue;
                       
                       if(opponentShips[j].number === lobbyData.opponentShips[i].number && opponentShips[j].letter === lobbyData.opponentShips[i].letter)
                       {
                           foundShip = opponentShips[j];
                           lobbyData.opponentShips[i] = foundShip;
                       }
                   }

                   if(foundShip == null && (opponentShips[j].isMiss || opponentShips[j].isHit)){
                       lobbyData.opponentShips.push(opponentShips[j]);
                       break;
                   }

               }
               
               
               
               for(var i = 0;lobbyData.opponentShips-length;i++)
               {
                   for(var j = 0;opponentShips.length;j++)
                   {
                       if(opponentShips[j].number === lobbyData.opponentShips[i].number && opponentShips[j].letter === lobbyData.opponentShips[i].letter)
                       {
                           lobbyData.opponentShips[i] = opponentShips[j];
                       }else{
                            if(opponentShips[j].isHit || opponentShips[j].isMiss)
                                lobbyData.opponentShips.push(opponentShips[j]);
                       }
                   }
               }

               $rootScope.$apply();
           }
       },
       methods: ['JoinMatch'],
       errorHandler: function(error)
       {
           console.error(error);
       }
   })

   lobbyData.joinMatch = function(username){
        hub.JoinMatch(username);

        var request = {};
        request.username = username;

        $http.post('/api/MatchMaking',request)
        .then(function(){
            
        },function(err){
            console.log(err);
        });
   }

   lobbyData.playWith = function(username){
       hub.JoinMatch(username);

       $http.get('/api/MatchMaking?username=' + username)
        .then(function(){
            lobbyData.gameStarted = true;
            lobbyData.opponent = username;
        },function(err){
            console.log(err);
        });
   }

   lobbyData.placeShip = function(number, letter){
        var request = {};
        
        
        if(lobbyData.currentUser === lobbyData.opponent){
            request.host = 'true';
            request.username = lobbyData.currentUser;
        }
        else{
            request.host = 'false';
            request.username = lobbyData.opponent;
        }

        request.letter = letter;
        request.number = number;

        $http.post('/api/Ships',request)
        .then(function(){ 
                var shipPart = {};
                shipPart.letter = letter;
                shipPart.number = number;
                shipPart.isHit = false;   
                shipPart.isMiss = false;            
                lobbyData.yourShips.push(shipPart);
                $rootScope.$apply;
            },function(error){
                alert('error occured');
                console.log(err);
            });
    }

    lobbyData.targetShip = function(number, letter){
        var request = {};

        if(lobbyData.currentUser === lobbyData.opponent){
            request.host = 'true';
            request.username = lobbyData.currentUser;
        }
        else{
            request.host = 'false';
            request.username = lobbyData.opponent;
        }

        request.letter = letter;
        request.number = number;

        $http.post('/api/Battle',request)
        .then(function(){ 
                
            },function(error){
                alert('error occured');
                console.log(err);
            });
    }
   return lobbyData;
}]);