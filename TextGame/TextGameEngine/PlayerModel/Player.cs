using System.Collections.Generic;
using TextGameEngine.MapObjects;

namespace TextGameEngine.PlayerModel
{
    public class Player
    {
        public PlayerStartObject StartOb { get; set; }
        public List<InventoryItem> Inventory { get; }

        public Player(PlayerStartObject st)
        {
            StartOb = st;
            Inventory = new List<InventoryItem>();
        }
    }
}