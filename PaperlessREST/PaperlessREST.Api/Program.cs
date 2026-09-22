using Dal;
using Microsoft.EntityFrameworkCore;
using PaperlessREST.Api.Endpoints;
using PaperlessREST.Bll;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IDocumentMetaService, DocumentMetaService>();
builder.Services.AddScoped<IRepository, DocumentMetaRepository>();
builder.Services.AddDbContext<MetadataDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseAuthorization();

app.MapDocumentEndpoint();

app.Run();