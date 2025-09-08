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
        private readonly Guid _empresaId;

        public CategoriaService(ICategoriaRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
            _empresaId = Guid.Parse(_currentUser.EmpresaId!);
        }

        public async Task<Categoria> CriarCategoriaAsync(CategoriaDto categoriaDto)
        {
            var categoria = new Categoria(_empresaId, categoriaDto.Nome, categoriaDto.Codigo);
            await _repo.AddAsync(categoria);
            await _repo.SaveChangesAsync();
            return categoria;
        }

        public async Task ExcluirCategoriaAsync(Categoria categoria)
        {
            if (categoria.EmpresaId != _empresaId)
                throw new Exception("Erro de pertencimento");            
            await _repo.DeleteAsync(categoria);
        }         

        public async Task<List<Categoria>> ListarCategoriasAsync()
        {
            return await _repo.GetAllAsync(_empresaId);
        }

        public async Task<Categoria?> ObterPorIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(_empresaId, id);
        }

        public async Task AtualizarCategoriaAsync(Guid id, CategoriaDto dto)
        {
            var categoria = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Categoria não encontrada");

            if (dto.Nome == null)
                throw new Exception("Nome da categoria não pode ser nulo");

            categoria.Atualizar(dto.Nome, dto.Codigo);

            await _repo.SaveChangesAsync();
        }

        public async Task<(List<Categoria> Categorias, int Total)> ListarCategoriasPagAsync(
            int page = 1, int pageSize = 10, string? nome = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _repo.Query(_empresaId); // vamos criar Query() no repositório

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
