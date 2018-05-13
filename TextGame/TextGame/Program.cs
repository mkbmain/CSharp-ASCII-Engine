using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextGameEngine.Location;
using TextGameEngine.MapObjects;
using TextGameEngine.PlayerModel;

namespace TextGame
{
    internal static class Program
    {
        static IEnumerable<LevelDto> AllLevels { get; set; }

        static void Main()
        {
            AllLevels = TextGameEngine.Map.MapBuilder.GetAllLevels();
            var level = AllLevels.FirstOrDefault();
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
                if (next == "exit")
                {
                    return;
                }

                level = AllLevels.FirstOrDefault(f => f.Name.ToLower() == next);
            }
        }

        static string PlayGame(LevelDto level, Player player)
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
            while (input.ToLower() != "exit")
            {
                var getAroundMe = LocationHelper.GetWhatsAroundPosition(playerPos, map);

                if (clear)
                {
                    Console.Clear();
                    Console.WriteLine($"-----");
                    Console.WriteLine(
                        $"|{getAroundMe.AllAround[0, 0]}{getAroundMe.AllAround[1, 0]}{getAroundMe.AllAround[2, 0]}|");
                    Console.WriteLine($"|{getAroundMe.AllAround[0, 1]}{player.StartOb}{getAroundMe.AllAround[2, 1]}|");
                    Console.WriteLine(
                        $"|{getAroundMe.AllAround[0, 2]}{getAroundMe.AllAround[1, 2]}{getAroundMe.AllAround[2, 2]}|");
                    Console.WriteLine($"-----");
                }

                if (getAroundMe.AllAround[1, 1].GetType() == typeof(MapCustomObject))
                {
                    var customob = (MapCustomObject) getAroundMe.AllAround[1, 1];
                    if (!string.IsNullOrWhiteSpace(customob.Message))
                    {
                        Console.WriteLine(customob.Message);
                    }

                    if (customob.AddItemId != null)
                    {
                        player.Inventory.Add(new InventoryItem {Id = (int) customob.AddItemId, Name = customob.Name});
                    }

                    level.Map[playerPos.XAxis, playerPos.YAxis] = new FloorMapObject();
                }

                if (getAroundMe.AllAround[1, 1].GetType() == typeof(MapExitObject))
                {
                    var customob = (MapExitObject) getAroundMe.AllAround[1, 1];

                    return customob.GOTO;
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
                    case "up":
                        moveToPoint = getAroundMe.Up;
                        movePos.YAxis = -1;
                        break;
                    case "down":
                        moveToPoint = getAroundMe.Down;
                        movePos.YAxis = 1;
                        break;
                    case "left":
                        moveToPoint = getAroundMe.Left;
                        movePos.XAxis = -1;
                        break;
                    case "right":
                        moveToPoint = getAroundMe.Right;
                        movePos.XAxis = 1;
                        break;
                }

                if (moveToPoint != null &&
                    moveToPoint.CanStandOn)
                {
                    if (moveToPoint.GetType() != typeof(MapCustomObject) ||
                        ((MapCustomObject) moveToPoint).RequiresItemId == null ||
                        player.Inventory.Select(x => x.Id)
                            .Contains((int) ((MapCustomObject) moveToPoint).RequiresItemId))
                    {
                        playerPos.XAxis += movePos.XAxis;
                        playerPos.YAxis += movePos.YAxis;
                    }

                    clear = false;
                    Console.WriteLine($"Can Not do that");
                }
            }

            return "";
        }
    }
}