using Ads.Web.Mvc.Models;
using App.Data.Entities;
using App.Models;
using AutoMapper;

namespace Ads.Web.Mvc.Mappings
{
    public class ViewModelMappingProfiles : Profile
    {
        public ViewModelMappingProfiles()
        {

            CreateMap<LoginDto, LoginViewModel>().ReverseMap();
            CreateMap<AdvertEntity,AdvertDto>().ReverseMap();
        }
    }
}
