namespace kendo_londrina.Application.DTOs;

public class RegistrarRecebimentoDto
{
    public DateTime DataRecebimento { get; set; }
    public string MeioRecebimento { get; set; } = string.Empty;
    public string ObsRecebimento { get; set; } = string.Empty;
}
