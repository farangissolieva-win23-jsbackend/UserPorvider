using Data.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace UserPorvider.Functions;

public class GetUser(ILogger<GetUser> logger, DataContext context)
{
    private readonly ILogger<GetUser> _logger = logger;
	private readonly DataContext _context = context;

	[Function("GetUserById")]
	public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{userId}")] HttpRequestData req,
			string userId)
	{
		try
		{
			var user = await _context.Users.FindAsync(userId);
			if (user == null)
			{
				return new NotFoundResult();
			}
			return new OkObjectResult(user);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while retrieving the user.");
			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}
