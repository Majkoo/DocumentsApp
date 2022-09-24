using System.Text;
using DocumentsApp.Data;
using DocumentsApp.Data.Authentication;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Models;
using DocumentsApp.Data.Models.AccountModels;
using DocumentsApp.Data.Models.FluentValidation;
using DocumentsApp.Data.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddControllers();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<DocumentsAppDbContext>();
builder.Services.AddScoped<DocumentsAppDbSeeder>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DtoMappingProfile>();
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

//Jwt issuing
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<DocumentsAppDbSeeder>();
await seeder.SeedAsync();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();


