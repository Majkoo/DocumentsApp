using AutoMapper;
using DocumentsApp.Api.MappingProfiles.ValueResolvers;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.Auth;
using DocumentsApp.Shared.Dtos.Document;

namespace DocumentsApp.Api.MappingProfiles;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<RegisterDto, Account>();

        CreateMap<Document, GetDocumentDto>()
            .ForMember(dest => dest.Description,
                opt => opt.PreCondition(src => src.Description != null))
            .ForMember(dest => dest.AccountName,
                opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(dest => dest.AccessLevel, 
                opt => opt.MapFrom<AccessLevelResolver>())
            .ForMember(dest => dest.isCurrentUserACreator,
                opt => opt.MapFrom<IsCurrentUserACreatorResolver>())
            .ForMember(dest => dest.isModifiable, opt => 
                opt.MapFrom<IsModifiableResolver>());
        
        CreateMap<AddDocumentDto, Document>();

        CreateMap<UpdateDocumentDto, Document>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcM) => srcM != null));
    }
}