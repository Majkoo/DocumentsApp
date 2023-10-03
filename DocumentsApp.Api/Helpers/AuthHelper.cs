using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DocumentsApp.Api.Helpers.Interfaces;
using DocumentsApp.Api.Models;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Dtos.Auth;
using DocumentsApp.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DocumentsApp.Api.Helpers;

public class AuthHelper : IAuthHelper
{
    private readonly SignInManager<Account> _signInManager;
    private readonly UserManager<Account> _userManager;
    private readonly IAccountRepo _accountRepo;
    private readonly JwtConfig _jwtConfig;

    public AuthHelper(
        SignInManager<Account> signInManager,
        UserManager<Account> userManager,
        IAccountRepo accountRepo,
        JwtConfig jwtConfig
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _accountRepo = accountRepo;
        _jwtConfig = jwtConfig;
    }

    public async Task<JwtDataDto> SignIn(LoginDto loginDto)
    {
        var acc =
            await _accountRepo.GetAccountByEmailAsync(loginDto.Email) ??
            await _accountRepo.GetAccountByUsernameAsync(loginDto.Email);

        if (acc is null) throw new UnauthorizedException("Invalid login credentials");

        var signInResult = await _signInManager.CheckPasswordSignInAsync(acc, loginDto.Password, false);

        if (signInResult.IsLockedOut) throw new UnauthorizedException("Account is locked out");
        if (signInResult.RequiresTwoFactor) throw new UnauthorizedException("Multi factor authentication is required");
        if (signInResult.IsNotAllowed) throw new UnauthorizedException("Not allowed");

        if (signInResult.Succeeded) return await GenerateJwtData(acc);

        throw new UnauthorizedException("Invalid login credentials");
    }

    public async Task<JwtDataDto> RefreshToken(string refreshToken)
    {
        var acc = await _accountRepo.GetAccountByRefreshToken(refreshToken);

        if (acc is null) throw new UnauthorizedException("Invalid refresh token");
        if (acc.RefreshTokenExp < DateTime.Now) throw new UnauthorizedException("Session expired");

        return await GenerateJwtData(acc);
    }

    public async Task<bool> SignUp(RegisterDto registerDto)
    {
        var newUser = new Account
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
        };

        var createResult = await _userManager.CreateAsync(newUser, registerDto.Password);
        if (createResult.Errors.Any())
            throw new BadRequestException(nameof(SignUp), createResult.Errors.First().Description);

        if (!createResult.Succeeded)
        {
            throw new BadRequestException(nameof(SignUp), "Failed to create user account.");
        }

        return createResult.Succeeded;
    }


    #region Private methods

    private async Task<JwtDataDto> GenerateJwtData(Account acc)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, acc.Id),
            new Claim(ClaimTypes.Name, acc.UserName!),
            new Claim(ClaimTypes.Email, acc.Email!),
            new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig.Issuer),
            new Claim(JwtRegisteredClaimNames.Aud, _jwtConfig.Audience)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationMinutes),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtAuthToken = tokenHandler.WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        acc.RefreshToken = refreshToken;
        acc.RefreshTokenExp = DateTime.Now.AddHours(_jwtConfig.RefreshExpirationHours);

        await _userManager.UpdateAsync(acc);

        var jwtDataDto = new JwtDataDto
        {
            AuthToken = jwtAuthToken,
            RefreshToken = refreshToken,
            Claims = new JwtClaimsDto
            {
                Username = acc.UserName!,
                UserEmail = acc.Email!,
            }
        };

        return jwtDataDto;
    }

    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
    }

    #endregion
}