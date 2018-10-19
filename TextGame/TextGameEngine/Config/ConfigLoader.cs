using System.Linq;
using TextGameEngine.Extensions;

namespace TextGameEngine.Config
{
    public class ConfigLoader
    {
        public static string GetFirstMapLevelFromConfig()
        {
            var file = IO.IOHelpers.GetConfigLines();
            string firstLevel = null;
            foreach (var line in file)
            {
                var part = line.ToLower().Split('=');
                if (part.Length != 2 || (part[0].Trim().Length > 1 && part[1].Trim() != "load"))
                {
                    continue;
                }

                switch (part[1].Trim())
                {
                    case "wall":
                        MapObjectDisplayChars.WallChar = part[0].FirstNoWhiteSpaceChar();
                        break;
                    case "player":
                        MapObjectDisplayChars.PlayerChar = part[0].FirstNoWhiteSpaceChar();
                        break;
                    case "exit":
                        MapObjectDisplayChars.ExitChar = part[0].FirstNoWhiteSpaceChar();
                        break;
                    case "floor":
                        MapObjectDisplayChars.FloorChar = part[0].FirstNoWhiteSpaceChar();
                        break;
                    case "load":
                        firstLevel = part[0].Trim();
                        break;
                }
            }

            return firstLevel;
        }
    }
}