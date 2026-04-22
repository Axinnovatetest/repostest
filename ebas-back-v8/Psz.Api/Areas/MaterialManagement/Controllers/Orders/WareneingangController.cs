using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.MaterialManagement.Orders.Models.Wareneingang;
using Psz.Core.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers.Orders
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class WareneingangController: ControllerBase
	{
		private const string MODULE = "Material Management | Orders";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.Wareneingang.GetResponseModel>>), 200)]
		public IActionResult Get(Core.MaterialManagement.Orders.Models.Wareneingang.GetRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Wareneingang.GetHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.Wareneingang.GetResponseModel>>), 200)]
		public IActionResult GetLagerort()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Wareneingang.GetLagerortHandler(this.GetCurrentUser(false))
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<SaveResponseModel>), 200)]
		public IActionResult Save(Core.MaterialManagement.Orders.Models.Wareneingang.SaveRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Wareneingang.SaveHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult GenerateReport(CreateReportRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Wareneingang.CreateReportHandler(this.GetCurrentUser(false), data)
				   .Handle();

				if(response.Success)
				{
					return Ok(new Psz.Core.Common.Models.ResponseModel<string>(Convert.ToBase64String(response.Body)));
				}
				else
				{
					return Ok(response);
				}

			} catch(Exception e)
			{

				return this.HandleException(e, data);
			}

		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GenerateReportAll(CreateReportRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Wareneingang.CreateReportAllHandler(this.GetCurrentUser(false), data)
				   .Handle();

				if(response.Success)
				{
					var responses = new List<string>();
					foreach(var item in response.Body)
					{
						responses.Add(Convert.ToBase64String(item));
					}
					return Ok(new Psz.Core.Common.Models.ResponseModel<List<string>>(responses));
				}
				else
				{
					return Ok(response);
				}

			} catch(Exception e)
			{

				return this.HandleException(e, data);
			}

		}
	}
}
