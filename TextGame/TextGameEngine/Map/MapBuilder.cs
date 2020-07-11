using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TextGameEngine.Config;
using TextGameEngine.MapObjects;

namespace TextGameEngine.Map
{
    /// <summary>
    /// Map builder this is in charge of getting and mapping map and vars from tgl files
    /// </summary>
    public static class MapBuilder
    {
        private static MapObjectBase[,] GetMap(IEnumerable<string> mapFileLines, IReadOnlyDictionary<char, MapObjectBase> lookup)
        {
            var mapFile = GetFileParts("map|", mapFileLines);
            var lines = mapFile.Split('\n');
            var height = lines.Length;
            var maxWidth = lines.Select(x => x.Length).OrderByDescending(x => x).FirstOrDefault();

            var outval = new MapObjectBase[maxWidth, height];
            for (int yAxis = 0; yAxis < lines.Length; yAxis++)
            {
                for (int xAxis = 0; xAxis < lines[yAxis].Length; xAxis++)
                {
                    if (lookup.TryGetValue(lines[yAxis][xAxis], out var ob))
                    {
                        outval[xAxis, yAxis] = ob;
                    }
                }
            }

            return outval;
        }

        private static Dictionary<string, MapCustomObject> GetObjects(IEnumerable<string> fileLines)
        {
            var customobjectsText = GetFileParts("ob|", fileLines);
            var customObdict = new Dictionary<string, MapCustomObject>();
            foreach (var line in customobjectsText.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var start = line.Split('=');
                var customob = JsonConvert.DeserializeObject<MapCustomObject>(start[1]);
                customObdict.Add(start[0].Trim(), customob);
            }

            return customObdict;
        }

        private static Dictionary<char, MapObjectBase> GetMapObjectLookUp(IReadOnlyDictionary<string, MapCustomObject> customLookup,
            IEnumerable<string> fileLines)
        {
            var customobjectsText = GetFileParts("var|", fileLines);
            var lookUp = new Dictionary<char, MapObjectBase>() {{' ', new FloorMapObject()}};
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
                        lookUp.Add(c, new WallObject(MapObjectDisplayChars.WallChar));
                        break;
                    case "player":
                        lookUp.Add(c, new PlayerStartObject(MapObjectDisplayChars.PlayerChar));
                        break;
                    case "exit":
                        if(start.Length < 3) { throw new Exception("Exit setup with no goto");}
                        lookUp.Add(c, new MapExitObject(MapObjectDisplayChars.ExitChar) { GoToLevel = start[2].Trim()});  // TODO BIGGIE
                        break;
                    default:
                        if (customLookup.TryGetValue(ob, out var mapob)) { lookUp.Add(c, mapob); }
                        break;
                }
            }
            return lookUp;
        }

        private static string GetFileParts(string startsWith, IEnumerable<string> mapFileLines)
        {
            var output = "";
            var mapBit = false;
            foreach (var line in mapFileLines)
            {
                if (string.IsNullOrWhiteSpace(line)) { continue; }
                if (mapBit == false && !line.StartsWith(startsWith)) { continue; }
                if (line.StartsWith(startsWith))
                {
                    mapBit = true;
                    continue;
                }
                
                if (line.Contains('|')) { return output; }
                output += $"{line}{Environment.NewLine}";
            }
            return output;
        }

        public static IEnumerable<LevelModel> GetAllLevels(string path = null)
        {
            return IO.IOHelpers.GetMapFiles(path).Select(f => GetLevel(f.Value, f.Key)).ToList();
        }

        private static LevelModel GetLevel(string filePath, string name = null)
        {
            var fileLines = File.ReadAllLines(filePath);
            var customObjects = GetObjects(fileLines);
            var lookup = GetMapObjectLookUp(customObjects, fileLines);

            return new LevelModel
            {
                Map = GetMap(fileLines, lookup),
                Name = name ?? filePath.Split(Path.DirectorySeparatorChar).LastOrDefault()?.Replace("TGM", "")
            };
        }
    }
}