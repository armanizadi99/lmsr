using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using Lmsr.Application.Interfaces;
namespace Lmsr.Infrastructure;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string? UserId => User?.FindFirst(ClaimTypes.NameIdentifier).Value;

    public string? UserName => User?.Identity?.Name;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<string> Roles =>
        User?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
        ?? Enumerable.Empty<string>();

    public IEnumerable<Claim> Claims =>
        User?.Claims ?? Enumerable.Empty<Claim>();
}
