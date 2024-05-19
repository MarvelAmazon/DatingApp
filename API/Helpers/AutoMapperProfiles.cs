namespace API;

using API.Entities;
using AutoMapper;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // This line creates a mapping from the AppUser class to the MemberDto class.
        // CreateMap<AppUser, MemberDto>();
        CreateMap<AppUser, MemberDto>()
           .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
           .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url));
        // This line creates a mapping from the Photo class to the PhotoDto class.
         CreateMap<Photo, PhotoDto>();
    }
}