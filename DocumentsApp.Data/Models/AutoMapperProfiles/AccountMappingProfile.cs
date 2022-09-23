using AutoMapper;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Models.AccountModels;

namespace DocumentsApp.Data.Models.AutoMappers;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<RegisterUserDto, User>()
            .ForMember(reg => reg.PasswordHash, opt => opt.Ignore()); //hashing in AccountService
    }
}