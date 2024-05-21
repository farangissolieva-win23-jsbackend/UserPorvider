
using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string? UserProfileId { get; set; }
    public virtual UserProfile? UserProfile { get; set; }
    public string? UserAddressId { get; set; }
    public virtual UserAddress? UserAddress { get; set; }
}
