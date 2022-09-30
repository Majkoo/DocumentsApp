using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Repos;

public class AccountRepo : IAccountRepo
{
    private readonly DocumentsAppDbContext _dbContext;

    public AccountRepo(DocumentsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Account> GetAccountByEmailAsync(string userEmail)
    {
        return await _dbContext.Accounts
            .SingleOrDefaultAsync(u => u.Email == userEmail);
    }

    public async Task<Account> InsertAccountAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
        return await _dbContext
            .Accounts
            .SingleOrDefaultAsync(u => u.Id == account.Id);
    }
}