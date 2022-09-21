using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DocumentsApp.Data.Entities;

public class DocumentsAppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DocumentsAppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("MariaDb");
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<DocAuthorization> DocAuthorizations { get; set; }
    public DbSet<Document> Documents { get; set; }
}