using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLink.Data;
using ServiceLink.Models;

var builder = WebApplication.CreateBuilder(args);

// --- Configure services (example) ---
// adjust these to match your existing service registrations
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// --- Build app ---
var app = builder.Build();

// --- DEV: seed roles & master user (only run in dev/demo) ---
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            var roles = new[] { "User", "Admin", "Provider", "MasterDemo" };

            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                {
                    var createRoleRes = await roleManager.CreateAsync(new IdentityRole(r));
                    logger.LogInformation("Ensure role {Role} created: {Ok}", r, createRoleRes.Succeeded);
                }
            }

            // CHANGE THESE CREDENTIALS BEFORE RUNNING
            var masterEmail = "master@servicelink.test";
            var masterPassword = "MasterPass123!";

            var master = await userManager.FindByEmailAsync(masterEmail);
            if (master == null)
            {
                master = new ApplicationUser
                {
                    UserName = masterEmail,
                    Email = masterEmail,
                    FullName = "Master Demo"
                };

                var createRes = await userManager.CreateAsync(master, masterPassword);
                if (createRes.Succeeded)
                {
                    logger.LogInformation("Created master user {Email}", masterEmail);
                }
                else
                {
                    logger.LogWarning("Failed creating master user: {Errors}", string.Join(", ", createRes.Errors.Select(e => e.Description)));
                }
            }

            foreach (var r in roles)
            {
                if (!await userManager.IsInRoleAsync(master, r))
                {
                    var addRoleRes = await userManager.AddToRoleAsync(master, r);
                    logger.LogInformation("Added role {Role} to {Email}: {Ok}", r, masterEmail, addRoleRes.Succeeded);
                }
            }

            logger.LogInformation("Role seeding complete.");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding roles/master user.");
        }
    }
}

// --- Middleware pipeline ---
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


app.MapRazorPages(); // if Identity scaffolding uses Razor Pages

app.Run();
