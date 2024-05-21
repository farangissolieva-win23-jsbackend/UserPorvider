﻿namespace Data.Models;

public class User
{
	public string? Id { get; set; }
	public string? UserName { get; set; }
	public string? NormalizedUserName { get; set; }
	public string? Email { get; set; }
	public string? NormalizedEmail { get; set; }
	public bool? EmailConfirmed { get; set; }
	public object? PhoneNumber { get; set; }
	public bool? PhoneNumberConfirmed { get; set; }
	public UserProfile? UserProfile { get; set; }
	public UserAddress? UserAddress { get; set; }
}
