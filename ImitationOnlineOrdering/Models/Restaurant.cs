using System.ComponentModel.DataAnnotations;

namespace ImitationOnlineOrdering
{
    public class Restaurant
    {

        public int RestaurantID { get; set; }
        [Display(Name = "Restaurant Name")]
        public required string RestaurantName { get; set; }        
        public Guid UserID { get; set; }

    }
}
