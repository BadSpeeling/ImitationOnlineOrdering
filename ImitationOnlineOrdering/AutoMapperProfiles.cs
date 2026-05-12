using AutoMapper;
using ImitationOnlineOrdering.Models;

namespace ImitationOnlineOrdering
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Restaurant, RestaurantPatchCommand>();
            CreateMap<Franchise, FranchisePatchCommand>();
        }
    }
}
