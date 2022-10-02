using DocumentsApp.Data.Entities;

namespace DocumentsApp.Data.Repos.Interfaces;

public interface IAccountRepo
{
    Task<Account> GetAccountByEmailAsync(string userEmail);
    Task<Account> GetAccountByUsernameAsync(string userName);
    Task<Account> InsertAccountAsync(Account account);
}