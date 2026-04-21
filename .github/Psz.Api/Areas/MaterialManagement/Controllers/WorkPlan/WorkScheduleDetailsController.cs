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
	public class WorkScheduleDetailsController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";


		[HttpGet("{WorkScheduleId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Get(int WorkScheduleId)
		{
			var errors = new List<string>();

			try
			{
				var workScheduleVM = new List<Core.Apps.WorkPlan.Models.WorkScheduleDetails.WorkScheduleDetailsViewModel>();
				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(WorkScheduleId);
				if(workScheduleDb == null)
				{
					errors.Add("Can't find workSchedule");
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
				}

				var workScheduleDetailsDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByWorkScheduleId(WorkScheduleId);
				if(workScheduleDetailsDb == null)
				{
					errors.Add("Can't find work Schdule Details.Db Error");
				}
				else
				{
					var articleWorkGroup = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(workScheduleDb.ArticleId);
					workScheduleDetailsDb.ForEach(wsdb => workScheduleVM.Add(new Core.Apps.WorkPlan.Models.WorkScheduleDetails.WorkScheduleDetailsViewModel(wsdb,
						string.Equals(wsdb.WorkAreaName, "t-w", StringComparison.OrdinalIgnoreCase) ? articleWorkGroup?.Artikelkurztext : wsdb.WorkAreaName)));
				}

				if(errors.Count == 0)
				{
					return Ok(new Models.Response<List<Core.Apps.WorkPlan.Models.WorkScheduleDetails.WorkScheduleDetailsViewModel>>() { Success = true, ResponseBody = workScheduleVM, Errors = errors } );
				}
				else
				{
					return Ok( new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Add(Core.Apps.WorkPlan.Models.WorkScheduleDetails.WorkScheduleDetailsViewModel data)
		{
			var errors = new List<string>();

			try
			{
				var addedId = 0;

				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Authentification: User not found.");
				}

				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(data.WorkScheduleId);
				if(workScheduleDb == null)
				{
					errors.Add("WorkSchedule not found");
					return Ok(new
					{
						response = new Models.Response<string>()
						{
							Success = false,
							ResponseBody = "",
							Errors = errors
						}
					});
				}


				var standardOperations = data.StandardOperationId == null ? null : Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get(data.StandardOperationId);
				var workScheduleDetailsDb = new Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity()
				{
					Amount = data.Amount,
					CountryId = data.CountryId,
					CreationTime = DateTime.Now,
					CreationUserId = user.Id,
					DepartementId = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get(data.WorkAreaId)?.DepartmentId ?? -1, // data.DepartementId,
					FromToolInsert = data.FromToolInsert,
					FromToolInsert2 = data.FromToolInsert2,
					HallId = data.HallId,
					WorkScheduleId = data.WorkScheduleId,
					LotSizeSTD = data.LotSizeSTD > 0 ? data.LotSizeSTD : 1,
					OperationDescriptionId = data.OperationDescriptionId,
					OperationNumber = data.OperationNumber,
					OperationTimeSeconds = data.OperationTimeSeconds,
					OperationTimeValueAdding = data.OperationTimeValueAdding,
					PredecessorOperation = data.PredecessorOperation,
					PredecessorSubOperation = data.PredecessorSubOperation,
					RelationOperationTime = standardOperations == null ? -1 : standardOperations.RelationOperationTime, //data.RelationOperationTime,
					SetupTimeMinutes = data.SetupTimeMinutes,
					StandardOccupancy = data.StandardOccupancy,
					StandardOperationId = data.StandardOperationId,
					SubOperationNumber = data.SubOperationNumber,
					TotalTimeOperation = data.TotalTimeOperation,
					WorkAreaId = data.WorkAreaId,
					WorkStationMachineId = data.WorkStationMachineId,
					OperationValueAdding = data.OperationValueAdding ?? null,
					Comment = data.Comment,
					OrderDisplayId = data.OrderDisplayId,
				};

				workScheduleDetailsDb = Core.Apps.WorkPlan.Helpers.WorkSchedule.setTotalTimeOperation(workScheduleDetailsDb);
				workScheduleDetailsDb.OperationTimeValueAdding = Core.Apps.WorkPlan.Helpers.WorkSchedule.GetOperationTimeValueAddng(workScheduleDetailsDb); // - update 2021-08-04 from Khelil, XLS
				addedId = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Insert(workScheduleDetailsDb);

				workScheduleDb.LastEditTime = DateTime.Now;
				workScheduleDb.LastEditUserId = user.Id;
				Article.EditArticle(user, workScheduleDb.ArticleId);

				Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Update(workScheduleDb);

				var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(workScheduleDb.HallId);
				var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(workScheduleDb.ArticleId);

				Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.WorkSchedule,
					$"{user.Username} Added a Work Schedule details Line of {workScheduleDb?.Name} for {hallDb?.Name}/{articleDb?.Name}.",
					user.Id);

				return Ok(new
				{
					response = new Models.Response<string>(true,
					$"Add Success <{addedId}>. Log Add success",
					errors)
				});
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Edit(Core.Apps.WorkPlan.Models.WorkScheduleDetails.WorkScheduleDetailsViewModel data)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var workScheduleDetailsDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Get(data.Id);
				if(workScheduleDetailsDb == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "workScheduleDetailsDb == null");
					throw new Core.Exceptions.NotFoundException();
				}

				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(data.WorkScheduleId);
				if(workScheduleDb == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "workScheduleDb == null");
					throw new Core.Exceptions.NotFoundException();
				}

				var standardOperations = data.StandardOperationId == null ? null : Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get(data.StandardOperationId);
				workScheduleDetailsDb.Amount = data.Amount;
				//workScheduleDetailsDb.Country_Id = model.CountryId;
				workScheduleDetailsDb.LastEditTime = DateTime.Now;
				workScheduleDetailsDb.LastEditUserId = user.Id;
				workScheduleDetailsDb.DepartementId = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get(data.WorkAreaId)?.DepartmentId ?? -1; //data.DepartementId;
				workScheduleDetailsDb.FromToolInsert = data.FromToolInsert;
				workScheduleDetailsDb.FromToolInsert2 = data.FromToolInsert2;
				workScheduleDetailsDb.HallId = data.HallId;
				//workScheduleDetailsDb.WorkScheduleId = model.WorkScheduleId;
				workScheduleDetailsDb.LotSizeSTD = data.LotSizeSTD > 0 ? data.LotSizeSTD : 1;
				workScheduleDetailsDb.OperationDescriptionId = data.OperationDescriptionId;
				workScheduleDetailsDb.OperationNumber = data.OperationNumber;
				workScheduleDetailsDb.OperationTimeSeconds = data.OperationTimeSeconds;
				workScheduleDetailsDb.OperationTimeValueAdding = data.OperationTimeValueAdding;
				workScheduleDetailsDb.PredecessorOperation = data.PredecessorOperation;
				workScheduleDetailsDb.PredecessorSubOperation = data.PredecessorSubOperation;
				workScheduleDetailsDb.RelationOperationTime = standardOperations == null ? -1 : standardOperations.RelationOperationTime; //data.RelationOperationTime;
				workScheduleDetailsDb.SetupTimeMinutes = data.SetupTimeMinutes;
				workScheduleDetailsDb.StandardOccupancy = data.StandardOccupancy;
				workScheduleDetailsDb.StandardOperationId = data.StandardOperationId;
				workScheduleDetailsDb.SubOperationNumber = data.SubOperationNumber;
				workScheduleDetailsDb.TotalTimeOperation = data.TotalTimeOperation;
				workScheduleDetailsDb.WorkAreaId = data.WorkAreaId;
				workScheduleDetailsDb.WorkStationMachineId = data.WorkStationMachineId;
				workScheduleDetailsDb.Comment = data.Comment;
				workScheduleDetailsDb.OperationValueAdding = data.OperationValueAdding ?? null;
				workScheduleDetailsDb.OrderDisplayId = data.OrderDisplayId;

				workScheduleDetailsDb = Core.Apps.WorkPlan.Helpers.WorkSchedule.setTotalTimeOperation(workScheduleDetailsDb);
				workScheduleDetailsDb.OperationTimeValueAdding = Core.Apps.WorkPlan.Helpers.WorkSchedule.GetOperationTimeValueAddng(workScheduleDetailsDb);
				Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Update(workScheduleDetailsDb);

				workScheduleDb.LastEditTime = DateTime.Now;
				workScheduleDb.LastEditUserId = user.Id;
				Article.EditArticle(user, workScheduleDb.ArticleId);

				var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(workScheduleDb.ArticleId);

				Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.WorkSchedule,
					$"{user.Username} edited a Work Schedule details Line of {workScheduleDb.Name} for {articleDb?.Name}.",
					user.Id);

				return Ok(new
				{
					response = new Models.Response<string>(true,
						$"Edit Success.Log Add success",
						new List<string>())
				});
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Delete(int id)
		{
			try
			{
				var workScheduleDetailsDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Get(id);
				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(workScheduleDetailsDb.WorkScheduleId);

				var user = this.GetCurrentUser();
				if(user == null)
				{
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = new List<string>() { "Can not find User.Authentification error" } } });
				}

				if(workScheduleDetailsDb == null || workScheduleDb == null)
				{
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = new List<string>() { "Can't find Work Schedule Details.DB error" } } });
				}

				Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Delete(id);
				workScheduleDb.LastEditTime = DateTime.Now;
				workScheduleDb.LastEditUserId = user.Id;

				Article.EditArticle(user, workScheduleDb.ArticleId);
				Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Update(workScheduleDb);

				var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(workScheduleDb.HallId);
				var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(workScheduleDb.ArticleId);

				Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.WorkSchedule,
					$"{user.Username} edited a Work Schedule details Line of {workScheduleDb.Name} for {hallDb?.Name}/{articleDb?.Name}.",
					user.Id);

				return Ok(new { response = new Models.Response<string>(true, $"Delete Success.Log Add success", new List<string>()) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult UpdateDisplayOrder(Core.Apps.WorkPlan.Models.WorkScheduleDetails.WorkScheduleDetailOrder data)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				if(data.Orders == null || data.Orders.Count == 0)
				{
					return Ok(new { response = new Models.Response<string>(true, "", new List<string>()) });
				}

				var workShudlesDetailsIds = data.Orders.Select(e => e.Id).ToList();
				var workSchedulesDetailsDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Get(workShudlesDetailsIds);

				foreach(var element in data.Orders)
				{
					var workScheduleDetailsDb = workSchedulesDetailsDb.Find(e => e.Id == element.Id);
					if(workScheduleDetailsDb == null)
					{
						continue;
					}

					Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.UpdateOrderDisplayId(workScheduleDetailsDb.Id,
						element.OrderDisplayId);
				}

				return Ok(new { response = new Models.Response<string>(true, $"Update Display Order Success. Log Add success", new List<string>()) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult UpdateOperationNumber(Core.Apps.WorkPlan.Models.WorkScheduleDetails.WorkScheduleDetailsOpNumbers data)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				if(data.OperationNumber == null || data.OperationNumber.Count == 0)
				{
					return Ok(new { response = new Models.Response<string>(true, "", new List<string>()) });
				}

				var workShudlesDetailsIds = data.OperationNumber.Select(e => e.Id).ToList();
				var workSchedulesDetailsDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Get(workShudlesDetailsIds);

				if(workSchedulesDetailsDb.Count == 0)
				{
					return Ok(new { response = new Models.Response<string>(true, "", new List<string>()) });
				}

				var workScheduleId = workSchedulesDetailsDb.First().WorkScheduleId;
				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(workScheduleId);

				foreach(var element in data.OperationNumber)
				{
					var workScheduleDetailsDb = workSchedulesDetailsDb.Find(e => e.Id == element.Id);
					if(workScheduleDetailsDb == null)
					{
						continue;
					}

					Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Update_OperationNumber_PredecessorOperation(workScheduleDetailsDb.Id,
						element.OperationNumber,
						element.PredecessorOperation);
				}

				var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(workScheduleDb.ArticleId);

				Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.WorkSchedule,
					$"{user.Username} changed workSchedule({workScheduleDb?.Name}) detail lines display order for {articleDb?.Name}.",
					user.Id);

				return Ok(new { response = new Models.Response<string>(true, $"Update OpNumbers Success.Log Add success", new List<string>()) });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

	}
}
