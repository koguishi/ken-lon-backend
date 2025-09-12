using kendo_londrina.Application.DTOs;
using kendo_londrina.Application.Services;
using kendo_londrina.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kendo_londrina.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{

    private readonly CategoriaService _service;
    public CategoriasController(CategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nome = null)
    {
        var (categorias, total) = await _service.ListarCategoriasPagAsync(page, pageSize, nome);

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        return Ok(new
        {
            totalItems = total,
            totalPages,
            currentPage = page,
            pageSize,
            categorias
        });
            
    }

    // GET: api/categorias/5
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<Categoria>> GetById(Guid id)
    {
        var categoria = await _service.ObterPorIdAsync(id);
        if (categoria is null) return NotFound();
        return Ok(categoria);
    }

    // POST: api/categorias
    [HttpPost]
    public async Task<ActionResult<Categoria>> Create([FromBody] CategoriaDto categoria)
    {
        try
        {
            await _service.CriarCategoriaAsync(categoria);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
    }

    // PUT: api/categorias/5
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CategoriaDto dto)
    {
        try
        {
            await _service.AtualizarCategoriaAsync(id, dto);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound();
            return BadRequest(ex.Message);
        }
        return NoContent();
    }

    // DELETE: api/categorias/5
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _service.ExcluirCategoriaAsync(id);
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
