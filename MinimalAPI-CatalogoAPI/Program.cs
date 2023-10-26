using Azure.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
#region Endpoints Categoria

app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

app.MapGet("/categorias", async (AppDbContext db) => { await db.Categorias.ToListAsync(); });

app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categorias.FindAsync(id)
    is Categoria categoria
            ? Results.Ok(categoria)
            : Results.NotFound("Categoria não encontrada");
});

app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
{
    if (categoria.CategoriaId != id)
    {
        return Results.BadRequest();
    }

    var categoriaDB = await db.Categorias.FindAsync(id);

    if (categoriaDB is null) return Results.NotFound();

    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);

});

app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    var catergoria = await db.Categorias.FindAsync(id);

    if (catergoria is null) return Results.NotFound();

    db.Categorias.Remove(catergoria);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

#endregion


#region Endpoints Produto

app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync());

app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
{
    //var produto = await db.Produtos.FindAsync(id);

    //if (produto is null) return Results.NotFound();

    //return Results.Ok(produto);    
    return await db.Produtos.FindAsync(id) is Produto produto ? Results.Ok(produto) : Results.NotFound();
});

app.MapPost("/produtos", async (Produto produto, AppDbContext db)=>
{

    db.Produtos.Add(produto);
    await db.SaveChangesAsync();

    return Results.Created($"Produto criado: {produto.ProdutoId}", produto);

});

app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
{

    if(produto.ProdutoId != id) return Results.BadRequest();

    var produtoDB = await db.Produtos.FindAsync(id);

    if(produtoDB is null) return Results.NotFound();

    produtoDB.Nome = produto.Nome;
    produtoDB.Descricao = produto.Descricao;
    produtoDB.Preco = produto.Preco;
    produtoDB.Estoque = produto.Estoque;
    produtoDB.DataCompra = produto.DataCompra;
    produtoDB.Imageml = produto.Imageml;
    produtoDB.CategoriaId = produto.CategoriaId;

    await db.SaveChangesAsync();
    return Results.Ok(produtoDB);
});

app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
{
    var produto = await db.Produtos.FindAsync(id);

    if (produto is null) return Results.NotFound();

    db.Produtos.Remove(produto);
    await db.SaveChangesAsync();

    return Results.NoContent();

});


#endregion


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

