using BugIssuer.Application;
using BugIssuer.Infrastructure;
using BugIssuer.Web;

using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure();
    builder.Services.AddRazorPages();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddControllersWithViews();
    builder.Services.AddAuthentication("FakeScheme")
                    .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>("FakeScheme", options => { });
    builder.Services.AddAuthorization();

    builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    });

    //builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);
    //builder.Services.Configure<IISOptions>(options =>
    //{
    //    options.AutomaticAuthentication = true;
    //});
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
    
    app.MapRazorPages();

    app.Run();
}
