using kendo_londrina.Domain;
using kendo_londrina.Domain.Entities;

namespace kendo_londrina.UnitTests.Domain;

public class PessoaTest
{
    [Fact]
    public void Constructor_ShouldAllowOptionalParametersToBeNull()
    {
        // Arrange
        var empresaId = Guid.NewGuid();
        var nome = "Sem CPF/CNPJ";

        // Act
        var pessoa = new Pessoa(empresaId, nome);

        // Assert
        Assert.Equal(empresaId, pessoa.EmpresaId);
        Assert.Equal(nome, pessoa.Nome);
        Assert.Null(pessoa.Cpf);
        Assert.Null(pessoa.Cnpj);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenCpfAndCnpjIsNotNullOrEmpty()
    {
        // Arrange
        var empresaId = Guid.NewGuid();
        var nome = "Fulano de Tal";
        var codigo = "123";
        var cpf = "11122233344";
        var cnpj = "12345678000199";

        // Act & Assert
        Assert.Throws<DomainException>(() => new Pessoa(empresaId, nome, codigo, cpf, cnpj));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenEmpresaIdIsEmpty()
    {
        // Arrange
        var empresaId = Guid.Empty;

        // Act & Assert
        Assert.Throws<DomainException>(() => new Pessoa(empresaId, "nome da pessoa"));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNomeIsNullOrEmpty()
    {
        // Arrange
        var empresaId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() => new Pessoa(empresaId, string.Empty));
    }

    [Fact]
    public void Atualizar_ShouldUpdateProperties()
    {
        
    }
}
