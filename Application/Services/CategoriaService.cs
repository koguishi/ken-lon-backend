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

        private List<SubCategoria> ToSubCategorias(
            Guid categoriaId, List<SubCategoriaDto> subCategoriasDto)
        {
            var subCategorias = new List<SubCategoria>();
            foreach (var subCatDto in subCategoriasDto)
            {
                var subCat = new SubCategoria(_empresaId, categoriaId, subCatDto.Nome, subCatDto.Codigo);
                subCategorias.Add(subCat);
            }
            return subCategorias;
        }

        public async Task<Categoria> CriarCategoriaAsync(CategoriaDto categoriaDto)
        {
            var categoria = new Categoria(_empresaId, categoriaDto.Nome);
            categoriaDto.SubCategorias.ForEach(s =>
                categoria.AdicionarSubcategoria(s.Nome));
            await _repo.AddAsync(categoria);
            await _repo.SaveChangesAsync();
            return categoria;
        }

        public async Task ExcluirCategoriaAsync(Guid id)
        {
            var categoria = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Categoria não encontrada");
            if (categoria.EmpresaId != _empresaId)
                throw new Exception("Erro de pertencimento");
            await _repo.DeleteAsync(categoria);
        }

        public async Task<List<CategoriaDto>> ListarCategoriasAsync()
        {
            var categorias = await _repo.GetAllAsync(_empresaId);
            return ToCategoriasDto(categorias);
        }

        public async Task<CategoriaDto?> ObterPorIdAsync(Guid id)
        {
            var categoria = await _repo.GetByIdAsync(_empresaId, id)
                ?? null;
            return ToCategoriaDto(categoria!);
        }

        public async Task AtualizarCategoriaAsync(Guid id, CategoriaDto dto)
        {
            var categoria = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Categoria não encontrada");

            if (dto.Nome == null)
                throw new Exception("Nome da categoria não pode ser nulo");

            categoria.Atualizar(dto.Nome, ToSubCategorias(categoria.Id, dto.SubCategorias), dto.Codigo);

            await _repo.SaveChangesAsync();
        }

        public async Task<(List<CategoriaDto> Categorias, int Total)> ListarCategoriasPagAsync(
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

            var categoriasDto = ToCategoriasDto(categorias);
            return (categoriasDto, total);
        }

        private static List<CategoriaDto> ToCategoriasDto(List<Categoria> categorias)
        {
            var categoriasDto = new List<CategoriaDto>();
            categorias.ForEach(categoria => {
                categoriasDto.Add(ToCategoriaDto(categoria));
            });
            return categoriasDto;
        }

        private static CategoriaDto ToCategoriaDto(Categoria? categoria)
        {
            if (categoria == null) return null!;
            return new CategoriaDto
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                SubCategorias = [.. categoria.SubCategorias.Select(s => new SubCategoriaDto
                {
                    Id = s.Id,
                    Nome = s.Nome
                })]
            };
        }
    }
}
