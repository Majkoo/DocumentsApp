using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Repos;

public class AccountRepo : IAccountRepo
{
    private readonly DocumentsAppDbContext _dbContext;

    public AccountRepo(DocumentsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await _dbContext.Accounts
            .Include(a => a.Documents)
            .ToListAsync();
    }
    
    public async Task<Account> GetAccountByIdAsync(string userId)
    {
        return await _dbContext.Accounts
            .SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<Account> GetAccountByEmailAsync(string userEmail)
    {
        return await _dbContext.Accounts
            .SingleOrDefaultAsync(u => u.Email == userEmail);
    }

    public async Task<Account> GetAccountByUsernameAsync(string userName)
    {
        return await _dbContext.Accounts
            .SingleOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<Account> InsertAccountAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
        return await _dbContext
            .Accounts
            .SingleOrDefaultAsync(u => u.Id == account.Id);
    }

    public async Task<IEnumerable<Account>> GetAllAccountsForDocumentAsync(string documentId)
    {
        var accessLevels = _dbContext.DocumentAccessLevels
            .Where(a => a.DocumentId == documentId);

        return await accessLevels
            .Select(a => GetAccountByIdAsync(a.AccountId).Result)
            .ToListAsync();
    }

}