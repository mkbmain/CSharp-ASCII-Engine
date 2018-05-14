namespace TextGameEngine.MapObjects
{
    public class MapObjectBase
    {
        protected MapObjectBase(char? customMapchar)
        {
            MapChar = customMapchar;
        }

        protected char? MapChar { get; set; }
        public bool CanStandOn { get; protected set; }

    }
}