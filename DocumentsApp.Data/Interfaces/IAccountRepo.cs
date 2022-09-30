using DocumentsApp.Data.Entities;

namespace DocumentsApp.Data.Interfaces;

public interface IAccountRepo
{
    Task<Account> GetAccountByEmailAsync(string userEmail);
    Task<Account> InsertAccountAsync(Account account);
}