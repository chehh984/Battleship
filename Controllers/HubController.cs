using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.AspNetCore.SignalR.Hubs;

namespace Battleship.Controllers
{
    public abstract class HubController<T> : Controller
        where T : Hub
    {
        private readonly IHubContext _hub;
        public IHubConnectionContext<dynamic> Clients { get; private set; }
        public IGroupManager Groups { get; private set; }
        protected HubController(IConnectionManager signalRConnectionManager)
        {
            var _hub = signalRConnectionManager.GetHubContext<T>();
            Clients = _hub.Clients;
            Groups = _hub.Groups;
        }
    }
}