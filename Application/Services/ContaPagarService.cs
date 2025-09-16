using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Application.DTOs;
using kendo_londrina.Infra.Data;

namespace kendo_londrina.Application.Services
{
    public class ContaPagarService
    {
        private readonly IUnitOfWork _uow;
        // private readonly IContaPagarRepository _uow.ContasPagar;
        // private readonly IPessoaRepository _uow.ContasPagarPessoa;
        // private readonly ICategoriaRepository _uow.ContasPagarCategoria;
        private readonly Guid _empresaId;

        public ContaPagarService(IUnitOfWork uow
            // , IContaPagarRepository repo
            // , IPessoaRepository repoPessoa
            // , ICategoriaRepository repoCategoria
            , ICurrentUserService currentUser)
        {
            _uow = uow;
            // _uow.ContasPagar = repo;
            // _uow.ContasPagarPessoa = repoPessoa;
            // _uow.ContasPagarCategoria = repoCategoria;
            _empresaId = Guid.Parse(currentUser.EmpresaId!);
        }

        private async Task VerificarVinculos(ContaPagarDto dto)
        {
            if (dto.PessoaId.HasValue)
            {
                var pessoa = await _uow.Pessoas.GetByIdAsync(_empresaId, dto.PessoaId.Value)
                    ?? throw new Exception("Pessoa não encontrada");
            }
            if (dto.CategoriaId.HasValue)
            {
                var categoria = await _uow.Categorias.GetByIdAsync(_empresaId, dto.CategoriaId.Value)
                    ?? throw new Exception("Categoria não encontrada");
                if (dto.SubCategoriaId.HasValue)
                {
                    bool existe = categoria.SubCategorias!
                        .Any(s => s.Id == dto.SubCategoriaId.Value);
                    if (!existe)
                        throw new Exception("Subcategoria não pertence a esta categoria");
                }
            }
        }

        public async Task<ContaPagarDto> CriarContaPagarAsync(ContaPagarDto dto, CancellationToken cancellationToken)
        {
            await VerificarVinculos(dto);
            var contaPagar = new ContaPagar(_empresaId
                , dto.Valor
                , dto.Vencimento
                , dto.Descricao
                , dto.Observacao
                , dto.PessoaId
                , dto.CategoriaId
                , dto.SubCategoriaId
            );
            var contaInseridaDto = ToDto(contaPagar);

            await _uow.BeginTransactionAsync();

            await _uow.ContasPagar.AddAsync(contaPagar);
            await _uow.ContasPagar.SaveChangesAsync();
            await _uow.Auditoria.LogAsync(
                typeof(ContaPagar).Name,
                "INSERIU",
                contaInseridaDto);

            await _uow.CommitAsync(cancellationToken);

            return contaInseridaDto;
        }

        public async Task ExcluirContaPagarAsync(Guid id, CancellationToken cancellationToken)
        {
            var contaPagar = await _uow.ContasPagar.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Pagar não encontrada");
            var contaPagarDto = ToDto(contaPagar);

            await _uow.BeginTransactionAsync();

            await _uow.ContasPagar.DeleteAsync(contaPagar);
            await _uow.Auditoria.LogAsync<ContaPagarDto>(
                typeof(ContaPagar).Name,
                "eXCLUIU",
                null,
                contaPagarDto);

            await _uow.CommitAsync(cancellationToken);
        }

        public async Task<List<ContaPagarDto>> ListarContasPagarAsync()
        {
            var contas = await _uow.ContasPagar.GetAllAsync(_empresaId);
            return ToListDto(contas);
        }

        public async Task<ContaPagarDto?> ObterPorIdAsync(Guid id)
        {
            var conta = await _uow.ContasPagar.GetByIdAsync(_empresaId, id);
            return (conta == null)
                ? null
                : ToDto(conta);
        }

        public async Task AlterarContaPagarAsync(Guid id, ContaPagarDto dto, CancellationToken cancellationToken)
        {
            await VerificarVinculos(dto);
            var contaPagar = await _uow.ContasPagar.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Pagar não encontrada");
            var dadosAntes = ToDto(contaPagar);
            dto.Id = id;

            await _uow.BeginTransactionAsync();

            contaPagar.Alterar(dto.Valor, dto.Vencimento, dto.Descricao, dto.Observacao
                , dto.PessoaId, dto.CategoriaId, dto.SubCategoriaId);
            await _uow.ContasPagar.SaveChangesAsync();
            await _uow.Auditoria.LogAsync(
                typeof(ContaPagar).Name,
                "ALTEROU",
                dto,
                dadosAntes);

            await _uow.CommitAsync(cancellationToken);
        }

        public async Task<(List<ContaPagarDto> ContasPagar, int Total)> ListarContasPagarPaginadoAsync(
            bool? pago, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _uow.ContasPagar.Query(_empresaId); // vamos criar Query() no repositório

            if (pago.HasValue)
                query = query.Where(a => a.Pago == pago);

            var total = await query.CountAsync();
            var contas = await query
                .OrderBy(a => a.Vencimento)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (ToListDto(contas), total);
        }

        private static List<ContaPagarDto> ToListDto(List<ContaPagar> contasPagar)
        {
            var dtoList = new List<ContaPagarDto>();
            foreach (var conta in contasPagar)
            {
                dtoList.Add(ToDto(conta));
            }
            return dtoList;
        }

        private static ContaPagarDto ToDto(ContaPagar contaPagar)
        {
            return new ContaPagarDto
            {
                Id = contaPagar.Id,
                Valor = contaPagar.Valor,
                Vencimento = contaPagar.Vencimento,
                Descricao = contaPagar.Descricao,
                Observacao = contaPagar.Observacao,
                Pago = contaPagar.Pago,
                DataPagamento = contaPagar.DataPagamento,
                MeioPagamento = contaPagar.MeioPagamento,
                ObsPagamento = contaPagar.ObsPagamento,
                Excluido = contaPagar.Excluido,
                DataExclusao = contaPagar.DataExclusao,
                MotivoExclusao = contaPagar.MotivoExclusao,
                PessoaId = contaPagar.PessoaId,
                CategoriaId = contaPagar.CategoriaId,
                SubCategoriaId = contaPagar.SubCategoriaId,
            };
        }
    }
}
