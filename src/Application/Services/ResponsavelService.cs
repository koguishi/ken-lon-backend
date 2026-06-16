using kendo_londrina.Domain;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;

namespace kendo_londrina.Application.Services;

public class ResponsavelService
{
    private readonly IResponsavelRepository _repo;
    private readonly IAlunoRepository _alunoRepo;
    private readonly Guid _empresaId;

    // Regra de negócio centralizada aqui
    private const decimal ValorPrimeiroAluno = 50m;
    private const decimal ValorAlunoPosterior = 10m;

    public ResponsavelService(IResponsavelRepository repo,
        IAlunoRepository alunoRepo,
        Guid empresaId)
    {
        _repo = repo;
        _alunoRepo = alunoRepo;
        _empresaId = empresaId;
    }

    public async Task<Responsavel> CriarAsync(string nome, string? telefone, string? email)
    {
        var responsavel = new Responsavel(_empresaId, nome, telefone, email);
        await _repo.AddAsync(responsavel);
        await _repo.SaveChangesAsync();
        return responsavel;
    }

    public async Task AtualizarAsync(Guid id, string nome, string? telefone, string? email)
    {
        var responsavel = await _repo.GetByIdAsync(_empresaId, id)
            ?? throw new Exception("Responsável não encontrado.");

        responsavel.Atualizar(nome, telefone, email);
        await _repo.SaveChangesAsync();
    }

    public async Task<IEnumerable<Responsavel>> ListarAsync() =>
        await _repo.GetAllAsync(_empresaId);

    public async Task<Responsavel?> ObterAsync(Guid id) =>
        await _repo.GetByIdAsync(_empresaId, id);

    public decimal CalcularValorMensalidade(int quantidadeAlunos)
    {
        if (quantidadeAlunos <= 0)
            throw new DomainException("Responsável deve ter ao menos um aluno.");

        if (quantidadeAlunos == 1)
            return ValorPrimeiroAluno;

        return ValorPrimeiroAluno + (ValorAlunoPosterior * (quantidadeAlunos - 1));
    }

    public async Task VincularAlunoAsync(Guid responsavelId, Guid alunoId)
    {
        var responsavel = await _repo.GetByIdAsync(_empresaId, responsavelId)
            ?? throw new Exception("Responsável não encontrado.");

        var aluno = await _alunoRepo.GetByIdAsync(alunoId)
            ?? throw new Exception("Aluno não encontrado.");

        if (aluno.ResponsavelId == responsavelId)
            throw new DomainException("Aluno já está vinculado a este responsável.");

        aluno.VincularResponsavel(responsavelId);
        await _alunoRepo.SaveChangesAsync();
    }

    public async Task DesvincularAlunoAsync(Guid responsavelId, Guid alunoId)
    {
        var aluno = await _alunoRepo.GetByIdAsync(alunoId)
            ?? throw new Exception("Aluno não encontrado.");

        if (aluno.ResponsavelId != responsavelId)
            throw new DomainException("Aluno não está vinculado a este responsável.");

        aluno.DesvincularResponsavel();
        await _alunoRepo.SaveChangesAsync();
    }
}
