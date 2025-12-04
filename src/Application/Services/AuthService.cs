using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using kendo_londrina.Domain;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

//TODO: implementar UnitOfWork

namespace kendo_londrina.Application.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmpresaRepository _repoEmpresa;
        private readonly ICurrentUserService _currentUser;
        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IEmpresaRepository repoEmpresa,
            ICurrentUserService currentUser)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _repoEmpresa = repoEmpresa;
            _currentUser = currentUser;
        }

        public async Task SelfRegisterUserAsync(string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("Erro ao criar usuário: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            var nomeFantasia = $"Empresa do user {user.UserName}";

            // Criar empresa
            var empresa = new Empresa(nomeFantasia, "PR", "Londrina");
            await _repoEmpresa.AddAsync(empresa);
            await _repoEmpresa.SaveChangesAsync();

            // Vincular empresa ao usuário
            var claims = new List<Claim>
            {
                new("EmpresaId", empresa.Id.ToString()),
                new("EmpresaRole", Role.Admin.ToString()),
                // new Claim("OutraClaim", OutraClaim)
                // ...
            };
            var claimResult = _userManager.AddClaimsAsync(user, claims).Result;
        }

        public async Task RegisterUserAsync(string email, string password, string? role)
        {
            if (!Enum.TryParse<Role>(role, out var status))
            {
                var msg = $"Roles permitidas: {string.Join(", ", Enum.GetNames<Role>())}";
                throw new BadHttpRequestException(msg);
            }

            var empresaId = _currentUser.EmpresaId
                ?? throw new Exception("Usuário atual não está vinculado a nenhuma empresa");

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("Erro ao criar usuário: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            // Vincular empresa ao usuário
            var claims = new List<Claim>
            {
                new("EmpresaId", empresaId.ToString()),
                new("EmpresaRole", role.ToString()),
                // new Claim("OutraClaim", OutraClaim)
                // ...
            };
            var claimResult = _userManager.AddClaimsAsync(user, claims).Result;            
        }        
        public async Task<string> Login(string email, string password)
        {
            ApplicationUser? user;
            try
            {
                user = await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                throw new InfraException("Erro ao acessar o banco de dados.", ex);
            }
            if (user == null)
                throw new UnauthorizedAccessException("Usuário/Senha inválido");
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Usuário/Senha inválido");

            return await GenerateJwtToken(user);
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
            };
            // adicionar claims cadastradas
            claims.AddRange(_userManager.GetClaimsAsync(user).Result);
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

        public async Task ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            // Don't reveal that the user does not exist or is not confirmed
            if (user == null || !user.EmailConfirmed)
                throw new BadHttpRequestException("Senha não foi redefinida");

            var validPassword = await _userManager.CheckPasswordAsync(user, currentPassword);
            // Don't reveal that the password is incorrect
            if (!validPassword)
                throw new BadHttpRequestException("Senha não foi redefinida");

            try
            {
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!result.Succeeded)
                    throw new BadHttpRequestException("Erro ao redefinir senha: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            catch (Exception ex)
            {
                throw new BadHttpRequestException(ex.Message);
            }
        }

    }
}
