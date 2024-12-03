using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Grosu_Olesea_Lab2.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Înregistrează contextul aplicației
builder.Services.AddDbContext<Grosu_Olesea_Lab2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Grosu_Olesea_Lab2Context")
    ?? throw new InvalidOperationException("Connection string 'Grosu_Olesea_Lab2Context' not found.")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value este 30 zile. Puteți schimba această valoare pentru scenarii de producție.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
