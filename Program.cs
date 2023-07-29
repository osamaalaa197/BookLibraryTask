using BookLibrary.Context;
using BookLibrary.Models;
using BookLibrary.Repository.BookRepository;
using BookLibrary.Repository.BorrowerRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region ConnectionDataBase
var connectionString = builder.Configuration.GetConnectionString("DataBase_Connections");
if (string.IsNullOrEmpty(connectionString))
{
    // Log or throw an exception to handle the missing connection string
    // For example, throw an exception:
    throw new Exception("Connection string 'DataBase_Connections' is missing or empty in the configuration.");
}
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer(connectionString);
});
#endregion

#region injection
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
#endregion

#region identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<DataBaseContext>();
#endregion




builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/User/LogIn";
    options.AccessDeniedPath = "/Book/GetAllBook";
    options.LogoutPath = "/Home/Index";
});

builder.Services.AddHttpContextAccessor(); // Required for session access.
builder.Services.AddSingleton<ISessionStore, DistributedSessionStore>();



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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=GetAllBook}/{id?}");
app.Run();

