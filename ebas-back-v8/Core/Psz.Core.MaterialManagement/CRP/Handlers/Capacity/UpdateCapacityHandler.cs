using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class UpdateCapacityHandler: IHandle<UpdateCapacityModel, ResponseModel<object>>
	{
		private UpdateCapacityModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public UpdateCapacityHandler(UpdateCapacityModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<object> Handle()
		{
			try
			{
				if(user == null)
				{
					throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
				}
				var createUserId = CapacityPlanAccess.GetCreationBy_CountryId_HallId_Year(this.data.CountryId,
					this.data.HallId ?? -1,
					this.data.Year)?.CreationUserId;
				var lastValidationEntity = CapacityPlanValidationLogAccess.GetLastByYearCountryHall(data.Year, data.CountryId, data.HallId ?? -1);
				if(lastValidationEntity == null)
				{
					return ResponseModel<object>.FailureResponse("Cannot edit capacity to empty plan.");
				}
				else
				{
					if((int)lastValidationEntity.ValidationStatus != (int)Enums.CapacityPlan.ValidationStatuses.Validated)
					{
						return ResponseModel<object>.FailureResponse("Cannot edit capacity while plan validation in progress.");
					}
				}


				// -
				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static ResponseModel<object> Perform(Identity.Models.UserModel user,
			UpdateCapacityModel data, bool fromPlanValidation = false)
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					#region > Validation
					var countryEntity = CountryAccess.Get(data.CountryId);
					if(countryEntity == null || countryEntity.IsArchived)
					{
						return ResponseModel<object>.FailureResponse("Country is not found");
					}

					Infrastructure.Data.Entities.Tables.WPL.HallEntity hallEntity = null;
					if(data.HallId.HasValue)
					{
						hallEntity = HallAccess.Get(data.HallId.Value);
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


					if(data.FirstWeekNumber > data.LastWeekNumber)
					{
						errors.Add(new ResponseModel<object>.ResponseError()
						{
							Value = "WeekEnd should be bigger than WeekStart.",
						});
					}

					var weeksNumber = Helpers.DateTimeHelper.GetWeeksNumberInYear(data.Year);
					if(data.LastWeekNumber > weeksNumber)
					{
						errors.Add(new ResponseModel<object>.ResponseError()
						{
							Value = "WeekEnd value is not valid.",
						});
					}

					if(!data.IsFirstVersion.HasValue || !data.IsFirstVersion.Value)
					{
						if(data.Year < DateTime.Today.Year)
						{
							errors.Add(new ResponseModel<object>.ResponseError()
							{
								Value = "Cannot update planning from the past.",
							});
						}

						// > Should be next N weeks
						if(fromPlanValidation == true)
						{
							if(!Helpers.Config.CanEdit(data.Year, data.LastWeekNumber, Enums.Main.CapacityType.CapacityPlan))
							{
								errors.Add(new ResponseModel<object>.ResponseError()
								{
									Value = $"Cannot edit capacity before KW {Helpers.Config.GetCapacityLastEditableWeek()}",
								});
							}
						}
						else
						{
							if(!Helpers.Config.CanEdit(data.Year, data.LastWeekNumber, Enums.Main.CapacityType.Capacity))
							{
								errors.Add(new ResponseModel<object>.ResponseError()
								{
									Value = $"Cannot edit capacity beyond KW {Helpers.Config.GetCapacityLastEditableWeek()}",
								});
							}
						}
					}

					if(errors.Count > 0)
					{
						return ResponseModel<object>.FailureResponse(errors.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList());
					}
					#endregion

					#region > Insert/Update Capacity data

					// if updating current year's plan, rebuild only next N weeks capacity
					if(data.Year == DateTime.Today.Year)
					{
						if(data.IsFirstVersion.HasValue && data.IsFirstVersion.Value)
						{
							data.FirstWeekNumber = 1;
						}
						else
						{
							data.FirstWeekNumber = Helpers.DateTimeHelper.GetIso8601WeekOfYear(DateTime.Today);
						}
					}

					var firstDayOfFirstWeek = Helpers.DateTimeHelper.FirstDateOfWeekISO8601(data.Year, data.FirstWeekNumber)
						 .Date;
					var lastDayOfLastWeek = Helpers.DateTimeHelper.FirstDateOfWeekISO8601(data.Year, data.LastWeekNumber)
						.AddDays(+6)
						.Date;

					var capacityPlanEntities = CapacityPlanAccess.Get_BY_CountryId_HallId_BETWEEN_WeekFirstDay_WeekLastDay(
						data.CountryId,
						hallEntity?.Id,
						firstDayOfFirstWeek,
						lastDayOfLastWeek);

					var operationIds = capacityPlanEntities.Select(e => e.OperationId).Distinct().ToList();
					var hallIds = capacityPlanEntities.Select(e => e.HallId).Distinct().ToList();
					var departementIds = capacityPlanEntities.Select(e => e.DepartementId).Distinct().ToList();
					var workAreaIds = capacityPlanEntities.Select(e => e.WorkAreaId).Distinct().ToList();
					var workStationIds = capacityPlanEntities.Where(e => e.WorkStationId.HasValue).Select(e => e.WorkStationId.Value).Distinct().ToList();

					var operationEntities = StandardOperationAccess.Get(/*operationIds*/).FindAll(e => !e.IsArchived);
					var hallEntities = HallAccess.Get(/*hallIds*/).FindAll(e => !e.IsArchived);
					var departementEntities = DepartmentAccess.Get(/*departementIds*/).FindAll(e => !e.IsArchived);
					var workAreaEntities = WorkAreaAccess.Get(/*workAreaIds*/).FindAll(e => !e.IsArchived);
					var workStationEntities = WorkStationMachineAccess.Get(/*workStationIds*/).FindAll(e => !e.IsArchived);

					var capacityEntities = CapacityAccess.Get_BY_CountryId_HallId_BETWEEN_WeekFirstDay_WeekLastDay(
						data.CountryId,
						hallEntity?.Id,
						firstDayOfFirstWeek,
						lastDayOfLastWeek);


					// - loop vars
					List<int> capacityToDeleteIds = new List<int>();
					List<CapacityEntity> capacityToInsert = new List<CapacityEntity>();

					for(int weekNumber = data.FirstWeekNumber; weekNumber < (data.LastWeekNumber + 1); weekNumber++)
					{
						var mondayDate = Helpers.DateTimeHelper.FirstDateOfWeekISO8601(data.Year, weekNumber).Date;

						var lastDayDate = mondayDate.Date.AddDays(+6);
						var lastWeekInstantDateTime = lastDayDate.Date.AddDays(+7);
						var lastWorkDaysInstantDateTime = lastDayDate.Date.AddDays(+5);

						var weekCapacityPlanEntities = capacityPlanEntities.FindAll(e => e.WeekFirstDay.Date == mondayDate);

						foreach(var capacityPlanEntity in weekCapacityPlanEntities)
						{
							var operationEntity = operationEntities.Find(e => e.Id == capacityPlanEntity.OperationId);
							var itemHallEntity = hallEntities.Find(e => e.Id == capacityPlanEntity.HallId);
							var departementEntity = departementEntities.Find(e => e.Id == capacityPlanEntity.DepartementId);
							var workAreaEntity = workAreaEntities.Find(e => e.Id == capacityPlanEntity.WorkAreaId);
							var workStationEntity = capacityPlanEntity.WorkStationId.HasValue
								? workStationEntities.Find(e => e.Id == capacityPlanEntity.WorkStationId.Value)
								: null;

							var existantCapacityEntity = capacityEntities
								.Find(e => e.WeekNumber == weekNumber
									&& e.OperationId == capacityPlanEntity.OperationId
									&& e.HallId == capacityPlanEntity.HallId
									&& e.DepartementId == capacityPlanEntity.DepartementId
									&& e.WorkAreaId == capacityPlanEntity.WorkAreaId
									&& e.WorkStationId == capacityPlanEntity.WorkStationId);
							if(existantCapacityEntity != null)
							{
								capacityToDeleteIds.Add(existantCapacityEntity.Id);
							}

							var calculateItem = new Models.CapacityPlan.CalculateItemModel(capacityPlanEntity,
								/*holidaysInWorkDays*/0,
								/*holidaysInWeekends*/0);
							var calculatedItem = CapacityPlan.CalculateItemHandler.Perform(calculateItem);

							capacityToInsert.Add(new CapacityEntity()
							{
								Id = -1,

								Year = data.Year,
								WeekNumber = weekNumber,
								WeekFirstDay = mondayDate,
								WeekLastDay = mondayDate.AddDays(+6),

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
								Version = (existantCapacityEntity?.Version ?? 0) + 1,
								LastUpdateTime = null,
								LastUpdateUserId = null,
								IsArchived = false,
								ArchiveTime = null,
								ArchiveUserId = null,
							});
						}
					}

					// - 
					CapacityAccess.Archive_Delete_Insert(
						fromPlanValidation == true ? data.Year : -1, // -1 to avoid Archive option. This comes from PlanValidation
						data.CountryId, data.HallId ?? -1, true, DateTime.Now, user.Id, data.FirstWeekNumber, data.LastWeekNumber,
						capacityToDeleteIds,
						capacityToInsert);
					#endregion

					return ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<object> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
