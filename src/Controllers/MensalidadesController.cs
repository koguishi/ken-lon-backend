using Microsoft.AspNetCore.Mvc;
using kendo_londrina.Application.Services;

namespace kendo_londrina.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MensalidadesController : ControllerBase
    {
        private readonly MensalidadeService _service;

        public MensalidadesController(MensalidadeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarMensalidadeDto dto)
        {
            var mensalidade = await _service.CriarMensalidadeAsync(dto.AlunoId, dto.AlunoNome, dto.Valor, dto.DataVencimento);
            return Ok(mensalidade);
        }

        [HttpPut("{id}/pagar")]
        public async Task<IActionResult> Pagar(Guid id, [FromBody] RegistrarPagamentoDto dto)
        {
            await _service.RegistrarPagamentoAsync(id, dto.MeioPagamento, dto.Observacao);
            return NoContent();
        }

        [HttpPut("{id}/excluir")]
        public async Task<IActionResult> Excluir(Guid id, [FromBody] ExcluirDto dto)
        {
            await _service.ExcluirMensalidadeAsync(id, dto.Motivo);
            return NoContent();
        }
    }

    public record CriarMensalidadeDto(Guid AlunoId, string AlunoNome, decimal Valor, DateTime DataVencimento);
    public record RegistrarPagamentoDto(string MeioPagamento, string? Observacao);
    public record ExcluirDto(string Motivo);
}
