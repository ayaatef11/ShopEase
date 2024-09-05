using Microsoft.EntityFrameworkCore;
using myshop.DataAccess;
using Stripe;
using MyShop.DataAccess.Implementation;
using myshop.DataAccess.Seeds;
//validations
//authentication
//views
//diagrams
//filters 
//specifications
//error filters 
//caching 
//extensions 
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    )) ;
builder.Services.Configure<StripeData>(builder.Configuration.GetSection("stripe"));

builder.Services.AddIdentity<IdentityUser,IdentityRole>(
    options=>options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4)
    ).AddDefaultTokenProviders().AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<IUserEmailStore<ApplicationUser>>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        // Migrate the database to the latest version
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        // Seed the roles, users, and claims
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Call your seeding methods here
        await DefaultUsers.SeedBasicUserAsync(userManager);
        await DefaultUsers.SeedSuperAdminUserAsync(userManager, roleManager);

        logger.LogInformation("Database seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();


app.MapControllerRoute(
    name: "Customer",
    pattern: "{area=Admin}/{controller=Dashboard}/{action=Display}");

app.Run();
