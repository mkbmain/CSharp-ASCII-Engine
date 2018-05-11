using System;
using System.Collections.Generic;
using System.Text;
using TextGameEngine.MapObjects;

namespace TextGameEngine.PlayerModel
{
    public class Player
    {
        public PlayerStartObject StartOb { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public Player(PlayerStartObject st)
        {
            StartOb = st;
            Inventory = new List<InventoryItem>();
        }



    }
}

