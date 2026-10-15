using Microsoft.EntityFrameworkCore;
using MiSolucion.Api.Models;

namespace MiSolucion.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Personaje> Personajes { get; set; }
    public DbSet<Clase> Clases { get; set; }
    public DbSet<Raza> Razas { get; set; }
    public DbSet<Habilidad> Habilidades { get; set; }
    public DbSet<Hechizo> Hechizos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clase>().HasData(
            new Clase { Id = 1, Nombre = "Bárbaro" },
            new Clase { Id = 2, Nombre = "Paladín" },
            new Clase { Id = 3, Nombre = "Pícaro" }
        );

        modelBuilder.Entity<Raza>().HasData(
            new Raza { Id = 1, Nombre = "Humano" },
            new Raza { Id = 2, Nombre = "Elfo" },
            new Raza { Id = 3, Nombre = "Enano" }
        );
    }
}