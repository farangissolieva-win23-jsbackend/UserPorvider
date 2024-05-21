namespace Data.Entities;

public class UserProfileEntity
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string FistName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string? ProfileImg { get; set; }
	public string? Biography { get; set; }
	public string? Telephone { get; set; }
}
