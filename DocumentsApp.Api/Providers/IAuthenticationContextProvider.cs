namespace DocumentsApp.Data.Auth.Interfaces;

public interface IAuthenticationContextProvider
{
    string GetUserId();
    string GetUserEmail();
    string GetUserName();
}