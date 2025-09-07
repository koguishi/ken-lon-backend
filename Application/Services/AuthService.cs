using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;
using Microsoft.AspNetCore.Identity;

namespace kendo_londrina.Application.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmpresaRepository _repoEmpresa;
        public AuthService(UserManager<ApplicationUser> userManager,
            IEmpresaRepository repoEmpresa)
        {
            _userManager = userManager;
            _repoEmpresa = repoEmpresa;
        }

        public async Task CriarVincularEmpresaAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("Usuário não encontrado");
            var nomeFantasia = $"Empresa do user {user.Id}";

            // Criar empresa vinculada ao usuário
            var empresa = new Empresa(nomeFantasia, "PR", "Londrina");
            await _repoEmpresa.AddAsync(empresa);
            await _repoEmpresa.SaveChangesAsync();

            user.VincularEmpresa(empresa.Id);

            // await _userManager.AddToRoleAsync(user, "Admin"); // papel de admin
            await _userManager.UpdateAsync(user);
            // preciso do savechanges?
            // await _context.SaveChangesAsync();
        }

    }
}
