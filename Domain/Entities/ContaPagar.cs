using kendo_londrina.Domain.Entities.BaseClasses;

namespace kendo_londrina.Domain.Entities;

public class ContaPagar : Entity
{
    public decimal Valor { get; private set; }
    public DateTime Vencimento { get; private set; }
    public string? Observacao { get; private set; }

    public bool Pago { get; private set; }
    public DateTime? DataPagamento { get; private set; }
    public string? MeioPagamento { get; private set; }
    public string? ObsPagamento { get; private set; }

    public bool Excluido { get; private set; }
    public DateTime? DataExclusao { get; private set; }
    public string? MotivoExclusao { get; private set; }

    // Chaves Estrangeiras (IDs) - Tornadas "nullable" para o seu requisito
    public int? PessoaId { get; private set; }
    public int? CategoriaId { get; private set; }
    public int? SubcategoriaId { get; private set; }

    // Propriedades de Navegação
    virtual public Pessoa? Pessoa { get; private set; }
    virtual public Categoria? Categoria { get; private set; }
    virtual public SubCategoria? SubCategoria { get; private set; }

    public void RegistrarPagamento(string meio, string? obs)
    {
        if (Excluido) throw new DomainException("Não é possível pagar uma conta excluída.");
        DataPagamento = DateTime.Now;
        MeioPagamento = meio;
        ObsPagamento = obs;
    }

    public void Excluir(string motivo)
    {
        if (Pago) throw new DomainException("Não é possível excluir conta paga.");
        Excluido = true;
        DataExclusao = DateTime.Now;
        MotivoExclusao = motivo;
    }
}
