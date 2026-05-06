using System.ComponentModel.DataAnnotations;

namespace ImitationOnlineOrdering.Models
{
    public class FranchisePatchCommand
    {
        public required int FranchiseID { get; set; }
        public string? FranchiseName { get; set; }
    }
}
