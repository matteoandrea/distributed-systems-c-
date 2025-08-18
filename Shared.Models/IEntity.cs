namespace Shared.Models;

public interface IEntity : IEntityTrack
{
    public Guid Id { get; init; }
}