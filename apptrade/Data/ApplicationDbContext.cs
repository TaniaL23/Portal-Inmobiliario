using apptrade.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace apptrade.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // DbSets del dominio
    public DbSet<Inmueble> Inmuebles { get; set; } = default!;
    public DbSet<Visita>   Visitas   { get; set; } = default!;
    public DbSet<Reserva>  Reservas  { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // índice único en Codigo
        b.Entity<Inmueble>()
         .HasIndex(i => i.Codigo)
         .IsUnique();

        // precisión para Precio (opcional)
        b.Entity<Inmueble>()
         .Property(i => i.Precio)
         .HasPrecision(18, 2);
    }
}
