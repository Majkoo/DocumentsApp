namespace DocumentsApp.Data.Auth.Interfaces;

public interface IAuthenticationContextProvider
{
    Task<string> GetUserId();
    Task<string> GetUserEmail();
    Task<string> GetUserName();
}