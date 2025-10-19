using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public interface ICurrentUserService
{
    string UserId { get; }
    string? Email { get; }
    string? EmpresaId { get; }
    string? EmpresaRole { get; }    
    IEnumerable<string> Roles { get; }
}

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string UserId =>
        User?.FindFirstValue(ClaimTypes.NameIdentifier) ??
        User?.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? string.Empty;

    public string? Email => User?.FindFirstValue(ClaimTypes.Email);

   public string? EmpresaId => 
        _httpContextAccessor.HttpContext?.User.FindFirst("EmpresaId")?.Value;

    public string? EmpresaRole => 
        _httpContextAccessor.HttpContext?.User.FindFirst("EmpresaRole")?.Value;    

    public IEnumerable<string> Roles =>
        User?.FindAll(ClaimTypes.Role).Select(r => r.Value) ?? Enumerable.Empty<string>();
}
