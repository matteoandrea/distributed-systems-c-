using System.Text;
// using HotChocolate.Authorization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Shared.Auth.Extensions;

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;


var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

service.AddJwtAuth(builder.Configuration);


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("", [Authorize] () =>
{
    return new { res = "heeee" };
});

app.Run();
