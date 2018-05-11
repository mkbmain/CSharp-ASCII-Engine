using TextGameEngine.MapObjects;

namespace TextGameEngine.Location.DTO
{
    public class ObjectsAroundPointModel
    {
        public MapObjectBase [,] AllAround { get; set; }

        public MapObjectBase Up => AllAround[1, 0];
        public MapObjectBase Down => AllAround[1, 2];
        public MapObjectBase Left => AllAround[0, 1];
        public MapObjectBase Right => AllAround[2, 1];
    }
}
