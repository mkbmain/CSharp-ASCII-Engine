namespace TextGameEngine.MapObjects
{
    public class MapCustomObject : MapObjectBase
    {
        public string Name { get; set; }

        public string Message { get; set; }
        public int? AddItemId { get; set; }

        public int? RequiresItemId { get; set; }

        public override string ToString()
        {
            return MapChar?.ToString() ?? " ";
        }

        public MapCustomObject(bool canStandOn = true, string name = "", string message = "", char? customMapchar = null,
            int? additemId = null, int? requiresItemId = null) : base(customMapchar)
        {
            RequiresItemId = requiresItemId;
            Name = name;
            Message = message;
            CanStandOn = canStandOn;
            AddItemId = additemId;

        }
    }

    public class MapExitObject : MapObjectBase
    {
        public MapExitObject(char? customMapchar = null) : base(customMapchar)
        {
            CanStandOn = true;
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

    public class MapObjectBase
    {
        public MapObjectBase(char? customMapchar)
        {
            MapChar = customMapchar;
        }

        public char? MapChar { get; set; }
        public bool CanStandOn { get; set; }

    }

}