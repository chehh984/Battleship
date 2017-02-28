using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Battleship.Hubs;
using Battleship.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;

namespace Battleship.Controllers{

    [Route("api/[controller]")]
    public class ShipsController : HubController<Broadcaster>
    {
        private IMemoryCache _cache;
        public ShipsController(IConnectionManager signalRConnectionManager, IMemoryCache memoryCache)
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
             Console.WriteLine("saving data for " + username);
             var matchModel = (MatchModel)_cache.Get(username);             
             
             if(host == "true")
             {
                 Console.WriteLine("saving data for host " + username);
                 if(matchModel.PlayerOneShips == null)
                    matchModel.PlayerOneShips = new List<ShipPartModel>();
                matchModel.PlayerOneShips.Add(new ShipPartModel(){number = number,letter = letter,isHit = false});
             }else{
                 Console.WriteLine("saving data for guest " + username);
                 if(matchModel.PlayerTwoShips == null)
                    matchModel.PlayerTwoShips = new List<ShipPartModel>();
                matchModel.PlayerTwoShips.Add(new ShipPartModel(){number = number,letter = letter,isHit = false});
             }

             _cache.Remove(username);
             _cache.Set(username,matchModel);
        }
    }
}