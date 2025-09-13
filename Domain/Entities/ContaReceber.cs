using kendo_londrina.Domain.Entities.BaseClasses;

namespace kendo_londrina.Domain.Entities;

public class ContaReceber : Entity
{
    public string? Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime Vencimento { get; private set; }
    public string? Observacao { get; private set; }

    public bool Recebido { get; private set; }
    public DateTime? DataRecebimento { get; private set; }
    public string? MeioRecebimento { get; private set; }
    public string? ObsRecebimento { get; private set; }

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
    private ContaReceber() { }

    public ContaReceber(Guid empresaId
        , decimal valor, DateTime vencimento
        , string? descricao, string? observacao = ""
        , Guid? pessoaId = null
        , Guid? categoriaId = null
        , Guid? subCategoriaId = null)
    {
        Descricao = descricao;
        EmpresaId = empresaId;
        Valor = valor;
        Vencimento = vencimento;
        Observacao = observacao ?? null;
        PessoaId = pessoaId ?? null;
        CategoriaId = categoriaId ?? null;
        SubCategoriaId = subCategoriaId ?? null;
    }

    public void Alterar(decimal valor, DateTime vencimento
        , string descricao
        , string? observacao = ""
        , Guid? pessoaId = null, Guid? categoriaId = null, Guid? subCategoriaId = null)
    {
        if (Excluido) throw new DomainException("Não é possível alterar uma conta excluída.");
        if (Recebido) throw new DomainException("Não é possível alterar um conta recebida.");
        Descricao = descricao;
        Valor = valor;
        Vencimento = vencimento;
        if (observacao != null)
            Observacao = observacao;
        PessoaId = pessoaId ?? null;
        CategoriaId = categoriaId ?? null;
        SubCategoriaId = subCategoriaId ?? null;
    }

    public void RegistrarRecebimento(string meio, string? obs)
    {
        if (Excluido) throw new DomainException("Não é possível receber uma conta excluída.");
        DataRecebimento = DateTime.Now;
        MeioRecebimento = meio;
        ObsRecebimento = obs;
    }

    public void Excluir(string motivo)
    {
        if (Recebido) throw new DomainException("Não é possível excluir conta recebida.");
        Excluido = true;
        DataExclusao = DateTime.Now;
        MotivoExclusao = motivo;
    }
}
