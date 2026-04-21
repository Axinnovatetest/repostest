using Microsoft.AspNetCore.Mvc;

namespace Psz.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class TempController: ControllerBase
	{
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "No Area" })]
		//public IActionResult GetAdUsers(string path, string username, string password, string filterValue)
		//{
		//    try
		//    {
		//        return Ok(Core.Program.ActiveDirectoryManager.GetUsersTest(path, username, password, filterValue));
		//    }
		//    catch (Exception e)
		//    {
		//        return this.HandleException(e);
		//    }
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "No Area" })]
		//public IActionResult CheckAdCredentials(string path, string username, string password)
		//{
		//    try
		//    {
		//        return Ok(Core.Program.ActiveDirectoryManager.CheckUserCrendentialsTest(path, username, password));
		//    }
		//    catch (Exception e)
		//    {
		//        return this.HandleException(e);
		//    }
		//}
	}
}