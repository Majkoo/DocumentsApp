using System.Text;
using DocumentsApp.Api.Helpers;
using DocumentsApp.Api.Helpers.Interfaces;
using DocumentsApp.Api.MiddleWare;
using DocumentsApp.Api.Models;
using DocumentsApp.Api.Services;
using DocumentsApp.Api.Validators;
using DocumentsApp.Data.Auth;
using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.MappingProfiles;
using DocumentsApp.Data.MappingProfiles.ValueResolvers;
using DocumentsApp.Data.Repos;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Data.Services.Interfaces;
using DocumentsApp.Shared.Configurations;
using DocumentsApp.Shared.Dtos.AccountDtos;
using DocumentsApp.Shared.Dtos.Auth;
using DocumentsApp.Shared.Dtos.DocumentDtos;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sieve.Models;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<DocumentsAppDbContext>(opts =>
{
    var connString = builder.Configuration.GetConnectionString("DefaultConnection");
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

builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();

builder.Services.AddScoped<ErrorHandlingMiddleWare>();
builder.Services.AddScoped<LogMiddleWare>();

builder.Services.AddScoped<IValidator<AddDocumentDto>, AddDocumentDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserNameDto>, UpdateUserNameValidator>();
builder.Services.AddScoped<IValidator<SieveModel>, SieveModelValidator>();

builder.Services.AddScoped<IValidator<RegisterDto>, RegisterAccountDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginAccountDtoValidator>();

builder.Services.AddScoped<IDocumentRepo, DocumentRepo>();
builder.Services.AddScoped<IAccessLevelRepo, AccessLevelRepo>();
builder.Services.AddScoped<IEncryptionKeyRepo, EncryptionKeyRepo>();

builder.Services.Configure<EncryptionKeySettings>(builder.Configuration.GetSection(nameof(EncryptionKeySettings)));

builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IShareDocumentService, ShareDocumentService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEncryptionKeyService, EncryptionKeyService>();

// builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
// ^ I don't think it's needed - UserManager takes care of everything
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IMailHelper, MailHelper>();
builder.Services.AddScoped<IAesCipher, AesCipher>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));


builder.Services.AddScoped<DocumentsAppDbSeeder>();

builder.Services.AddScoped<AccessLevelResolver>();
builder.Services.AddScoped<IsCurrentUserACreatorResolver>();
builder.Services.AddScoped<IsModifiableResolver>();
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<DtoMappingProfile>(); });

builder.Services.AddScoped<IAuthenticationContextProvider, AuthenticationContextProvider>();
builder.Services.AddHostedService<EncryptionKeyGenerator>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<ISieveProcessor, DocumentsAppSieveProcessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(); // Use controllers for API endpoints


var jwtConfig = new JwtConfig();
builder.Configuration.GetSection("JwtConfig").Bind(jwtConfig);
builder.Services.AddSingleton(jwtConfig);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            SaveSigninToken = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
        };
    });
var authConfig = new AuthConfig();
builder.Configuration.GetSection("SignInConfig").Bind(authConfig);
builder.Services.AddSingleton(authConfig);
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.AllowedForNewUsers = authConfig.LockOutEnabledOnSignUp;
    options.SignIn.RequireConfirmedAccount = authConfig.RequireConfirmedAccount;
    options.SignIn.RequireConfirmedPhoneNumber = authConfig.RequireConfirmedPhoneNumber;
    options.SignIn.RequireConfirmedEmail = authConfig.RequireConfirmedEmail;
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();
app.UseMiddleware<LogMiddleWare>();
app.UseMiddleware<ErrorHandlingMiddleWare>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();