using kendo_londrina.Application.DTOs;
using kendo_londrina.Application.Services;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            
        // return Ok(new
        // {
        //     Total = total,
        //     Page = page,
        //     PageSize = pageSize,
        //     Alunos = alunos
        // });
    }

    // private readonly KendoLondrinaContext _context;

    // public AlunosController(KendoLondrinaContext context)
    // {
    //     _context = context;
    // }

    // GET: api/alunos
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<Aluno>>> GetAll()
    // {
    //     var alunos = await _context.Alunos.AsNoTracking().ToListAsync();
    //     return Ok(alunos);
    // }

    // GET: api/alunos?page=1&pageSize=10
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<Aluno>>> GetAll(
    //     [FromQuery] int page = 1,
    //     [FromQuery] int pageSize = 10)
    // {
    //     if (page <= 0) page = 1;
    //     if (pageSize <= 0) pageSize = 10;

    //     var totalAlunos = await _context.Alunos.CountAsync();
    //     var totalPages = (int)Math.Ceiling(totalAlunos / (double)pageSize);

    //     var alunos = await _context.Alunos
    //         .AsNoTracking()
    //         .OrderBy(a => a.Nome)
    //         .Skip((page - 1) * pageSize)
    //         .Take(pageSize)
    //         .ToListAsync();

    //     var response = new
    //     {
    //         totalItems = totalAlunos,
    //         totalPages,
    //         currentPage = page,
    //         pageSize,
    //         data = alunos
    //     };

    //     return Ok(response);
    // }

    // GET: api/alunos/5
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<Aluno>> GetById(Guid id)
    {
        var aluno = await _service.ObterPorIdAsync(id);
        // var aluno = await _context.Alunos.FindAsync(id);
        if (aluno is null) return NotFound();
        return Ok(aluno);
    }

    // POST: api/alunos
    [HttpPost]
    public async Task<ActionResult<Aluno>> Create([FromBody] Aluno aluno)
    {
        await _service.CriarAlunoAsync(aluno.Nome, aluno.DataNascimento,
            aluno.Cpf, aluno.TelCelular, aluno.Email);
        // if (!ModelState.IsValid) return ValidationProblem(ModelState);
        // _context.Alunos.Add(aluno);
        // await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = aluno.Id }, aluno);
    }

    // PUT: api/alunos/5
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Aluno dto)
    {
        if (id != dto.Id) return BadRequest("ID do caminho difere do corpo.");
        // if (!await _context.Alunos.AnyAsync(a => a.Id == id)) return NotFound();

        // _context.Entry(dto).State = EntityState.Modified;
        // await _context.SaveChangesAsync();

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

        // var aluno = await _context.Alunos.FindAsync(id);
        // if (aluno is null) return NotFound();

        // _context.Alunos.Remove(aluno);
        // await _context.SaveChangesAsync();
        return NoContent();
    }
}
