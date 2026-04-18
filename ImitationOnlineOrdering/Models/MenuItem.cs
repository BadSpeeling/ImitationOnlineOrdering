namespace ImitationOnlineOrdering.Models
{
    public class MenuItem
    {

        public int MenuItemID { get; set; }
        public required string MenuItemName { get; set; }
        public decimal Price { get; set; }        
        public int RestaurantID { get; set; }

        public MenuItem Clone ()
        {
            return new MenuItem()
            {
                MenuItemID = MenuItemID,
                MenuItemName = MenuItemName,
                Price = Price,
                RestaurantID = RestaurantID,
            };
        }

    }
}
