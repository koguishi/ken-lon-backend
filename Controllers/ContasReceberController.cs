using kendo_londrina.Application.DTOs;
using kendo_londrina.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kendo_londrina.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContasReceberController(ContaReceberService service) : ControllerBase
{
    private readonly ContaReceberService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? recebido = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var (contas, total) = await _service.ListarContasReceberPaginadoAsync(
            recebido, page, pageSize);

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        return Ok(new
        {
            totalItems = total,
            totalPages,
            currentPage = page,
            pageSize,
            contas
        });
            
    }

    // GET: api/contasreceber/5
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<ContaReceberDto>> GetById(Guid id)
    {
        var conta = await _service.ObterPorIdAsync(id);
        if (conta is null) return NotFound();
        return Ok(conta);
    }

    // POST: api/contasreceber
    [HttpPost]
    public async Task<ActionResult<ContaReceberDto>> Create([FromBody] ContaReceberDto dto)
    {
        try
        {
            var conta = await _service.CriarContaReceberAsync(dto);
            dto.Id = conta.Id;
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound(ex.Message);
            return BadRequest(ex.Message);
        }
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    // PUT: api/contasreceber/5
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ContaReceberDto dto)
    {
        try
        {
            await _service.AlterarContaReceberAsync(id, dto);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound(ex.Message);
            return BadRequest(ex.Message);
        }
        return NoContent();
    }

    // DELETE: api/contasreceber/5
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _service.ExcluirContaReceberAsync(id);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound();
            return BadRequest(new { message = ex.Message });
        }
        return NoContent();
    }
}
