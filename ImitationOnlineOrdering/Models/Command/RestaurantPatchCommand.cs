namespace ImitationOnlineOrdering.Models
{
    public class RestaurantPatchCommand
    {

        public required int RestaurantID { get; set; }
        public string? RestaurantName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }        
        public string? Zip { get; set; }

    }
}
