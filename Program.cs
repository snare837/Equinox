using Microsoft.EntityFrameworkCore;
using Equinox.Models;

var builder = WebApplication.CreateBuilder(args);

// === Register services ===
builder.Services.AddMemoryCache();          // Required for session
builder.Services.AddSession();              // Basic session setup
builder.Services.AddControllersWithViews(); // MVC

builder.Services.AddDbContext<EquinoxContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EquinoxContext")));

var app = builder.Build();

// === Middleware pipeline ===
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Optional, but included in NFLTeams
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();       // Between routing and auth
app.UseAuthorization();

// === Routing ===
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



