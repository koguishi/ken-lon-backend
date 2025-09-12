using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Application.DTOs;
using kendo_londrina.Infra.Data;

namespace kendo_londrina.Application.Services
{
    public class CategoriaService
    {
        private readonly KendoLondrinaContext _context;
        private readonly ICategoriaRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly Guid _empresaId;

        public CategoriaService(KendoLondrinaContext context
            , ICategoriaRepository repo
            , IUnitOfWork unitOfWork
            , ICurrentUserService currentUser)
        {
            _context = context;
            _repo = repo;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _empresaId = Guid.Parse(_currentUser.EmpresaId!);
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

            // TODO: transferir a validação para o domínio
            // if (dto.Nome == null)
            //     throw new Exception("Nome da categoria não pode ser nulo");

            // Atualiza os dados da categoria
            categoria.Atualizar(dto.Nome, dto.Codigo);

            // Subcategorias atuais no banco
            var existentes = categoria.SubCategorias.ToList();

            // Subcategorias que vieram no DTO
            var novas = dto.SubCategorias;

            // Remover as que não estão mais presentes
            foreach (var existente in existentes)
            {
                if (!novas.Any(s => s.Id == existente.Id))
                    categoria.ExcluirSubcategoria(existente.Id);
            }

            // inserir e atualizar as que vieram no DTO
            foreach (var nova in novas)
            {
                var existente = existentes.FirstOrDefault(s => s.Id == nova.Id);
                if (existente == null)
                {
                    // Novo registro
                    var subCategoria = categoria.AdicionarSubcategoria(nova.Nome);

                    // explicitamente State = Added
                    // para evitar Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException
                    _context.Entry(subCategoria).State = EntityState.Added;
                }
                else
                {
                    // Atualiza existente
                    categoria.AlterarSubcategoria(existente.Id, nova.Nome);
                }
            }
            await _unitOfWork.CommitAsync();
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
                    CategoriaId = s.CategoriaId,
                    Id = s.Id,
                    Nome = s.Nome,
                    Codigo = s.Codigo
                })]
            };
        }
    }
}
