using Shared.Models;

namespace ProductService.Models;

public sealed class Product : IEntity
{
    #region Default
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedBy { get; set; } = DateTime.UtcNow;
    public Guid CreatedAt { get; set; }
    public DateTime UpdatedBy { get; set; } = DateTime.UtcNow;
    public Guid UpdatedAt { get; set; }
    public DateTime DeletedBy { get; set; }
    public Guid DeletedAt { get; set; } = Guid.Empty;
    #endregion


}