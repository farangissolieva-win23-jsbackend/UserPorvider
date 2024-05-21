
using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string? UserProfileId { get; set; }
    public virtual UserProfileEntity? UserProfile { get; set; }
    public string? UserAddressId { get; set; }
    public virtual UserAddressEntity? UserAddress { get; set; }
}
