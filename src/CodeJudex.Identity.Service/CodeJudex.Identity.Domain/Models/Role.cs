using Microsoft.AspNetCore.Identity;

namespace CodeJudex.Identity.Domain.Models;

public class Role : IdentityRole<Guid>
{
    public Role() : base() { }
    public Role(string roleName) : base(roleName) { }
}