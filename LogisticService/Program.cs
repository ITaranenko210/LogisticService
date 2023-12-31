using LogisticService.Data;
using LogisticService.Data.ApplicationUsers;
using LogisticService.Data.Emails;
using LogisticService.Data.Options;
using LogisticService.Data.WorkCases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        builder.Services.AddScoped<IEmailRepository, EmailRepository>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IWorkCaseRepository, WorkCaseRepository>();
        



        builder.Services.AddDefaultIdentity<ApplicationUser >(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();
        builder.Services.Configure<MailOptions>(builder.Configuration.GetSection(MailOptions.Position));
        builder.Services.Configure<GoogleMapsOptions>(builder.Configuration.GetSection(GoogleMapsOptions.Position));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Manager", "Member" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        app.Run();
    }
}