using AutoMapper;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Models.AccountModels;
using DocumentsApp.Data.Models.EntityModels.DocumentModels;

namespace DocumentsApp.Data.Models;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<RegisterUserDto, User>()
            .ForMember(reg => reg.PasswordHash, opt => opt.Ignore()); //hashing in AccountService
        
        CreateMap<AddDocumentDto, Document>();
    }
}