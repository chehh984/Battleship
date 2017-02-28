using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Battleship.Hubs;
using Battleship.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;

namespace Battleship.Controllers{

    [Route("api/[controller]")]
    public class BattleController : HubController<Broadcaster>
    {
        private IMemoryCache _cache;
        public BattleController(IConnectionManager signalRConnectionManager, IMemoryCache memoryCache)
        : base(signalRConnectionManager)
        {
            _cache = memoryCache;
        }

        [HttpPost]
        public void Post([FromBody] Dictionary<string,string> request)
        {
             string username = request["username"];
             string host = request["host"];
             string letter = request["letter"];
             string number = request["number"];
             Console.WriteLine("trying hit for " + username);
             var matchModel = (MatchModel)_cache.Get(username);             
             
             if(host == "true")
             {
                 Console.WriteLine("trying to hit player two");
                //var foundShip = matchModel.PlayerTwoShips.Find(x=>x.letter == letter && x.number == number);
                ShipPartModel foundShip = null;

                foreach(var part in matchModel.PlayerTwoShips)
                {
                    Console.WriteLine("trying: " + part.letter + " " + part.number + " against: " + letter + number);
                    if(number == part.number && letter == part.letter){
                        foundShip = part;
                    }
                }
                
                if(foundShip != null){
                    foundShip.isHit = true;
                    Console.WriteLine("found a hit!, player two");
                }else{
                    matchModel.PlayerTwoShips.Add(new ShipPartModel(){letter = letter,number=number,isHit = false, isMiss = true});
                    Console.WriteLine("Missed! player two");
                }

             }else{
                Console.WriteLine("trying to hit player one");
                //var foundShip = matchModel.PlayerOneShips.Find(x=>x.letter == letter && x.number == number);

                ShipPartModel foundShip = null;

                foreach(var part in matchModel.PlayerOneShips)
                {
                    Console.WriteLine("trying: " + part.letter + " " + part.number + " against: " + letter + number);
                    if(number == part.number && letter == part.letter){
                        foundShip = part;
                    }
                }
                
                if(foundShip != null){
                    foundShip.isHit = true;
                    Console.WriteLine("found a hit!, player one");
                }else{
                    matchModel.PlayerOneShips.Add(new ShipPartModel(){letter = letter,number=number,isHit = false, isMiss = true});
                    Console.WriteLine("Missed! player one");
                }
             }

             _cache.Remove(username);
             _cache.Set(username,matchModel);

             Clients.Group(username).Shoot(matchModel);
        }

        public MatchModel Get(string username)
        {
            return (MatchModel)_cache.Get(username);
        }
    }
}