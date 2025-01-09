using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Services;

var builder = WebApplication.CreateBuilder(args);

var connection = new SqliteConnection(builder.Configuration.GetConnectionString("TennisMatchesContext"));
connection.Open();

builder.Services.AddDbContext<TennisMatchesContext>(options =>
    options.UseSqlite(connection));

builder.Services.AddSingleton<OngoingMatchesStorage>();
builder.Services.AddScoped<MatchScoreCalculationService>();
builder.Services.AddScoped<FinishedMatchesArchiveService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.Lifetime.ApplicationStopping.Register(connection.Close);

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
app.MapControllerRoute(
    name: "match-score",
    pattern: "match-score",
    defaults: new { controller = "MatchScore", action = "GetMatchScore" });

app.Run();
