using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CRP.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Psz.Api.Areas.CRP.Controllers
{
	[Authorize]
	[Area("CRP")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class DelforController: ControllerBase
	{
		private const string MODULE = "CRP | DELFOR";
		private readonly IDelforService _delforService;

		public DelforController(IDelforService delforService)
		{
			_delforService = delforService;
		}

		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.CRP.Models.Delfor.DeliveryForcastLineItemModel>>), 200)]
		public IActionResult ImportDelforFromXLS([FromForm] Core.Common.Models.IAttachmentRequestModel data)
		{
			try
			{
				// AllowAnonymous <<<<<<<
				var user = this.GetCurrentUser();
				if(user == null)
				{
					return Ok("Authentication: User not found");
				}

				var file = data.AttachmentFile;
				if(file == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "File to upload is null.");
					return BadRequest("No file sent.");
				}

				if(file.Length > 0)
				{
					// Save file to temp dir
					var tempFilePath = System.IO.Path.GetTempPath();
					var filePath = System.IO.Path.Combine(tempFilePath, DateTime.Now.ToString("yyyyMMddTHHmmss_") + file.FileName);

					var fileName = System.IO.Path.GetFileName(file.FileName);
					if(!System.IO.Directory.Exists(tempFilePath))
					{
						System.IO.Directory.CreateDirectory(tempFilePath);
					}

					using(var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
					{
						file.CopyTo(fileStream);
					}
					return Ok(
						_delforService.ImportDeflorFromExcel(user,
						new Core.Common.Models.ImportFileModel
						{
							FilePath = filePath,
							CheckFrequency = data.CheckFrequency,
							CommaSeperator = data.CommaSeperator,
						}));
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CRP.Models.Delfor.DeliveryForcastHeaderModel>), 200)]
		public IActionResult GetCustomerInformations(int Id)
		{
			try
			{
				return Ok(_delforService.GetCustomerInformations(this.GetCurrentUser(), Id));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CRP.Models.CustomerModel>>), 200)]
		public IActionResult GetCustomersList()
		{
			try
			{
				return Ok(_delforService.GetCustomersForDelfor(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult SaveDelfor(Core.CRP.Models.Delfor.DeliveryForcastModel model)
		{
			try
			{
				return Ok(_delforService.SaveDelfor(this.GetCurrentUser(), model));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetDelforDraft()
		{
			var data = _delforService.DelforDraft(this.GetCurrentUser());
			if(data.Body != null)
			{
				return File(data.Body, "application/xlsx", $"Delfor.xlsx");
			}
			else
			{
				return Ok("Empty file sent.");
			}
		}
	}
}