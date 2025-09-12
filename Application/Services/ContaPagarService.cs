using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Application.DTOs;

namespace kendo_londrina.Application.Services
{
    public class ContaPagarService
    {
        private readonly IContaPagarRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly Guid _empresaId;

        public ContaPagarService(IContaPagarRepository repo
            , ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
            _empresaId = Guid.Parse(_currentUser.EmpresaId!);
        }

        public async Task<ContaPagar> CriarContaPagarAsync(ContaPagarDto dto)
        {
            var contaPagar = new ContaPagar(
                _empresaId, dto.Valor, dto.Vencimento
            );
            await _repo.AddAsync(contaPagar);
            await _repo.SaveChangesAsync();
            return contaPagar;
        }

        public async Task ExcluirContaPagarAsync(Guid id)
        {
            var contaPagar = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Pagar não encontrada");
            await _repo.DeleteAsync(contaPagar);            
        }         

        public async Task<List<ContaPagar>> ListarContasPagarAsync()
        {
            return await _repo.GetAllAsync(_empresaId);
        }

        public async Task<ContaPagar?> ObterPorIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(_empresaId, id);
        }

        public async Task AtualizarContaPagarAsync(Guid id, ContaPagarDto dto)
        {
            var contaPagar = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Pagar não encontrada");

            contaPagar.Alterar(dto.Valor, dto.Vencimento, dto.Observacao);
            await _repo.SaveChangesAsync();
        }

        public async Task<(List<ContaPagar> ContasPagar, int Total)> ListarContasPagarPaginadoAsync(
            bool? pago, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _repo.Query(_empresaId); // vamos criar Query() no repositório

            if (pago.HasValue)
                query = query.Where(a => a.Pago == pago);

            var total = await query.CountAsync();
            var contas = await query
                .OrderBy(a => a.Vencimento)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (contas, total);
        }        
    }
}
