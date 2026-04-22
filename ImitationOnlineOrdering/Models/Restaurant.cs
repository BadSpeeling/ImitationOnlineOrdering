using System.ComponentModel.DataAnnotations;

namespace ImitationOnlineOrdering.Models
{
    public class Restaurant
    {

        public int RestaurantID { get; set; }
        [Display(Name = "Restaurant Name")]
        public required string RestaurantName { get; set; }        
        public Guid? RestaurantManagerUserID { get; set; }
        public int FranchiseID { get; set; }
        public ICollection<MenuItem>? MenuItems { get; set; }

        public Restaurant Clone ()
        {
            return new Restaurant()
            {
                RestaurantID = RestaurantID,
                RestaurantName = RestaurantName,
                RestaurantManagerUserID = RestaurantManagerUserID,
                FranchiseID = FranchiseID,
                MenuItems = MenuItems?.Select(m => m.Clone()).ToList(),
            };
        }

    }
}
