using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Apps.Support.Interfaces;
using Psz.Core.Apps.Support.Models.Feedback;
using Psz.Core.Apps.Support.Models.FeedbackLogs;
using Psz.Core.Apps.Support.Models.Logs;
using Psz.Core.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Support.Controllers
{
	[Authorize]
	[Area("Support")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class FeedbackController: ControllerBase
	{

		private const string MODULE = "Support";
		private IFeedbacksService _feedbackService;
		private ILogService _logService;
		public FeedbackController(IFeedbacksService feedbacksService, ILogService logService)
		{
			this._feedbackService = feedbacksService;
			this._logService = logService;
		}
		/************************AddFeedback**************************************/
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddFeedback(CreateFeedbackRequestModel data)
		{
			try
			{
				return Ok(this._feedbackService.CreateFeedback(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data); // - logger to save context in case of exception
			}

			//try
			//{
			//	return Ok(new Core.Apps.Support.Handlers.Feedback.CreateHandler(this.GetCurrentUser(), data).Handle());
			//} catch(Exception e)
			//{
			//	return this.HandleException(e);
			//}
		}

		/************************GetFeedbacks**************************************/

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<GetFeedbackByModuleResponseModel>), 200)]
		public IActionResult GetFeedbackByModule(GetFeedbacksRequestModel data)
		{


			try
			{
				return Ok(this._feedbackService.GetFeedbackByModule(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, int>>>), 200)]
		public IActionResult GetFeedbackCounts()
		{
			try
			{
				return Ok(this._feedbackService.GetModulesFeedbackCount(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, int>>>), 200)]
		public IActionResult UpdateFeedbackTreated(int id)
		{
			try
			{
				return Ok(this._feedbackService.UpdateFeedbackTreated(this.GetCurrentUser(), id));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetFeedbacksModules()
		{
			try
			{
				return Ok(this._feedbackService.GetFeedbacksModules(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetFeedbacksSubmdoules(string module)
		{
			try
			{
				return Ok(this._feedbackService.GetFeedbacksSubmodules(this.GetCurrentUser(), module));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateFeedbackPriority(UpdatePriorityRequestModel data)
		{
			try
			{
				return Ok(this._feedbackService.UpdatePriority(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<GetFeedbacksResponseModel>), 200)]
		public IActionResult GetFeedbackById(int Id)
		{
			try
			{
				return Ok(this._feedbackService.GetFeedbackById(this.GetCurrentUser(), Id));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<FeedbackLogResponseModel>), 200)]
		public IActionResult GetLogs(FeedbackLogRequestModel request)
		{
			try
			{
				return Ok(this._logService.GetLogs(this.GetCurrentUser(), request));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<GetFeedbackByUrlResponseModel>), 200)]
		public IActionResult GetFeedbacksByPageUrl(GetFeedbackByUrlRequestModel request)
		{
			try
			{
				return Ok(this._feedbackService.GetFeedbacksByPageUrl(this.GetCurrentUser(), request));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult MarkAllTreated(List<int> feedbacksIds)
		{
			try
			{
				return Ok(this._feedbackService.MarkAllTreated(this.GetCurrentUser(), feedbacksIds));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]

		public IActionResult UpdateTreated(FeedbackLogUpdateModel data)
		{
			try
			{
				return Ok(this._logService.UpdateLogTreated(data, this.GetCurrentUser()));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


	}
}
