using kendo_londrina.Application.DTOs;

namespace kendo_londrina.Application.Services
{
    public class FichaFinanceiraService
    {
        private readonly Guid _empresaId;
        private readonly ContaReceberService _serviceContaReceber;

        public FichaFinanceiraService(
            ICurrentUserService currentUser,
            ContaReceberService serviceContaReceber)
        {
            _empresaId = Guid.Parse(currentUser.EmpresaId!);
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
            Guid pessoaId, int ano, bool? recebido = null)
        {
            var vencimentoInicial = new DateTime(ano, 1, 1);
            var vencimentoFinal = new DateTime(ano, 12, 31);
            var (contas, total) = await _serviceContaReceber.ListarPorPessoaVencimentoAsync(
                pessoaId, vencimentoInicial, vencimentoFinal, recebido
            );

            var dto = new FichaFinanceiraDto()
            {
                JobId = Guid.NewGuid(),
                NomePessoa = "",
                Ano = ano,
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
}
