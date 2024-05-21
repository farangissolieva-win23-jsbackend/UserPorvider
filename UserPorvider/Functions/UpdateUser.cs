using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

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
			[HttpTrigger(AuthorizationLevel.Function, "put", Route = "updateUsers/{userId}")] HttpRequestData req,
			string userId)
		{
			try
			{
				var user = await _context.Users.FindAsync(userId);
				if (user == null)
				{
					return new NotFoundResult();
				}
				var requestBody = await req.ReadAsStringAsync();
				var updatedUser = JsonSerializer.Deserialize<ApplicationUser>(requestBody!);

				_context.Entry(user).CurrentValues.SetValues(updatedUser!);
				await _context.SaveChangesAsync();

				return new OkObjectResult(user);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while updating the user.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
