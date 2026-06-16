using kendo_londrina.Application.Services;
using kendo_londrina.Domain;

namespace kendo_londrina.UnitTests.Application;

public class ResponsavelServiceTest
{
    private readonly ResponsavelService _service;

    public ResponsavelServiceTest()
    {
        // Sem banco, sem mocks complexos — só a regra de negócio
        _service = new ResponsavelService(null!, null!, Guid.Empty);
    }

    [Fact]
    public void CalcularValor_ComUmAluno_DeveRetornar50()
    {
        var valor = _service.CalcularValorMensalidade(1);
        Assert.Equal(50m, valor);
    }

    [Fact]
    public void CalcularValor_ComDoisAlunos_DeveRetornar60()
    {
        var valor = _service.CalcularValorMensalidade(2);
        Assert.Equal(60m, valor);
    }

    [Fact]
    public void CalcularValor_ComTresAlunos_DeveRetornar70()
    {
        var valor = _service.CalcularValorMensalidade(3);
        Assert.Equal(70m, valor);
    }

    [Fact]
    public void CalcularValor_ComZeroAlunos_DeveLancarExcecao()
    {
        Assert.Throws<DomainException>(() => _service.CalcularValorMensalidade(0));
    }

    [Fact]
    public void CalcularValor_ComNumeroNegativo_DeveLancarExcecao()
    {
        Assert.Throws<DomainException>(() => _service.CalcularValorMensalidade(-1));
    }
}