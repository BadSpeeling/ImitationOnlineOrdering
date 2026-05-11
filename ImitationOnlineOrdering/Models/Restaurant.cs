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

        public required string StreetAddress { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Zip { get; set; }

        public ICollection<MenuItem>? MenuItems { get; set; }

        public Franchise? Franchise { get; set; }

    }
}
