using Microsoft.AspNetCore.Authentication.Cookies;
using NguyenLeMinhDungFall2024RazorPages;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSignalR();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";  // Đường dẫn đến trang đăng nhập
        options.AccessDeniedPath = "/Login";
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

app.MapRazorPages();

app.MapHub<SignalRHub>("/SignalRHub");

app.MapGet("/", async context =>
{
    context.Response.Redirect("/Login"); 
    await Task.CompletedTask;
});

app.Run();
