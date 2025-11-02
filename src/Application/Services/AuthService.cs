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
        private readonly ICurrentUserService _currentUser;
        public AuthService(UserManager<ApplicationUser> userManager,
            IEmpresaRepository repoEmpresa,
            ICurrentUserService currentUser)
        {
            _userManager = userManager;
            _repoEmpresa = repoEmpresa;
            _currentUser = currentUser;
        }

        public async Task SelfRegisterUserAsync(string email, string password)
        {
            //TODO: usar UnitOfWork
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmpresaRole = "Admin"
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("Erro ao criar usuário: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            await CriarVincularEmpresaAsync(user.Id);
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

            await _userManager.UpdateAsync(user);
        }

        public async Task RegisterUserAsync(string email, string password)
        {
            var empresaId = _currentUser.EmpresaId
                ?? throw new Exception("Usuário atual não está vinculado a nenhuma empresa");
            // var empresa = _repoEmpresa.GetByIdAsync(Guid.Parse(empresaId))
            //     ?? throw new Exception("Empresa não encontrada");

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmpresaRole = "User"
            };

            //TODO: usar UnitOfWork
            // var result = await _userManager.CreateAsync(user, password);

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("Erro ao criar usuário: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            await VincularEmpresaAsync(empresaId, user.Id);
        }        

        public async Task VincularEmpresaAsync(string empresaId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("Usuário não encontrado");
            var nomeFantasia = $"Empresa do user {user.Id}";

            user.VincularEmpresa(Guid.Parse(empresaId));

            await _userManager.UpdateAsync(user);
        }
    }
}
