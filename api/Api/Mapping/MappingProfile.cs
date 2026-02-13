using AutoMapper;
using Core.Entities;
using Api.Dtos.Universities;
using Api.Dtos.Majors;
using Api.Dtos.DocumentTypes;
using Api.Dtos.Professors;
using Api.Dtos.Courses;
using Api.Dtos.Resources;
using Api.Dtos.Resource;
using Api.Dtos.Users;
using Api.Dtos.Auth;
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
        CreateMap<Professor, ProfessorResponseDto>();
        CreateMap<ProfessorDto, Professor>();

        // Course Mappings :-
        CreateMap<CourseResponseDto, Course>();
        CreateMap<CourseDto, Course>();

        // Reverse Mappings :-
        CreateMap<CreateResourceDto, Resource>()
            .ForMember(dest => dest.StoredFileName, opt => opt.Ignore())
            .ForMember(dest => dest.Extension, opt => opt.Ignore())
            .ForMember(dest => dest.Path, opt => opt.Ignore())
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(x => false))
            .ForMember(dest => dest.DownloadsCount, opt => opt.MapFrom(x => 0));

        CreateMap<Resource, ResourceResponseDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course!.Name))
            .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.DocumentType!.Name))
            .ForMember(dest => dest.UploadederName, opt => opt.MapFrom(src => src.Uploader!.Name))
            .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => src.Extension.ToString()));

        // User Response Mappings :-
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        CreateMap<User, AuthResponseDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Token, opt => opt.Ignore());
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore());
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.Ignore());
        CreateMap<UpdateProfileDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
    }
}
