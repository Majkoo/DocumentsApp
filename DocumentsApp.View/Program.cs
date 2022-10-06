using DocumentsApp.Data.Auth;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Dtos.DocumentDtos;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.MappingProfiles;
using DocumentsApp.Data.MiddleWare;
using DocumentsApp.Data.Repos;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Data.Services;
using DocumentsApp.Data.Sieve;
using DocumentsApp.Data.Validators.FluentValidation;
using DocumentsApp.Shared.Dtos.AccountDtos;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);

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

    opts.SignIn.RequireConfirmedEmail = false;
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

#endregion

#region Business Logic Services

builder.Services.AddScoped<IDocumentService, DocumentService>();

#endregion

#region Helper Services

builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

#endregion

#region Other Services

builder.Services.AddScoped<DocumentsAppDbSeeder>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DtoMappingProfile>();
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<ISieveProcessor, DocumentsAppSieveProcessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

#endregion

#endregion

#region Auth Config

builder.Services.AddAuthenticationCore();

#endregion

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


