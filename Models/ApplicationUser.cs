namespace LinguaQuest.Web.Models;

public class ApplicationUser
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string UserName { get; set; } = string.Empty;
	public string DisplayName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
