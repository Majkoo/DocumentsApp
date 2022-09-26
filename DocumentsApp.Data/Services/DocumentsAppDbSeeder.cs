using System.Collections.ObjectModel;
using DocumentsApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Data.Services;

public class DocumentsAppDbSeeder
{
    private readonly DocumentsAppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public DocumentsAppDbSeeder(DocumentsAppDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        if (!_context.Users.Any()) await _context.AddAsync(GetUser());
        await _context.SaveChangesAsync();
    }
    
    private User GetUser()
    {
        var testUser = new User()
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
                },
                new Document()
                {
                    Name = "testdoc2",
                    Description = "test document description",
                    Content = "lorem100",
                }
            }
        };
        testUser.PasswordHash = _passwordHasher.HashPassword(testUser, "password"); 
        return testUser;
    }
    
}