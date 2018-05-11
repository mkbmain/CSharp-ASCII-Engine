using System;
using System.Collections.Generic;
using System.Text;

namespace TextGameEngine.MapObjects
{
    public class MapExitObject : MapObjectBase
    {
        public MapExitObject()
        {
            CanStandOn = true;
        }
    }
    public class WallObject : MapObjectBase
    {
        public override string ToString()
        {
            return "█";
        }
        public WallObject()
        {
            CanStandOn = false;
        }
    }
    public class PlayerMapObject : MapObjectBase
    {
        public override string ToString()
        {
            return "º";
        }
        public PlayerMapObject()
        {
            CanStandOn = true;
        }
    }
    public class FloorMapObject : MapObjectBase
    {
        public override string ToString()
        {
            return " ";
        }
        public FloorMapObject()
        {
            CanStandOn = true;
        }
    }
    public class MapObjectBase
    {
        public bool CanStandOn { get; set; }

    }

    
}
