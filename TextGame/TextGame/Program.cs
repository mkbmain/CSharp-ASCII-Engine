using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextGameEngine.Location;
using TextGameEngine.MapObjects;
using TextGameEngine.PlayerModel;

namespace TextGame
{
    class Program
    {
        public static IEnumerable<LevelDto> AllLevels { get; set; }

        static void Main()
        {

            AllLevels = TextGameEngine.Map.MapBuilder.GetAllLevels();
            var level = AllLevels.FirstOrDefault();
            var map = level.Map;

            // from this point on player pos is here and not in map 
            var playerPos = LocationHelper.GetFirstObjectFromMap<PlayerStartObject>(map);
            var player = new Player((PlayerStartObject)map[playerPos.XAxis, playerPos.YAxis]);

            while (true)
            {
                var next = PlayGame(level, player);
                if (next == "exit") { return; }
                level = AllLevels.FirstOrDefault(f => f.Name.ToLower() == next);
            }
        }

        static string PlayGame(LevelDto level, Player player)
        {
            var map = level.Map;

            // from this point on player pos is here and not in map 
            var playerPos = LocationHelper.GetFirstObjectFromMap<PlayerStartObject>(map);
            player.StartOb = (PlayerStartObject)map[playerPos.XAxis, playerPos.YAxis];
            map[playerPos.XAxis, playerPos.YAxis] = new FloorMapObject();


            var input = "";
            var clear = true;
            while (input.ToLower() != "exit")
            {
                var getAroundMe = TextGameEngine.Location.LocationHelper.GetWhatsAroundPosition(playerPos, map);

                if (clear)
                {
                    Console.Clear();
                    Console.WriteLine($"-----");
                    Console.WriteLine($"|{getAroundMe.AllAround[0, 0]}{getAroundMe.AllAround[1, 0]}{ getAroundMe.AllAround[2, 0]}|");
                    Console.WriteLine($"|{getAroundMe.AllAround[0, 1]}{player.StartOb}{ getAroundMe.AllAround[2, 1]}|");
                    Console.WriteLine($"|{getAroundMe.AllAround[0, 2]}{getAroundMe.AllAround[1, 2]}{ getAroundMe.AllAround[2, 2]}|");
                    Console.WriteLine($"-----");
                }
                if (getAroundMe.AllAround[1, 1].GetType() == typeof(MapCustomObject))
                {
                    var customob = (MapCustomObject)getAroundMe.AllAround[1, 1];
                    if (!string.IsNullOrWhiteSpace(customob.Message)) { Console.WriteLine(customob.Message); }
                    if (customob.AddItemId != null) { player.Inventory.Add(new InventoryItem { Id = (int)customob.AddItemId, Name = customob.Name }); }

                    level.Map[playerPos.XAxis, playerPos.YAxis] = new FloorMapObject();
                }

                if (getAroundMe.AllAround[1, 1].GetType() == typeof(MapExitObject))
                {
                    var customob = (MapExitObject)getAroundMe.AllAround[1, 1];

                    return customob.GOTO;
                }

                clear = true;

                var inventory = player.Inventory.Select(x => x.Name).Aggregate("", (a, b) => $"{a},{b}");
                Console.WriteLine($" Options are Up Down Left Right PickUp  (not case sensetive) items:{inventory}");

                input = Console.ReadLine();

                input = input?.ToLower();
                if (input == "up" && getAroundMe.Up.CanStandOn)
                {
                    if (getAroundMe.Up.GetType() != typeof(MapCustomObject) || ((MapCustomObject)getAroundMe.Up).RequiresItemId == null)
                    {
                        playerPos.YAxis = playerPos.YAxis - 1;
                        continue;
                    }

                    if (player.Inventory.Select(x => x.Id).Contains((int)((MapCustomObject)getAroundMe.Up).RequiresItemId))
                    {
                        playerPos.YAxis = playerPos.YAxis - 1;
                        continue;
                    }


                }
                else if (input == "down" && getAroundMe.Down.CanStandOn)
                {
                    if (getAroundMe.Down.GetType() != typeof(MapCustomObject) || ((MapCustomObject)getAroundMe.Down).RequiresItemId == null)
                    {
                        playerPos.YAxis = playerPos.YAxis + 1;
                        continue;
                    }

                    if (player.Inventory.Select(x => x.Id).Contains((int)((MapCustomObject)getAroundMe.Down).RequiresItemId))
                    {
                        playerPos.YAxis = playerPos.YAxis + 1;
                        continue;
                    }
                }
                else if (input == "left" && getAroundMe.Left.CanStandOn)
                {
                    if (getAroundMe.Left.GetType() != typeof(MapCustomObject) || ((MapCustomObject)getAroundMe.Left).RequiresItemId == null)
                    {
                        playerPos.XAxis = playerPos.XAxis - 1;
                        continue;
                    }

                    if (player.Inventory.Select(x => x.Id).Contains((int)((MapCustomObject)getAroundMe.Left).RequiresItemId))
                    {
                        playerPos.XAxis = playerPos.XAxis - 1;
                        continue;
                    }
                }
                else if (input == "right" && getAroundMe.Right.CanStandOn)
                {
                    if (getAroundMe.Right.GetType() != typeof(MapCustomObject) || ((MapCustomObject)getAroundMe.Right).RequiresItemId == null)
                    {
                        playerPos.XAxis = playerPos.XAxis + 1;
                        continue;
                    }

                    if (player.Inventory.Select(x => x.Id).Contains((int)((MapCustomObject)getAroundMe.Right).RequiresItemId))
                    {
                        playerPos.XAxis = playerPos.XAxis + 1;
                        continue;
                    }
                }

                clear = false;
                Console.WriteLine($"Can Not do that");
            }

            return "";
        }
    }
}
