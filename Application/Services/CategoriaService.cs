using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Application.DTOs;

namespace kendo_londrina.Application.Services
{
    public class CategoriaService
    {
        private readonly ICategoriaRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly Guid _userId;

        public CategoriaService(ICategoriaRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
            _userId = Guid.Parse(_currentUser.UserId);
        }

        public async Task<Categoria> CriarCategoriaAsync(CategoriaDto categoriaDto)
        {
            var categoria = new Categoria(_userId, categoriaDto.Nome, categoriaDto.Codigo);
            await _repo.AddAsync(categoria);
            await _repo.SaveChangesAsync();
            return categoria;
        }

        public async Task ExcluirCategoriaAsync(Categoria categoria)
        {
            await _repo.DeleteAsync(_userId, categoria);
        }         

        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            return await _repo.GetAllAsync(_userId);
        }

        public async Task<Categoria?> ObterPorIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(_userId, id);
        }

        public async Task AtualizarCategoriaAsync(Guid id, CategoriaDto dto)
        {
            var pessoa = await _repo.GetByIdAsync(_userId, id)
                ?? throw new Exception("Categoria não encontrada");

            if (dto.Nome == null)
                throw new Exception("Nome da categoria não pode ser nulo");

            pessoa.Atualizar(dto.Nome, dto.Codigo);

            await _repo.SaveChangesAsync();
        }

        public async Task<(List<Categoria> Categorias, int Total)> ListarCategoriasPagAsync(
            int page = 1, int pageSize = 10, string? nome = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _repo.Query(_userId); // vamos criar Query() no repositório

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(a => a.Nome.Contains(nome));            

            var total = await query.CountAsync();
            var categorias = await query
                .OrderBy(a => a.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (categorias, total);
        }        
    }
}
