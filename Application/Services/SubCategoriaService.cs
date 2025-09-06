using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Application.DTOs;

namespace kendo_londrina.Application.Services
{
    public class SubCategoriaService
    {
        private readonly ISubCategoriaRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly Guid _userId;

        public SubCategoriaService(ISubCategoriaRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
            _userId = Guid.Parse(_currentUser.UserId);
        }

        public async Task<SubCategoria> CriarSubCategoriaAsync(SubCategoriaDto subCategoriaDto)
        {
            var subCategoria = new SubCategoria(_userId, subCategoriaDto.CategoriaId, subCategoriaDto.Nome, subCategoriaDto.Codigo);
            await _repo.AddAsync(subCategoria);
            await _repo.SaveChangesAsync();
            return subCategoria;
        }

        public async Task ExcluirSubCategoriaAsync(SubCategoria subCategoria)
        {
            if (subCategoria.UserId != _userId)
                throw new Exception("Erro de pertencimento");            
            await _repo.DeleteAsync(subCategoria);
        }         

        public async Task<List<SubCategoria>> ListarSubCategoriasAsync()
        {
            return await _repo.GetAllAsync(_userId);
        }

        public async Task<SubCategoria?> ObterPorIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(_userId, id);
        }

        public async Task AtualizarSubCategoriaAsync(Guid id, SubCategoriaDto dto)
        {
            var subCategoria = await _repo.GetByIdAsync(_userId, id)
                ?? throw new Exception("SubCategoria não encontrada");

            if (dto.Nome == null)
                throw new Exception("Nome da subcategoria não pode ser nulo");

            subCategoria.Atualizar(dto.Nome, dto.Codigo);

            await _repo.SaveChangesAsync();
        }

        public async Task<(List<SubCategoria> SubCategorias, int Total)> ListarSubCategoriasPagAsync(
            int page = 1, int pageSize = 10, string? nome = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _repo.Query(_userId); // vamos criar Query() no repositório

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(a => a.Nome.Contains(nome));            

            var total = await query.CountAsync();
            var subCategorias = await query
                .OrderBy(a => a.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (subCategorias, total);
        }        
    }
}
