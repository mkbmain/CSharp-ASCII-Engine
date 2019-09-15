using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextGameEngine.IO
{
    public static class IOHelpers
    {
        public static Dictionary<string, string> GetMapFiles(string path = "")
        {

            return Directory.GetFiles(string.IsNullOrWhiteSpace(path) ? Environment.CurrentDirectory : path)
                 .Where(f => f.EndsWith(".TGL"))
                 .ToDictionary(f => f.Replace(Environment.CurrentDirectory, "").Substring(1), f => f);
        }

        public static IEnumerable<string> GetConfigLines(string path = "")
        {
            var file = Directory.GetFiles(string.IsNullOrWhiteSpace(path) 
                                          ? Environment.CurrentDirectory 
                                          : path).FirstOrDefault(f => f.EndsWith(".load"));
            return string.IsNullOrEmpty(file) ? new string[0] : File.ReadAllLines(file);
        }
    }
}
