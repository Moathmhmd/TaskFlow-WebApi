using Microsoft.AspNetCore.Identity;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new BadRequestException("Email is required");

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
            throw new Exception("Invalid credentials");

        var validPassword = await _userManager
            .CheckPasswordAsync(user, dto.Password);

        if (!validPassword)
            throw new Exception("Invalid credentials");

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new BadRequestException("Email is required");

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingUser is not null)
            throw new Exception("Email already exists");

        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description);

            throw new Exception(string.Join(", ", errors));
        }

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }
}
