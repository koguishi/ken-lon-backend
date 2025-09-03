using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;

namespace kendo_londrina.Infra.Repositories
{
    public class MensalidadeRepository : IMensalidadeRepository
    {
        private readonly KendoLondrinaContext _context;

        public MensalidadeRepository(KendoLondrinaContext context)
        {
            _context = context;
        }

        public async Task<Mensalidade?> GetByIdAsync(Guid id)
        {
            return await _context.Mensalidades.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Mensalidade mensalidade)
        {
            await _context.Mensalidades.AddAsync(mensalidade);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
