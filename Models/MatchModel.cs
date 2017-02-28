using System.Collections.Generic;

namespace Battleship.Models
{
    public class MatchModel
    {
        public string Id {get;set;}
        public string PlayerOne{get;set;}
        public string PlayerTwo{get;set;}
        public List<ShipPartModel> PlayerOneShips{get;set;}
        public List<ShipPartModel> PlayerTwoShips{get;set;}
        public List<ShipPartModel> PlayerOneHits {get;set;}
        public List<ShipPartModel> PlayerTwoHits {get;set;}
        public string Winner {get;set;}
    }
}