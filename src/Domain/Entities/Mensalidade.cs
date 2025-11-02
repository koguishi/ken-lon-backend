using kendo_londrina.Domain.Entities.BaseClasses;

namespace kendo_londrina.Domain.Entities
{
    public class Mensalidade : Entity
    {
        public Guid AlunoId { get; private set; }
        public string? AlunoNome { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public DateTime? DataPagamento { get; private set; }
        public string? MeioPagamento { get; private set; }
        public string? Observacao { get; private set; }
        public bool Excluida { get; private set; }
        public string? MotivoExclusao { get; private set; }
        public DateTime? DataExclusao { get; private set; }
        virtual public Aluno? Aluno { get; private set; }

        private Mensalidade() { } // para EF Core

        public Mensalidade(Guid alunoId, string alunoNome, decimal valor, DateTime vencimento)
        {
            AlunoId = alunoId;
            AlunoNome = alunoNome;
            Valor = valor;
            DataVencimento = vencimento;
        }

        public void RegistrarPagamento(string meio, string? obs)
        {
            if (Excluida) throw new InvalidOperationException("Não é possível pagar uma mensalidade excluída.");
            DataPagamento = DateTime.Now;
            MeioPagamento = meio;
            Observacao = obs;
        }

        public void Excluir(string motivo)
        {
            if (DataPagamento != null) throw new InvalidOperationException("Não é possível excluir mensalidade já paga.");
            Excluida = true;
            MotivoExclusao = motivo;
            DataExclusao = DateTime.Now;
        }
    }
}
