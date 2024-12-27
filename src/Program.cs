using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TennisMatchesContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TennisMatchesContext")));

builder.Services.AddSingleton<OngoingMatchesStorage>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<TennisMatchesContext>();
    await DbInitializer.InitializeAsync(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=NewMatch}/{action=Index}");

app.Run();
