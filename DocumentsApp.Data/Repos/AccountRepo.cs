using DocumentsApp.Data.Dtos.AccountDtos;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Repos;

public interface IAccountRepo
{
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task<Account> UpdateAccountAsync(Account account);
    Task<Account> GetAccountByEmailAsync(string userEmail);
    Task<Account> InsertAccountAsync(Account account);
}

public class AccountRepo : IAccountRepo
{
    private readonly DocumentsAppDbContext _dbContext;

    public AccountRepo(DocumentsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await _dbContext
            .Accounts
            .ToListAsync();
    }

    public async Task<Account> UpdateAccountAsync(Account account)
    {
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();

        return await _dbContext
            .Accounts
            .FirstOrDefaultAsync(a => a.Id == account.Id);
    }

    public async Task<Account> GetAccountByEmailAsync(string userEmail)
    {
        return await _dbContext
            .Accounts
            .SingleOrDefaultAsync(a => a.Email == userEmail);
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