using Microsoft.EntityFrameworkCore;
using MinimalAPI_CatalogoAPI.Context;
using MinimalAPI_CatalogoAPI.Models;

namespace MinimalAPI_CatalogoAPI.ApiEndpoints;

public static class ProdutosEndpoints
{
    public static void MapProdutosEndpoints(this WebApplication app)
    {
        app.MapGet("/produtos", async (AppDbContext db) =>
        {
            var produtos = await db.Produtos.AsNoTracking().ToListAsync();
            if (produtos is null) return Results.NotFound();

            return Results.Ok(produtos);
        }).WithTags("Produtos").RequireAuthorization();


        app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
        {
            //var produto = await db.Produtos.FindAsync(id);

            //if (produto is null) return Results.NotFound();

            //return Results.Ok(produto);    
            return await db.Produtos.FindAsync(id) is Produto produto ? Results.Ok(produto) : Results.NotFound();
        });

        app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
        {

            db.Produtos.Add(produto);
            await db.SaveChangesAsync();

            return Results.Created($"Produto criado: {produto.ProdutoId}", produto);

        });

        app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
        {

            if (produto.ProdutoId != id) return Results.BadRequest();

            var produtoDB = await db.Produtos.FindAsync(id);

            if (produtoDB is null) return Results.NotFound();

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
    }
}
