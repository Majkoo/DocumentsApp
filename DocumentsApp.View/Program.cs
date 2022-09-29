using System.Text;
using DocumentsApp.Data;
using DocumentsApp.Data.Authentication;
using DocumentsApp.Data.Dtos.AccountDtos;
using DocumentsApp.Data.Dtos.DocumentDtos;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.MappingProfiles;
using DocumentsApp.Data.MiddleWare;
using DocumentsApp.Data.Repos;
using DocumentsApp.Data.Services;
using DocumentsApp.Data.Validators.FluentValidation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

#region Backend Services

#region DbContext

builder.Services.AddDbContext<DocumentsAppDbContext>(opts =>
{
    var connString = builder.Configuration.GetConnectionString("LocalMariaDb");
    opts.UseMySql(
        connString,
        ServerVersion.AutoDetect(connString),
        x => x.MigrationsAssembly("DocumentsApp.Data")
    );
});

#endregion

#region Auth Config


var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = "Bearer";
    opt.DefaultScheme = "Bearer";
    opt.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

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

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

#endregion

#region Helper Services

builder.Services.AddScoped<IAccountContextService, AccountContextService>();
builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

#endregion

#region Other Services

builder.Services.AddScoped<DocumentsAppDbSeeder>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DtoMappingProfile>();
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

#endregion

#endregion

#region Frontend Services

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

#endregion

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseStaticFiles();

app.UseMiddleware<ErrorHandlingMiddleWare>();

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<DocumentsAppDbSeeder>();
await seeder.SeedAsync();

app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();


