using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class WorkStationMachineController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Get()
		{
			var errors = new List<string>();

			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var workStationMachineDb = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.GetList();

				var workStationMachineViewModel = new List<Core.Apps.WorkPlan.Models.WorkStationMachine.WorkStationMachineViewModel>();

				workStationMachineDb.ForEach(wsm => workStationMachineViewModel.Add(new Core.Apps.WorkPlan.Models.WorkStationMachine.WorkStationMachineViewModel(wsm)));

				return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.WorkStationMachine.WorkStationMachineViewModel>>() { Success = true, ResponseBody = workStationMachineViewModel, Errors = errors } });
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
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var workStationDb = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.Get(Id);
				if(workStationDb == null)
				{
					throw new Core.Exceptions.NotFoundException();
				}

				var workStationMachineViewModel = new Core.Apps.WorkPlan.Models.WorkStationMachine.WorkStationMachineViewModel(workStationDb);

				return Ok(new { response = new Models.Response<Core.Apps.WorkPlan.Models.WorkStationMachine.WorkStationMachineViewModel>() { Success = true, ResponseBody = workStationMachineViewModel, Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Add(Core.Apps.WorkPlan.Models.WorkStationMachine.WorkStationMachineViewModel model)
		{
			var errors = new List<string>();
			try
			{
				var addedId = 0;

				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var workAreaDb = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get(model.WorkAreaId);
				if(workAreaDb == null)
				{
					throw new Core.Exceptions.NotFoundException();
				}

				var workStationMachineDb = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.GetByNameWorkAreaId(model.Name, model.WorkAreaId); // no same name in the same WorkArea???
				if(workStationMachineDb != null)
				{
					errors.Add("Cannot Add with same name in the same WorkArea");
				}

				if(errors.Count == 0)
				{
					workStationMachineDb = new Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity()
					{
						Name = model.Name.Trim(),
						HallId = model.HallId,
						Type = (int)model.Type,
						CreationTime = DateTime.Now,
						CreationUserId = user.Id,
						CountryId = model.CountryId,
						WorkAreaId = model.WorkAreaId,
						IsArchived = false,
					};

					Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.Insert(workStationMachineDb);
				}
				var type = model.Type == Core.Apps.WorkPlan.Enums.WorkStationMachineEnums.WorkStationMachineTypes.Machine ? "Machine" : "Work Station";
				if(errors.Count == 0)
				{
					Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry,
						$"{user.Username} Add a {type}.",
						user.Id);
					return Ok(new { response = new Models.Response<string>(true, $"Add Success <{addedId}>.Log Add success", errors) });
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

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Edit(Core.Apps.WorkPlan.Models.WorkStationMachine.WorkStationMachineViewModel model)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var workAreaDb = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get(model.WorkAreaId);
				if(workAreaDb == null)
				{
					throw new Core.Exceptions.NotFoundException();
				}

				var workStationMachineDb = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.Get(model.Id);
				if(workStationMachineDb == null)
				{
					errors.Add("Cannot find Work Station/Machine to edit");
				}

				var ws = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.GetByNameWorkAreaId(model.Name, model.WorkAreaId); // no same name in the same WorkArea???
				if(ws != null && ws.Id != workStationMachineDb.Id)
				{
					errors.Add($"WorkStation name exists in work area [{workAreaDb.Name}]");
				}

				if(errors.Count == 0)
				{

					workStationMachineDb.Name = model.Name.Trim();
					workStationMachineDb.HallId = workAreaDb.HallId;
					workStationMachineDb.Type = (int)model.Type;
					workStationMachineDb.LastEditTime = DateTime.Now;
					workStationMachineDb.LastEditUserId = user.Id;
					workStationMachineDb.CountryId = workAreaDb.CountryId;
					workStationMachineDb.WorkAreaId = workAreaDb.Id;
					workStationMachineDb.IsArchived = false;

					var done = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.Update(workStationMachineDb);
					if(done == -1)
						errors.Add("Cannot edit to Db. DB Error");
				}

				var type = model.Type == Core.Apps.WorkPlan.Enums.WorkStationMachineEnums.WorkStationMachineTypes.Machine
					? "Machine"
					: "Work Station";

				if(errors.Count == 0)
				{
					Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry,
						$"{user.Username} Edited a {type}.",
						user.Id);
					return Ok(new { response = new Models.Response<string>(true, $"Edit Success.Log Add success", errors) });
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
					throw new Core.Exceptions.UnauthorizedException();
				}

				var type = "Work station/Machine";

				var workStationMachineDb = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.Get(Id);
				if(workStationMachineDb == null)
				{
					throw new Core.Exceptions.NotFoundException();
				}
				else
				{
					workStationMachineDb.IsArchived = true;
					workStationMachineDb.DeleteTime = DateTime.Now;
					workStationMachineDb.DeleteUserId = user.Id;

					type = (Core.Apps.WorkPlan.Enums.WorkStationMachineEnums.WorkStationMachineTypes)workStationMachineDb.Type == Core.Apps.WorkPlan.Enums.WorkStationMachineEnums.WorkStationMachineTypes.Machine ? "Machine" : "Work Station";

					Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.Update(workStationMachineDb);
				}

				if(errors.Count == 0)
				{
					Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry,
						$"{user.Username} Deleted a {type} .",
						user.Id);
					return Ok(new { response = new Models.Response<string>(true, $"Delete Success.Log Add success", errors) });
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

		[HttpGet("{workAreaId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetByWorkAreaId(int workAreaId)
		{
			var errors = new List<string>();

			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var workAreaDb = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get(workAreaId);
				if(workAreaDb == null)
				{
					throw new Core.Exceptions.NotFoundException();
				}

				var workStationMachineDb = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.GetByWorkAreaId(workAreaId);
				if(workStationMachineDb == null)
				{
					throw new Core.Exceptions.NotFoundException();
				}

				var workStationMachineViewModel = new List<Core.Apps.WorkPlan.Models.WorkStationMachine.WorkStationMachineViewModel>();
				workStationMachineDb.ForEach(wsm => workStationMachineViewModel.Add(new Core.Apps.WorkPlan.Models.WorkStationMachine.WorkStationMachineViewModel(wsm)));

				return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.WorkStationMachine.WorkStationMachineViewModel>>() { Success = true, ResponseBody = workStationMachineViewModel, Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportToExcel()
		{
			try
			{
				var data = Core.Apps.WorkPlan.Handlers.WorkStationMachine.ExportToExcel();
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"WorkStations-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
