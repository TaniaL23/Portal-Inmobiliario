using apptrade.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB: SQLite
var cs = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db";
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(cs));

// Identity (habilita roles si los usas en el seed)
builder.Services.AddDefaultIdentity<IdentityUser>(o =>
{
    o.SignIn.RequireConfirmedAccount = false; // ponlo true si tu profe exige confirmación
    o.Password.RequiredLength = 8;
    o.Password.RequireDigit = true;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
})
// .AddRoles<IdentityRole>() // descomenta si tu SeedData crea rol "Broker"
.AddEntityFrameworkStores<ApplicationDbContext>();

// MVC + Razor Pages (necesario para /Identity/Account/*)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Si quieres mantener las páginas de error/migraciones de EF:
// dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    // app.UseMigrationsEndPoint(); // requiere el paquete de Diagnostics EF
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();     // <-- necesario para Bootstrap/imagenes

app.UseRouting();

app.UseAuthentication();  // <-- necesario para [Authorize] en P3
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();      // <-- necesario para Identity UI

// (siembra opcional, si lo usas en P1)
// await SeedData.InitializeAsync(app.Services);

app.Run();
