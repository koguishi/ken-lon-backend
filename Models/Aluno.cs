using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace kendo_londrina.Models;

public class Aluno_OLD
{
    // public Guid EscolaId
    // {
    //     get { return Guid.Parse("b3e53df9-b128-4227-81fe-cc0b9ad9720b"); }
    // }

    public Guid EscolaId { get; set; } = Guid.Parse("b3e53df9-b128-4227-81fe-cc0b9ad9720b");
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(15)]
    public string? Cpf { get; set; }

    [MaxLength(20)]
    public string? TelCelular { get; set; }

    [EmailAddress, MaxLength(200)]
    public string? Email { get; set; }

    public DateTime? DataNascimento { get; set; }

    // [MaxLength(20)]
    // public string? Plano { get; set; }

    // public DateTime? DataInicio { get; set; }

    [MaxLength(100)]
    public string? Nacionalidade { get; set; }

    [MaxLength(2)]
    public string? UfNascimento { get; set; }

    [MaxLength(100)]
    public string? CidadeNascimento { get; set; }

    [MaxLength(1)]
    public string? Sexo { get; set; }

    [MaxLength(50)]
    public string? Rg { get; set; }

    [MaxLength(50)]
    public string? Religiao { get; set; }    

}
