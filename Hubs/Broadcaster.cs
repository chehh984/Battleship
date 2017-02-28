using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Battleship.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Hubs;
using System;

namespace Battleship.Hubs
{
    [HubName("broadcaster")]
    public class Broadcaster : Hub
    {
        public override Task OnConnected()
        {
            // Set connection id for just connected client only
            return Clients.Client(Context.ConnectionId).SetConnectionId(Context.ConnectionId);

        }
 
        // Server side methods called from client
        public Task JoinMatch(string matchId)
        {
            Console.WriteLine("Joined group " + matchId);
            return Groups.Add(Context.ConnectionId, matchId.ToString());
        }
 
        public Task LeaveMatch(string matchId)
        {
            return Groups.Remove(Context.ConnectionId, matchId.ToString());
        }
    } 

}