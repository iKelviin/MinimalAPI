using Microsoft.EntityFrameworkCore;
using MinimalAPI_CatalogoAPI.Models;

namespace MinimalAPI_CatalogoAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            //Categoria 
            mb.Entity<Categoria>().HasKey(x => x.CategoriaId);

            mb.Entity<Categoria>().Property(x => x.Nome).HasMaxLength(100).IsRequired();

            mb.Entity<Categoria>().Property(x => x.Descricao).HasMaxLength(150).IsRequired();


            //Produto
            mb.Entity<Produto>().HasKey(x => x.ProdutoId);

            mb.Entity<Produto>().Property(x => x.Nome).HasMaxLength(100).IsRequired();

            mb.Entity<Produto>().Property(x => x.Descricao).HasMaxLength(150).IsRequired();

            mb.Entity<Produto>().Property(x => x.Imageml).HasMaxLength(100);

            mb.Entity<Produto>().Property(x => x.Preco).HasPrecision(14, 2);


            //Relacionamento
            mb.Entity<Produto>().HasOne<Categoria>(x=> x.Categoria).WithMany(p=> p.Produtos).HasForeignKey(x=>x.CategoriaId);
        }
    }
}

