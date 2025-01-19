using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Data;
using TennisScoreboard.Services;

var builder = WebApplication.CreateBuilder(args);

var connection = new SqliteConnection(builder.Configuration.GetConnectionString("TennisMatchesContext"));
connection.Open();

builder.Services.AddDbContext<TennisMatchesContext>(options => options.UseSqlite(connection));
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddSingleton<OngoingMatchesService>();
builder.Services.AddScoped<MatchScoreCalculationService>();
builder.Services.AddScoped<MatchesHistoryService>();
builder.Services.AddScoped<PlayersService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.Lifetime.ApplicationStopping.Register(connection.Close);

using (var scope = app.Services.CreateScope()) {
    var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await initializer.InitializeAsync();
}

app.UseExceptionHandler("/error");
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();
