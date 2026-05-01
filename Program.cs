using LinguaQuest.Web.Components;
using LinguaQuest.Web.Data;
using LinguaQuest.Web.Services;
using LinguaQuest.Web.Services.GameModes;
using LinguaQuest.Web.Services.Interfaces;
using LinguaQuest.Web.Services.Timers;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/login";
		options.LogoutPath = "/auth/logout";
		options.AccessDeniedPath = "/login";
		options.SlidingExpiration = true;
		options.ExpireTimeSpan = TimeSpan.FromDays(7);
	});

builder.Services.AddSingleton<ApplicationDbContext>();

builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IWordService, WordService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserLearningSettingsService, UserLearningSettingsService>();
builder.Services.AddScoped<IGameMode, QuickFindMode>();
builder.Services.AddScoped<IGameMode, WordOnMindMode>();
builder.Services.AddScoped<IGameTimer, CountdownGameTimer>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

await DbInitializer.InitializeAsync(app.Services, app.Configuration);

app.MapPost("/auth/register", async (HttpContext httpContext, [FromForm] RegisterViewModel model, [FromForm] string? returnUrl, IAuthService authService) =>
	{
		try
		{
			var result = await authService.RegisterAsync(model, httpContext);
			if (!result.Success)
			{
				var redirectUrl = "/register?error=" + Uri.EscapeDataString(result.Error ?? "Registration failed.");
				if (!string.IsNullOrWhiteSpace(returnUrl))
				{
					redirectUrl += "&returnUrl=" + Uri.EscapeDataString(returnUrl);
				}

				return Results.Redirect(redirectUrl);
			}

			return Results.Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/game" : returnUrl);
		}

		catch (Exception)
		{
			var redirectUrl = "/register?error=" + Uri.EscapeDataString("Не вдалося підключитися до MongoDB. Перевір connection string, IP access і credentials.");
			if (!string.IsNullOrWhiteSpace(returnUrl))
			{
				redirectUrl += "&returnUrl=" + Uri.EscapeDataString(returnUrl);
			}

			return Results.Redirect(redirectUrl);
		}
	})
	.DisableAntiforgery();

app.MapPost("/auth/login", async (HttpContext httpContext, [FromForm] LoginViewModel model, [FromForm] string? returnUrl, IAuthService authService) =>
	{
		try
		{
			var result = await authService.LoginAsync(model, httpContext);
			if (!result.Success)
			{
				var redirectUrl = "/login?error=" + Uri.EscapeDataString(result.Error ?? "Login failed.");
				if (!string.IsNullOrWhiteSpace(returnUrl))
				{
					redirectUrl += "&returnUrl=" + Uri.EscapeDataString(returnUrl);
				}

				return Results.Redirect(redirectUrl);
			}

			return Results.Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/game" : returnUrl);
		}

		catch (Exception)
		{
			var redirectUrl = "/login?error=" + Uri.EscapeDataString("Не вдалося підключитися до MongoDB. Перевір connection string, IP access і credentials.");
			if (!string.IsNullOrWhiteSpace(returnUrl))
			{
				redirectUrl += "&returnUrl=" + Uri.EscapeDataString(returnUrl);
			}

			return Results.Redirect(redirectUrl);
		}
	})
	.DisableAntiforgery();

app.MapPost("/auth/logout", async (HttpContext httpContext, IAuthService authService) =>
	{
		await authService.LogoutAsync(httpContext);
		return Results.Redirect("/");
	})
	.DisableAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
