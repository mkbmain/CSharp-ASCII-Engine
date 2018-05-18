using System.Linq;

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
                if (part.Length != 2 || (part[1].Trim().Length > 1 && part[0].Trim() != "load"))
                {
                    continue;
                }
                switch (part[0].Trim())
                {
                    case "wall":
                        MapObjectDisplayChars.WallChar = part[1].Trim().FirstOrDefault();
                        break;
                    case "player":
                        MapObjectDisplayChars.PlayerChar = part[1].Trim().FirstOrDefault();
                        break;
                    case "exit":
                        MapObjectDisplayChars.ExitChar = part[1].Trim().FirstOrDefault();
                        break;
                    case "floor":
                        MapObjectDisplayChars.FloorChar = part[1].Trim().FirstOrDefault();
                        break;
                    case "load":
                        firstLevel = part[1].Trim();
                        break;
                }
            }
            return firstLevel;
        }
    }
}
