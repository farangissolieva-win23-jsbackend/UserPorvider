namespace Data.Entities;

public class UserAddressEntity
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string AddressLine_1 { get; set; } = null!;
	public string? AddressLine_2 { get; set; }
	public string PostalCode { get; set; } = null!;
	public string City { get; set; } = null!;

    public static UserAddressEntity FromModel(Models.UserAddress userAddress)
    {
        return new UserAddressEntity
        {
            AddressLine_1 = userAddress.AddressLine_1!,
            AddressLine_2 = userAddress.AddressLine_2,
            PostalCode = userAddress.PostalCode!,
            City = userAddress.City!
        };
    }

}
