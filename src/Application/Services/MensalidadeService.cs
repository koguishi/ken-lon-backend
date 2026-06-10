using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;

namespace kendo_londrina.Application.Services
{
    public class MensalidadeService
    {
        private readonly IMensalidadeRepository _repo;
        private readonly IResponsavelRepository _responsavelRepo;
        private readonly ResponsavelService _responsavelService;
        private readonly Guid _empresaId;        

        public MensalidadeService(
            IMensalidadeRepository repo,
            IResponsavelRepository responsavelRepo,
            ResponsavelService responsavelService,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _responsavelRepo = responsavelRepo;
            _responsavelService = responsavelService;
            _empresaId = Guid.Parse(currentUser.EmpresaId!);
        }

        // Novo método — usa Responsavel
        public async Task<Mensalidade> CriarParaResponsavelAsync(Guid responsavelId, DateTime vencimento)
        {
            var responsavel = await _responsavelRepo.GetByIdComAlunosAsync(_empresaId, responsavelId)
                ?? throw new Exception("Responsável não encontrado.");

            var qtdAlunos = responsavel.Alunos?.Count() ?? 0;
            var valor = _responsavelService.CalcularValorMensalidade(qtdAlunos);

            var mensalidade = new Mensalidade(responsavelId, valor, vencimento);
            await _repo.AddAsync(mensalidade);
            await _repo.SaveChangesAsync();
            return mensalidade;
        }

        // Método antigo — mantido intacto para não quebrar nada        
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
