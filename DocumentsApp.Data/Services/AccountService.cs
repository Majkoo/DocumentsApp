using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DocumentsApp.Data.Authentication;
using DocumentsApp.Data.Dtos.EntityModels.AccountModels;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DocumentsApp.Data.Services;

public interface IAccountService
{
    Task RegisterUser(RegisterUserDto dto);
    Task<string> GenerateJwt(LoginUserDto dto);
}

public class AccountService : IAccountService
{
    private readonly DocumentsAppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;

    public AccountService(DocumentsAppDbContext context, IMapper mapper, IPasswordHasher<User> passwordHasher,
        AuthenticationSettings authenticationSettings)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
    }
    
    public async Task RegisterUser(RegisterUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<string> GenerateJwt(LoginUserDto dto)
    {
        var user =  await _context
            .Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user is null) throw new BadRequestException("Wrong username or password");

        var pwdResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        
        if (pwdResult is PasswordVerificationResult.Failed) throw new BadRequestException("Wrong username or password");

        //TODO?
        //include all access levels for documents in claim
        
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.UserName}")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred
        );
        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}