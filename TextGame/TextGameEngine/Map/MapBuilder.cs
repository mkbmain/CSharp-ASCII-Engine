using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TextGameEngine.MapObjects;

namespace TextGameEngine.Map
{

    public class MapBuilder
    {
        private static MapObjectBase[,] GetMap(string[] mapFileLines, Dictionary<char, MapObjectBase> lookup)
        {
            var mapFile = GetFileParts("map|", mapFileLines);
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
                    if (!lookup.TryGetValue(i, out var ob)) { continue; }
                    outval[xAxis, yAxis] = ob;
                    xAxis++;
                }

                yAxis++;
            }

            return outval;
        }

        private static Dictionary<string, MapCustomObject> GetObjects(string[] fileLines)
        {
            var customobjectsText = GetFileParts("ob|", fileLines);
            var customObdict = new Dictionary<string, MapCustomObject>();
            foreach (var line in customobjectsText.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(line)) { continue; }
                var start = line.Split('=');
                var customob = JsonConvert.DeserializeObject<MapCustomObject>(start[1]);
                customObdict.Add(start[0].Trim(), customob);
            }

            return customObdict;
        }

        private static Dictionary<char, MapObjectBase> GetKeyLookUp(Dictionary<string, MapCustomObject> customLookup, string[] fileLines)
        {
            var customobjectsText = GetFileParts("var|", fileLines);
            var lookUp = new Dictionary<char, MapObjectBase>() { { ' ', new FloorMapObject() } };
            foreach (var line in customobjectsText.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(line)) { continue;}
                var start = line.Split('=');
                if (start[0].Trim().Length > 1) { continue; }

                var c = start[0].Trim()[0];
                if (lookUp.ContainsKey(c)) { continue; }
                var ob = start[1].Trim();

                switch (ob)
                {
                    case "wall":
                        lookUp.Add(c, new WallObject());
                        break;
                    case "player":
                        lookUp.Add(c, new PlayerStartObject());
                        break;
                    case "exit":
                        lookUp.Add(c, new WallObject('e'));  // TODO BIGGIE
                        break;
                    default:
                        if (customLookup.TryGetValue(ob, out var mapob)) { lookUp.Add(c, mapob); }
                        break;
                }
            }

            return lookUp;
        }


        private static string GetFileParts(string startsWith, string[] mapFileLines)
        {
            var output = "";
            var mapBit = false;
            foreach (var line in mapFileLines)
            {
                if (string.IsNullOrWhiteSpace(line)) { continue; }
                if (mapBit == false && !line.StartsWith(startsWith)) { continue; }
                if (line.StartsWith(startsWith)) { mapBit = true; continue; }
                if (line.Contains('|')) { return output; }
                output += $"{line.ToLower()}{Environment.NewLine}";
            }
            return output;
        }

        public static IEnumerable<LevelDto> GetAllLevels(string path = null)
        {
            var allMap = TextGameEngine.IO.IOHelpers.GetMapFiles(path);
            var list = new List<LevelDto>();
            foreach (var f in allMap)
            {
                list.Add(GetLevel(f.Value, f.Key));
            }

            return list;
        }



        public static LevelDto GetLevel(string filePath, string name = null)
        {
            var fileLines = File.ReadAllLines(filePath);
            var customObjects = GetObjects(fileLines);
            var lookup = GetKeyLookUp(customObjects, fileLines);

            return new LevelDto
            {
                Map = GetMap(fileLines, lookup),
                Name = name == null ? filePath.Split(Path.DirectorySeparatorChar).LastOrDefault().Replace("TGM", "") : name
            };
        }



    }
}
