using System.Linq;
using TextGameEngine.MapObjects;

namespace TextGameEngine.Map
{
    public class MapBuilder
    {

        public static MapObjectBase[,] GetLevel(string mapFile)
        {
            var lines = mapFile.Split('\n');
            var height = lines.Length;
            var maxWidth = lines.OrderByDescending(x => x.Length).FirstOrDefault().Length;

            var outval = new MapObjectBase[maxWidth, height];
            var yAxis = 0;

            foreach (var line in lines)
            {
                var xAxis = 0;
                foreach (var i in line)
                {
                    switch (i.ToString().ToLower())
                    {
                        case "x":
                            outval[xAxis, yAxis] = new WallObject();
                            break;
                        case " ":
                            outval[xAxis, yAxis] = new FloorMapObject();
                            break;
                        case "p":
                            outval[xAxis, yAxis] = new PlayerMapObject();
                            break;
                        case "e":
                            outval[xAxis, yAxis] = new MapExitObject();
                            break;
                    }

                    xAxis++;
                }

                yAxis++;
            }

            return outval;
        }


    }
}
