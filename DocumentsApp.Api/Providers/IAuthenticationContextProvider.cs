namespace DocumentsApp.Api.Providers;

public interface IAuthenticationContextProvider
{
    string GetUserId();
    string GetUserEmail();
    string GetUserName();
}