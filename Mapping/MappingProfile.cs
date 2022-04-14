using LibAPI.Entities;
using LibAPI.Models;
using LibAPI.Models.Entity;
using AutoMapper;

namespace LibAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserData>().ReverseMap();
            CreateMap<ApplicationRole, ApplicationRoleData>().ReverseMap();
            CreateMap<ApplicationUserData, TableUserModel>().ReverseMap();
        }
    }
}