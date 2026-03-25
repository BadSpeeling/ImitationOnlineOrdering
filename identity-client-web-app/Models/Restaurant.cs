namespace identity_client_web_app
{
    public class Restaurant
    {

        public int RestaurantID { get; set; }
        public required string RestaurantName { get; set; }        
        public Guid OwnerID { get; set; }

    }
}
