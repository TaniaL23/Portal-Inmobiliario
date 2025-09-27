using apptrade.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace apptrade.Data
{
public static class SeedData
{
public static async Task InitializeAsync(IServiceProvider sp)
{
using var scope = sp.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await ctx.Database.MigrateAsync();


var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();


// Roles
if (!await roleMgr.RoleExistsAsync("Broker"))
await roleMgr.CreateAsync(new IdentityRole("Broker"));


// Usuarios
async Task<IdentityUser> EnsureUser(string email, string pass, bool broker)
{
var u = await userMgr.FindByEmailAsync(email);
if (u == null)
{
u = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
await userMgr.CreateAsync(u, pass);
}
if (broker && !await userMgr.IsInRoleAsync(u, "Broker"))
await userMgr.AddToRoleAsync(u, "Broker");
return u;
}


var broker = await EnsureUser("broker@test.com", "Passw0rd!", true);
var cliente = await EnsureUser("cliente@test.com", "Passw0rd!", false);


// Semilla Inmuebles
if (!await ctx.Inmuebles.AnyAsync())
{
ctx.Inmuebles.AddRange(
new Inmueble{ Codigo="DEP001", Titulo="Depa moderno en Miraflores", Imagen="https://images.unsplash.com/photo-1501183638710-841dd1904471", Tipo=TipoInmueble.Departamento, Ciudad="Lima", Direccion="Av. Larco 123", Dormitorios=2, Banos=2, MetrosCuadrados=78, Precio=450000, Activo=true },
new Inmueble{ Codigo="CAS001", Titulo="Casa en Surco con jardín", Imagen="https://images.unsplash.com/photo-1564013799919-ab600027ffc6", Tipo=TipoInmueble.Casa, Ciudad="Lima", Direccion="Calle Los Álamos 456", Dormitorios=3, Banos=3, MetrosCuadrados=160, Precio=720000, Activo=true },
new Inmueble{ Codigo="OFI001", Titulo="Oficina céntrica San Isidro", Imagen="https://images.unsplash.com/photo-1524758631624-e2822e304c36", Tipo=TipoInmueble.Oficina, Ciudad="Lima", Direccion="Av. Rivera Navarrete 789", Dormitorios=0, Banos=2, MetrosCuadrados=95, Precio=380000, Activo=true },
new Inmueble{ Codigo="LOC001", Titulo="Local comercial en Barranco", Imagen="https://images.unsplash.com/photo-1497366216548-37526070297c", Tipo=TipoInmueble.Local, Ciudad="Lima", Direccion="Jr. Unión 321", Dormitorios=0, Banos=1, MetrosCuadrados=60, Precio=210000, Activo=true }
);
await ctx.SaveChangesAsync();
}
}
}
}