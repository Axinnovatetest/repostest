using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class StandardOperationDescriptionController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
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
					var stdOperationDescriptionViewModel = new List<Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel>();
					var stdoperationdescritioni18ndb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.GetByStandardOperationDescription(stdOperationDescritionDb.Select(x => x.Id));
					var Users = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(stdOperationDescritionDb.Select(x => x.Creation_User_Id).ToList());
					var EditUsers = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(stdOperationDescritionDb.Select(x => x.Last_Edit_User_Id ?? 0).ToList());

					foreach(var so in stdOperationDescritionDb)
					{
						var createUser = Users.FirstOrDefault(x => x.Id == so.Creation_User_Id);
						var editUser = Users.FirstOrDefault(x => x.Id == so.Last_Edit_User_Id);

						var stdoperationdescritioni18ndbVar = stdoperationdescritioni18ndb.Where(x => so.Id == x.IdStandardOperationDescription);

						stdOperationDescriptionViewModel.Add(new Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel(so, stdoperationdescritioni18ndb, createUser?.Username ?? "", editUser?.Username ?? ""));
					}


					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel>>() { Success = true, ResponseBody = stdOperationDescriptionViewModel, Errors = errors } });
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
				var stdOperationDescriptionDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Get(Id);
				if(stdOperationDescriptionDb == null)
					errors.Add("Error Db Can't get std operation description");
				else
				{
					var stdOperationDescriptioni18nDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.GetByStandardOperationDescription(Id);
					var stdOperationDescriptionViewModel = new Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel(stdOperationDescriptionDb, stdOperationDescriptioni18nDb);

					return Ok(new { response = new Models.Response<Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel>() { Success = true, ResponseBody = stdOperationDescriptionViewModel, Errors = errors } });
				}
				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Add(Core.Apps.WorkPlan.Models.StandardOperationDescription.CreateModel model)
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
					var addedId = Core.Apps.WorkPlan.Handlers.StandardOperationDescription.Create(model, user);
					if(addedId.Success)
					{
						Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
							$"{user.Username} Added a Suboperation for {model.Description}.",
							user.Id);
					}

					return Ok(addedId);
				}
				else
				{
					return Ok(new Models.Response<int> { Success = false, ResponseBody = 0, Errors = errors });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Edit(Core.Apps.WorkPlan.Models.StandardOperationDescription.CreateModel model)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					return Ok(Core.Common.Models.ResponseModel<int>.FailureResponse(new List<string> { "User not found" }));
				}

				var done = Core.Apps.WorkPlan.Handlers.StandardOperationDescription.Edit(model, user);
				if(done.Success)
				{
					Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"{user.Username} Edited a Suboperation for {model.Description}.",
							user.Id);
				}
				return Ok(done);
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
					errors.Add("User not found");
				}

				var stdOpDescDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Get(Id);
				if(stdOpDescDb == null)
				{
					errors.Add("Suboperation not found");
				}
				else
				{
					stdOpDescDb.Is_Archived = true;

					Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionAccess.Update(stdOpDescDb);
				}

				if(errors.Count == 0)
				{
					Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
						$"{user.Username} Deleted a Suboperation for {stdOpDescDb.Description}.",
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

		[HttpGet("{OpertationId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
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
						var stdOperationDescriptionViewModel = new List<Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel>();
						var standardOperationDescriptioni18nDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationDescriptionI18NAccess.GetByStandardOperation(OpertationId);
						stdOperationDescritionDb.ForEach(so => stdOperationDescriptionViewModel.Add(new Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel(so, standardOperationDescriptioni18nDb)));

						return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.StandardOperation.StdOperationDescriptionViewModel>>() { Success = true, ResponseBody = stdOperationDescriptionViewModel, Errors = errors } });
					}
				}

				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
