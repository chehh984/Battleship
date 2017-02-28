using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Battleship.Hubs;
using Battleship.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;

namespace Battleship.Controllers{

    [Route("api/[controller]")]
    public class LobbyController : HubController<Broadcaster>
    {
        private IMemoryCache _cache;
        public LobbyController(IConnectionManager signalRConnectionManager, IMemoryCache memoryCache)
        : base(signalRConnectionManager)
        {
            _cache = memoryCache;
        }

        [HttpPost]
        public async void Post([FromBody] Dictionary<string,string> request)
        {
             List<string> cacheEntry;

             string username = request["username"];
             
             var model = new MatchModel();

             model.PlayerOneHits = new List<ShipPartModel>();
             model.PlayerOneShips = new List<ShipPartModel>();
             model.PlayerTwoHits = new List<ShipPartModel>();
             model.PlayerTwoShips = new List<ShipPartModel>();             

            _cache.Set(username,model);

             Console.WriteLine("got" + username);
            
            if (!_cache.TryGetValue("LobbyUsers", out cacheEntry))
            {                
                cacheEntry = new List<string>();
                cacheEntry.Add(username);              
                
            }else{
                cacheEntry = (List<string>)_cache.Get("LobbyUsers");
                cacheEntry.Add(username);
                cacheEntry.Remove("LobbyUsers");
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3600));                
            _cache.Set("LobbyUsers", cacheEntry, cacheEntryOptions);

            _cache.Set(username, new MatchModel());
            await Clients.All.AddToLobby(cacheEntry);
        }

        
        [HttpGet]
        public List<string> Get()
        {
            List<string> cacheEntry;
            
            if (!_cache.TryGetValue("LobbyUsers", out cacheEntry))
            {
                return new List<string>();
            }

            return cacheEntry;
        }
    }
}