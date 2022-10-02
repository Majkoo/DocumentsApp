using DocumentsApp.Shared.Dtos.AccountDtos;

namespace DocumentsApp.Data.Services.Interfaces;

public interface IIdentityService
{
    Task<bool> SignIn(LoginAccountDto loginDto);
    Task<bool> SignUp(RegisterAccountDto registerAccountDto);
    Task<bool> SignOut();
}