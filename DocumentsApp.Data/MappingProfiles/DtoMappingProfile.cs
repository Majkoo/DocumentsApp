using AutoMapper;
using DocumentsApp.Data.Dtos.AccountDtos;
using DocumentsApp.Data.Dtos.DocumentDtos;
using DocumentsApp.Data.Entities;

namespace DocumentsApp.Data.MappingProfiles;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        // nie musisz uzywac ignore,
        // jak automapper sam nie znajdzie takiego samego pola to automatycznie je zignoruje

        CreateMap<RegisterUserDto, User>()
            .ForMember(
                reg => reg.PasswordHash,
                opt => opt.Ignore()); //hashing in AccountService

        // jezeli dobrze rozumiem,
        // to context chyba nie moze nigdy byc nullem (?)
        // nie chodzilo o src.Content?

        CreateMap<Document, GetDocumentDto>()
            .ForMember(
                m => m.Description,
                opt => opt.PreCondition(
                    (src, dest, srcM) => srcM != null));
        
        CreateMap<AddDocumentDto, Document>();

        CreateMap<UpdateDocumentDto, Document>()
            .ForAllMembers(opt => opt.Condition(
                (src, dest, srcM) => srcM != null));
    }
}