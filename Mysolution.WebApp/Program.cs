using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using MySolution.ApiIntergration;
using MySolution.Models.System.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(60));
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<LoginRequest>());
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Account/Login";
});
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<IOrderApiClient, OrderApiClient>();
builder.Services.AddTransient<ISlideApiClient, SlideApiClient>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
