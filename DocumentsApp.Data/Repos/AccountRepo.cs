using DocumentsApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Repos;

public interface IAccountRepo
{
    Task<User> GetUserByEmailAsync(string userEmail);
    Task<User> InsertUserAsync(User user);
}

public class AccountRepo : IAccountRepo
{
    private readonly DocumentsAppDbContext _dbContext;

    public AccountRepo(DocumentsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User> GetUserByEmailAsync(string userEmail)
    {
        return await _dbContext
            .Users
            .SingleOrDefaultAsync(u => u.Email == userEmail);
    }

    public async Task<User> InsertUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return await _dbContext
            .Users
            .SingleOrDefaultAsync(u => u.Id == user.Id);
    }
}