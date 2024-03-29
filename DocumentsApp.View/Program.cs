using DocumentsApp.Data.Auth;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.MappingProfiles;
using DocumentsApp.Data.MappingProfiles.ValueResolvers;
using DocumentsApp.Data.MiddleWare;
using DocumentsApp.Data.Repos;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Data.Services;
using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Data.Sieve;
using DocumentsApp.Data.Validators.FluentValidation;
using DocumentsApp.Shared.Configurations;
using DocumentsApp.Shared.Dtos.AccountDtos;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);

#region Frontend Services

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddScoped<ProtectedSessionStorage>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthenticationContextProvider, AuthenticationContextProvider>();

#endregion

#region Backend Services

#region DbContext & UserIdentity

builder.Services.AddDbContext<DocumentsAppDbContext>(opts =>
{
    var connString = builder.Configuration.GetConnectionString("LocalMariaDb");
    opts.UseMySql(
        connString,
        ServerVersion.AutoDetect(connString),
        x => x.MigrationsAssembly("DocumentsApp.Data")
    );
});

builder.Services.AddIdentity<Account, IdentityRole>(opts =>
    {
        opts.Password.RequiredLength = 8;
        opts.User.RequireUniqueEmail = true;

        opts.SignIn.RequireConfirmedEmail = true;
        opts.SignIn.RequireConfirmedAccount = false;
        opts.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<DocumentsAppDbContext>()
    .AddDefaultTokenProviders();

#endregion

#region Middleware

builder.Services.AddScoped<ErrorHandlingMiddleWare>();

#endregion

#region Validators

builder.Services.AddScoped<IValidator<RegisterAccountDto>, RegisterAccountDtoValidator>();
builder.Services.AddScoped<IValidator<AddDocumentDto>, AddDocumentDtoValidator>();
builder.Services.AddScoped<IValidator<LoginAccountDto>, LoginAccountDtoValidator>();

#endregion

#region Repositories

builder.Services.AddScoped<IDocumentRepo, DocumentRepo>();
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAccessLevelRepo, AccessLevelRepo>();
builder.Services.AddScoped<IEncryptionKeyRepo, EncryptionKeyRepo>();

builder.Services.Configure<EncryptionKeySettings>(builder.Configuration.GetSection(nameof(EncryptionKeySettings)));

#endregion

#region Business Logic Services

builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IShareDocumentService, ShareDocumentService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEncryptionKeyService, EncryptionKeyService>();

#endregion

#region Helper Services

builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IMailHelper, MailHelper>();
builder.Services.AddScoped<IAesCipher, AesCipher>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

#endregion

#region Other Services

builder.Services.AddScoped<DocumentsAppDbSeeder>();

builder.Services.AddScoped<AccessLevelResolver>();
builder.Services.AddScoped<IsCurrentUserACreatorResolver>();
builder.Services.AddScoped<IsModifiableResolver>();
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<DtoMappingProfile>(); });

builder.Services.AddHostedService<EncryptionKeyGenerator>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<ISieveProcessor, DocumentsAppSieveProcessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

#endregion

#endregion

#region Auth Config

builder.Services.AddAuthenticationCore();

#endregion

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DocumentsAppDbSeeder>();
    await seeder.SeedAsync();

    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<ErrorHandlingMiddleWare>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapControllers();
});

app.Run();