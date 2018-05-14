namespace TextGameEngine.MapObjects
{
    public class MapCustomObject : MapObjectBase
    {
        public string Name { get; }

        public string Message { get; }
        public int? AddItemId { get; }

        public int? RequiresItemId { get; }

        public override string ToString()
        {
            return MapChar?.ToString() ?? " ";
        }

        public MapCustomObject(bool canStandOn = true, string name = "", string message = "", 
            char? customMapchar = null,int? additemId = null, int? requiresItemId = null) : base(customMapchar)
        {
            RequiresItemId = requiresItemId;
            Name = name;
            Message = message;
            CanStandOn = canStandOn;
            AddItemId = additemId;
        }
    }
}