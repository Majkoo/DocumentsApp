using System.Collections.ObjectModel;
using DocumentsApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Data.Services;

public class DocumentsAppDbSeeder
{
    private readonly DocumentsAppDbContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public DocumentsAppDbSeeder(DocumentsAppDbContext context, IPasswordHasher<Account> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        if (!_context.Accounts.Any()) await _context.AddAsync(GetAccount());
        await _context.SaveChangesAsync();
    }
    
    private Account GetAccount()
    {
        var testAccount = new Account()
        {
            UserName = "SeededUser",
            Email = "seededuser@email.com",
            PasswordHash = "placeholder",
            Documents = new Collection<Document>()
            {
                new Document()
                {
                    Name = "testdoc1",
                    Description = "test document description",
                    Content = "lorem100",
                    DateCreated = DateTime.Now
                },
                new Document()
                {
                    Name = "testdoc2",
                    Description = "test document description",
                    Content = "lorem100",
                    DateCreated = DateTime.Now
                }
            }
        };
        testAccount.PasswordHash = _passwordHasher.HashPassword(testAccount, "password"); 
        return testAccount;
    }
    
}