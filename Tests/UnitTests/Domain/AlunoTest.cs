using kendo_londrina.Domain;
using kendo_londrina.Domain.Entities;

namespace kendo_londrina.UnitTests.Domain;

public class AlunoTest
{
    [Fact]
    public void VincularResponsavel_DeveAtribuirResponsavelId()
    {
        var aluno = new Aluno("João", DateTime.Today);
        var responsavelId = Guid.NewGuid();

        aluno.VincularResponsavel(responsavelId);

        Assert.Equal(responsavelId, aluno.ResponsavelId);
    }

    [Fact]
    public void VincularResponsavel_ComIdVazio_DeveLancarExcecao()
    {
        var aluno = new Aluno("João", DateTime.Today);

        Assert.Throws<DomainException>(() => aluno.VincularResponsavel(Guid.Empty));
    }

    [Fact]
    public void DesvincularResponsavel_DeveNulificarResponsavelId()
    {
        var aluno = new Aluno("João", DateTime.Today);
        aluno.VincularResponsavel(Guid.NewGuid());

        aluno.DesvincularResponsavel();

        Assert.Null(aluno.ResponsavelId);
    }
}
