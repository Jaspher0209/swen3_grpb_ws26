
namespace PaperlessREST.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthorization();

        app.MapGet("/api/test", (HttpContext httpContext) =>
        {
            return "Hello World!";
        })
        .WithName("Test")
        .WithOpenApi();

        app.Run();
    }
}
