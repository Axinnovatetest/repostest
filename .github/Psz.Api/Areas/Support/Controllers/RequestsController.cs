using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Support.Controllers
{
	[ApiController]
	[Area("Support")]
	[Route("api/[area]/[controller]/[action]")]
	public class RequestsController: ControllerBase
	{
		private const string MODULE = "Support";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Support.Models.Feedback.FeedbackGetModel>>), 200)]
		public IActionResult GetAll()
		{
			try
			{
				return Ok(new Core.Apps.Support.Handlers.Request.GetAllHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Support.Models.Request.ProjectRequest>), 200)]
		public IActionResult Get(int Id)
		{
			try
			{
				return Ok(new Core.Apps.Support.Handlers.Request.GetHandler(this.GetCurrentUser(), Id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.Apps.Support.Models.Request.ProjectRequest data)
		{
			try
			{
				return Ok(new Core.Apps.Support.Handlers.Request.AddHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Edit(Core.Apps.Support.Models.Request.ProjectRequest data)
		{
			try
			{
				return Ok(new Core.Apps.Support.Handlers.Request.EditHandler(this.GetCurrentUser(), data, 0).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Validate(Core.Apps.Support.Models.Request.ProjectRequest data)
		{
			try
			{
				return Ok(new Core.Apps.Support.Handlers.Request.ValidateHandler(this.GetCurrentUser(), data, 1).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Refuse(Core.Apps.Support.Models.Request.ProjectRequest data)
		{
			try
			{
				return Ok(new Core.Apps.Support.Handlers.Request.EditHandler(this.GetCurrentUser(), data, 2).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UploadFiles([FromForm] Core.Apps.Support.Models.Request.FilesModel data)
		{
			try
			{
				return Ok(new Core.Apps.Support.Handlers.Request.UploadFileHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult DownloadFile(int Id)
		{
			try
			{

				var response = new Core.Apps.Support.Handlers.Request.DownloadFileHandler(this.GetCurrentUser(), Id).Handle();
				return File(response.Body.FileContent, response.Body.ContentType, response.Body.Name);
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteFile(int data)
		{
			try
			{
				return Ok(new Core.Apps.Support.Handlers.Request.DeleteFileHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
