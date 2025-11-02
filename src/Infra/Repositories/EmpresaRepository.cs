using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;

namespace kendo_londrina.Infrastructure.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly KendoLondrinaContext _context;

        public EmpresaRepository(KendoLondrinaContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        async Task<Empresa?> IEmpresaRepository.GetByIdAsync(Guid id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            return empresa;
        }

        public async Task AddAsync(Empresa empresa)
        {
            await _context.Empresas.AddAsync(empresa);
        }

        public Task DeleteAsync(Empresa empresa)
        {
            _context.Empresas.Remove(empresa);
            return _context.SaveChangesAsync();
        }

        Task<List<Empresa>> IEmpresaRepository.GetAllAsync()
        {
            return _context.Empresas.ToListAsync();
        }

        IQueryable<Empresa> IEmpresaRepository.Query()
        {
            return _context.Empresas.AsQueryable();
        }
    }
}
