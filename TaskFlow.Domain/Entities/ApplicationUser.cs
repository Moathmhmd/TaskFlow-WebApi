using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public ICollection<Project> Projects { get; set; } = [];
}
