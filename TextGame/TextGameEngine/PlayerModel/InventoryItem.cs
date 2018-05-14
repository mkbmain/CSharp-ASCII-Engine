namespace TextGameEngine.PlayerModel
{
    /// <summary>
    /// Inventory item
    /// think this is a bit of a waste with both name and id but suppose could have 5 key's that are all unique
    /// might make qty a option 
    /// </summary>
    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
