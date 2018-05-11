using System;
using System.IO;
using System.Linq;
using TextGameEngine.Location;
using TextGameEngine.MapObjects;
using TextGameEngine.PlayerModel;

namespace TextGame
{
    class Program
    {
        static void Main()
        {

            var allLevels = TextGameEngine.Map.MapBuilder.GetAllLevels();
            var level = allLevels.FirstOrDefault();
            var map = level.Map;
            var input = "";
            // from this point on player pos is here and not in map 
            var playerPos = LocationHelper.GetFirstObjectFromMap<PlayerStartObject>(map);
            var player = new Player((PlayerStartObject) map[playerPos.XAxis, playerPos.YAxis]);
            map[playerPos.XAxis, playerPos.YAxis] = new FloorMapObject();

            var clear = true;
            while (input.ToLower() != "exit")
            {

                var getAroundMe = TextGameEngine.Location.LocationHelper.GetWhatsAroundPosition(playerPos, map);
                if (clear)
                {
                    Console.Clear();
                    Console.WriteLine($"-----");
                    Console.WriteLine($"|{getAroundMe.AllAround[0,0]}{getAroundMe.AllAround[1, 0]}{ getAroundMe.AllAround[2, 0]}|");
                    Console.WriteLine($"|{getAroundMe.AllAround[0, 1]}{player.StartOb}{ getAroundMe.AllAround[2, 1]}|");
                    Console.WriteLine($"|{getAroundMe.AllAround[0, 2]}{getAroundMe.AllAround[1, 2]}{ getAroundMe.AllAround[2, 2]}|");
                    Console.WriteLine($"-----");
                }

                clear = true;

                Console.WriteLine($" Options are Up Down Left Right PickUp  (not case sensetive)");

                input = Console.ReadLine();

                input = input?.ToLower();
                switch (input)
                {
                    case "up" when getAroundMe.Up.CanStandOn:
                        playerPos.YAxis = playerPos.YAxis - 1;
                        continue;
                    case "down" when getAroundMe.Down.CanStandOn:
                        playerPos.YAxis = playerPos.YAxis + 1;
                        continue;
                    case "left" when getAroundMe.Left.CanStandOn:
                        playerPos.XAxis = playerPos.XAxis - 1;
                        continue;
                    case "right" when getAroundMe.Right.CanStandOn:
                        playerPos.XAxis = playerPos.XAxis + 1;
                        continue;
                }

                clear = false;
                Console.WriteLine($"Can Not do that");
            }

        }
    }
}
