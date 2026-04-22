using System.ComponentModel.DataAnnotations;

namespace ImitationOnlineOrdering.Models
{
    public class Franchise
    {

        public int FranchiseID { get; set; }
        [Display(Name = "Franchise Name")]
        public required string FranchiseName { get; set; }
        public Guid? FranchiseOwnerUserID { get; set; }

        public Franchise Clone()
        {
            return new Franchise()
            {
                FranchiseID = FranchiseID,
                FranchiseName = FranchiseName,
                FranchiseOwnerUserID = FranchiseOwnerUserID
            };
        }

    }
}
