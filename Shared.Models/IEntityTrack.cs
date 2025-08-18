namespace Shared.Models;

public interface IEntityTrack
{
    public DateTime CreatedBy { get; init; }
    public Guid CreatedAt { get; init; }

    public DateTime UpdatedBy { get; init; }
    public Guid UpdatedAt { get; init; }

    public DateTime? DeletedBy { get; init; }
    public Guid? DeletedAt { get; init; }
}