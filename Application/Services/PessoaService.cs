using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Application.DTOs;

namespace kendo_londrina.Application.Services
{
    public class PessoaService
    {
        private readonly IPessoaRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly Guid _empresaId;

        public PessoaService(IPessoaRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
            _empresaId = Guid.Parse(_currentUser.EmpresaId!);
        }

        public async Task<PessoaDto> CriarPessoaAsync(PessoaDto pessoaDto)
        {
            var pessoa = new Pessoa(_empresaId, pessoaDto.Nome, pessoaDto.Codigo, pessoaDto.Cpf, pessoaDto.Cnpj);
            await _repo.AddAsync(pessoa);
            await _repo.SaveChangesAsync();
            return ToPessoaDto(pessoa);
        }

        public async Task ExcluirPessoaAsync(Guid id)
        {
            var pessoa = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Pessoa não encontrada");
            await _repo.DeleteAsync(pessoa);
        }

        public async Task<List<PessoaDto>> ListarPessoasAsync()
        {
            var pessoas = await _repo.GetAllAsync(_empresaId);
            return ToPessoasDto(pessoas);
        }

        public async Task<PessoaDto?> ObterPorIdAsync(Guid id)
        {
            var pessoa = await _repo.GetByIdAsync(_empresaId, id);
            return ToPessoaDto(pessoa);
        }

        public async Task AtualizarPessoaAsync(Guid id, PessoaDto dto)
        {
            var pessoa = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Pessoa não encontrado");

            // TODO: fazer esta validação no Domain
            if (dto.Nome == null)
                throw new Exception("Nome do pessoa não pode ser nulo");

            pessoa.Atualizar(dto.Nome, dto.Codigo, dto.Cpf, dto.Cnpj);

            await _repo.SaveChangesAsync();
        }

        public async Task<(List<PessoaDto> Pessoas, int Total)> ListarPessoasPagAsync(
            int page = 1, int pageSize = 10, string? nome = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _repo.Query(_empresaId); // vamos criar Query() no repositório

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
