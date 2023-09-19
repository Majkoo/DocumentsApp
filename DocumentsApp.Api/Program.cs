using System.Text;
using DocumentsApp.Api.Helpers;
using DocumentsApp.Api.Helpers.Interfaces;
using DocumentsApp.Api.MappingProfiles;
using DocumentsApp.Api.MappingProfiles.ValueResolvers;
using DocumentsApp.Api.MiddleWare;
using DocumentsApp.Api.Models;
using DocumentsApp.Api.Providers;
using DocumentsApp.Api.Services;
using DocumentsApp.Api.Services.Interfaces;
using DocumentsApp.Api.Validators;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Configurations;
using DocumentsApp.Shared.Dtos.Account;
using DocumentsApp.Shared.Dtos.Auth;
using DocumentsApp.Shared.Dtos.Document;
using DocumentsApp.Shared.Dtos.ShareDocument;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
    })
    .AddEntityFrameworkStores<DocumentsAppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();

builder.Services.AddScoped<ErrorHandlingMiddleWare>();
builder.Services.AddScoped<LogMiddleWare>();

builder.Services.AddScoped<IValidator<AddDocumentDto>, AddDocumentDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateDocumentDto>, UpdateDocumentDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserNameDto>, UpdateUserNameDtoValidator>();
builder.Services.AddScoped<IValidator<ShareDocumentDto>, ShareDocumentDtoValidator>();
builder.Services.AddScoped<IValidator<SieveModel>, SieveModelValidator>();

builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();

builder.Services.AddScoped<IDocumentRepo, DocumentRepo>();
builder.Services.AddScoped<IAccessLevelRepo, AccessLevelRepo>();
builder.Services.AddScoped<IEncryptionKeyRepo, EncryptionKeyRepo>();

// builder.Services.Configure<EncryptionKeySettings>(builder.Configuration.GetSection(nameof(EncryptionKeySettings)));

builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IShareDocumentService, ShareDocumentService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEncryptionKeyService, EncryptionKeyService>();

builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IMailHelper, MailHelper>();
builder.Services.AddScoped<IAesCipher, AesCipher>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

// builder.Services.AddScoped<DocumentsAppDbSeeder>();

builder.Services.AddScoped<AccessLevelResolver>();
builder.Services.AddScoped<IsCurrentUserACreatorResolver>();
builder.Services.AddScoped<IsModifiableResolver>();
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<DtoMappingProfile>(); });

builder.Services.AddScoped<IAuthenticationContextProvider, AuthenticationContextProvider>();
// builder.Services.AddHostedService<EncryptionKeyGenerator>();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));
builder.Services.AddScoped<ISieveProcessor, DocumentsAppSieveProcessor>();
builder.Services.AddHttpContextAccessor();

var jwtConfig = new JwtConfig();
builder.Configuration.GetSection("JwtConfig").Bind(jwtConfig);
builder.Services.AddSingleton(jwtConfig);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
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
        
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                // Call this to skip the default logic and avoid using the default response
                context.HandleResponse();
            
                var httpContext = context.HttpContext;
                var statusCode = StatusCodes.Status401Unauthorized;
            
                var routeData = httpContext.GetRouteData();
                var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
            
                var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var problemDetails = factory.CreateProblemDetails(httpContext, statusCode, detail: context.Error);
            
                var result = new ObjectResult(problemDetails) { StatusCode = statusCode };
                await result.ExecuteResultAsync(actionContext);
            }
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
builder.Services.AddAuthentication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
    {
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
        
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        option.EnablePersistAuthorization();
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<LogMiddleWare>();
app.UseMiddleware<ErrorHandlingMiddleWare>();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();