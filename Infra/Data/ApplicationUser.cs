using kendo_londrina.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace kendo_londrina.Infra.Data;

public class ApplicationUser : IdentityUser
{
    // Campos extras se precisar
    // public string? Nome { get; set; }
    public string? EmpresaRole { get; set; }
    public Guid? EmpresaId { get; private set; }
    public Empresa? Empresa { get; private set; }
    public void VincularEmpresa(Guid empresaId)
    {
        EmpresaId = empresaId;
    }
}