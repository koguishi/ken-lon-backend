using kendo_londrina.Application.DTOs;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace kendo_londrina.Application.Services
{
    public class CategoriaService
    {
        // para evitar Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException
        // o contexto é usado na alteração da Categoria com a inclusão de uma nova SubCategoria
        //  _context.Entry(subCategoria).State = EntityState.Added;
        private readonly KendoLondrinaContext _context;
        private readonly IUnitOfWork _uow;
        private readonly Guid _empresaId;

        public CategoriaService(KendoLondrinaContext context
            , IUnitOfWork unitOfWork
            , ICurrentUserService currentUser)
        {
            _context = context;
            _uow = unitOfWork;
            _empresaId = Guid.Parse(currentUser.EmpresaId!);
        }

        public async Task<CategoriaDto> CriarCategoriaAsync(CategoriaDto dto, CancellationToken cancellationToken)
        {
            var categoria = new Categoria(_empresaId, dto.Nome);
            dto.SubCategorias.ForEach(s =>
                categoria.AdicionarSubcategoria(s.Nome));
            var categoriaInseridaDto = ToCategoriaDto(categoria);

            await _uow.BeginTransactionAsync();

            await _uow.Categorias.AddAsync(categoria);
            await _uow.Categorias.SaveChangesAsync();
            await _uow.Auditoria.LogAsync(
                typeof(Categoria).Name,
                "INSERIU",
                categoriaInseridaDto);

            await _uow.CommitAsync(cancellationToken);
            return categoriaInseridaDto;
        }

        public async Task ExcluirCategoriaAsync(Guid id, CancellationToken cancellationToken)
        {
            var categoria = await _uow.Categorias.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Categoria não encontrada");
            var categoriaExcluidaDto = ToCategoriaDto(categoria);

            await _uow.Categorias.LoadContasPagarAsync(categoria);
            if (categoria.ContasPagar!.Count > 0)
                throw new Exception("Categoria tem contas a pagar vinculadas");

            await _uow.Categorias.LoadContasReceberAsync(categoria);
            if (categoria.ContasReceber!.Count > 0)
                throw new Exception("Categoria tem contas a receber vinculadas");

            await _uow.BeginTransactionAsync();

            await _uow.Categorias.DeleteAsync(categoria);
            await _uow.Auditoria.LogAsync<Categoria>(
                typeof(Categoria).Name,
                "EXCLUIU",
                null,
                categoriaExcluidaDto);

            await _uow.CommitAsync(cancellationToken);
        }

        public async Task<List<CategoriaDto>> ListarCategoriasAsync()
        {
            var categorias = await _uow.Categorias.GetAllAsync(_empresaId);
            return ToCategoriasDto(categorias);
        }

        public async Task<CategoriaDto?> ObterPorIdAsync(Guid id)
        {
            var categoria = await _uow.Categorias.GetByIdAsync(_empresaId, id)
                ?? null;
            return ToCategoriaDto(categoria!);
        }

        public async Task AtualizarCategoriaAsync(Guid id, CategoriaDto dto, CancellationToken cancellationToken)
        {
            var categoria = await _uow.Categorias.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Categoria não encontrada");
            var dadosAntes = ToCategoriaDto(categoria);
            dto.Id = id;

            await _uow.BeginTransactionAsync();

            // Atualiza os dados da categoria
            categoria.Atualizar(dto.Nome, dto.Codigo);

            // Subcategorias atuais no banco
            var existentes = (categoria.SubCategorias == null) ? [] : categoria.SubCategorias.ToList();

            // Subcategorias que vieram no DTO
            var novas = dto.SubCategorias;

            // Remover as que não estão mais presentes
            foreach (var existente in existentes)
            {
                var contaReceber = await _uow.ContasReceber.GetBySubCategoriaAsync(_empresaId, existente.Id)
                    ?? throw new Exception($"Categoria {existente.Nome} tem contas a receber vinculadas");

                var contaPagar = await _uow.ContasPagar.GetBySubCategoriaAsync(_empresaId, existente.Id)
                    ?? throw new Exception($"Categoria {existente.Nome} tem contas a pagar vinculadas");

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

            await _uow.Auditoria.LogAsync(
                typeof(Categoria).Name,
                "ALTEROU",
                dto,
                dadosAntes);

            await _uow.CommitAsync(cancellationToken);
        }

        public async Task<(List<CategoriaDto> Categorias, int Total)> ListarCategoriasPagAsync(
            int page = 1, int pageSize = 10, string? nome = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _uow.Categorias.Query(_empresaId); // vamos criar Query() no repositório

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
                SubCategorias = (categoria.SubCategorias == null)
                ? [] : [.. categoria.SubCategorias.Select(s => new SubCategoriaDto
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
