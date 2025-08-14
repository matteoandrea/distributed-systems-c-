using IdentityService.Data;
using IdentityService.Endpoints;
using IdentityService.Extensions;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDatabaseConfiguration(builder.Configuration)
    .AddIdentityConfiguration()
    .AddJwtAuthentication(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapAuthEndpoints();

if (args.Length == 1 && args[0].Equals("seeddata", StringComparison.CurrentCultureIgnoreCase))
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);

    return;
}


app.Run();

