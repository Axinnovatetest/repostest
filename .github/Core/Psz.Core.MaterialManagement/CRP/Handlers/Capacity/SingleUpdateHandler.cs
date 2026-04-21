using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class SingleUpdateHandler: IHandle<SingleCapacityUpdateModel, ResponseModel<CapacityModel>>
	{
		private SingleCapacityUpdateModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public SingleUpdateHandler(SingleCapacityUpdateModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<CapacityModel> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}
					var createUserId = CapacityPlanAccess.GetCreationBy_CountryId_HallId_Year(this.data.CountryId,
						this.data.HallId,
						this.data.Year)?.CreationUserId;
					var lastValidationEntity = CapacityPlanValidationLogAccess.GetLastByYearCountryHall(data.Year, data.CountryId, data.HallId);
					if(lastValidationEntity == null)
					{
						return ResponseModel<CapacityModel>.FailureResponse("Cannot edit capacity to empty plan.");
					}
					else
					{
						if((int)lastValidationEntity.ValidationStatus != (int)Enums.CapacityPlan.ValidationStatuses.Validated)
						{
							return ResponseModel<CapacityModel>.FailureResponse("Cannot edit capacity while plan in validation pipeline.");
						}
					}

					return Perform(this.user, this.data);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<CapacityModel> Perform(Identity.Models.UserModel user,
			SingleCapacityUpdateModel data)
		{
			#region > Validation
			var errors = new List<string>();
			foreach(var errorMessage in data.Validate())
			{
				errors.Add(errorMessage);
			}

			var capacityEntity = CapacityAccess.Get(data.CapacityId);
			if(capacityEntity == null)
				errors.Add("Capacity not found");

			if(data.Year < DateTime.Today.Year)
				errors.Add("Cannot change Capacity from the past");

			if(!Helpers.Config.CanEdit(data.Year, data.WeekNumber, Enums.Main.CapacityType.Capacity))
				errors.Add($"Cannot edit capacity beyond KW {Helpers.Config.GetCapacityLastEditableWeek()}");

			var countryEntity = CountryAccess.Get(data.CountryId);
			if(countryEntity == null)
				errors.Add("Country not found");

			var departementEntity = DepartmentAccess.Get(data.DepartementId);
			if(departementEntity == null)
				errors.Add("Department not found");

			var hallEntity = HallAccess.Get(data.HallId);
			if(hallEntity == null)
				errors.Add("hall not found");

			var operationEntity = StandardOperationAccess.Get(data.OperationId);
			if(operationEntity == null)
				errors.Add("Operation not found");

			var workAreaEntity = WorkAreaAccess.Get(data.WorkAreaId);
			if(workAreaEntity == null)
				errors.Add("Work Area not found");

			var workStationEntity = WorkStationMachineAccess.Get(data.WorkStationId ?? -1);
			if(data.WorkStationId.HasValue && workStationEntity == null)
				errors.Add("Work Station not found");

			if(errors.Count > 0)
				return ResponseModel<CapacityModel>.FailureResponse(errors);
			#endregion

			var calculateItem = data as Models.CapacityPlan.CalculateItemModel;
			var calculatedItem = CapacityPlan.CalculateItemHandler.Perform(calculateItem);

			//var similarOperations = CapacityAccess.Get(
			//    countryId: data.CountryId,
			//    year: data.Year,
			//    weekNumberFrom: data.WeekNumber,
			//    weekNumberUntil: data.WeekNumber,
			//    operationId: data.OperationId,
			//    hallId: data.HallId,
			//    departementId: data.DepartementId,
			//    workAreaId: data.WorkAreaId,
			//    workStationId: data.WorkStationId);

			//CapacityAccess.Update_IsArchived_ArchiveTime_ArchiveUserId(similarOperations?.Select(x => x.Id)?.ToList(), true, DateTime.Now, user.Id);
			//var isSimilarCapacities = similarOperations != null && similarOperations.Count > 0;

			var mondayDate = Helpers.DateTimeHelper.FirstDateOfWeekISO8601(capacityEntity.Year, capacityEntity.WeekNumber).Date;
			var newCapacity = new CapacityEntity()
			{
				Id = -1,

				Year = data.Year,
				WeekNumber = data.WeekNumber,
				WeekFirstDay = mondayDate,
				WeekLastDay = mondayDate.AddDays(+6),

				OperationId = data.OperationId,
				OperationName = operationEntity.Name,
				CountryId = countryEntity.Id,
				CountryName = countryEntity.Name,
				HallId = data.HallId,
				HallName = hallEntity?.Name,
				DepartementId = data.DepartementId,
				DepartementName = departementEntity.Name,
				WorkAreaId = data.WorkAreaId,
				WorkAreaName = workAreaEntity.Name,
				WorkStationId = data.WorkStationId,
				WorkStationName = workStationEntity?.Name,
				FormToolInsert = data.FormToolInsert,

				HrHardResourcesNumber = data.HrHardResourcesNumber /*+ (similarOperations?.Sum(x => x.HrHardResourcesNumber) ?? 0)*/,
				Factor1HrDaily = /* isSimilarCapacities? 0 : */data.HrF1PersonPerMachine,
				Factor2HrDaily = /* isSimilarCapacities ? 0 : */ data.HrF2UtilisationRate,
				ProductivityHrDaily = /* isSimilarCapacities ? 0 : */ data.HrProductivity,

				SoftRessourcesNumberDaily = data.SrSoftResourcesNumber /*+ (similarOperations?.Sum(x => x.SoftRessourcesNumberDaily) ?? 0)*/,
				Factor3SrDaily = /* isSimilarCapacities ? 0 : */ data.SrF3AttendanceLevel,
				ProductivitySrDaily = /* isSimilarCapacities ? 0 : */ data.SrProductivity,

				ShiftsNumberWeekly = data.ShiftsPerWeek/*  + (similarOperations?.Sum(x => x.ShiftsNumberWeekly) ?? 0) */,
				SpecialShiftsWeekly = data.SpecialShiftsPerWeek /*+ (similarOperations?.Sum(x => x.SpecialShiftsWeekly) ?? 0)*/,
				SpecialHoursWeekly = data.SpecialHours /*+ (similarOperations?.Sum(x => x.SpecialHoursWeekly) ?? 0)*/,
				WorkingHoursPerShift = /* isSimilarCapacities ? 0 : */ data.WorkingHoursPerShift,

				AvailableHrDaily = calculatedItem.AvailableHr /*+ (similarOperations?.Sum(x => x.AvailableHrDaily) ?? 0)*/,
				AvailableSrDaily = calculatedItem.AvailableSr /*+ (similarOperations?.Sum(x => x.AvailableSrDaily) ?? 0)*/,

				Attendance = calculatedItem.AttendancePerWeek /*+ (similarOperations?.Sum(x => x.Attendance) ?? 0)*/,
				PlanCapacity = calculatedItem.PlannableCapacites /*  + (similarOperations?.Sum(x => x.ShiftsNumberWeekly) ?? 0) */,
				RequiredEmployees = calculatedItem.RequiredEmployeesNumber /*+ (similarOperations?.Sum(x => x.RequiredEmployees) ?? 0)*/,

				CreationTime = DateTime.Now,
				CreationUserId = user.Id,
				Version = 0,
				LastUpdateTime = null,
				LastUpdateUserId = null,
				IsArchived = false,
				ArchiveTime = null,
				ArchiveUserId = null,
			};


			//CapacityAccess.Update_IsArchived_ArchiveTime_ArchiveUserId(capacityEntity.Id, true, DateTime.Now, user.Id);
			//CapacityAccess.Insert(newCapacity);

			// - transactional
			CapacityAccess.Update_IsArchived_ArchiveTime_ArchiveUserId_AND_Insert(capacityEntity.Id, true, DateTime.Now, user.Id, newCapacity);

			// -
			return ResponseModel<CapacityModel>.SuccessResponse(/*new CapacityModel(CapacityAccess.Get(insertedId))*/null);
		}

		public ResponseModel<CapacityModel> Validate()
		{
			throw new NotImplementedException();
		}
		static CapacityPlanEntity toCapacityPlan(int userId, CapacityEntity capacityEntity)
		{
			if(capacityEntity == null)
				return null;

			return new CapacityPlanEntity
			{
				ArchiveTime = null,
				ArchiveUserId = null,
				Attendance = capacityEntity.Attendance,
				AvailableSrDaily = capacityEntity.AvailableSrDaily,
				CountryId = capacityEntity.CountryId,
				CountryName = capacityEntity.CountryName,
				CreationTime = DateTime.Now,
				CreationUserId = userId,
				DepartementId = capacityEntity.DepartementId,
				DepartementName = capacityEntity.DepartementName,
				Factor1HrDaily = capacityEntity.Factor1HrDaily,
				Factor2HrDaily = capacityEntity.Factor2HrDaily,
				Factor3SrDaily = capacityEntity.Factor3SrDaily,
				FormToolInsert = capacityEntity.FormToolInsert,
				HallId = capacityEntity.HallId,
				HallName = capacityEntity.HallName,
				Id = -1,
				IsArchived = false,
				LastUpdateTime = null,
				LastUpdateUserId = null,
				OperationId = capacityEntity.OperationId,
				OperationName = capacityEntity.OperationName,
				PlanCapacity = capacityEntity.PlanCapacity,
				ProductivityHrDaily = capacityEntity.ProductivityHrDaily,
				ProductivitySrDaily = capacityEntity.ProductivitySrDaily,
				RequiredEmployees = capacityEntity.RequiredEmployees,
				SoftRessourcesNumberDaily = capacityEntity.SoftRessourcesNumberDaily,
				Version = 1,
				WeekNumber = capacityEntity.WeekNumber,
				WorkAreaId = capacityEntity.WorkAreaId,
				WorkAreaName = capacityEntity.WorkAreaName,
				Year = capacityEntity.Year,

				WorkStationId = capacityEntity.WorkStationId,
				WorkStationName = capacityEntity.WorkStationName,
				WeekFirstDay = capacityEntity.WeekFirstDay,
				WeekLastDay = capacityEntity.WeekLastDay,

				WorkingHoursPerShift = capacityEntity.WorkingHoursPerShift,
				ShiftsNumberWeekly = capacityEntity.ShiftsNumberWeekly, /* TYPE CHANGED */
				SpecialHoursWeekly = capacityEntity.SpecialHoursWeekly, /* TYPE CHANGED */
				SpecialShiftsWeekly = capacityEntity.SpecialShiftsWeekly, /* TYPE CHANGED */

				HrHardResourcesNumber = capacityEntity.HrHardResourcesNumber, /* NEW */
				AvailableHrDaily = capacityEntity.AvailableHrDaily, /* NEW */
			};
		}
	}
}
