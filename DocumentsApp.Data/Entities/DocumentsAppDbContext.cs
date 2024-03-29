﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocumentsApp.Data.Entities;

public class DocumentsAppDbContext : IdentityDbContext
{
    public DocumentsAppDbContext(DbContextOptions<DocumentsAppDbContext> options): base(options) {}

    public DbSet<Account> Accounts { get; set; }
    public DbSet<DocumentAccessLevel> DocumentAccessLevels { get; set; }
    public DbSet<Document> Documents { get; set; }

    public DbSet<EncryptionKey> EncryptionKeys { get; set; }
}