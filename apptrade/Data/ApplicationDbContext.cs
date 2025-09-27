using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using apptrade.Models;


namespace apptrade.Data
{
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
: base(options) { }


public DbSet<Inmueble> Inmuebles => Set<Inmueble>();
public DbSet<Visita> Visitas => Set<Visita>();
public DbSet<Reserva> Reservas => Set<Reserva>();


protected override void OnModelCreating(ModelBuilder b)
{
base.OnModelCreating(b);


b.Entity<Inmueble>()
.HasIndex(x => x.Codigo)
.IsUnique();


b.Entity<Visita>()
.HasOne(v => v.Inmueble)
.WithMany(i => i.Visitas)
.HasForeignKey(v => v.InmuebleId)
.OnDelete(DeleteBehavior.Cascade);


b.Entity<Reserva>()
.HasOne(r => r.Inmueble)
.WithMany(i => i.Reservas)
.HasForeignKey(r => r.InmuebleId)
.OnDelete(DeleteBehavior.Cascade);


// checks de dominio
b.Entity<Inmueble>()
.ToTable(t =>
{
t.HasCheckConstraint("CK_Inmueble_M2_Positive", "MetrosCuadrados > 0");
t.HasCheckConstraint("CK_Inmueble_Precio_Positive", "Precio > 0");
});
}
}
}