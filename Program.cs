using Alea.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Alea.Areas.Identity.Data;
using Alea.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AleaContext>(optios => optios.UseSqlServer(builder.Configuration.GetConnectionString("KonekcioniString")));

builder.Services.AddDefaultIdentity<AleaUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AleaContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
app.UseStaticFiles();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "pregled",
        pattern: "{controller=Pregled}/{action=PregledTop10}/{id?}");

});

app.UseMiddleware<ApiKeyMiddleware>();

app.MapRazorPages();
app.Run();
