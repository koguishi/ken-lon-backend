using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Application.DTOs;

namespace kendo_londrina.Application.Services
{
    public class ContaPagarService
    {
        private readonly IContaPagarRepository _repo;
        private readonly IPessoaRepository _repoPessoa;
        private readonly ICategoriaRepository _repoCategoria;
        private readonly ICurrentUserService _currentUser;
        private readonly Guid _empresaId;

        public ContaPagarService(IContaPagarRepository repo
            , IPessoaRepository repoPessoa
            , ICategoriaRepository repoCategoria
            , ICurrentUserService currentUser)
        {
            _repo = repo;
            _repoPessoa = repoPessoa;
            _repoCategoria = repoCategoria;
            _currentUser = currentUser;
            _empresaId = Guid.Parse(_currentUser.EmpresaId!);
        }

        private async Task VerificarVinculos(ContaPagarDto dto)
        {
            if (dto.PessoaId.HasValue)
            {
                var pessoa = await _repoPessoa.GetByIdAsync(_empresaId, dto.PessoaId.Value)
                    ?? throw new Exception("Pessoa não encontrada");
            }
            if (dto.CategoriaId.HasValue)
            {
                var categoria = await _repoCategoria.GetByIdAsync(_empresaId, dto.CategoriaId.Value)
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

        public async Task<ContaPagarDto> CriarContaPagarAsync(ContaPagarDto dto)
        {
            await VerificarVinculos(dto);
            var contaPagar = new ContaPagar(_empresaId
                , dto.Valor
                , dto.Vencimento
                , dto.Observacao
                , dto.PessoaId
                , dto.CategoriaId
                , dto.SubCategoriaId
            );
            await _repo.AddAsync(contaPagar);
            await _repo.SaveChangesAsync();
            return ConvertToDto(contaPagar);
        }

        public async Task ExcluirContaPagarAsync(Guid id)
        {
            var contaPagar = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Pagar não encontrada");
            await _repo.DeleteAsync(contaPagar);
        }

        public async Task<List<ContaPagarDto>> ListarContasPagarAsync()
        {
            var contas = await _repo.GetAllAsync(_empresaId);
            return ConvertToListDto(contas);
        }

        public async Task<ContaPagarDto?> ObterPorIdAsync(Guid id)
        {
            var conta = await _repo.GetByIdAsync(_empresaId, id);
            return (conta == null)
                ? null
                : ConvertToDto(conta);
        }

        public async Task AlterarContaPagarAsync(Guid id, ContaPagarDto dto)
        {
            await VerificarVinculos(dto);
            var contaPagar = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Pagar não encontrada");

            contaPagar.Alterar(dto.Valor, dto.Vencimento, dto.Observacao
                , dto.PessoaId, dto.CategoriaId, dto.SubCategoriaId);
            await _repo.SaveChangesAsync();
        }

        public async Task<(List<ContaPagarDto> ContasPagar, int Total)> ListarContasPagarPaginadoAsync(
            bool? pago, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _repo.Query(_empresaId); // vamos criar Query() no repositório

            if (pago.HasValue)
                query = query.Where(a => a.Pago == pago);

            var total = await query.CountAsync();
            var contas = await query
                .OrderBy(a => a.Vencimento)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (ConvertToListDto(contas), total);
        }

        private static List<ContaPagarDto> ConvertToListDto(List<ContaPagar> contasPagar)
        {
            var dtoList = new List<ContaPagarDto>();
            foreach (var conta in contasPagar)
            {
                dtoList.Add(ConvertToDto(conta));
            }
            return dtoList;
        }

        private static ContaPagarDto ConvertToDto(ContaPagar contaPagar)
        {
            return new ContaPagarDto
            {
                Id = contaPagar.Id,
                Valor = contaPagar.Valor,
                Vencimento = contaPagar.Vencimento,
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
