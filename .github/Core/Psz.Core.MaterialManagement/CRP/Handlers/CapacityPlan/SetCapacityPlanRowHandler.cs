using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.CapacityPlan;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlan
{
	public class SetCapacityPlanRowHandler: IHandle<SetCapacityPlanModel, ResponseModel<object>>
	{
		private SetCapacityPlanModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public SetCapacityPlanRowHandler(SetCapacityPlanModel data,
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

					#region > Validation
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

					var errors = new List<ResponseModel<object>.ResponseError>();

					if(this.data.Year < DateTime.Today.Year)
					{
						errors.Add(new ResponseModel<object>.ResponseError()
						{
							Value = "Year should be current of later",
						});
					}

					if(this.data.FirstWeekNumber > this.data.LastWeekNumber)
					{
						errors.Add(new ResponseModel<object>.ResponseError()
						{
							Value = "First KW should be smaller of equal to last KW",
						});
					}
					else
					{
						var weeksNumber = Helpers.DateTimeHelper.GetWeeksNumberInYear(this.data.Year);
						if(this.data.LastWeekNumber > weeksNumber)
						{
							errors.Add(new ResponseModel<object>.ResponseError()
							{
								Value = "Last KW invalid",
							});
						}
					}

					var createUserIdPlan = CapacityPlanAccess.GetCreationBy_CountryId_HallId_Year(this.data.CountryId,
						this.data.HallId ?? -1,
						this.data.Year)?.CreationUserId;
					var lastValidationPlanEntity = CapacityPlanValidationLogAccess.GetLastByYearCountryHall(data.Year, data.CountryId, data.HallId ?? -1);
					if(lastValidationPlanEntity == null)
					{
						if(createUserIdPlan != null && this.user.Id != createUserIdPlan)
						{
							return ResponseModel<object>.FailureResponse("Only owner can edit plan.");
						}
					}
					else
					{
						if((int)lastValidationPlanEntity.ValidationStatus != (int)Enums.CapacityPlan.ValidationStatuses.Rejected
							&& (int)lastValidationPlanEntity.ValidationStatus != (int)Enums.CapacityPlan.ValidationStatuses.Unvalidated)
						{
							return ResponseModel<object>.FailureResponse("Cannot edit plan in validation pipeline.");
						}
					}

					var currentCapacitiesInUse = CapacityAccess.Get_BY_CountryId_HallId_BETWEEN_WeekFirstDay_WeekLastDay(this.data.CountryId, this.data.HallId, DateTime.Today, Helpers.Config.GetLastCapacityEditableDay());
					// > Should be next week or after
					if(currentCapacitiesInUse != null && currentCapacitiesInUse.Count > 0 && !Helpers.Config.CanEdit(this.data.Year, this.data.FirstWeekNumber, Enums.Main.CapacityType.CapacityPlan))
					{
						errors.Add(new ResponseModel<object>.ResponseError()
						{
							Value = $"Cannot set plan earlier than KW {Helpers.Config.GetCapacityLastEditableWeek()}",
						});
					}

					if(this.data.Items == null || this.data.Items.Count == 0)
					{
						errors.Add(new ResponseModel<object>.ResponseError()
						{
							Value = "No data",
						});
					}

					if(errors.Count > 0)
					{
						return ResponseModel<object>.FailureResponse(errors.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList());
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
							errors.Add(new ResponseModel<object>.ResponseError()
							{
								Value = $"Position {i}: {errorMessage}",
							});
						}

						var operationEntity = operationEntities.Find(e => e.Id == item.OperationId);
						if(operationEntity == null)
						{
							errors.Add(new ResponseModel<object>.ResponseError()
							{
								Value = $"Position {i}: Operation [{item.OperationName}] not found",
							});
						}

						var itemHallEntity = hallEntities.Find(e => e.Id == item.HallId);
						if(itemHallEntity == null || itemHallEntity.CountryId != countryEntity.Id)
						{
							errors.Add(new ResponseModel<object>.ResponseError()
							{
								Value = $"Position {i}: Hall [{item.HallName}] not found",
							});
						}

						var departementEntity = departementEntities.Find(e => e.Id == item.DepartementId);
						if(departementEntity == null)
						{
							errors.Add(new ResponseModel<object>.ResponseError()
							{
								Value = $"Position {i}: Department [{item.DepartementName}] not found",
							});
						}

						var workAreaEntity = workAreaEntities.Find(e => e.Id == item.WorkAreaId);
						if(workAreaEntity == null
							|| (workAreaEntity.DepartmentId != departementEntity?.Id
								&& workAreaEntity.HallId != itemHallEntity.Id))
						{
							errors.Add(new ResponseModel<object>.ResponseError()
							{
								Value = $"Position {i}: Work-Area [{item.WorkAreaName}] not found",
							});
						}

						var workStationEntity = item.WorkStationId.HasValue
							? workStationEntities.Find(e => e.Id == item.WorkStationId.Value)
							: null;
						if((item.WorkStationId.HasValue && workStationEntity == null)
							|| (workStationEntity != null && workStationEntity.WorkAreaId != workAreaEntity?.Id))
						{
							errors.Add(new ResponseModel<object>.ResponseError()
							{
								Value = $"Position {i}: Work-Station/Machine [{item.WorkStationName}] not found",
							});
						}
					}

					if(errors.Count > 0)
					{
						return ResponseModel<object>.FailureResponse(errors.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList());
					}
					#endregion

					#region > Calculate
					var calculatedItems = new List<CalculatedItemModel>();

					foreach(var item in this.data.Items)
					{
						calculatedItems.Add(CalculateItemHandler.Perform(item));
					}
					#endregion

					#region > Remove old values
					var oldCapacities = new List<CapacityPlanEntity>();
					foreach(var item in data.Items)
					{
						oldCapacities.AddRange(CapacityPlanAccess.Get_BY_Country_Hall_Week_Operation_Department_WorkArea_BETWEEN_Weeks(
								data.CountryId,
								item.HallId,
								data.Year,
								item.OperationId,
								item.DepartementId,
								item.WorkAreaId,
								item.WorkStationId,
								data.FirstWeekNumber,
								data.LastWeekNumber,
								null) ?? new List<CapacityPlanEntity>());

						// - Delete from Capacities - No need anymore, capacities are added only at last Validation
						//CapacityAccess.Delete(data.Year,
						//    data.CountryId,
						//    data.FirstWeekNumber,
						//    data.LastWeekNumber,
						//    item.OperationId,
						//    item.HallId,
						//    item.DepartementId,
						//    item.WorkAreaId,
						//    item.WorkStationId);
					}

					// - Archive in CapacityPlans
					CapacityPlanAccess.Update_IsArchived_ArchiveTime_ArchiveUserId(oldCapacities.Select(x => x.Id)?.ToList(),
									true,
									DateTime.Now,
									user.Id);
					#endregion

					#region > Insert/Update Plan data
					var firstDayOfFirstWeek = Helpers.DateTimeHelper.FirstDateOfWeekISO8601(this.data.Year, this.data.FirstWeekNumber)
						.Date;
					var lastDayOfLastWeek = Helpers.DateTimeHelper.FirstDateOfWeekISO8601(this.data.Year, this.data.LastWeekNumber)
						.AddDays(+6)
						.Date;

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

							CapacityPlanAccess.Delete(calculatedItem.Id);
							CapacityPlanAccess.Insert(new CapacityPlanEntity()
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

								AvailableHrDaily = calculatedItem.AvailableHr,
								AvailableSrDaily = calculatedItem.AvailableSr,

								ShiftsNumberWeekly = calculatedItem.ShiftsPerWeek,
								SpecialShiftsWeekly = calculatedItem.SpecialShiftsPerWeek,
								SpecialHoursWeekly = calculatedItem.SpecialHours,
								WorkingHoursPerShift = calculatedItem.WorkingHoursPerShift,

								Attendance = calculatedItem.AttendancePerWeek,
								PlanCapacity = calculatedItem.PlannableCapacites,
								RequiredEmployees = calculatedItem.RequiredEmployeesNumber,

								CreationTime = DateTime.Now,
								CreationUserId = user.Id,
								Version = 1,
								LastUpdateTime = null,
								LastUpdateUserId = null,
								IsArchived = false,
								ArchiveTime = null,
								ArchiveUserId = null,
							});
						}
					}
					#endregion

					//// > Update Capacity data - 2021-09-07 - moved to last Vlaidation !!!!
					//return Capacity.UpdateCapacityHandler.Perform(user, new Models.Capacity.UpdateCapacityModel()
					//{
					//    CountryId = this.data.CountryId,
					//    HallId = this.data.HallId,
					//    Year = this.data.Year,
					//    FirstWeekNumber = this.data.FirstWeekNumber,
					//    LastWeekNumber = this.data.LastWeekNumber,
					//});

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
	}
}
