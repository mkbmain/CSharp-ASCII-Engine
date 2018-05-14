using System.Collections.Generic;
using TextGameEngine.MapObjects;

namespace TextGameEngine.PlayerModel
{
    /// <summary>
    /// Player object might need to add live and stats
    /// </summary>
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