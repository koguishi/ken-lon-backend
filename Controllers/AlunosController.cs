using kendo_londrina.Application.DTOs;
using kendo_londrina.Application.Services;
using kendo_londrina.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace kendo_londrina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlunosController : ControllerBase
{

    private readonly AlunoService _service;
    public AlunosController(AlunoService service)
    {
        _service = service;
    }

    // [HttpGet("paginado")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var (alunos, total) = await _service.ListarAlunosPaginadosAsync(page, pageSize);

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        return Ok(new
        {
            totalItems = total,
            totalPages,
            currentPage = page,
            pageSize,
            data = alunos
        });
            
    }

    // GET: api/alunos/5
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<Aluno>> GetById(Guid id)
    {
        var aluno = await _service.ObterPorIdAsync(id);
        if (aluno is null) return NotFound();
        return Ok(aluno);
    }

    // POST: api/alunos
    [HttpPost]
    public async Task<ActionResult<Aluno>> Create([FromBody] AlunoDto aluno)
    {
        await _service.CriarAlunoAsync(aluno);
        return CreatedAtAction(nameof(GetById), new { id = aluno.Id }, aluno);
    }

    // PUT: api/alunos/5
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AlunoDto dto)
    {
        await _service.AtualizarAlunoAsync(id, new AlunoDto
        {
            Nome = dto.Nome,
            Cpf = dto.Cpf,
            TelCelular = dto.TelCelular,
            Email = dto.Email,
            DataNascimento = dto.DataNascimento,
            Nacionalidade = dto.Nacionalidade,
            UfNascimento = dto.UfNascimento,
            CidadeNascimento = dto.CidadeNascimento,
            Sexo = dto.Sexo,
            Rg = dto.Rg,
            Religiao = dto.Religiao
        });
        return NoContent();
    }

    // DELETE: api/alunos/5
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var aluno = await _service.ObterPorIdAsync(id);
        if (aluno is null) return NotFound();
        await _service.ExcluirAlunoAsync(aluno);

        return NoContent();
    }
}
