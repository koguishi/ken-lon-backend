using kendo_londrina.Application.DTOs;
using kendo_londrina.Application.DTOs.FichaFinanceira;
using kendo_londrina.Infra.PDF_Generators;
using kendo_londrina.Infra.Storages;

namespace kendo_londrina.Application.Services;

public class FichaFinanceiraService
{
    private readonly Guid _empresaId;
    private readonly PessoaService _servicePessoa;
    private readonly ContaReceberService _serviceContaReceber;
    private readonly IPdfGenerator _pdfGen;
    private readonly IFileStorage _fileStorage;

    public FichaFinanceiraService(
        IPdfGenerator pdfGen,
        IFileStorage fileStorage,
        ICurrentUserService currentUser,
        PessoaService servicePessoa,
        ContaReceberService serviceContaReceber)
    {
        _pdfGen = pdfGen;
        _fileStorage = fileStorage;
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
            // JobId = Guid.NewGuid(),
            JobId = $"ficha-finan-{pessoa.Nome}-{vencimentoInicial.ToString("ddMMyy")}-{vencimentoFinal.ToString("ddMMyy")}",
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

    public async Task<FileInfoDto> GerarPDFAsync(
        Guid pessoaId,
        DateTime vencimentoInicial,
        DateTime vencimentoFinal,
        bool? recebido = null)
    {
        var dto = await GerarFichaFinanceiraDtoAsync(pessoaId, vencimentoInicial, vencimentoFinal, recebido);

        var ms = _pdfGen.FichaFinanceira(dto);
        Console.WriteLine($"    >>>  PDF gerado !!!");

        var nomePdf = $"ficha-finan-{dto.NomePessoa}-{dto.VencimentoInicial.ToString("ddMMyy")}-{dto.VencimentoFinal.ToString("ddMMyy")}.pdf";
        // === ARMAZENAR NO R2 ===
        var fileInfoDto = await _fileStorage.UploadPdfAsync(
            ms,
            nomePdf
        );
        Console.WriteLine($"    >>>  Enviado para R2 !!!");
        return fileInfoDto;
    }

    public async Task<FileInfoDto> GetPdfPreSignedURL(string bucketName, string key)
    {
        return await _fileStorage.FilePreSignedURL(bucketName, key);
    }    

}
