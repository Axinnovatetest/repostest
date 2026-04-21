using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlan;
using Psz.Core.MaterialManagement.CRP.Models.CapacityPlan;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class AddCapacitiesHandler: IHandle<SetCapacityPlanModel, ResponseModel<object>>
	{
		private SetCapacityPlanModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public AddCapacitiesHandler(SetCapacityPlanModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<object> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}

					// - 
					this.data.Items = mergeSimilarOps(this.data.Items);

					#region > Validation
					var createUserId = CapacityPlanAccess.GetCreationBy_CountryId_HallId_Year(this.data.CountryId,
						this.data.HallId ?? -1,
						this.data.Year)?.CreationUserId;
					var lastValidationEntity = CapacityPlanValidationLogAccess.GetLastByYearCountryHall(data.Year, data.CountryId, data.HallId ?? -1);
					if(lastValidationEntity == null)
					{
						return ResponseModel<object>.FailureResponse("Cannot add capacity to empty plan.");
					}
					else
					{
						if((int)lastValidationEntity.ValidationStatus != (int)Enums.CapacityPlan.ValidationStatuses.Validated)
						{
							return ResponseModel<object>.FailureResponse("Cannot add capacity while plan validation in progress.");
						}
					}

					var countryEntity = CountryAccess.Get(this.data.CountryId);
					if(countryEntity == null || countryEntity.IsArchived)
					{
						return ResponseModel<object>.FailureResponse("Country is not found");
					}

					Infrastructure.Data.Entities.Tables.WPL.HallEntity hallEntity = null;
					if(this.data.HallId.HasValue)
					{
						hallEntity = HallAccess.Get(this.data.HallId.Value);
						if(hallEntity == null || hallEntity.IsArchived)
						{
							return ResponseModel<object>.FailureResponse("Hall is not found");
						}
						else if(hallEntity.CountryId != countryEntity.Id)
						{
							return ResponseModel<object>.FailureResponse("Hall is not valid");
						}
					}

					var errors = new List<string>();

					if(this.data.Year < DateTime.Today.Year)
					{
						errors.Add("Date is not valid");
					}

					if(this.data.FirstWeekNumber > this.data.LastWeekNumber)
					{
						errors.Add("Date is not valid");
					}
					else
					{
						var weeksNumber = Helpers.DateTimeHelper.GetWeeksNumberInYear(this.data.Year);
						if(this.data.LastWeekNumber > weeksNumber)
						{
							errors.Add("LastWeek is not valid");
						}
					}

					// > Should be today week or after
					if(data.Year < DateTime.Today.Year)
						errors.Add("Cannot set Capacity from the past");

					if(!Helpers.Config.CanEdit(this.data.Year, this.data.LastWeekNumber, Enums.Main.CapacityType.Capacity))
					{
						errors.Add($"Cannot set capacity beyond KW {Helpers.Config.GetCapacityLastEditableWeek()}");
					}

					if(this.data.Items == null || this.data.Items.Count == 0)
					{
						errors.Add("No data");
					}

					if(errors.Count > 0)
					{
						return ResponseModel<object>.FailureResponse(errors);
					}

					var operationIds = this.data.Items.Select(e => e.OperationId).Distinct().ToList();
					var hallIds = this.data.Items.Select(e => e.HallId).Distinct().ToList();
					var departementIds = this.data.Items.Select(e => e.DepartementId).Distinct().ToList();
					var workAreaIds = this.data.Items.Select(e => e.WorkAreaId).Distinct().ToList();
					var workStationIds = this.data.Items.Where(e => e.WorkStationId.HasValue).Select(e => e.WorkStationId.Value).Distinct().ToList();

					var operationEntities = StandardOperationAccess.Get(operationIds).FindAll(e => !e.IsArchived);
					var hallEntities = HallAccess.Get(hallIds).FindAll(e => !e.IsArchived);
					var departementEntities = DepartmentAccess.Get(departementIds).FindAll(e => !e.IsArchived);
					var workAreaEntities = WorkAreaAccess.Get(workAreaIds).FindAll(e => !e.IsArchived);
					var workStationEntities = WorkStationMachineAccess.Get(workStationIds).FindAll(e => !e.IsArchived);

					int i = 0;
					foreach(var item in this.data.Items)
					{
						i++;

						foreach(var errorMessage in item.Validate())
						{
							errors.Add($"Item {i}: {errorMessage}");
						}

						var operationEntity = operationEntities.Find(e => e.Id == item.OperationId);
						if(operationEntity == null)
						{
							errors.Add($"Item {i}: Operation [{item.OperationName}] not found");
						}

						var itemHallEntity = hallEntities.Find(e => e.Id == item.HallId);
						if(itemHallEntity == null || itemHallEntity.CountryId != countryEntity.Id)
						{
							errors.Add($"Item {i}: Hall [{item.HallName}] not found");
						}

						var departementEntity = departementEntities.Find(e => e.Id == item.DepartementId);
						if(departementEntity == null)
						{
							errors.Add($"Item {i}: Department [{item.DepartementName}] not found");
						}

						var workAreaEntity = workAreaEntities.Find(e => e.Id == item.WorkAreaId);
						if(workAreaEntity == null
							|| (workAreaEntity.DepartmentId != departementEntity.Id
								&& workAreaEntity.HallId != itemHallEntity.Id))
						{
							errors.Add($"Item {i}: Work-Area [{item.WorkAreaName}] not found");
						}

						var workStationEntity = item.WorkStationId.HasValue
							? workStationEntities.Find(e => e.Id == item.WorkStationId.Value)
							: null;
						if((item.WorkStationId.HasValue && workStationEntity == null)
							|| (workStationEntity != null && workStationEntity.WorkAreaId != workAreaEntity.Id))
						{
							errors.Add($"Item {i}: Work-Station/Machine [{item.WorkStationName}] not found");
						}

						// -
						for(int ii = data.FirstWeekNumber; ii < data.LastWeekNumber + 1; ii++)
						{
							var capacityPlanEntities = CapacityPlanAccess.Get_BY_Country_Hall_Week_Operation_Department_WorkArea(data.CountryId,
							   data.HallId,
							   data.Year,
							   ii,
							   item.OperationId,
							   item.DepartementId,
							   item.WorkAreaId,
							   item.WorkStationId);
							if(capacityPlanEntities != null && capacityPlanEntities.Count > 0)
							{
								errors.Add($"Item {i}: Capacity Plan exists with same  {item.OperationName}/{item.DepartementName}/{item.HallName}/{item.WorkAreaName}{(item.WorkStationId.HasValue ? $"/{item.WorkStationName}" : "")} in week {ii}.");
							}
						}
					}

					if(errors.Count > 0)
					{
						return ResponseModel<object>.FailureResponse(errors);
					}

					#endregion

					#region > Calculate
					var calculatedItems = new List<CalculatedItemModel>();

					foreach(var item in this.data.Items)
					{
						calculatedItems.Add(CalculateItemHandler.Perform(item));
					}
					#endregion

					#region > Insert/Update data
					var firstDayOfFirstWeek = Helpers.DateTimeHelper.FirstDateOfWeekISO8601(this.data.Year, this.data.FirstWeekNumber).Date;
					var lastDayOfLastWeek = Helpers.DateTimeHelper.FirstDateOfWeekISO8601(this.data.Year, this.data.LastWeekNumber).AddDays(+6).Date;

					for(int weekNumber = this.data.FirstWeekNumber; weekNumber < (this.data.LastWeekNumber + 1); weekNumber++)
					{
						var mondayDate = Helpers.DateTimeHelper.FirstDateOfWeekISO8601(this.data.Year, weekNumber);

						foreach(var calculatedItem in calculatedItems)
						{
							var operationEntity = operationEntities.Find(e => e.Id == calculatedItem.OperationId);
							var itemHallEntity = hallEntities.Find(e => e.Id == calculatedItem.HallId);
							var departementEntity = departementEntities.Find(e => e.Id == calculatedItem.DepartementId);
							var workAreaEntity = workAreaEntities.Find(e => e.Id == calculatedItem.WorkAreaId);
							var workStationEntity = calculatedItem.WorkStationId.HasValue
								? workStationEntities.Find(e => e.Id == calculatedItem.WorkStationId.Value)
								: null;

							//// - find all similar capacities for merge
							//var similarOperations = CapacityAccess.Get(
							//    countryId: data.CountryId,
							//    year: data.Year,
							//    weekNumberFrom: weekNumber,
							//    weekNumberUntil: weekNumber,
							//    operationId: calculatedItem.OperationId,
							//    hallId: data.HallId,
							//    departementId: calculatedItem.DepartementId,
							//    workAreaId: calculatedItem.WorkAreaId,
							//    workStationId: calculatedItem.WorkStationId);
							//CapacityAccess.Update_IsArchived_ArchiveTime_ArchiveUserId(similarOperations?.Select(x => x.Id)?.ToList(), true, DateTime.Now, user.Id);

							CapacityAccess.Insert(new CapacityEntity()
							{
								Id = -1,

								Year = this.data.Year,
								WeekNumber = weekNumber,
								WeekFirstDay = mondayDate.Date,
								WeekLastDay = mondayDate.AddDays(+6).Date,

								OperationId = calculatedItem.OperationId,
								OperationName = operationEntity?.Name,
								CountryId = countryEntity.Id,
								CountryName = countryEntity.Name,
								HallId = calculatedItem.HallId,
								HallName = itemHallEntity?.Name,
								DepartementId = calculatedItem.DepartementId,
								DepartementName = departementEntity?.Name,
								WorkAreaId = calculatedItem.WorkAreaId,
								WorkAreaName = workAreaEntity?.Name,
								WorkStationId = calculatedItem.WorkStationId,
								WorkStationName = workStationEntity?.Name,
								FormToolInsert = calculatedItem.FormToolInsert,

								HrHardResourcesNumber = calculatedItem.HrHardResourcesNumber,
								Factor1HrDaily = calculatedItem.HrF1PersonPerMachine,
								Factor2HrDaily = calculatedItem.HrF2UtilisationRate,
								ProductivityHrDaily = calculatedItem.HrProductivity,

								SoftRessourcesNumberDaily = calculatedItem.SrSoftResourcesNumber,
								Factor3SrDaily = calculatedItem.SrF3AttendanceLevel,
								ProductivitySrDaily = calculatedItem.SrProductivity,

								ShiftsNumberWeekly = calculatedItem.ShiftsPerWeek,
								SpecialShiftsWeekly = calculatedItem.SpecialShiftsPerWeek,
								SpecialHoursWeekly = calculatedItem.SpecialHours,
								WorkingHoursPerShift = calculatedItem.WorkingHoursPerShift,

								AvailableHrDaily = calculatedItem.AvailableHr /*+ (similarOperations?.Sum(x => x.AvailableHrDaily) ?? 0)*/,
								AvailableSrDaily = calculatedItem.AvailableSr /*+ (similarOperations?.Sum(x => x.AvailableSrDaily) ?? 0)*/,

								Attendance = calculatedItem.AttendancePerWeek /*+ (similarOperations?.Sum(x => x.Attendance) ?? 0)*/,
								PlanCapacity = calculatedItem.PlannableCapacites /*+ (similarOperations?.Sum(x => x.PlanCapacity) ?? 0)*/,
								RequiredEmployees = calculatedItem.RequiredEmployeesNumber /*+ (similarOperations?.Sum(x => x.RequiredEmployees) ?? 0)*/,

								CreationTime = DateTime.Now,
								CreationUserId = user.Id,
								Version = 0,
								LastUpdateTime = null,
								LastUpdateUserId = null,
								IsArchived = false,
								ArchiveTime = null,
								ArchiveUserId = null,
							});
						}
					}
					#endregion

					return ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		ResponseModel<object> IHandle<SetCapacityPlanModel, ResponseModel<object>>.Validate()
		{
			throw new NotImplementedException();
		}
		static List<CalculateItemModel> mergeSimilarOps(List<CalculateItemModel> calculateItemModels)
		{
			if(calculateItemModels == null || calculateItemModels.Count <= 0)
				return calculateItemModels;

			var results = new List<CalculateItemModel>();

			foreach(var item in calculateItemModels)
			{
				var idx = results.FindIndex(x => x.OperationId == item.OperationId
				&& x.DepartementId == item.DepartementId && x.HallId == item.HallId
				&& x.WorkAreaId == item.WorkAreaId && x.WorkStationId == item.WorkStationId);
				if(idx >= 0)
				{
					results[idx].HrHardResourcesNumber += item.HrHardResourcesNumber;
					results[idx].HrF1PersonPerMachine += item.HrF1PersonPerMachine;
					results[idx].HrF2UtilisationRate += item.HrF2UtilisationRate;
					results[idx].HrProductivity += item.HrProductivity;

					results[idx].SrSoftResourcesNumber += item.SrSoftResourcesNumber;
					results[idx].SrF3AttendanceLevel += item.SrF3AttendanceLevel;
					results[idx].SrProductivity += item.SrProductivity;

					results[idx].ShiftsPerWeek += item.ShiftsPerWeek;
					results[idx].SpecialShiftsPerWeek += item.SpecialShiftsPerWeek;
					results[idx].SpecialHours += item.SpecialHours;
					results[idx].WorkingHoursPerShift += item.WorkingHoursPerShift;
				}
				else
				{
					results.Add(item);
				}
			}
			return results;
		}
	}
}
