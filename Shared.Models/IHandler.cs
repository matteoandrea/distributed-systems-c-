namespace Shared.Models;

public interface IHandler
{
    Task<Microsoft.AspNetCore.Http.IResult> HandleAsync();
}