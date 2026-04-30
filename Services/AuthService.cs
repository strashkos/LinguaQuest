using System.Security.Claims;
using LinguaQuest.Web.Data;
using LinguaQuest.Web.Enums;
using LinguaQuest.Web.Models;
using LinguaQuest.Web.Services.Interfaces;
using LinguaQuest.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace LinguaQuest.Web.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

    public AuthService(ApplicationDbContext db, IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(RegisterViewModel model, HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            return (false, "Заповніть username, email і пароль.");
        }

        if (!string.Equals(model.Password, model.ConfirmPassword, StringComparison.Ordinal))
        {
            return (false, "Паролі не збігаються.");
        }

        if (model.Password.Length < 6)
        {
            return (false, "Пароль має містити щонайменше 6 символів.");
        }

        var existingUser = await _db.Users.Find(user => user.UserName == model.UserName || user.Email == model.Email).FirstOrDefaultAsync(cancellationToken);
        if (existingUser is not null)
        {
            return (false, "Користувач з таким ім'ям або email уже існує.");
        }

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.UserName.Trim(),
            DisplayName = string.IsNullOrWhiteSpace(model.DisplayName) ? model.UserName.Trim() : model.DisplayName.Trim(),
            Email = model.Email.Trim(),
            PasswordHash = string.Empty,
            CreatedUtc = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

        await _db.Users.InsertOneAsync(user, cancellationToken: cancellationToken);

        await _db.UserLearningSettings.InsertOneAsync(new UserLearningSettings
        {
            UserId = user.Id,
            SourceLanguage = LearningLanguage.Ukrainian,
            TargetLanguage = model.TargetLanguage,
            Level = model.Level,
            WordsPerSession = 5
        }, cancellationToken: cancellationToken);

        await SignInAsync(httpContext, user, cancellationToken);
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> LoginAsync(LoginViewModel model, HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(model.UserNameOrEmail) || string.IsNullOrWhiteSpace(model.Password))
        {
            return (false, "Введіть логін і пароль.");
        }

        var query = Builders<ApplicationUser>.Filter.Or(
            Builders<ApplicationUser>.Filter.Eq(user => user.UserName, model.UserNameOrEmail.Trim()),
            Builders<ApplicationUser>.Filter.Eq(user => user.Email, model.UserNameOrEmail.Trim()));

        var user = await _db.Users.Find(query).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return (false, "Невірний логін або пароль.");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return (false, "Невірний логін або пароль.");
        }

        await SignInAsync(httpContext, user, cancellationToken);
        return (true, null);
    }

    public async Task LogoutAsync(HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private static async Task SignInAsync(HttpContext httpContext, ApplicationUser user, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.DisplayName),
            new(ClaimTypes.Email, user.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = true });
    }
}