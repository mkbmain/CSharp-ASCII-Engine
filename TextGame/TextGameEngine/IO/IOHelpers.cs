using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextGameEngine.IO
{
    public class IOHelpers
    {
        public static Dictionary<string, string> GetMapFiles(string path = "")
        {

            return Directory.GetFiles(string.IsNullOrWhiteSpace(path) ? Environment.CurrentDirectory : path)
                 .Where(f => f.EndsWith(".TGL"))
                 .ToDictionary(f => f.Replace(Environment.CurrentDirectory, "").Substring(1), f => f);
        }
    }
}
