using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;

namespace UserPorvider.Functions
{
	public class DeleteUser(ILogger<DeleteUser> logger, DataContext context)
	{
		private readonly ILogger<DeleteUser> _logger = logger;
		private readonly DataContext _context = context;

		[Function("DeleteUser")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deleteUsers/{userId}")] HttpRequestData req,
			string userId)
		{
			try
			{
				var user = await _context.Users.FindAsync(userId);
				if (user == null)
				{
					return new NotFoundResult();
				}
			
				_context.Users.Remove(user);
				await _context.SaveChangesAsync();

				return new OkResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while deleting the user.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
