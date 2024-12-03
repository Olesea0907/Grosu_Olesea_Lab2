using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Grosu_Olesea_Lab2.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register the application database context
builder.Services.AddDbContext<Grosu_Olesea_Lab2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Grosu_Olesea_Lab2Context")
    ?? throw new InvalidOperationException("Connection string 'Grosu_Olesea_Lab2Context' not found.")));

// Register the Identity database context
builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Grosu_Olesea_Lab2Context")
    ?? throw new InvalidOperationException("Connection string 'Grosu_Olesea_Lab2Context' not found.")));

// Configure Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<LibraryIdentityContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure Authentication middleware is active
app.UseAuthorization();

app.MapRazorPages();

app.Run();
