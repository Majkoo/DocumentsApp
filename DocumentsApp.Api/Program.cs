using System.Text;
using DocumentsApp.Api.Helper;
using DocumentsApp.Api.Model;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Data.Services;
using DocumentsApp.Data.Sieve;
using DocumentsApp.Shared.Configurations;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);


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

// builder.Services.AddScoped<ErrorHandlingMiddleWare>();


// builder.Services.AddScoped<IValidator<RegisterDto>, RegisterAccountDtoValidator>();
// builder.Services.AddScoped<IValidator<AddDocumentDto>, AddDocumentDtoValidator>();
// builder.Services.AddScoped<IValidator<LoginDto>, LoginAccountDtoValidator>();


// builder.Services.AddScoped<IDocumentRepo, DocumentRepo>();
// builder.Services.AddScoped<IAccessLevelRepo, AccessLevelRepo>();
// builder.Services.AddScoped<IEncryptionKeyRepo, EncryptionKeyRepo>();




builder.Services.Configure<EncryptionKeySettings>(builder.Configuration.GetSection(nameof(EncryptionKeySettings)));



// builder.Services.AddScoped<IDocumentService, DocumentService>();
// builder.Services.AddScoped<IShareDocumentService, ShareDocumentService>();
// builder.Services.AddScoped<IAccountService, AccountService>();
// builder.Services.AddScoped<IEncryptionKeyService, EncryptionKeyService>();


// builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
// builder.Services.AddScoped<IMailService, MailService>();
// builder.Services.AddScoped<IMailHelper, MailHelper>();
// builder.Services.AddScoped<IAesCipher, AesCipher>();
//
// builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));


builder.Services.AddScoped<DocumentsAppDbSeeder>();

// builder.Services.AddScoped<AccessLevelResolver>();
// builder.Services.AddScoped<IsCurrentUserACreatorResolver>();
// builder.Services.AddScoped<IsModifiableResolver>();
// builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<DtoMappingProfile>(); });

// builder.Services.AddHostedService<EncryptionKeyGenerator>();

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
var signInConfig = new AuthConfig();
builder.Configuration.GetSection("SignInConfig").Bind(signInConfig);
builder.Services.AddSingleton(signInConfig);
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = signInConfig.RequireConfirmedAccount;
    options.SignIn.RequireConfirmedPhoneNumber = signInConfig.RequireConfirmedPhoneNumber;
    options.SignIn.RequireConfirmedEmail = signInConfig.RequireConfirmedEmail;
});

builder.Services.AddAuthorization();




// builder.Services.AddScoped<>()

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();