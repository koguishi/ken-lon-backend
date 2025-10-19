using kendo_londrina.Application.DTOs;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace kendo_londrina.Application.Services
{
    public class PessoaService
    {
        private readonly IUnitOfWork _uow;
        private readonly Guid _empresaId;

        public PessoaService(IUnitOfWork uow, ICurrentUserService currentUser)
        {
            _uow = uow;
            _empresaId = Guid.Parse(currentUser.EmpresaId!);
        }

        public async Task<PessoaDto> CriarPessoaAsync(PessoaDto pessoaDto, CancellationToken cancellationToken)
        {
            var pessoa = new Pessoa(_empresaId, pessoaDto.Nome, pessoaDto.Codigo, pessoaDto.Cpf, pessoaDto.Cnpj);
            var pessoaInseridaDto = ToPessoaDto(pessoa);
            await _uow.BeginTransactionAsync();
            await _uow.Pessoas.AddAsync(pessoa);
            await _uow.Pessoas.SaveChangesAsync();
            await _uow.Auditoria.LogAsync(
                typeof(Pessoa).Name,
                "INSERIU",
                pessoaInseridaDto);
            await _uow.CommitAsync(cancellationToken);
            return pessoaInseridaDto;
        }

        public async Task ExcluirPessoaAsync(Guid id, CancellationToken cancellationToken)
        {
            var pessoa = await _uow.Pessoas.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Pessoa não encontrada");
            var pessoaExcluidaDto = ToPessoaDto(pessoa);

            await _uow.Pessoas.LoadContasPagarAsync(pessoa);
            if (pessoa.ContasPagar!.Count > 0)
                throw new Exception("Pessoa tem contas a pagar vinculadas");

            await _uow.Pessoas.LoadContasReceberAsync(pessoa);
            if (pessoa.ContasReceber!.Count > 0)
                throw new Exception("Pessoa tem contas a receber vinculadas");

            await _uow.BeginTransactionAsync();

            await _uow.Pessoas.DeleteAsync(pessoa);
            await _uow.Auditoria.LogAsync<Pessoa>(
                typeof(Pessoa).Name,
                "EXCLUIU",
                null,
                pessoaExcluidaDto);

            await _uow.CommitAsync(cancellationToken);
        }

        public async Task<List<PessoaDto>> ListarPessoasAsync()
        {
            var pessoas = await _uow.Pessoas.GetAllAsync(_empresaId);
            return ToPessoasDto(pessoas);
        }

        public async Task<PessoaDto?> ObterPorIdAsync(Guid id)
        {
            var pessoa = await _uow.Pessoas.GetByIdAsync(_empresaId, id);
            return ToPessoaDto(pessoa);
        }

        public async Task AtualizarPessoaAsync(Guid id, PessoaDto dto, CancellationToken cancellationToken)
        {
            var pessoa = await _uow.Pessoas.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Pessoa não encontrada");
            var dadosAntes = ToPessoaDto(pessoa);

            // TODO: fazer esta validação no Domain
            // if (dto.Nome == null)
            //     throw new Exception("Nome do pessoa não pode ser nulo");

            dto.Id = id;

            await _uow.BeginTransactionAsync();

            pessoa.Atualizar(dto.Nome, dto.Codigo, dto.Cpf, dto.Cnpj);
            await _uow.Pessoas.SaveChangesAsync();
            await _uow.Auditoria.LogAsync(
                typeof(Pessoa).Name,
                "ALTEROU",
                dto,
                dadosAntes);

            await _uow.CommitAsync(cancellationToken);
        }

        public async Task<(List<PessoaDto> Pessoas, int Total)> ListarPessoasPagAsync(
            int page = 1, int pageSize = 10, string? nome = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _uow.Pessoas.Query(_empresaId);

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(a => a.Nome.Contains(nome));

            var total = await query.CountAsync();
            var pessoas = await query
                .OrderBy(a => a.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (ToPessoasDto(pessoas), total);
        }
        
        private static List<PessoaDto> ToPessoasDto(List<Pessoa> pessoas)
        {
            var pessoasDto = new List<PessoaDto>();
            pessoas.ForEach(pessoa =>
            {
                pessoasDto.Add(ToPessoaDto(pessoa));
            });
            return pessoasDto;
        }

        private static PessoaDto ToPessoaDto(Pessoa? pessoa)
        {
            if (pessoa == null) return null!;
            return new PessoaDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Codigo = pessoa.Codigo,
                Cpf = pessoa.Cpf,
                Cnpj = pessoa.Cnpj,
            };
        }
    }
}
