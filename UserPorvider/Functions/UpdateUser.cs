using Data.Contexts;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
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
				var user = await _context.Users.FindAsync(userId);
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
