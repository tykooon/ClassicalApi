using ClassicalApi.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IComposerRepository, ComposerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapGet("/composers", () =>
{
    using var context = app.Services.CreateScope();
    var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
    return repo.GetAll();
});

app.MapGet("/composer/{id:int}", (int id) =>
{
    using var context = app.Services.CreateScope();
    var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
    return repo.GetById(id);
});

app.MapGet("/composer/{name}", (string name) =>
{
    using var context = app.Services.CreateScope();
    var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
    return repo.GetByLastName(name);
});

app.Run();