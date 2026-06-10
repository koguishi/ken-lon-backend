using kendo_londrina.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace kendo_londrina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResponsaveisController : ControllerBase
{
    private readonly ResponsavelService _service;

    public ResponsaveisController(ResponsavelService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Listar() =>
        Ok(await _service.ListarAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(Guid id)
    {
        var responsavel = await _service.ObterAsync(id);
        return responsavel is null ? NotFound() : Ok(responsavel);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] ResponsavelDto dto)
    {
        var responsavel = await _service.CriarAsync(dto.Nome, dto.Telefone, dto.Email);
        return CreatedAtAction(nameof(Obter), new { id = responsavel.Id }, responsavel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] ResponsavelDto dto)
    {
        await _service.AtualizarAsync(id, dto.Nome, dto.Telefone, dto.Email);
        return NoContent();
    }
}

public record ResponsavelDto(string Nome, string? Telefone, string? Email);
