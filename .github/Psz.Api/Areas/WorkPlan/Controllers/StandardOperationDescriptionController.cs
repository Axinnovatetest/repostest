using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.WorkPlan.Controllers
{
	[Authorize]
	[Area("WorkPlan")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class StandardOperationDescriptionController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Get()
		{
			var errors = new List<string>();
			try
			{
				var stdOperationDescritionDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Get();
				if(stdOperationDescritionDb == null)
					errors.Add("Standard operation descrition not found.");
				else
				{
					var stdOperationDescriptionViewModel = new List<Psz.Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel>();
					foreach(var so in stdOperationDescritionDb)
					{
						var stdOperationDescritioni18nDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.GetByStandardOperationDescription(so.Id);
						stdOperationDescriptionViewModel.Add(new Psz.Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel(so, stdOperationDescritioni18nDb));
					}


					return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel>>() { Success = true, ResponseBody = stdOperationDescriptionViewModel, Errors = errors } });
				}

				return Ok(new { response = new Api.Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Get(int Id)
		{
			var errors = new List<string>();
			try
			{
				var stdOperationDescriptionDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Get(Id);
				if(stdOperationDescriptionDb == null)
					errors.Add("Error Db Can't get std operation description");
				else
				{
					var stdOperationDescriptioni18nDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.GetByStandardOperationDescription(Id);
					var stdOperationDescriptionViewModel = new Psz.Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel(stdOperationDescriptionDb, stdOperationDescriptioni18nDb);

					return Ok(new { response = new Api.Models.Response<Psz.Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel>() { Success = true, ResponseBody = stdOperationDescriptionViewModel, Errors = errors } });
				}
				return Ok(new { response = new Api.Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Add(Psz.Core.Apps.WorkPlan.Models.StandardOperationDescription.CreateModel model)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Cannot get connected user");
				}

				if(errors.Count == 0)
				{
					var addedId = Psz.Core.Apps.WorkPlan.Handlers.StandardOperationDescription.Create(model, user);

					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"{user.Username} Added a Suboperation for {model.Description}.",
						user.Id);
					return Ok(addedId);
				}
				else
				{
					return Ok(new { response = new Api.Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Edit(Psz.Core.Apps.WorkPlan.Models.StandardOperationDescription.CreateModel model)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("User not found");
				}

				if(errors.Count == 0)
				{
					var done = Psz.Core.Apps.WorkPlan.Handlers.StandardOperationDescription.Edit(model, user);
					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"{user.Username} Edited a Suboperation for {model.Description}.",
							user.Id);
					return Ok(done);
				}
				else
				{
					return Ok(new { response = new Api.Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Delete(int Id)
		{
			var errors = new List<string>();

			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("User not found");
				}

				var stdOpDescDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Get(Id);
				if(stdOpDescDb == null)
				{
					errors.Add("Suboperation not found");
				}
				else
				{
					stdOpDescDb.ArchiveTime = DateTime.Now;
					stdOpDescDb.ArchiveUserId = user.Id;
					stdOpDescDb.IsArchived = true;

					Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Update(stdOpDescDb);
				}

				if(errors.Count == 0)
				{
					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"{user.Username} Deleted a Suboperation for {stdOpDescDb.Description}.",
						user.Id);
					return Ok(new { response = new Api.Models.Response<string>(true, $"Delete Success. Log Add success", errors) });
				}
				else
				{
					return Ok(new { response = new Api.Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{OpertationId}")]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult GetByOperationId(int OpertationId)
		{
			var errors = new List<string>();
			try
			{
				var operationDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get(OpertationId);
				if(operationDb == null)
				{
					errors.Add("Standard operation not found");
				}
				else
				{
					var stdOperationDescritionDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.GetByOperationId(OpertationId);
					if(stdOperationDescritionDb == null)
					{
						errors.Add("Suboperation not found");
					}
					else
					{
						var stdOperationDescriptionViewModel = new List<Psz.Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel>();
						var standardOperationDescriptioni18nDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.GetByStandardOperation(OpertationId);
						stdOperationDescritionDb.ForEach(so => stdOperationDescriptionViewModel.Add(new Psz.Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel(so, standardOperationDescriptioni18nDb)));

						return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel>>() { Success = true, ResponseBody = stdOperationDescriptionViewModel, Errors = errors } });
					}
				}

				return Ok(new { response = new Api.Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
