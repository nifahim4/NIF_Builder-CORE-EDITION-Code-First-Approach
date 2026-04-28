using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NIF_Builder.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "Create",
    pattern: "Create.Project",
    defaults: new { controller = "Projects", action = "Create" }
);
app.MapControllerRoute(
    name: "Edit",
    pattern: "{id}.Edit.Project",
    defaults: new { controller = "Projects", action = "Edit" }
);

app.MapControllerRoute(
    name: "Create",
    pattern: "Create.Equipment",
    defaults: new { controller = "Equipments", action = "Create" }
);
app.MapControllerRoute(
    name: "Edit",
    pattern: "{id}.Edit.Equipment",
    defaults: new { controller = "Equipments", action = "Edit" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
