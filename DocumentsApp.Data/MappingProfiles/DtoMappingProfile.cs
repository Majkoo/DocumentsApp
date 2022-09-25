using AutoMapper;
using DocumentsApp.Data.Dtos.EntityModels.AccountModels;
using DocumentsApp.Data.Dtos.EntityModels.DocumentModels;
using DocumentsApp.Data.Entities;

namespace DocumentsApp.Data.MappingProfiles;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<RegisterUserDto, User>()
            .ForMember(reg => reg.PasswordHash, opt => opt.Ignore()); //hashing in AccountService

        CreateMap<Document, GetDocumentDto>()
            .ForMember(m => m.Description, opt => opt.PreCondition((src, dest, srcM) => srcM != null));
        
        CreateMap<AddDocumentDto, Document>();

        CreateMap<UpdateDocumentDto, Document>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcM) => srcM != null));
    }
}