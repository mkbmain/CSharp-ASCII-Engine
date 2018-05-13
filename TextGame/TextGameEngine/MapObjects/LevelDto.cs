using System;
using System.Collections.Generic;
using System.Text;
using TextGameEngine.Location;

namespace TextGameEngine.MapObjects
{
    public class LevelDto
    {
        public string Name { get; set; }

        public Positon LastPlayerPos { get; set; }

        public MapObjectBase[,] Map { get; set; }
    }
}
