namespace IdentityService.Handlers.Abstractions;

public interface IHandler
{
    Task<IResult> HandleAsync();
}