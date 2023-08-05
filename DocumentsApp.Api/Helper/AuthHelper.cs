using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DocumentsApp.Api.Model;
using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Exceptions;
using DocumentsApp.Data.Repos;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DocumentsApp.Api.Helper;

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
        JwtConfig options
        )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _accountRepo = accountRepo;
        _jwtConfig = options;
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

        if (signInResult.Succeeded)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, acc.Id),
                new Claim(ClaimTypes.Name, acc.UserName!),
                new Claim(ClaimTypes.Email, acc.Email!),
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtAuthToken = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

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
        
        throw new UnauthorizedException("Invalid login credentials");
    }
    
    public async Task<JwtDataDto> SignUp(RegisterDto registerDto)
        {
            // Create a new user account.
            var newUser = new Account
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            newUser.PasswordHash = _userManager.PasswordHasher.HashPassword(newUser, registerDto.Password);
            
            var createResult = await _userManager.CreateAsync(newUser);
            if (createResult.Errors.Any()) throw new BadRequestException(createResult.Errors.First().Description);
            
            if (!createResult.Succeeded)
            {
                throw new BadRequestException("Failed to create user account.");
            }
            
            // Generate JWT token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id),
                new Claim(ClaimTypes.Name, newUser.UserName),
                new Claim(ClaimTypes.Email, newUser.Email),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtAuthToken = tokenHandler.WriteToken(token);

            // Generate RefreshToken
            var refreshToken = GenerateRefreshToken();
            
            // Save RefreshToken associated with the user account (you need to implement this).
            var jwtDataDto = new JwtDataDto
            {
                AuthToken = jwtAuthToken,
                RefreshToken = refreshToken,
                Claims = new JwtClaimsDto
                {
                    Username = newUser.UserName,
                    UserEmail = newUser.Email,
                }
            };

            return jwtDataDto;
        }
    
    
    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString("N");
    }
}