using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class StandardOperationController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
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
					var stdOperationViewModel = new List<Core.Apps.WorkPlan.Models.StandardOperation.CreateModel>();
					foreach(var so in stdOperationsDb)
					{
						var stdOperationsi18nDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.GetByStandardOperation(so.Id);
						stdOperationViewModel.Add(new Core.Apps.WorkPlan.Models.StandardOperation.CreateModel(so, stdOperationsi18nDb));
					}

					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.StandardOperation.CreateModel>>() { Success = true, ResponseBody = stdOperationViewModel, Errors = errors } });
				}
				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
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
					var stdOperationViewModel = new Core.Apps.WorkPlan.Models.StandardOperation.CreateModel(stdOperationDb, stdOperationi18nDb);

					return Ok(new { response = new Models.Response<Core.Apps.WorkPlan.Models.StandardOperation.CreateModel>() { Success = true, ResponseBody = stdOperationViewModel, Errors = errors } });
				}
				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Add(Core.Apps.WorkPlan.Models.StandardOperation.CreateModel model)
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
					List<string> _errors;
					addedId = Core.Apps.WorkPlan.Handlers.StandardOperation.Create(model, user, out _errors);
					if(addedId == -1)
						errors.Add("Cannot add to Db.");
					if(_errors.Count > 0)
					{
						errors.AddRange(_errors);
					}
				}

				if(errors.Count == 0)
				{
					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"{user.Username} Added a Standard Operation {model.Name}.",
						user.Id);
					return Ok(new  Models.Response<string>(true, $"Add Success<{addedId}> . Log Add success", errors) );
				}
				else
				{
					return Ok( new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Edit(Core.Apps.WorkPlan.Models.StandardOperation.CreateModel model)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (user.Access.WorkPlan.StandardOperationUpdate) == false)
				{
					Core.Common.Models.ResponseModel<int>.AccessDeniedResponse();
				}

				var response = Core.Apps.WorkPlan.Handlers.StandardOperation.Edit(model, user);

				if(response.Success)
				{
					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"{user.Username} Edited a Standard Operation {model.Name}.",
						user.Id);
				}

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
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
					return Ok(new { response = new Models.Response<string>(true, $"Delete Success. Log Add success", errors) });
				}
				else
				{
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
