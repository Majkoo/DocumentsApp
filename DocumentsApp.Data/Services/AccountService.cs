﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DocumentsApp.Data.Authentication;
using DocumentsApp.Data.Dtos.EntityModels.AccountModels;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DocumentsApp.Data.Services;

public interface IAccountService
{
    Task RegisterUserAsync(RegisterUserDto dto);
    Task<string> GenerateJwtAsync(LoginUserDto dto);
}

public class AccountService : IAccountService
{
    private readonly IAccountRepo _accountRepo;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;

    public AccountService(IAccountRepo accountRepo, IMapper mapper, IPasswordHasher<User> passwordHasher,
        AuthenticationSettings authenticationSettings)
    {
        _accountRepo = accountRepo;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
    }
    
    public async Task RegisterUserAsync(RegisterUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        await _accountRepo.InsertUserAsync(user);
    }

    public async Task<string> GenerateJwtAsync(LoginUserDto dto)
    {
        var user = await _accountRepo.GetUserByEmailAsync(dto.Email);

        if (user is null) throw new BadRequestException("Wrong username or password");

        var pwdResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        
        if (pwdResult is PasswordVerificationResult.Failed) throw new BadRequestException("Wrong username or password");

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