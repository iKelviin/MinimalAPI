using Microsoft.EntityFrameworkCore;
using MinimalAPI_CatalogoAPI.Context;
using MinimalAPI_CatalogoAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Definir os Endpoints

app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

app.MapGet("/categorias", async (Categoria categoria, AppDbContext db) => { await db.Categorias.ToListAsync(); });

app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categorias.FindAsync(id)
    is Categoria categoria
            ? Results.Ok(categoria)
            : Results.NotFound("Categoria não encontrada");
});

app.MapPut("/categorias/{id:int}", async(int id, AppDbContext db) =>
{

})


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
