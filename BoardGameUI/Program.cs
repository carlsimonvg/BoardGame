using Blazored.SessionStorage;
using BoardGameModels;
using BoardGameUI.Data;
using BoardGameUI.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var appSettingSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingSection);

builder.Services.AddTransient<ValidateHeaderHandler>();
builder.Services.AddSingleton<BoardGameService>();
builder.Services.AddSingleton<HttpClient>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddHttpClient<IWishListService<BoardGame>, WishListService<BoardGame>>()
    .AddHttpMessageHandler<ValidateHeaderHandler>();
builder.Services.AddHttpClient<IWishListService<Player>, WishListService<Player>>()
    .AddHttpMessageHandler<ValidateHeaderHandler>();
builder.Services.AddHttpClient<IWishListService<Play>, WishListService<Play>>()
	.AddHttpMessageHandler<ValidateHeaderHandler>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
