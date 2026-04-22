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
	public class departementController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Get()
		{
			var errors = new List<string>();
			try
			{
				var departmentDb = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get();
				if(departmentDb != null)
				{
					var departmentViewModel = new List<Core.Apps.WorkPlan.Models.Department.CreateModel>();
					foreach(var d in departmentDb)
					{
						var departmenti18nDb = Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.GetByDepartment(d.Id);
						departmentViewModel.Add(new Core.Apps.WorkPlan.Models.Department.CreateModel(d, departmenti18nDb));
					}

					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Department.CreateModel>> { Errors = errors, ResponseBody = departmentViewModel, Success = true } });
				}
				return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Department.CreateModel>> { Errors = errors, ResponseBody = null, Success = false } });
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
				var departmentDb = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get(Id);
				if(departmentDb == null)
					errors.Add("Department not found.");
				if(errors.Count == 0)
				{
					var departmenti18nDb = Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.GetByDepartment(Id);
					var departmentViewModel = new Core.Apps.WorkPlan.Models.Department.CreateModel(departmentDb, departmenti18nDb);
					return Ok(new
					{
						response = new Models.Response<Core.Apps.WorkPlan.Models.Department.CreateModel>
						{
							Errors = errors,
							ResponseBody = departmentViewModel,
							Success = true
						}
					});
				}
				return Ok(new { response = new Models.Response<string> { Errors = errors, ResponseBody = "", Success = false } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Models.Response<int>), 200)]
		public IActionResult Add(Core.Apps.WorkPlan.Models.Department.CreateModel model)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					return Ok(new
					{
						response = new Models.Response<string>
						{
							Success = false,
							Errors = new List<string>() { "Connected User not found" },
						}
					});
				}

				var insertedId = Core.Apps.WorkPlan.Handlers.Department.Create(model, this.GetCurrentUser());

				Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
					$"{user.Username} added a department {model.Name}",
					user.Id);

				return Ok(new { response = new Models.Response<int> { Success = true, ResponseBody = insertedId } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Edit(Core.Apps.WorkPlan.Models.Department.CreateModel model)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					var errors = new List<string>();
					errors.Add("User not found. Try again later.");
					return Ok(new { response = new Models.Response<string> { Errors = errors, ResponseBody = "", Success = false } });
				}

				var departmentDb = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get(model.Id);
				if(departmentDb == null)
				{
					var errors = new List<string>();
					errors.Add("department not found");
					return Ok(new { response = new Models.Response<string> { Errors = errors, ResponseBody = "", Success = false } });
				}

				Core.Apps.WorkPlan.Handlers.Department.Edit(model, user);

				Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition,
					$"{user.Username} added a department{model.Name}",
					user.Id);

				return Ok(new { response = new Models.Response<string>(true, "Edit Success. Log Success") });
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
				var departmentDb = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get(Id);
				if(departmentDb == null)
				{
					errors.Add("department not found");
				}

				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("User not found");
				}

				if(errors.Count == 0)
				{
					departmentDb.ArchiveTime = DateTime.Now;
					departmentDb.IsArchived = true;
					departmentDb.ArchiveUserId = user.Id;

					Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Update(departmentDb);
				}

				if(errors.Count == 0)
				{
					Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition, $"{user.Username} added a department{departmentDb.Name}", user.Id);
					return Ok(new { response = new Models.Response<string>(true, "Delete Success. Log Success", errors) });
				}
				else
				{
					return Ok(new { response = new Models.Response<string> { Errors = errors, ResponseBody = "", Success = false } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}