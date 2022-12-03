// dotcover disable
using System.Diagnostics.CodeAnalysis;
using Hw10.Configuration;
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMemoryCache()
    .AddControllersWithViews();

builder.Services
    .AddMathCalculator()
    .AddCachedMathCalculator();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calculator}/{action=Index}/{id?}");
app.Run();

namespace Hw10
{
    [ExcludeFromCodeCoverage]
    public partial class Program { }
}