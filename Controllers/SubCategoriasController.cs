using kendo_londrina.Application.DTOs;
using kendo_londrina.Application.Services;
using kendo_londrina.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kendo_londrina.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SubCategoriasController : ControllerBase
{

    private readonly SubCategoriaService _service;
    public SubCategoriasController(SubCategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nome = null)
    {
        var (csubCtegorias, total) = await _service.ListarSubCategoriasPagAsync(page, pageSize, nome);

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        return Ok(new
        {
            totalItems = total,
            totalPages,
            currentPage = page,
            pageSize,
            csubCtegorias
        });
            
    }

    // GET: api/subCategorias/5
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<SubCategoria>> GetById(Guid id)
    {
        var subCategoria = await _service.ObterPorIdAsync(id);
        if (subCategoria is null) return NotFound();
        return Ok(subCategoria);
    }

    // POST: api/subCategorias
    [HttpPost]
    public async Task<ActionResult<SubCategoria>> Create([FromBody] SubCategoriaDto dto)
    {
        try
        {
            await _service.CriarSubCategoriaAsync(dto);
        }
        catch (System.Exception ex)
        {
            if (ex.Message.Contains("não encontrad"))
                return NotFound(new { message = ex.Message });
            return BadRequest(new { message = ex.Message });
        }
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    // PUT: api/subCategorias/5
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SubCategoriaDto dto)
    {
        await _service.AtualizarSubCategoriaAsync(id, dto);
        return NoContent();
    }

    // DELETE: api/subCategorias/5
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var subCategoria = await _service.ObterPorIdAsync(id);
        if (subCategoria is null) return NotFound();
        await _service.ExcluirSubCategoriaAsync(subCategoria);

        return NoContent();
    }
}
