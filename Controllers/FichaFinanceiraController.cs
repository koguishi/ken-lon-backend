using kendo_londrina.Application.Services;
using kendo_londrina.Infra.MessageQueue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace kendo_londrina.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FichaFinanceiraController : ControllerBase
{
    private readonly FichaFinanceiraService _service;
    private readonly IMessageQueue _messageQueue;
    public FichaFinanceiraController(
        FichaFinanceiraService service,
        IMessageQueue messageQueue)
    {
        _service = service;
        _messageQueue = messageQueue;
    }

    [HttpGet("{pessoaId:Guid}/ano/{ano:int}")]
    public async Task<IActionResult> FichaAnual(
        Guid pessoaId,
        int ano,
        [FromQuery] bool? recebido = null)
    {
        var (contas, total) = await _service.FichaAnualAsync(
            pessoaId, ano, recebido);

        return Ok(new
        {
            totalItems = total,
            contas
        });
    }

    [HttpPost("{pessoaId:Guid}/ano/{ano:int}/gerar-pdf")]
    public async Task<IActionResult> EnviarFilaGeracaoPdf(
        Guid pessoaId,
        int ano,
        [FromQuery] bool? recebido = null)
    {

        var dto = await _service.GerarFichaFinanceiraDtoAsync(
            pessoaId, ano, recebido);

        // mandar para aws sqs
        _messageQueue.SendAsync(JsonSerializer.Serialize(dto)).Wait();

        // o projeto worker:
        //  - processa fila (gerar PDF e subir para CloudFlare R2)
        //  - retornar URL do PDF

        return Ok(new
        {
            message = "Pedido enfileirado com sucesso",
            pessoa = dto.NomePessoa,
            ano = dto.Ano,
            jobId = dto.JobId,
        });
    }

}
