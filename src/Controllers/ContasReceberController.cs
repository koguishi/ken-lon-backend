using kendo_londrina.Application.DTOs;
using kendo_londrina.Application.DTOs.ContaReceber;
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
            page, pageSize, recebido);

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
    public async Task<ActionResult> Create([FromBody] ContaReceberInsercaoDto dto)
    {
        try
        {
            var contas = await _service.CriarContasReceberAsync(dto, HttpContext.RequestAborted);
            dto.Id = contas[0].Id;
            return contas.Count == 1
                ? CreatedAtAction(nameof(GetById), new { id = contas[0].Id }, dto)
                : Created("", contas);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/contasreceber/5
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ContaReceberAlteracaoDto dto)
    {
        try
        {
            await _service.AlterarContaReceberAsync(id, dto, HttpContext.RequestAborted);
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
            await _service.ExcluirContaReceberAsync(id, HttpContext.RequestAborted);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound();
            return BadRequest(new { message = ex.Message });
        }
        return NoContent();
    }

    // PATCH: api/contasreceber/registrar-recebimento/5
    [HttpPatch("registrar-recebimento/{id:Guid}")]
    public async Task<IActionResult> RegistrarRecebimento(Guid id, [FromBody] RegistrarRecebimentoDto dto)
    {
        try
        {
            await _service.RegistrarRecebimentoAsync(id, dto, HttpContext.RequestAborted);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound(ex.Message);
            return BadRequest(ex.Message);
        }
        return NoContent();
    }

    // PATCH: api/contasreceber/estornar-recebimento/5
    [HttpPatch("estornar-recebimento/{id:Guid}")]
    public async Task<IActionResult> EstornarRecebimento(Guid id, [FromBody] EstornarRecebimentoDto dto)
    {
        try
        {
            await _service.EstornarRecebimentoAsync(id, dto, HttpContext.RequestAborted);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound(ex.Message);
            return BadRequest(ex.Message);
        }
        return NoContent();
    }
    
    // GET: api/contasreceber/
    [HttpGet("by-pessoa/{pessoaId:Guid}")]
    public async Task<IActionResult> GetByPessoaId(
        Guid pessoaId, 
        [FromQuery] DateTime? vencimentoInicial,
        [FromQuery] DateTime? vencimentoFinal,
        [FromQuery] bool? recebido = null)
    {
        var (contas, total) = await _service.ListarPorPessoaVencimentoAsync(
            pessoaId, vencimentoInicial, vencimentoFinal, recebido);

        return Ok(new
        {
            totalItems = total,
            contas
        });
    }

}
