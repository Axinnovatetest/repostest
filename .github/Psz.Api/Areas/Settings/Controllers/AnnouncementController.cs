using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CRP.Models.FA;
using Psz.Core.Support.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Settings.Controllers
{
	[Authorize]
	[Area("Settings")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class AnnouncementController: ControllerBase
	{

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.Support.Models.AnnouncementModel>>), 200)]
		public IActionResult GetAnnouncement()
		{
			try
			{
				return Ok(new Core.Support.Handlers.Announcements.GetAnnouncementHandler(this.GetCurrentUser())
								.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.Support.Models.AnnouncementModel>>), 200)]
		public IActionResult GetAdminAnnouncement()
		{
			try
			{
				return Ok(new Core.Support.Handlers.Announcements.GetAnnouncementAdminHandler(this.GetCurrentUser())
								.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateReleaseAnnouncement(AnnouncementModel model)
		{
			try
			{
				var response = new Core.Support.Handlers.Announcements.UpdateAnnouncementHandler(model,this.GetCurrentUser())
				   .Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult MarkAnnouncementAsRead(int announcementId)
		{
			try
			{
				return Ok(new Core.Support.Handlers.Announcements.MarkAnnouncementAsReadHandler(this.GetCurrentUser(),announcementId)
								.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddReleaseAnnouncement(AnnouncementModel model)
		{
			try
			{
				var response = new Core.Support.Handlers.Announcements.AddAnnouncementHandler(model, this.GetCurrentUser())
				   .Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

	}
}
