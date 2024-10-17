using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using todo_app.Data;
using todo_app.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 32))));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TodoContext>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var configuration = services.GetRequiredService<IConfiguration>();

    string adminEmail = configuration["AdminUser:Email"] ?? throw new InvalidOperationException("Admin email is not configured.");
    string adminPassword = configuration["AdminUser:Password"] ?? throw new InvalidOperationException("Admin password is not configured.");

    await SeedData.Initialize(services, adminEmail, adminPassword);
}


// Configure the HTTP request pipeline.
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

app.Run();
