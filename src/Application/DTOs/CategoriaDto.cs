namespace kendo_londrina.Application.DTOs;

public class CategoriaDto
{
    public Guid? Id { get; set; }
    public required string Nome { get; set; }
    public string? Codigo { get; set; }
    public List<SubCategoriaDto> SubCategorias { get; set; } = [];
}
