namespace ImitationOnlineOrdering
{
    public class MenuItem
    {

        public int MenuItemID { get; set; }
        public required string MenuItemName { get; set; }
        public decimal Price { get; set; }
        public int RestaurantID { get; set; }

    }
}
