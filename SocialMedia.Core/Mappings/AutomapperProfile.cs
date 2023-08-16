using AutoMapper;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.DTOs;

namespace SocialMedia.Core.Mappings
{
  public class AutomapperProfile : Profile
  {

    public AutomapperProfile()
    {
      CreateMap<Post, PostDto>()
        .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
        .ReverseMap()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PostId));
        //.ForMember(dest => dest.User, opt => opt.Ignore())
        //.ForMember(dest => dest.Comments, opt => opt.Ignore());

    }
  }
}
