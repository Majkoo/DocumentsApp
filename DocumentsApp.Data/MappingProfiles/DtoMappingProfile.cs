using AutoMapper;
using DocumentsApp.Data.Auth;
using DocumentsApp.Data.Dtos.DocumentDtos;
using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.AccountDtos;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace DocumentsApp.Data.MappingProfiles;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<RegisterAccountDto, Account>();

        CreateMap<Document, GetDocumentDto>()
            .ForMember(dest => dest.Description, opt => opt.PreCondition(src => src.Description != null))
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.UserName));
        
        CreateMap<AddDocumentDto, Document>();

        CreateMap<UpdateDocumentDto, Document>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcM) => srcM != null));
    }
}