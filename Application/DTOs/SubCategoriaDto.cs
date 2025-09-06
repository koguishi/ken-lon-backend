namespace kendo_londrina.Application.DTOs;
public class SubCategoriaDto
{
    public Guid? Id { get; set; }
    public required Guid CategoriaId { get; set; }
    public required string Nome { get; set; }
    public string? Codigo { get; set; }
}
