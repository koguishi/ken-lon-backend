using kendo_londrina.Application.DTOs;
using kendo_londrina.Application.Services;
using kendo_londrina.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kendo_londrina.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PessoasController(PessoaService service) : ControllerBase
{

    private readonly PessoaService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nome = null)
    {
        var (pessoas, total) = await _service.ListarPessoasPagAsync(page, pageSize, nome);

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        return Ok(new
        {
            totalItems = total,
            totalPages,
            currentPage = page,
            pageSize,
            pessoas
        });
            
    }

    // GET: api/pessoas/5
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<Pessoa>> GetById(Guid id)
    {
        var pessoa = await _service.ObterPorIdAsync(id);
        if (pessoa is null) return NotFound();
        return Ok(pessoa);
    }

    // POST: api/pessoas
    [HttpPost]
    public async Task<ActionResult<Pessoa>> Create([FromBody] PessoaDto pessoa)
    {
        await _service.CriarPessoaAsync(pessoa);
        return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, pessoa);
    }

    // PUT: api/pessoas/5
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PessoaDto dto)
    {
        try
        {
            await _service.AtualizarPessoaAsync(id, dto);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound();
            return BadRequest(new { message = ex.Message });
        }
        return NoContent();
    }

    // DELETE: api/pessoas/5
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var pessoa = await _service.ObterPorIdAsync(id);
        if (pessoa is null) return NotFound();
        await _service.ExcluirPessoaAsync(pessoa.Id);

        return NoContent();
    }
}
