using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DocumentsApp.Data.Authentication;
using DocumentsApp.Data.Dtos.AccountDtos;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Repos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sieve.Services;

namespace DocumentsApp.Data.Services;

public interface IAccountService
{
    Task RegisterAccountAsync(RegisterAccountDto dto);
    Task UpdateUserNameAsync(UpdateUserNameDto dto);
    Task<string> GenerateJwtAsync(LoginAccountDto dto);
}

public class AccountService : IAccountService
{
    private readonly IAccountRepo _accountRepo;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IAccountContextService _accountContextService;

    public AccountService(IAccountRepo accountRepo, IMapper mapper, IPasswordHasher<Account> passwordHasher,
        AuthenticationSettings authenticationSettings, IAccountContextService accountContextService)
    {
        _accountRepo = accountRepo;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _accountContextService = accountContextService;
    }
    
    public async Task RegisterAccountAsync(RegisterAccountDto dto)
    {
        var user = _mapper.Map<Account>(dto);
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        await _accountRepo.InsertAccountAsync(user);
    }

    public async Task UpdateUserNameAsync(UpdateUserNameDto dto)
    {
        var user = await _accountRepo.GetAccountByEmailAsync(_accountContextService.User
            .FindFirst(c => c.Type == ClaimTypes.Email)?.Value);
        
        var passResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (passResult is PasswordVerificationResult.Failed) 
            throw new BadRequestException("Password is wrong");

        user.UserName = dto.NewUserName;

        await _accountRepo.UpdateAccountAsync(user);
    }

    public async Task UpdateUserPasswordAsync(UpdateUserPasswordDto dto)
    {
        
    }

    public async Task<string> GenerateJwtAsync(LoginAccountDto dto)
    {
        var account = await _accountRepo.GetAccountByEmailAsync(dto.Email);
        if (account is null) 
            throw new BadRequestException("Wrong username or password");

        var pwdResult = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, dto.Password);
        if (pwdResult is PasswordVerificationResult.Failed) 
            throw new BadRequestException("Wrong username or password");

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{account.UserName}"),
            new Claim(ClaimTypes.Email, $"{account.Email}")
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