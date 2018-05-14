using TextGameEngine.Location;

namespace TextGameEngine.MapObjects
{
    /// <summary>
    /// Level Model contains all details of a level/
    /// </summary>
    public class LevelModel
    {
        /// <summary>
        /// Name Of Level
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Player last positon this is useful for when comming back to level
        /// but does mean we do not support multi ways to a level\map
        /// </summary>
        public Positon LastPlayerPos { get; set; }

        /// <summary>
        /// The map as a multi Dimensional array
        /// </summary>
        public MapObjectBase[,] Map { get; set; }
    }
}