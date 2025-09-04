using Microsoft.EntityFrameworkCore;
using kendo_londrina.Domain.Entities;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Application.DTOs;

namespace kendo_londrina.Application.Services
{
    public class AlunoService
    {
        private readonly IAlunoRepository _repo;

        public AlunoService(IAlunoRepository repo)
        {
            _repo = repo;
        }

        public async Task<Aluno> CriarAlunoAsync(string nome, DateTime dataNascimento,
            string? cpf = null, string? telCelular = null, string? email = null)
        {
            var aluno = new Aluno(nome, dataNascimento, cpf, telCelular, email);
            await _repo.AddAsync(aluno);
            await _repo.SaveChangesAsync();
            return aluno;
        }

        // public async Task ExcluirAlunoAsync(Guid id)
        // {
        //     var aluno = await _repo.GetByIdAsync(id) ?? throw new Exception("Aluno não encontrado");
        //     await _repo.DeleteAsync(aluno);
        // }

        public async Task ExcluirAlunoAsync(Aluno aluno)
        {
            await _repo.DeleteAsync(aluno);
        }         

        public async Task<List<Aluno>> ListarAlunosAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Aluno?> ObterPorIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AtualizarAlunoAsync(Guid id, AlunoDto dto)
        {
            var aluno = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Aluno não encontrado");

            if (dto.Nome == null)
                throw new Exception("Nome do aluno não pode ser nulo");

            if (dto.DataNascimento == null)
                throw new Exception("Data de nascimento do aluno não pode ser nula");

            aluno.Atualizar(dto.Nome, dto.DataNascimento.Value,
                dto.Cpf, dto.TelCelular, dto.Email,
                dto.Nacionalidade, dto.UfNascimento, dto.CidadeNascimento,
                dto.Sexo, dto.Rg, dto.Religiao);

            await _repo.SaveChangesAsync();
        }

        public async Task<(List<Aluno> Alunos, int Total)> ListarAlunosPaginadosAsync(
            int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _repo.Query(); // vamos criar Query() no repositório
            var total = await query.CountAsync();
            var alunos = await query
                .OrderBy(a => a.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (alunos, total);
        }        
    }
}
