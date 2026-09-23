using PaperlessREST.Dal;
using Microsoft.EntityFrameworkCore;
using PaperlessREST.Api.Endpoints;
using PaperlessREST.Api.Exceptions;
using PaperlessREST.Bll;
using DbContext = PaperlessREST.Dal.DbContext;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Default") 
                       ?? builder.Configuration["connectionStrings:Default"];

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IDocumentMetaRepository, DocumentMetaDocumentMetaRepository>();
builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseNpgsql(connectionString);
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.WriteLine, LogLevel.Information);
    }
});

builder.Services.AddCors();

builder.Services.AddAuthorization();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseCors(policy =>
    {
        // todo map to ui
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
    
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        
        var context = services.GetRequiredService<DbContext>();
        context.Database.Migrate();
    }
}

app.UseExceptionHandler(option => { });

app.UseAuthorization();

app.MapDocumentEndpoint();

app.Run();
public partial class Program { }