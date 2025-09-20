using kendo_londrina.Application.DTOs;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace kendo_londrina.Application.Services
{
    public class ContaReceberService
    {
        private readonly IUnitOfWork _uow;
        private readonly Guid _empresaId;

        public ContaReceberService(IUnitOfWork uow
            , ICurrentUserService currentUser)
        {
            _uow = uow;
            _empresaId = Guid.Parse(currentUser.EmpresaId!);
        }

        private async Task VerificarVinculos(ContaReceberDto dto)
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

        public async Task<ContaReceberDto> CriarContaReceberAsync(
            ContaReceberDto dto, CancellationToken cancellationToken)
        {
            await VerificarVinculos(dto);
            var contaReceber = new ContaReceber(_empresaId
                , dto.Valor
                , dto.Vencimento
                , dto.Descricao
                , dto.Observacao
                , dto.PessoaId
                , dto.CategoriaId
                , dto.SubCategoriaId
            );
            var contaInseridaDto = ToDto(contaReceber);

            await _uow.BeginTransactionAsync();
            await _uow.ContasReceber.AddAsync(contaReceber);
            await _uow.Auditoria.LogAsync(
                typeof(ContaReceber).Name,
                "iNSERIU",
                contaInseridaDto);
            await _uow.CommitAsync(cancellationToken);
            return contaInseridaDto;
        }

        public async Task ExcluirContaReceberAsync(Guid id,
            CancellationToken cancellationToken)
        {
            var contaReceber = await _uow.ContasReceber.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Receber não encontrada");
            var contaReceberDto = ToDto(contaReceber);

            await _uow.BeginTransactionAsync();
            await _uow.ContasReceber.DeleteAsync(contaReceber);
            await _uow.Auditoria.LogAsync<ContaReceberDto>(
                typeof(ContaReceber).Name,
                "eXCLUIU",
                null,
                contaReceberDto);
            await _uow.CommitAsync(cancellationToken);
        }

        public async Task<List<ContaReceberDto>> ListarContasReceberAsync()
        {
            var contas = await _uow.ContasReceber.GetAllAsync(_empresaId);
            return ToListDto(contas);
        }

        public async Task<ContaReceberDto?> ObterPorIdAsync(Guid id)
        {
            var conta = await _uow.ContasReceber.GetByIdAsync(_empresaId, id);
            return (conta == null)
                ? null
                : ToDto(conta);
        }

        public async Task AlterarContaReceberAsync(Guid id,
            ContaReceberDto dto, CancellationToken cancellationToken)
        {
            await VerificarVinculos(dto);
            var contaReceber = await _uow.ContasReceber.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Receber não encontrada");

            var dadosAntes = ToDto(contaReceber);

            contaReceber.Alterar(dto.Valor, dto.Vencimento, dto.Descricao, dto.Observacao
                , dto.PessoaId, dto.CategoriaId, dto.SubCategoriaId);

            dto.Id = id;
            await _uow.BeginTransactionAsync();
            await _uow.ContasReceber.SaveChangesAsync();
            await _uow.Auditoria.LogAsync(
                typeof(ContaReceber).Name,
                "aLTEROU",
                dto,
                dadosAntes);
            await _uow.CommitAsync(cancellationToken);
        }

        public async Task<(List<ContaReceberDto> ContasReceber, int Total)> ListarContasReceberPaginadoAsync(
            bool? recebido, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _uow.ContasReceber.Query(_empresaId);

            if (recebido.HasValue)
                query = query.Where(a => a.Recebido == recebido);

            var total = await query.CountAsync();
            var contas = await query
                .OrderBy(a => a.Vencimento)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (ToListDto(contas), total);
        }

        private static List<ContaReceberDto> ToListDto(List<ContaReceber> contasReceber)
        {
            var dtoList = new List<ContaReceberDto>();
            foreach (var conta in contasReceber)
            {
                dtoList.Add(ToDto(conta));
            }
            return dtoList;
        }

        private static ContaReceberDto ToDto(ContaReceber contaReceber)
        {
            return new ContaReceberDto
            {
                Id = contaReceber.Id,
                Valor = contaReceber.Valor,
                Vencimento = contaReceber.Vencimento,
                Descricao = contaReceber.Descricao,
                Observacao = contaReceber.Observacao,
                Recebido = contaReceber.Recebido,
                DataRecebimento = contaReceber.DataRecebimento,
                MeioRecebimento = contaReceber.MeioRecebimento,
                ObsRecebimento = contaReceber.ObsRecebimento,
                Excluido = contaReceber.Excluido,
                DataExclusao = contaReceber.DataExclusao,
                MotivoExclusao = contaReceber.MotivoExclusao,
                PessoaId = contaReceber.PessoaId,
                CategoriaId = contaReceber.CategoriaId,
                SubCategoriaId = contaReceber.SubCategoriaId,
                PessoaNome = contaReceber.Pessoa != null
                    ? contaReceber.Pessoa!.Nome : null,
                CategoriaNome = contaReceber.Categoria != null
                    ? contaReceber.Categoria!.Nome : null,
                SubCategoriaNome = contaReceber.SubCategoria != null
                    ? contaReceber.SubCategoria!.Nome : null,
            };
        }

        public async Task RegistrarRecebimentoAsync(Guid id, RegistrarRecebimentoDto dto,
            CancellationToken cancellationToken)
        {
            var contaReceber = await _uow.ContasReceber.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Receber não encontrada");

            var dadosAntes = ToDto(contaReceber);

            contaReceber.RegistrarRecebimento(dto.MeioRecebimento
                , dto.DataRecebimento, dto.ObsRecebimento);

            await _uow.BeginTransactionAsync();
            await _uow.ContasReceber.SaveChangesAsync();
            await _uow.Auditoria.LogAsync(
                typeof(ContaReceber).Name,
                "Registrou Recebimento",
                dto,
                dadosAntes);
            await _uow.CommitAsync(cancellationToken);
        }

        public async Task EstornarRecebimentoAsync(Guid id, EstornarRecebimentoDto dto
            , CancellationToken cancellationToken)
        {
            var contaReceber = await _uow.ContasReceber.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Receber não encontrada");

            var dadosAntes = ToDto(contaReceber);

            contaReceber.EstornarRecebimento(dto.Observacao);

            await _uow.BeginTransactionAsync();
            await _uow.ContasReceber.SaveChangesAsync();
            await _uow.Auditoria.LogAsync(
                typeof(ContaReceber).Name,
                "Estornou Recebimento",
                dto,
                dadosAntes);
            await _uow.CommitAsync(cancellationToken);
        }

    }
}
