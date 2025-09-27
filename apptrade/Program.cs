using apptrade.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- DB: SQLite
var cs = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db";
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(cs));

// --- Identity (Individual)
builder.Services.AddDefaultIdentity<IdentityUser>(o =>
{
    o.SignIn.RequireConfirmedAccount = false;
    o.Password.RequiredLength = 8;
    o.Password.RequireDigit = true;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// --- MVC + Razor Pages
builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// (Opcional) Semilla mínima si la usas en P1
// await SeedData.InitializeAsync(app.Services);

app.Run();
