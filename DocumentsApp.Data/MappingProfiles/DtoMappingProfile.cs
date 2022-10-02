using AutoMapper;
using DocumentsApp.Data.Dtos.AccountDtos;
using DocumentsApp.Data.Dtos.DocumentDtos;
using DocumentsApp.Data.Entities;

namespace DocumentsApp.Data.MappingProfiles;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<RegisterAccountDto, Account>();

        CreateMap<Document, GetDocumentDto>()
            .ForMember(dest => dest.Description, opt => opt.PreCondition(src => src.Description != null))
            .ForPath(getD => getD.AccountName, opt => opt.MapFrom(d => d.Name));
        
        CreateMap<AddDocumentDto, Document>();

        CreateMap<UpdateDocumentDto, Document>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcM) => srcM != null));
    }
}