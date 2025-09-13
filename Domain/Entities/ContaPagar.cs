using kendo_londrina.Application.DTOs;
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
    public Guid? PessoaId { get; private set; }
    public Guid? CategoriaId { get; private set; }
    public Guid? SubCategoriaId { get; private set; }

    // Propriedades de Navegação
    virtual public Pessoa? Pessoa { get; private set; }
    virtual public Categoria? Categoria { get; private set; }
    virtual public SubCategoria? SubCategoria { get; private set; }

    // Construtor vazio para EF Core
    private ContaPagar() { }

    public ContaPagar(Guid empresaId
        , decimal valor
        , DateTime vencimento
        , string? observacao = ""
        , Guid? pessoaId = null
        , Guid? categoriaId = null
        , Guid? subCategoriaId = null)
    {
        EmpresaId = empresaId;
        Valor = valor;
        Vencimento = vencimento;
        Observacao = observacao ?? null;
        PessoaId = pessoaId ?? null;
        CategoriaId = categoriaId ?? null;
        SubCategoriaId = subCategoriaId ?? null;
    }

    public void Alterar(decimal valor, DateTime vencimento, string? observacao = ""
        , Guid? pessoaId = null, Guid? categoriaId = null, Guid? subCategoriaId = null)
    {
        if (Excluido) throw new DomainException("Não é possível alterar uma conta excluída.");
        if (Pago) throw new DomainException("Não é possível alterar um conta paga.");
        Valor = valor;
        Vencimento = vencimento;
        if (observacao != null)
            Observacao = observacao;
        PessoaId = pessoaId ?? null;
        CategoriaId = categoriaId ?? null;
        SubCategoriaId = subCategoriaId ?? null;
    }

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
