namespace TextGameEngine.MapObjects
{


    public class MapExitObject : MapObjectBase
    {
        public override string ToString()
        {
            return MapChar?.ToString() ?? "#";
        }

        public string GoToLevel { get; set; }
        public MapExitObject(char? customMapchar = null,string go = "") : base(customMapchar)
        {
            CanStandOn = true;
            GoToLevel = go;
        }
    }

    public class WallObject : MapObjectBase
    {
        public override string ToString()
        {
            return MapChar?.ToString() ?? "█";
        }

        public WallObject(char? customMapchar = null) : base(customMapchar)
        {
            CanStandOn = false;
            MapChar = customMapchar;
        }
    }

    public class PlayerStartObject : MapObjectBase
    {
        public override string ToString()
        {
            return MapChar?.ToString() ?? "º";
        }

        public PlayerStartObject(char? customMapchar = null) : base(customMapchar)
        {
            CanStandOn = true;

        }
    }

    public class FloorMapObject : MapObjectBase
    {
        public override string ToString()
        {
            return MapChar?.ToString() ?? " ";
        }

        public FloorMapObject(char? customMapchar = ' ') : base(customMapchar)
        {
            CanStandOn = true;

        }
    }
}