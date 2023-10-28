using Microsoft.EntityFrameworkCore;
using MinimalAPI_CatalogoAPI.Context;
using MinimalAPI_CatalogoAPI.Models;

namespace MinimalAPI_CatalogoAPI.ApiEndpoints;

public static class CategoriasEndpoints
{
    public static void MapCategoriasEndpoints(this WebApplication app)
    {
        app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
        {
            db.Categorias.Add(categoria);
            await db.SaveChangesAsync();

            return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
        });

        app.MapGet("/categorias", async (AppDbContext db) =>
        {
            var categorias = await db.Categorias.AsNoTracking().ToListAsync();

            return Results.Ok(categorias);
        }).WithTags("Categorias").RequireAuthorization();

        app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
        {
            return await db.Categorias.FindAsync(id)
            is Categoria categoria
                    ? Results.Ok(categoria)
                    : Results.NotFound("Categoria n�o encontrada");
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
    }
}
