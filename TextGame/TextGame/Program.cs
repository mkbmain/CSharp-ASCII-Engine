﻿using System;
using System.Linq;
using TextGameEngine.Config;
using TextGameEngine.Location;
using TextGameEngine.MapObjects;
using TextGameEngine.PlayerModel;

namespace TextGame
{
    internal static class Program
    {
        private static LevelModel[] AllLevels { get; set; }

        static void Main()
        {
            var firstLevelName = ConfigLoader.GetFirstMapLevelFromConfig();
            AllLevels = TextGameEngine.Map.MapBuilder.GetAllLevels().ToArray();

            var level = string.IsNullOrEmpty(firstLevelName) ? AllLevels.FirstOrDefault() : AllLevels.FirstOrDefault(f => f.Name.ToLower() == firstLevelName);
            if (level == null)
            {
                Console.WriteLine("No Levels Detected");
                return;
            }

            var map = level.Map;

            // from this point on player pos is here and not in map 
            var playerPos = LocationHelper.GetFirstObjectFromMap<PlayerStartObject>(map);
            var player = new Player((PlayerStartObject) map[playerPos.XAxis, playerPos.YAxis])
            {
                StartOb = (PlayerStartObject) map[playerPos.XAxis, playerPos.YAxis]
            };
            while (true)
            {
                var next = PlayGame(level, player);
                level = AllLevels.FirstOrDefault(f => string.Equals(f.Name, next, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        static string PlayGame(LevelModel level, Player player)
        {
            var map = level.Map;

            // from this point on player pos is here and not in map 
            var playerPos = level.LastPlayerPos ?? LocationHelper.GetFirstObjectFromMap<PlayerStartObject>(map);
            level.LastPlayerPos = new Positon();
            if (map[playerPos.XAxis, playerPos.YAxis].GetType() == typeof(PlayerStartObject))
            {
                map[playerPos.XAxis, playerPos.YAxis] = new FloorMapObject();
            }


            var input = "";
            var clear = true;
            while (input?.ToLower() != "exit")
            {
                var getAroundMe = LocationHelper.GetWhatsAroundPosition(playerPos, map);

                if (clear)
                {
                    Console.Clear();
                    Console.WriteLine($"-----");
                    Console.WriteLine($"|{getAroundMe.AllAround[0, 0]}{getAroundMe.AllAround[1, 0]}{getAroundMe.AllAround[2, 0]}|");
                    Console.WriteLine($"|{getAroundMe.AllAround[0, 1]}{player.StartOb}{getAroundMe.AllAround[2, 1]}|");
                    Console.WriteLine($"|{getAroundMe.AllAround[0, 2]}{getAroundMe.AllAround[1, 2]}{getAroundMe.AllAround[2, 2]}|");
                    Console.WriteLine($"-----");
                }

                if (getAroundMe.AllAround[1, 1].GetType() == typeof(MapCustomObject))
                {
                    var mapCustomObject = (MapCustomObject) getAroundMe.AllAround[1, 1];
                    if (!string.IsNullOrWhiteSpace(mapCustomObject.Message))
                    {
                        Console.WriteLine(mapCustomObject.Message);
                    }

                    if (mapCustomObject.AddItemId != null)
                    {
                        player.Inventory.Add(new InventoryItem {Id = (int) mapCustomObject.AddItemId, Name = mapCustomObject.Name});
                    }

                    level.Map[playerPos.XAxis, playerPos.YAxis] = new FloorMapObject();
                }

                if (getAroundMe.AllAround[1, 1].GetType() == typeof(MapExitObject))
                {
                    var mapExitObject = (MapExitObject) getAroundMe.AllAround[1, 1];

                    return mapExitObject.GoToLevel;
                }

                level.LastPlayerPos.XAxis = playerPos.XAxis;
                level.LastPlayerPos.YAxis = playerPos.YAxis;
                clear = true;

                var inventory = player.Inventory.Select(x => x.Name).Aggregate("", (a, b) => $"{a},{b}");
                Console.WriteLine($" Options are Up Down Left Right PickUp  (not case sensetive) items:{inventory}");

                input = Console.ReadLine();

                input = input?.ToLower();
                MapObjectBase moveToPoint = null;
                var movePos = new Positon {XAxis = 0, YAxis = 0};
                switch (input)
                {
                    case "exit":
                        Environment.Exit(1);
                        break;
                    case "up":
                    case "u":
                        moveToPoint = getAroundMe.Up;
                        movePos.YAxis = -1;
                        break;
                    case "down":
                    case "d":
                        moveToPoint = getAroundMe.Down;
                        movePos.YAxis = 1;
                        break;
                    case "left":
                    case "l":
                        moveToPoint = getAroundMe.Left;
                        movePos.XAxis = -1;
                        break;
                    case "right":
                    case "r":
                        moveToPoint = getAroundMe.Right;
                        movePos.XAxis = 1;
                        break;
                }

                // ok i admit this is a lot of conditions 
                // essentially it can't be null and it can be stood on
                if (moveToPoint != null && moveToPoint.CanStandOn &&
                    // its not a custom object
                    (moveToPoint.GetType() != typeof(MapCustomObject) ||
                     // if it is a custom object does not need a item
                     ((MapCustomObject) moveToPoint).RequiresItemId == null ||
                     // if it is a custom object and needs a item we have it in player inv
                     player.Inventory.Select(x => x.Id).Contains((int) ((MapCustomObject) moveToPoint).RequiresItemId)))
                {
                    playerPos.XAxis += movePos.XAxis;
                    playerPos.YAxis += movePos.YAxis;
                    continue;
                }

                clear = false;
                Console.WriteLine($"Can Not do that");
            }

            return "";
        }
    }
}