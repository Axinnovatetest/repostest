using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation
{
	public class ValidateHandler: IHandle<Models.CapacityPlanValidation.ValidateModel, ResponseModel<int>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }
		private Models.CapacityPlanValidation.ValidateModel data { get; set; }

		public ValidateHandler(Identity.Models.UserModel user, Models.CapacityPlanValidation.ValidateModel data)
		{
			this.user = user;
			this.data = data;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.CapacityPlanLock)
			{
				try
				{
					Validate();

					//-
					var planEntities = CapacityPlanAccess.GetBy_CountryId_HallId_Year(data.CountryId, data.HallId, data.Year);
					var countryEntity = CountryAccess.Get(data.CountryId);
					var HallEntity = HallAccess.Get(data.HallId);


					#region >>> Archive, Capacity & Email <<<
					if(data.Level == 2)
					{
						int firstWeek = 1;
						bool isFirstPlan = CapacityAccess.Get_Count_BY_CountryId_HallId_Year(data.Year, data.CountryId, data.HallId) <= 0;
						// - if validating current Year
						if(data.Year == DateTime.Today.Year)
						{
							firstWeek = isFirstPlan ? 1 : Helpers.Config.GetCapacityLastEditableWeek();
						}
						else
						{
							// - take the smallest WeekNumber form corresponding CapacityPlan
							firstWeek = (CapacityPlanAccess.GetBy_CountryId_HallId_Year(this.data.CountryId, this.data.HallId, this.data.Year)
								?? new List<CapacityPlanEntity>())
								.Aggregate((i1, i2) => i1.WeekNumber < i2.WeekNumber ? i1 : i2)?.WeekNumber ?? 1;
						}


						var maxWeekNumber = Helpers.DateTimeHelper.GetWeeksNumberInYear(data.Year);

						// - Archive prev Capacities if any - done in Capacity. Update w fromPlanValidation:true
						// - Update Capacity data - convert whole (or futur plan of) Year into Capacity
						var responseUpdateCapacity = Capacity.UpdateCapacityHandler.Perform(user, new Models.Capacity.UpdateCapacityModel()
						{
							CountryId = this.data.CountryId,
							HallId = this.data.HallId,
							Year = this.data.Year,
							FirstWeekNumber = this.data.Year > DateTime.Today.Year
								? firstWeek
								: Math.Min(maxWeekNumber, Helpers.Config.GetCapacityLastEditableWeek()),
							LastWeekNumber = maxWeekNumber,
							IsFirstVersion = isFirstPlan
						}, fromPlanValidation: true);

						if(!responseUpdateCapacity.Success
							&& responseUpdateCapacity.Errors != null
							&& responseUpdateCapacity.Errors.Count > 0)
							return ResponseModel<int>.FailureResponse(responseUpdateCapacity.Errors.Select(x => x.Value).ToList());

						// - Log
						CapacityPlanValidationLogAccess.Insert(new CapacityPlanValidationLogEntity
						{
							CountryId = countryEntity.Id,
							CountryName = countryEntity.Name,
							HallId = HallEntity.Id,
							HallName = HallEntity.Name,
							ValidationLevel = data.Level,
							ValidationStatus = (int)Enums.CapacityPlan.ValidationStatuses.Validated,
							ValidationStatusName = Enums.CapacityPlan.ValidationStatuses.Validated.GetDescription(),
							ValidationTime = DateTime.Now,
							ValidationUserId = user.Id,
							Year = data.Year,
							ValidationReason = ""
						});

						// -
						var planOwner = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(
										CapacityPlanAccess.GetCreationBy_CountryId_HallId_Year(this.data.CountryId, this.data.HallId, this.data.Year)?.CreationUserId ?? -1);
						var content = $"<br/><span style='font-size:1.15em;'><strong>{user.Name?.ToUpper()}</strong> has just confirmed your validation request for Capacity Plan <strong>[{countryEntity.Name} - {HallEntity.Name} - {data.Year}]</strong>. <br/>Edition is no longer possible on this plan, unless unvalidated."
						+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}#/crp/planning'>here</a>";
						Helpers.Email.Send(user, "Plan Validation", content, new List<string> { planOwner.Email });
					}
					else
					{
						// - Log
						CapacityPlanValidationLogAccess.Insert(new CapacityPlanValidationLogEntity
						{
							CountryId = countryEntity.Id,
							CountryName = countryEntity.Name,
							HallId = HallEntity.Id,
							HallName = HallEntity.Name,
							ValidationLevel = data.Level,
							ValidationStatus = (int)Enums.CapacityPlan.ValidationStatuses.Invalidation,
							ValidationStatusName = Enums.CapacityPlan.ValidationStatuses.Invalidation.GetDescription(),
							ValidationTime = DateTime.Now,
							ValidationUserId = user.Id,
							Year = data.Year,
							ValidationReason = ""
						});
						var nextValidationUsers = CapacityPlanValidationUserAccess.GetByCountryUserLevel(data.CountryId, data.Level + 1);
						var content = $"<br/><span style='font-size:1.15em;'><strong>{user.Name?.ToUpper()}</strong> has just sent a validation request for Capacity Plan <strong>[{countryEntity.Name} - {HallEntity.Name} - {data.Year}]</strong>.<br/>Please consider processing it ASAP."
						+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}#/crp/planning'>here</a>";
						Helpers.Email.Send(user, "Plan Validation", content, nextValidationUsers?.Select(x => x.UserEmail)?.ToList());
					}
					#endregion

					// - Validation
					var id = CapacityPlanValidationAccess.Insert(planEntities.Select(x => CopyFromPlan(x, data.Level, DateTime.Now, user.Id))?.ToList());

					// -
					return ResponseModel<int>.SuccessResponse(id);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<int> Validate()
		{
			if(user == null)
			{
				throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
			}

			if(this.data.Year < DateTime.Today.Year)
				return ResponseModel<int>.FailureResponse("Cannot change past plans.");


			if(CountryAccess.Get(data.CountryId) == null)
				return ResponseModel<int>.FailureResponse("Country not found.");

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id) == null)
				return ResponseModel<int>.FailureResponse("User not found.");

			var planEntities = Infrastructure.Data.Access.Tables.MTM.CapacityPlanAccess.GetBy_CountryId_HallId_Year(data.CountryId, data.HallId, data.Year);
			if(planEntities == null || planEntities.Count <= 0)
				return ResponseModel<int>.FailureResponse("Plan not found.");

			return ResponseModel<int>.SuccessResponse();
		}
		CapacityPlanValidationEntity CopyFromPlan(CapacityPlanEntity capacityPlanEntity, int validationLevel, DateTime validationTime, int validationUserId)
		{
			if(capacityPlanEntity == null)
				return null;

			return new CapacityPlanValidationEntity
			{
				ArchiveTime = capacityPlanEntity.ArchiveTime,
				ArchiveUserId = capacityPlanEntity.ArchiveUserId,
				Attendance = capacityPlanEntity.Attendance,
				AvailableHrDaily = capacityPlanEntity.AvailableHrDaily,
				AvailableSrDaily = capacityPlanEntity.AvailableSrDaily,
				CountryId = capacityPlanEntity.CountryId,
				CountryName = capacityPlanEntity.CountryName,
				CreationTime = capacityPlanEntity.CreationTime,
				CreationUserId = capacityPlanEntity.CreationUserId,
				DepartementId = capacityPlanEntity.DepartementId,
				DepartementName = capacityPlanEntity.DepartementName,
				Factor1HrDaily = capacityPlanEntity.Factor1HrDaily,
				Factor2HrDaily = capacityPlanEntity.Factor2HrDaily,
				Factor3SrDaily = capacityPlanEntity.Factor3SrDaily,
				FormToolInsert = capacityPlanEntity.FormToolInsert,
				HallId = capacityPlanEntity.HallId,
				HallName = capacityPlanEntity.HallName,
				HrHardResourcesNumber = capacityPlanEntity.HrHardResourcesNumber,
				Id = capacityPlanEntity.Id,
				IsArchived = capacityPlanEntity.IsArchived,
				LastUpdateTime = capacityPlanEntity.LastUpdateTime,
				LastUpdateUserId = capacityPlanEntity.LastUpdateUserId,
				OperationId = capacityPlanEntity.OperationId,
				OperationName = capacityPlanEntity.OperationName,
				PlanCapacity = capacityPlanEntity.PlanCapacity,
				ProductivityHrDaily = capacityPlanEntity.ProductivityHrDaily,
				ProductivitySrDaily = capacityPlanEntity.ProductivitySrDaily,
				RequiredEmployees = capacityPlanEntity.RequiredEmployees,
				ShiftsNumberWeekly = capacityPlanEntity.ShiftsNumberWeekly,
				SoftRessourcesNumberDaily = capacityPlanEntity.SoftRessourcesNumberDaily,
				SpecialHoursWeekly = capacityPlanEntity.SpecialHoursWeekly,
				SpecialShiftsWeekly = capacityPlanEntity.SpecialShiftsWeekly,
				ValidationLevel = validationLevel,
				ValidationTime = validationTime,
				ValidationUserId = validationUserId,
				Version = capacityPlanEntity.Version,
				WeekFirstDay = capacityPlanEntity.WeekFirstDay,
				WeekLastDay = capacityPlanEntity.WeekLastDay,
				WeekNumber = capacityPlanEntity.WeekNumber,
				WorkAreaId = capacityPlanEntity.WorkAreaId,
				WorkAreaName = capacityPlanEntity.WorkAreaName,
				WorkingHoursPerShift = capacityPlanEntity.WorkingHoursPerShift,
				WorkStationId = capacityPlanEntity.WorkStationId,
				WorkStationName = capacityPlanEntity.WorkStationName,
				Year = capacityPlanEntity.Year,
			};
		}
	}
}
