using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Battleship.Hubs;
using Battleship.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;

namespace Battleship.Controllers{

    [Route("api/[controller]")]
    public class MatchMakingController : HubController<Broadcaster>
    {
        private IMemoryCache _cache;
        public MatchMakingController(IConnectionManager signalRConnectionManager, IMemoryCache memoryCache)
        : base(signalRConnectionManager)
        {
            _cache = memoryCache;
        }

        [HttpPost]
        public async void Post([FromBody] Dictionary<string,string> request)
        {            
            string username = request["username"];

            
            List<string> lobbyUsers;
            List<string> readyUsers;
            
            if (!_cache.TryGetValue("LobbyUsers", out lobbyUsers))
            {
                lobbyUsers = new List<string>();
            }else{
                _cache.Remove("LobbyUsers");
            }

            if (!_cache.TryGetValue("ReadyUsers", out readyUsers))
            {
                readyUsers = new List<string>();
            }else{
                _cache.Remove("ReadyUsers");
            }

            lobbyUsers.Remove(username);
            readyUsers.Add(username);

            _cache.Set("ReadyUsers",readyUsers);
            _cache.Set("LobbyUsers",lobbyUsers);

            await Clients.All.UserReady(username);
        }

        [HttpGet]
        public void Get(string username)
        {
            Console.WriteLine("broadcasting to group " + username);
            Clients.Group(username).StartGame(username);
        }
    }
}