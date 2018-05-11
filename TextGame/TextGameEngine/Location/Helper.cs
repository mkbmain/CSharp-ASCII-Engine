using System.Collections.Generic;
using System.Linq;
using TextGameEngine.Location.DTO;
using TextGameEngine.MapObjects;

namespace TextGameEngine.Location
{
    public class LocationHelper
    {
        private static IEnumerable<Positon> GetObjectFromMapBase<T>(MapObjectBase[,] map, bool all = true) where T : MapObjectBase
        {
            var positons = new List<Positon>();
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == null || map[i, j].GetType() != typeof(T)) { continue; }

                    positons.Add(new Positon { XAxis = i, YAxis = j });
                    if (all) { return positons; }
                }
            }

            return positons;
        }
        public static Positon GetFirstObjectFromMap<T>(MapObjectBase[,] map) where T : MapObjectBase
        {
            return GetObjectFromMapBase<T>(map, true).FirstOrDefault();
        }
        public static IEnumerable<Positon> GetAllObjectFromMap<T>(MapObjectBase[,] map) where T : MapObjectBase
        {
            return GetObjectFromMapBase<T>(map);
        }


        public static ObjectsAroundPointModel GetWhatsAroundPosition(Positon pos, MapObjectBase[,] map)
        {
            var test = new ObjectsAroundPointModel
            {
                AllAround = new MapObjectBase[3, 3]
            };
            test.AllAround[0, 0] = map[pos.XAxis - 1, pos.YAxis - 1]; test.AllAround[1, 0] = map[pos.XAxis, pos.YAxis - 1]; test.AllAround[2, 0] = map[pos.XAxis + 1, pos.YAxis - 1];
            test.AllAround[0, 1] = map[pos.XAxis - 1, pos.YAxis]; test.AllAround[1, 1] = map[pos.XAxis, pos.YAxis]; test.AllAround[2, 1] = map[pos.XAxis + 1, pos.YAxis];
            test.AllAround[0, 2] = map[pos.XAxis - 1, pos.YAxis + 1]; test.AllAround[1, 2] = map[pos.XAxis, pos.YAxis + 1]; test.AllAround[2, 2] = map[pos.XAxis + 1, pos.YAxis + 1];
            return test;
        }

    }
}
