using System.Security.Claims;
namespace Lmsr.Application.Interfaces;
public interface IUserContext
{
    string? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
    IEnumerable<string> Roles { get; }
    IEnumerable<Claim> Claims { get; }
}