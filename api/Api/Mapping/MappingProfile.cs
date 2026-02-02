using AutoMapper;
using Core.Entities;
using Api.Dtos.Universities;
namespace Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // University Mappings
        CreateMap<University, UniversityResponseDto>();
        CreateMap<UniversityDto, University>();

        // Major Mappings
    }
}
