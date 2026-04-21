using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.MaterialManagement.Dashboard;
using Psz.Core.MaterialManagement.Interfaces;
using Psz.Core.MaterialManagement.Orders.Models.Rahmen;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Api.Areas.MaterialManagement.Controllers.Orders
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class RahmenController: ControllerBase
	{
		private const string MODULE = "Material Management | Rahmen";
		private readonly IDashboardService _dashboardService;
		public RahmenController(IDashboardService dashboardService)
		{
			_dashboardService = dashboardService;
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Orders.Models.Rahmen.UpdateRAStatusModel>), 200)]
		public IActionResult UpdateRaStatus(Psz.Core.MaterialManagement.Orders.Models.Rahmen.UpdateRAStatusModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Rahmen.UpdateRAStatusHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public async Task<IActionResult> GetPurchaseRahmenReportFile(GetPurchaseBlanketReportRequestModel data)
		{
			try
			{
				var response = await new Psz.Core.MaterialManagement.Orders.Handlers.Rahmen.GetPurchaseBlanketReportHandler(
						this.GetCurrentUser(), data).HandleAsync();

				if(response.Success)
				{
					return Ok(new Psz.Core.Common.Models.ResponseModel<string>
					{
						Body = response.Body != null && response.Body.Length > 0 ? Convert.ToBase64String(response.Body) : "",
						Errors = response.Errors.Select(x => new Psz.Core.Common.Models.ResponseModel<string>.ResponseError(x.Value))?.ToList(),
						Infos = response.Infos,
						Success = response.Success,
						Warnings = response.Warnings,

					});
				}
				else
				{
					return Ok(response);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Models.DashboardResponseModel>), 200)]
		public IActionResult GetDashboardData(int data)
		{
			try
			{
				return Ok(this._dashboardService.GetDashboardData(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
	}
}
