using ServiceLink.Data;
using ServiceLink.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using ServiceLink.Services; // adjust namespace if different

var builder = WebApplication.CreateBuilder(args);

// --- services -------------------------------------------------
builder.Services.AddControllersWithViews();

// ADD THIS: register Razor Pages support (required for Identity UI)
builder.Services.AddRazorPages();

// your existing db / identity wiring (example)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IEmailSender, NoopEmailSender>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    // password options if any...
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// configure cookie paths if you set them
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();

// --- pipeline -------------------------------------------------
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

// KEEP MVC routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ADD THIS: map Razor Pages so /Identity/* routes work
app.MapRazorPages();

app.Run();
