using Microsoft.EntityFrameworkCore;
using trocitos.mvc.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["CONNECTION_STRING"] ?? throw new InvalidOperationException("Connection string 'CONNECTION_STRING' not found.");

// var connectionString = builder.Configuration.GetConnectionString("TrocitosDbConnection") ?? throw new InvalidOperationException("Connection string 'TrocitosDbConnecion' not found.");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Adding DB context

builder.Services.AddDbContext<TrocitosDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 32))
    );
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started.");

app.MapRazorPages();

app.Run();
