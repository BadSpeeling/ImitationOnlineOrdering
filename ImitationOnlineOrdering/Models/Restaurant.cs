using System.ComponentModel.DataAnnotations;

namespace ImitationOnlineOrdering.Models
{
    public class Restaurant
    {

        public int RestaurantID { get; set; }
        [Display(Name = "Restaurant Name")]
        public Guid? RestaurantManagerUserID { get; set; }
        public int FranchiseID { get; set; }

        [Display(Name = "Street Address")]        
        public required string StreetAddress { get; set; }
        public required string City { get; set; }
        [Length(2, 2)]
        public required string State { get; set; }
        [Length(5, 5)]
        public required string Zip { get; set; }

        public ICollection<MenuItem>? MenuItems { get; set; }

        public Franchise? Franchise { get; set; }

    }
}
