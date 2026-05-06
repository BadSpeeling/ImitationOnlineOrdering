namespace ImitationOnlineOrdering.Models
{
    public class RestaurantPatchCommand
    {

        public required int RestaurantID { get; set; }
        public string? RestaurantName { get; set; }        

    }
}
