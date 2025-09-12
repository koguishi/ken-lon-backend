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

        public async Task<Pessoa> CriarPessoaAsync(PessoaDto pessoaDto)
        {
            var pessoa = new Pessoa(_empresaId, pessoaDto.Nome, pessoaDto.Codigo, pessoaDto.Cpf, pessoaDto.Cnpj);
            await _repo.AddAsync(pessoa);
            await _repo.SaveChangesAsync();
            return pessoa;
        }

        public async Task ExcluirPessoaAsync(Guid id)
        {
            var pessoa = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Pessoa não encontrada");
            await _repo.DeleteAsync(pessoa);
        }         

        public async Task<List<Pessoa>> ListarPessoasAsync()
        {
            return await _repo.GetAllAsync(_empresaId);
        }

        public async Task<Pessoa?> ObterPorIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(_empresaId, id);
        }

        public async Task AtualizarPessoaAsync(Guid id, PessoaDto dto)
        {
            var pessoa = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Pessoa não encontrado");

            if (dto.Nome == null)
                throw new Exception("Nome do pessoa não pode ser nulo");

            pessoa.Atualizar(dto.Nome, dto.Codigo, dto.Cpf, dto.Cnpj);

            await _repo.SaveChangesAsync();
        }

        public async Task<(List<Pessoa> Pessoas, int Total)> ListarPessoasPagAsync(
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

            return (pessoas, total);
        }        
    }
}
