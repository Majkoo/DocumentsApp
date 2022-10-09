using DocumentsApp.Data.Entities;

namespace DocumentsApp.Data.Repos.Interfaces;

public interface IAccountRepo
{
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account> GetAccountByIdAsync(string userId);
    Task<Account> GetAccountByEmailAsync(string userEmail);
    Task<Account> GetAccountByUsernameAsync(string userName);
    Task<Account> InsertAccountAsync(Account account);
    
    Task<IEnumerable<Account>> GetAllAccountsForDocumentAsync(string documentId);
}