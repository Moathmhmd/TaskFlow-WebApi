using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;


public interface IJwtService
{
    string GenerateToken(ApplicationUser user);
}
