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
	public class StandardOperationController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Get()
		{
			var errors = new List<string>();

			try
			{
				var stdOperationsDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get();
				if(stdOperationsDb == null)
				{
					errors.Add("Error Db Can't get std operations");
				}
				else
				{
					var stdOperationViewModel = new List<Psz.Core.Apps.WorkPlan.Models.StandardOperation.CreateModel>();
					foreach(var so in stdOperationsDb)
					{
						var stdOperationsi18nDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.GetByStandardOperation(so.Id);
						stdOperationViewModel.Add(new Psz.Core.Apps.WorkPlan.Models.StandardOperation.CreateModel(so, stdOperationsi18nDb));
					}

					return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.WorkPlan.Models.StandardOperation.CreateModel>>() { Success = true, ResponseBody = stdOperationViewModel, Errors = errors } });
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
				var stdOperationDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get(Id);
				if(stdOperationDb == null)
				{
					errors.Add("Standard operation not found");
				}
				else
				{
					var stdOperationi18nDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.GetByStandardOperation(Id);
					var stdOperationViewModel = new Psz.Core.Apps.WorkPlan.Models.StandardOperation.CreateModel(stdOperationDb, stdOperationi18nDb);

					return Ok(new { response = new Api.Models.Response<Psz.Core.Apps.WorkPlan.Models.StandardOperation.CreateModel>() { Success = true, ResponseBody = stdOperationViewModel, Errors = errors } });
				}
				return Ok(new { response = new Api.Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Add(Psz.Core.Apps.WorkPlan.Models.StandardOperation.CreateModel model)
		{
			var errors = new List<string>();
			try
			{
				var addedId = 0;

				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Cannot get connected user");
				}
				else
				{
					addedId = Psz.Core.Apps.WorkPlan.Handlers.StandardOperation.Create(model, user);
					if(addedId == -1)
						errors.Add("Cannot add to Db. DB Error");
				}

				if(errors.Count == 0)
				{
					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"{user.Username} Added a Standard Operation {model.Name}.",
						user.Id);
					return Ok(new { response = new Api.Models.Response<string>(true, $"Add Success<{addedId}> . Log Add success", errors) });
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
		public IActionResult Edit(Psz.Core.Apps.WorkPlan.Models.StandardOperation.CreateModel model)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();

				Psz.Core.Apps.WorkPlan.Handlers.StandardOperation.Edit(model, user);

				if(errors.Count == 0)
				{
					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"{user.Username} Edited a Standard Operation {model.Name}.",
						user.Id);
					return Ok(new { response = new Api.Models.Response<string>(true, $"Edit Success. Log Add success", errors) });
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
					errors.Add("Can't get connected user");
				}

				var stdOpDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get(Id);
				if(stdOpDb == null)
				{
					errors.Add("Standard operation not found");
				}
				else
				{
					stdOpDb.ArchiveTime = DateTime.Now;
					stdOpDb.ArchiveUserId = user.Id;
					stdOpDb.IsArchived = true;

					var done = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Update(stdOpDb);
					if(done == -1)
						errors.Add("Can't delete standard operation");
				}

				if(errors.Count == 0)
				{
					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"{user.Username} Deleted a Standard Operation {stdOpDb.Name}.",
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
	}
}
