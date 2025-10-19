using kendo_londrina.Application.DTOs.FichaFinanceira;
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

    [HttpPost("pdf-queue")]
    public async Task<IActionResult> EnfileirarPDF(
        [FromBody] FichaFinanceiraRequestDto request)
    {
        try
        {
            var dto = await _service.GerarFichaFinanceiraDtoAsync(
                request.PessoaId, request.VencimentoInicial, request.VencimentoFinal, request.Recebido);

            // mandar para aws sqs
            _messageQueue.SendAsync(JsonSerializer.Serialize(dto)).Wait();

            // o projeto worker:
            //  - processa fila (gerar PDF e subir para CloudFlare R2)
            //  - retornar URL do PDF

            return Ok(new
            {
                message = "Pedido de geração de PDF-Ficha Financeira enfileirado com sucesso",
                jobId = dto.JobId,
            });
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound(ex.Message);
            return BadRequest(ex.Message);
        }
    }

}
