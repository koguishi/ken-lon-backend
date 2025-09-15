using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Application.DTOs;

namespace kendo_londrina.Application.Services
{
    public class ContaReceberService
    {
        private readonly IContaReceberRepository _repo;
        private readonly IPessoaRepository _repoPessoa;
        private readonly ICategoriaRepository _repoCategoria;
        private readonly ICurrentUserService _currentUser;
        private readonly Guid _empresaId;
        private readonly AuditoriaService _auditoriaService;

        public ContaReceberService(IContaReceberRepository repo
            , IPessoaRepository repoPessoa
            , ICategoriaRepository repoCategoria
            , ICurrentUserService currentUser
            , AuditoriaService auditoriaService)
        {
            _repo = repo;
            _repoPessoa = repoPessoa;
            _repoCategoria = repoCategoria;
            _currentUser = currentUser;
            _empresaId = Guid.Parse(_currentUser.EmpresaId!);
            _auditoriaService = auditoriaService;
        }

        private async Task VerificarVinculos(ContaReceberDto dto)
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

        public async Task<ContaReceberDto> CriarContaReceberAsync(ContaReceberDto dto)
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
            await _repo.AddAsync(contaReceber);

            var contaInseridaDto = ToDto(contaReceber);
            await _auditoriaService.LogAsync(
                typeof(ContaReceber).Name,
                "iNSERIU",
                contaInseridaDto);

            await _repo.SaveChangesAsync();
            return contaInseridaDto;
        }

        public async Task ExcluirContaReceberAsync(Guid id)
        {
            var contaReceber = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Receber não encontrada");
            var contaReceberDto = ToDto(contaReceber);
            await _repo.DeleteAsync(contaReceber);
            await _auditoriaService.LogAsync<ContaReceberDto>(
                typeof(ContaReceber).Name,
                "eXCLUIU",
                null,
                contaReceberDto);
        }

        public async Task<List<ContaReceberDto>> ListarContasReceberAsync()
        {
            var contas = await _repo.GetAllAsync(_empresaId);
            return ToListDto(contas);
        }

        public async Task<ContaReceberDto?> ObterPorIdAsync(Guid id)
        {
            var conta = await _repo.GetByIdAsync(_empresaId, id);
            return (conta == null)
                ? null
                : ToDto(conta);
        }

        public async Task AlterarContaReceberAsync(Guid id, ContaReceberDto dto)
        {
            await VerificarVinculos(dto);
            var contaReceber = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Receber não encontrada");

            var dadosAntes = ToDto(contaReceber);

            contaReceber.Alterar(dto.Valor, dto.Vencimento, dto.Descricao, dto.Observacao
                , dto.PessoaId, dto.CategoriaId, dto.SubCategoriaId);

            dto.Id = id;
            await _auditoriaService.LogAsync(
                typeof(ContaReceber).Name,
                "aLTEROU",
                dto,
                dadosAntes);
            await _repo.SaveChangesAsync();
        }

        public async Task<(List<ContaReceberDto> ContasReceber, int Total)> ListarContasReceberPaginadoAsync(
            bool? recebido, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _repo.Query(_empresaId); // vamos criar Query() no repositório

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
            };
        }

        public async Task RegistrarRecebimentoAsync(Guid id, RegistrarRecebimentoDto dto)
        {
            var contaReceber = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Receber não encontrada");

            var dadosAntes = ToDto(contaReceber);

            contaReceber.RegistrarRecebimento(dto.MeioRecebimento
                , dto.DataRecebimento, dto.ObsRecebimento);

            await _auditoriaService.LogAsync(
                typeof(ContaReceber).Name,
                "Registrou Recebimento",
                dto,
                dadosAntes);
            await _repo.SaveChangesAsync();
        }

        public async Task EstornarRecebimentoAsync(Guid id, EstornarRecebimentoDto dto)
        {
            var contaReceber = await _repo.GetByIdAsync(_empresaId, id)
                ?? throw new Exception("Conta a Receber não encontrada");

            var dadosAntes = ToDto(contaReceber);

            contaReceber.EstornarRecebimento(dto.Observacao);

            await _auditoriaService.LogAsync(
                typeof(ContaReceber).Name,
                "Estornou Recebimento",
                dto,
                dadosAntes);
            await _repo.SaveChangesAsync();
        }
    }
}
