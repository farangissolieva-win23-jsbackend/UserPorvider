using Data.Contexts;
using Data.Entities;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace UserPorvider.Functions
{
	public class UpdateUser
	{
		private readonly ILogger<UpdateUser> _logger;
		private readonly DataContext _context;

		public UpdateUser(ILogger<UpdateUser> logger, DataContext context)
		{
			_logger = logger;
			_context = context;
		}

		[Function("UpdateUser")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "put", Route = "updateUsers/{userId}")] HttpRequest req,
			string userId)
		{
			string body = null!;
			try
			{
				body = await new StreamReader(req.Body).ReadToEndAsync();
                var user = await _context.Users
				.Include(u => u.UserProfile)
				.Include(u => u.UserAddress)
				.FirstOrDefaultAsync(u => u.Id == userId);
              
				if (user == null)
				{
					return new BadRequestResult();
				}
				if (body == null)
				{
					return new BadRequestResult();
				}
				else
				{
					var updatedUser = JsonConvert.DeserializeObject<User>(body);
					if (updatedUser != null)
					{
						_context.Entry(user).CurrentValues.SetValues(updatedUser);

                        if (user.UserProfile != null && updatedUser.UserProfile != null)
                        {
                            _context.Entry(user.UserProfile).CurrentValues.SetValues(updatedUser.UserProfile);
                        }
                        if(user.UserProfile == null && updatedUser.UserProfile != null)
                        {
                            user.UserProfile = UserProfileEntity.FromModel(updatedUser.UserProfile);
                        }

                        if (user.UserAddress != null && updatedUser.UserAddress != null)
                        {
                            _context.Entry(user.UserAddress).CurrentValues.SetValues(updatedUser.UserAddress);
                        }
                        else if (user.UserAddress == null && updatedUser.UserAddress != null && updatedUser.UserAddress.AddressLine_1 != null && updatedUser.UserAddress.City != null && updatedUser.UserAddress.PostalCode != null)
                        {
                            user.UserAddress = UserAddressEntity.FromModel(updatedUser.UserAddress);
                        }

                        await _context.SaveChangesAsync();
					
						return new OkResult();
					}
					return new BadRequestResult();
				}
	
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while updating the user.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
