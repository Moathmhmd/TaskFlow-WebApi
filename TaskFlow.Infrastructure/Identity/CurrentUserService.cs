using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId()
    {
        return _httpContextAccessor
            .HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("User not found");
    }
}