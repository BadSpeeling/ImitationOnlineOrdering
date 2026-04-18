using System.ComponentModel.DataAnnotations;

namespace ImitationOnlineOrdering.Models
{
    public class Restaurant
    {

        public int RestaurantID { get; set; }
        [Display(Name = "Restaurant Name")]
        public required string RestaurantName { get; set; }        
        public Guid UserID { get; set; }
        public ICollection<MenuItem>? MenuItems { get; set; }

        public Restaurant Clone ()
        {
            return new Restaurant()
            {
                RestaurantID = RestaurantID,
                RestaurantName = RestaurantName,
                UserID = UserID,
                MenuItems = MenuItems?.Select(m => m.Clone()).ToList(),
            };
        }

    }
}
