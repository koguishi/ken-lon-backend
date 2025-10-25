using System.Text.Json;
using pdf_gen_worker.Queues;
using pdf_gen_worker.Storages;
using pdf_gen_worker.PDF_Generators;

namespace pdf_gen_worker;

public class Worker(
    ILogger<Worker> logger,
    IMessageQueueConsumer queueConsumer,
    IFileStorage storage,
    IPdfGenerator pdfGen
    ) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IMessageQueueConsumer _queueConsumer = queueConsumer;
    private readonly IFileStorage _storage = storage;
    private readonly IPdfGenerator _pdfGen = pdfGen;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _queueConsumer.ConsumeAsync("fichas_financeiras", ProcessarMensagemAsync, stoppingToken);
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        return;
    }

    private record PedidoFicha(Guid PessoaId, int Ano, string EmailDestino);

    private async Task ProcessarMensagemAsync(string json)
    {
        try
        {
            var dtoFicha = JsonSerializer.Deserialize<FichaFinanceiraDto>(json)!;
            Console.WriteLine($"📄 Gerando ficha para {dtoFicha.NomePessoa} ({dtoFicha.Ano})...");
            Console.WriteLine($"jobId: {dtoFicha.JobId}");

            // === GERA O PDF NA MEMÓRIA ===
            var ms = _pdfGen.FichaFinanceira(dtoFicha);
            Console.WriteLine($"    >>>  PDF gerado !!!");

            var nomePdf = $"ficha-finan-{dtoFicha.NomePessoa}-{dtoFicha.VencimentoInicial.ToString("ddMMyy")}-{dtoFicha.VencimentoFinal.ToString("ddMMyy")}.pdf";
            // === ARMAZENAR NO R2 ===
            await _storage.UploadPdfAsync(
                ms,
                nomePdf
            );
            Console.WriteLine($"    >>>  Enviado para R2 !!!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao processar mensagem: {ex.Message}");
            throw new Exception(ex.Message);
        }

    }
}
