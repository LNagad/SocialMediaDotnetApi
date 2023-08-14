using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Mappings
{
  public class AutomapperProfile : Profile
  {

    public AutomapperProfile()
    {
      CreateMap<Post, PostDto>()
        .ReverseMap();
        //.ForMember(dest => dest.User, opt => opt.Ignore())
        //.ForMember(dest => dest.Comments, opt => opt.Ignore());
        
    }
  }
}
