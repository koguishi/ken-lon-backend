namespace kendo_londrina.Domain.Entities;

public class Empresa
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string NomeFantasia { get; private set; } = string.Empty;
    public string? Cpf { get; private set; }
    public string? Cnpj { get; private set; }
    public string? RazaoSocial { get; private set; }
    public string? Cep { get; private set; }
    public string Uf { get; private set; } = string.Empty;
    public string Cidade { get; private set; } = string.Empty;
    public string? Bairro { get; private set; }
    public string? Endereco { get; private set; }
    public string? Telefone { get; private set; }
    public string? Email { get; private set; }
    public string? Website { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime? CreatedOn { get; private set; }
    public string? EditedBy { get; private set; }
    public DateTime? EditedOn { get; private set; }
    virtual public IEnumerable<Pessoa>? Pessoas { get; private set; }
    virtual public IEnumerable<Categoria>? Categorias { get; private set; }
    virtual public IEnumerable<SubCategoria>? SubCategorias { get; private set; }

    public Empresa(string nomeFantasia,
        string uf, string cidade)
    {
        Id = Guid.NewGuid();
        NomeFantasia = nomeFantasia;
        Uf = uf;
        Cidade = cidade;
    }

    // Construtor vazio para EF Core
    private Empresa() { }

    public void Atualizar(
        string nomeFantasia,
        string uf,
        string cidade,
        string? cpf = null,
        string? cnpj = null,
        string? razaoSocial = null,
        string? cep = null,
        string? bairro = null,
        string? endereco = null,
        string? telefone = null,
        string? email = null,
        string? website = null)
    {
        NomeFantasia = nomeFantasia;
        Uf = uf;
        Cidade = cidade;
        Cpf = cpf;
        Cnpj = cnpj;
        RazaoSocial = razaoSocial;
        Cep = cep;
        Bairro = bairro;
        Endereco = endereco;
        Telefone = telefone;
        Email = email;
        Website = website;
    }
}
