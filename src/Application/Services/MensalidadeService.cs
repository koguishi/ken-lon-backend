using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;

namespace kendo_londrina.Application.Services
{
    public class MensalidadeService
    {
        private readonly IMensalidadeRepository _repo;

        public MensalidadeService(IMensalidadeRepository repo)
        {
            _repo = repo;
        }

        public async Task<Mensalidade> CriarMensalidadeAsync(Guid alunoId, string alunoNome, decimal valor, DateTime vencimento)
        {
            var mensalidade = new Mensalidade(alunoId, alunoNome, valor, vencimento);
            await _repo.AddAsync(mensalidade);
            await _repo.SaveChangesAsync();
            return mensalidade;
        }

        public async Task RegistrarPagamentoAsync(Guid id, string meio, string? obs)
        {
            var mensalidade = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Mensalidade não encontrada");

            mensalidade.RegistrarPagamento(meio, obs);
            await _repo.SaveChangesAsync();
        }

        public async Task ExcluirMensalidadeAsync(Guid id, string motivo)
        {
            var mensalidade = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Mensalidade não encontrada");

            mensalidade.Excluir(motivo);
            await _repo.SaveChangesAsync();
        }
    }
}
