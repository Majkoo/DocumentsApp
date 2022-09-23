using DocumentsApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data;

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
            Email = "seededUser@email.com",
            PasswordHash = "placeholder",
            Documents = new List<Document>()
            {
                new Document()
                {
                    Description = "testdocument1ooooooooooooh",
                    Content = "oooooooooooooooooooooooooooooooh",
                },
                new Document()
                {
                    Description = "testdocument2",
                    Content = "ok",
                }
            }
        };
        testUser.PasswordHash = _passwordHasher.HashPassword(testUser, "admin"); //admin is pwd
        return testUser;
    }
    
}