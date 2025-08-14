using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapScalarApiReference();
app.UseHttpsRedirection();

app.MapGet("", () =>
{
    return new { res = "heeee" };
});

app.Run();
