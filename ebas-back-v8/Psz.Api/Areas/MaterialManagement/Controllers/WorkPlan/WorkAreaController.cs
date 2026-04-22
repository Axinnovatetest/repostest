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
	public class WorkAreaController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel>), 200)]
		public IActionResult Get()
		{
			var errors = new List<string>();
			try
			{

				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Can't get connected user");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var workAreaDb = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.GetList();
				if(workAreaDb == null)
				{
					errors.Add("Can't find workArea in DB. Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var workAreaViewModel = new List<Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel>();

				workAreaDb.ForEach(wa => workAreaViewModel.Add(new Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel(wa)));

				return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel>>() { Success = true, ResponseBody = workAreaViewModel, Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel), 200)]
		public IActionResult Get(int Id)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Can't get connected user");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				// Check if workArea exist in DB
				var workAreaDb = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get(Id);
				if(workAreaDb == null)
				{
					errors.Add("Can't find the workArea in DB.Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var workAreaViewModel = new Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel(workAreaDb);

				return Ok(new { response = new Models.Response<Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel>() { Success = true, ResponseBody = workAreaViewModel, Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(string), 200)]
		public IActionResult Add(Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel model)
		{
			var errors = new List<string>();
			try
			{
				if(string.IsNullOrWhiteSpace(model.Name))
				{
					errors.Add("Name invalid");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				//check if current user is connected.
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Can't get connected user");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(model.Hall_Id);
				if(hallDb == null)
				{
					errors.Add("Can't find the hall in DB.Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
				var countryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(model.CountryId);
				if(countryDb == null)
				{
					errors.Add("Can't find the Country in DB.Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var wa = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.GetByHallIdAndName(model.Hall_Id, model.Name);
				if(wa != null && wa.Count > 0)
				{
					errors.Add("Work area name exists");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var workArea = new Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity()
				{
					Name = model.Name.Trim(),
					HallId = model.Hall_Id,
					CreationTime = DateTime.Now,
					CreationUserId = user.Id,
					IsArchived = false,
					CountryId = model.CountryId,
					DepartmentId = model.Department_Id
				};

				var addedId = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Insert(workArea);
				if(addedId == -1)
				{
					errors.Add("Cannot add to Db. DB Error");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry, $"{user.Username} Added a User in {hallDb.Name}/{countryDb.Name}.", user.Id);

				return Ok(new { response = new Models.Response<string>(true, $"Add Success<{addedId}>. Log Add success", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(string), 200)]
		public IActionResult Edit(Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel model)
		{
			var errors = new List<string>();
			try
			{
				if(string.IsNullOrWhiteSpace(model.Name))
				{
					errors.Add("Name invalid");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Can't get connected user");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var workAreaDb = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get(model.Id);
				if(workAreaDb == null)
				{
					errors.Add("Can't find the workArea in DB.Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(model.Hall_Id);
				if(hallDb == null)
				{
					errors.Add("Can't find the hall in DB.Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
				var countryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(model.CountryId);
				if(countryDb == null)
				{
					errors.Add("Can't find the Country in DB.Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var wa = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.GetByHallIdAndName(model.Hall_Id, model.Name);
				if(wa != null && wa.Count>0 && wa.FirstOrDefault().Id != workAreaDb.Id)
				{
					errors.Add("Work area name exists");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				workAreaDb.CountryId = model.CountryId;
				workAreaDb.Name = model.Name.Trim();
				workAreaDb.HallId = model.Hall_Id;
				workAreaDb.LastEditTime = DateTime.Now;
				workAreaDb.LastEditUserId = user.Id;
				workAreaDb.DepartmentId = model.Department_Id;

				var done = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Update(workAreaDb);
				if(done == -1)
				{
					errors.Add("Can't edit workArea.DB Error");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry, $"{user.Username} Edited a User in {hallDb.Name}/{countryDb.Name}.", user.Id);

				return Ok(new { response = new Models.Response<string>(true, "Edit Success.", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(string), 200)]
		public IActionResult Delete(int Id)
		{
			var errors = new List<string>();
			try
			{
				//check if current user is connected.
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Can't get connected user");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				// Check if workArea exist in DB
				var workAreaDb = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get(Id);
				if(workAreaDb == null)
				{
					errors.Add("Can't find the workArea in DB.Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
				var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(workAreaDb.HallId);
				if(hallDb == null)
				{
					errors.Add("Can't find the hall in DB.Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
				var countryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(workAreaDb.CountryId);
				if(countryDb == null)
				{
					errors.Add("Can't find the Country in DB.Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.ArchiveByWorkAreaId(workAreaDb.Id);

				workAreaDb.ArchiveTime = DateTime.Now;
				workAreaDb.ArchiveUserId = user.Id;
				workAreaDb.IsArchived = true;

				var done = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Update(workAreaDb);
				if(done == -1)
				{
					errors.Add("Can't Delete workArea. DB Error");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry, $"{user.Username} Deleted a User in {hallDb.Name}/{countryDb.Name}.", user.Id);

				return Ok(new { response = new Models.Response<string>(true, "Delete Success", errors) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{HallId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel>), 200)]
		public IActionResult GetByHallId(int HallId)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Can't get connected user");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(HallId);
				if(hallDb == null)
				{
					errors.Add("Can't find the hall in DB. Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				var countryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(hallDb.CountryId);
				if(countryDb == null)
				{
					errors.Add("Can't find the Country in DB. Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}
				// Check if workArea exist in DB
				var workAreaDb = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.GetByHallId(HallId);
				if(workAreaDb == null)
				{
					errors.Add("Can't find the workArea in DB. Error Db");
					return Ok(new { response = new Models.Response<string> { Success = false, ResponseBody = "", Errors = errors } });
				}

				//Return the Work Area
				var workAreaViewModel = new List<Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel>();
				workAreaDb.ForEach(wa => workAreaViewModel.Add(new Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel(wa)));

				return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.WorkArea.WorkAreaViewModel>>() { Success = true, ResponseBody = workAreaViewModel, Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportWAToExcel()
		{
			try
			{
				var data = Core.Apps.WorkPlan.Handlers.WorkArea.ExportToExcel();
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"WorkAreas-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
	}
}
