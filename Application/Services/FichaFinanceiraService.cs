using kendo_londrina.Application.DTOs;
using kendo_londrina.Application.DTOs.FichaFinanceira;

namespace kendo_londrina.Application.Services;

public class FichaFinanceiraService
{
    private readonly Guid _empresaId;
    private readonly PessoaService _servicePessoa;
    private readonly ContaReceberService _serviceContaReceber;

    public FichaFinanceiraService(
        ICurrentUserService currentUser,
        PessoaService servicePessoa,
        ContaReceberService serviceContaReceber)
    {
        _empresaId = Guid.Parse(currentUser.EmpresaId!);
        _servicePessoa = servicePessoa;
        _serviceContaReceber = serviceContaReceber;
    }

    public async Task<(List<ContaReceberDto>, int Total)> FichaAnualAsync(
        Guid pessoaId, int ano, bool? recebido = null)
    {
        var vencimentoInicial = new DateTime(ano, 1, 1);
        var vencimentoFinal = new DateTime(ano, 12, 31);
        return await _serviceContaReceber.ListarPorPessoaVencimentoAsync(
            pessoaId, vencimentoInicial, vencimentoFinal, recebido
        );
    }

    public async Task<FichaFinanceiraDto> GerarFichaFinanceiraDtoAsync(
        Guid pessoaId,
        DateTime vencimentoInicial,
        DateTime vencimentoFinal,
        bool? recebido = null)
    {
        var (contas, total) = await _serviceContaReceber.ListarPorPessoaVencimentoAsync(
            pessoaId, vencimentoInicial, vencimentoFinal, recebido
        );
        if (total == 0)
            throw new Exception("Título não encontrado para os filtros informados");

        var pessoa = await _servicePessoa.ObterPorIdAsync(pessoaId)
            ?? throw new Exception("Pessoa não encontrada");

        var dto = new FichaFinanceiraDto()
        {
            JobId = Guid.NewGuid(),
            NomePessoa = pessoa.Nome,
            VencimentoInicial = vencimentoInicial,
            VencimentoFinal = vencimentoFinal,
            Titulos = [.. contas.Select(c => new TituloDto
            {
                Vencimento = c.Vencimento,
                Valor = c.Valor,
                DataLiquidacao = c.DataRecebimento
            })]
        };
        return dto;
    }

}
