namespace TextGameEngine.MapObjects
{
    public abstract class MapObjectBase
    {
        protected MapObjectBase(char? customMapchar)
        {
            MapChar = customMapchar;
        }

        // needs to be public used in json and mapping
        // ReSharper disable once MemberCanBeProtected.Global
        public char? MapChar { get; set; }
        public bool CanStandOn { get; protected set; }
    }
}