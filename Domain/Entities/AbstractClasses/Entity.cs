namespace kendo_londrina.Domain.Entities.AbstractClasses;
public abstract class Entity
{
    // public Guid EscolaId { get; protected set; } = Guid.Parse("b3e53df9-b128-4227-81fe-cc0b9ad9720b");
    public Guid UserId { get; protected set; }
    public Guid Id { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public DateTime? CreatedOn { get; protected set; }
    public string? EditedBy { get; protected set; }
    public DateTime? EditedOn { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}