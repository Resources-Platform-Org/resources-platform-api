using AutoMapper;
using Core.Entities;
using Api.Dtos.Universities;
using Api.Dtos.Majors;
using Api.Dtos.DocumentTypes;
using Api.Dtos.Professors;
using Api.Dtos.Courses;
using Api.Dtos.Resources;
using Api.Dtos.Users;
using Api.Dtos.Auth;
namespace Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // University Mappings :-
        CreateMap<University, UniversityResponseDto>();
        CreateMap<UniversityDto, University>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Majors, opt => opt.Ignore());

        // Major Mappings :-
        CreateMap<Major, MajorResponseDto>()
            .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.University!.Name));
        CreateMap<MajorDto, Major>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.University, opt => opt.Ignore())
            .ForMember(dest => dest.CourseMajors, opt => opt.Ignore());

        // Document Type Mappings :-
        CreateMap<DocumentType, DocumentTypeResponseDto>();
        CreateMap<DocumentTypeDto, DocumentType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Resources, opt => opt.Ignore());

        // Professor Mappings :-
        CreateMap<Professor, ProfessorResponseDto>();
        CreateMap<ProfessorDto, Professor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Phone, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Courses, opt => opt.Ignore());

        // Course Mappings :-
        CreateMap<Course, CourseResponseDto>()
            .ForMember(dest => dest.Professors, opt => opt.MapFrom(src => src.Professors.Select(p => p.Name).ToList()))
            .ForMember(dest => dest.CourseMajors, opt => opt.MapFrom(src => src.CourseMajors.Select(cm => cm.Major!.Name).ToList()))
            .ForMember(dest => dest.FileCount, opt => opt.MapFrom(src => src.Resources.Count));
        CreateMap<CourseDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CourseMajors, opt => opt.Ignore())
            .ForMember(dest => dest.Professors, opt => opt.Ignore())
            .ForMember(dest => dest.Resources, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Reverse Mappings :-
        CreateMap<CreateResourceDto, Resource>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StoredFileName, opt => opt.Ignore())
            .ForMember(dest => dest.Extension, opt => opt.Ignore())
            .ForMember(dest => dest.Path, opt => opt.Ignore())
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(x => false))
            .ForMember(dest => dest.DownloadsCount, opt => opt.MapFrom(x => 0))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DocumentType, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.Uploader, opt => opt.Ignore());

        CreateMap<Resource, ResourceResponseDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course!.Name))
            .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.DocumentType!.Name))
            .ForMember(dest => dest.UploaderName, opt => opt.MapFrom(src => src.Uploader!.Name))
            .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => src.Extension.ToString()))
            .ForMember(dest => dest.DownloadUrl, opt => opt.Ignore());

        // User Response Mappings :-
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        CreateMap<User, AuthResponseDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiresOn, opt => opt.Ignore());
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UploadedResources, opt => opt.Ignore());
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.Ignore());
        CreateMap<UpdateProfileDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UploadedResources, opt => opt.Ignore());
    }
}
