using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameEngine.MapObjects
{
    public class LevelDto
    {
        public string Name { get; set; }

        public MapObjectBase[,] Map { get; set; }
    }
}
