using AutoMapper;
using Core.Entities;
using Api.Dtos.Universities;
using Api.Dtos.Majors;
using Api.Dtos.DocumentTypes;
namespace Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // University Mappings :-
        CreateMap<University, UniversityResponseDto>();
        CreateMap<UniversityDto, University>();

        // Major Mappings :-
        CreateMap<Major, MajorResponseDto>();
        CreateMap<MajorDto, Major>();

        // Document Type Mappings :-
        CreateMap<DocumentType, DocumentTypeResponseDto>();
        CreateMap<DocumentTypeDto, DocumentType>();

        // Professor Mappings :-

    }
}
