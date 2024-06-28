using AutoMapper;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<MatchDto,MatchDomainModel>().ReverseMap(); // dto to domain and domain to dto
            //CreateMap<MatchDto,MatchDomainModel>() // dto to domain

            //Example to Map FullName to Name
            CreateMap<NotMatchDto,NotMatchDomainModel>()
                .ForMember(x=>x.Name,opt => opt.MapFrom(x=>x.FullName))
                .ReverseMap();

            CreateMap<Region,RegionDto>().ReverseMap();
            CreateMap<Region, AddRegionRequestDto>().ReverseMap();
            CreateMap<Region, UpdateRegionRequestDto>().ReverseMap();
            CreateMap<Walk,AddWalkRequestDto>().ReverseMap();
            CreateMap<Walk,WalkDto>().ReverseMap();
            CreateMap<Difficulty,DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();
        }
    }

    // Example match DTO and Domain Model
    public class MatchDto
    {
        public string? FullName { get; set; }
    }

    public class MatchDomainModel
    {
        public string? FullName { get; set; }
    }


    // Example not match DTO and Domain Model
    public class NotMatchDto
    {
        public string? FullName { get; set; }
    }

    public class NotMatchDomainModel
    {
        public string? Name { get; set; }
    }
}
