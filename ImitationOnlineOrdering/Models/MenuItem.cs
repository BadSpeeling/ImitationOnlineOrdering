namespace ImitationOnlineOrdering.Models
{
    public class MenuItem
    {

        public int MenuItemID { get; set; }
        public required string MenuItemName { get; set; }
        public required decimal Price { get; set; }        
        public required int RestaurantID { get; set; }        

        public Restaurant? Restaurant { get; set; }

    }
}
