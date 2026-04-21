using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Psz.Api.Areas.SalesDistribution.Controllers
{
	[Authorize]
	[Area("SalesDistribution")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ArticlesController: ControllerBase
	{

	}
}