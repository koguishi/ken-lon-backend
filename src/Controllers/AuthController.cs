using kendo_londrina.Application.Services;
using kendo_londrina.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(AuthService authService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _authService = authService;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("self-register")]
    public async Task<IActionResult> SelfRegister([FromBody] RegisterDto dto)
    {
        try
        {
            await _authService.SelfRegisterUserAsync(dto.Email, dto.Password);
            return Ok("Usuário criado com sucesso");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        // var user = new ApplicationUser
        // {
        //     UserName = dto.Email,
        //     Email = dto.Email,
        //     // Nome = dto.Nome
        // };

        // var result = await _userManager.CreateAsync(user, dto.Password);

        // await _authService.CriarVincularEmpresaAsync(user.Id);

        // if (result.Succeeded)
        //     return Ok("Usuário criado com sucesso");

        // return BadRequest(result.Errors);
    }

    [Authorize(Policy = "EmpresaAdmin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            await _authService.RegisterUserAsync(dto.Email, dto.Password);
            return Ok("Usuário criado com sucesso");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return Unauthorized("Usuário/Senha inválido");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded) return Unauthorized("Usuário/Senha inválido");

        // Gerar token JWT
        var token = await GenerateJwtToken(user);

        return Ok(new { token });
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var jwtKey = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("JWT key is not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new("EmpresaId", user.EmpresaId?.ToString() ?? ""),
            new("EmpresaRole", user.EmpresaRole ?? "")
        };

        // adicionar roles (se tiver)
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class RegisterDto
{
    // public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
