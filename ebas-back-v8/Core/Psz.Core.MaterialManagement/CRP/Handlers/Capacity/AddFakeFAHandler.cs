using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class AddFakeFAHandler: IHandle<FakeFARequestModel, ResponseModel<int>>
	{
		private FakeFARequestModel data { get; set; }
		private Identity.Models.UserModel user { get; set; }

		public AddFakeFAHandler(FakeFARequestModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					if(user == null)
					{
						throw new SharedKernel.Exceptions.UnauthorizedException();
					}

					return Perform(this.user, this.data);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<int> Perform(Identity.Models.UserModel user,
			FakeFARequestModel data)
		{
			#region > Validation
			var countryEntity = CountryAccess.Get(data.CountryId);
			if(countryEntity == null || countryEntity.IsArchived)
			{
				return ResponseModel<int>.FailureResponse("Country is not found");
			}
			#endregion

			var faConfig = Infrastructure.Data.Access.Tables.MTM.ConfigurationHeaderAccess.GetByCountryHall(data.CountryId, data.UnitId);
			if(faConfig != null)
			{
				var faConfigDetails = Infrastructure.Data.Access.Tables.MTM.ConfigurationDetailsAccess.GetByHeaders(new List<int> { faConfig.Id });

				var weeks1 = new List<ConfigurationDetailsEntity>();
				var weeks2 = new List<ConfigurationDetailsEntity>();
				var weeks3 = new List<ConfigurationDetailsEntity>();
				if(data.Quantity <= faConfig.ProductionOrderThreshold)
				{
					weeks1 = faConfigDetails.FindAll(x => x.DepartmentWeekNumber == 1 && x.IsLowerThanThreshold == true);
					weeks2 = faConfigDetails.FindAll(x => x.DepartmentWeekNumber == 2 && x.IsLowerThanThreshold == true);
				}
				else
				{
					weeks1 = faConfigDetails.FindAll(x => x.DepartmentWeekNumber == 1 && x.IsLowerThanThreshold == false);
					weeks2 = faConfigDetails.FindAll(x => x.DepartmentWeekNumber == 2 && x.IsLowerThanThreshold == false);
					weeks3 = faConfigDetails.FindAll(x => x.DepartmentWeekNumber == 3 && x.IsLowerThanThreshold == false);
				}

				// - WPLs
				var wpls = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetByArticleId(data.ArticleId);
				if(wpls != null && wpls.Count > 0)
				{
					var wpl = wpls.Find(x => x.IsActive);
					if(wpl != null)
					{
						var wplDetails = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByWorkScheduleId(wpl.Id);
						if(wplDetails != null && wplDetails.Count > 0)
						{
							// - W1
							var week1Operations = wplDetails.FindAll(x => weeks1.FindIndex(y => y.DepartmentId == x.DepartementId) >= 0);
							if(week1Operations != null && week1Operations.Count > 0)
							{
								Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.Insert(
									week1Operations.Select(x => getCapacity(Helpers.DateTimeHelper.GetIso8601WeekOfYear(data.Date), data.Quantity, x))
									?.ToList());
							}

							// - W2
							var week2Operations = wplDetails.FindAll(x => weeks2.FindIndex(y => y.DepartmentId == x.DepartementId) >= 0);
							if(week2Operations != null && week2Operations.Count > 0)
							{
								Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.Insert(
									week2Operations.Select(x => getCapacity(Helpers.DateTimeHelper.GetIso8601WeekOfYear(data.Date) - 1, data.Quantity, x))
									?.ToList());
							}

							// - W3
							if(weeks3 != null && weeks3.Count > 0)
							{
								var week3Operations = wplDetails.FindAll(x => weeks3.FindIndex(y => y.DepartmentId == x.DepartementId) >= 0);
								if(week3Operations != null && week3Operations.Count > 0)
								{
									Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.Insert(
										week3Operations.Select(x => getCapacity(Helpers.DateTimeHelper.GetIso8601WeekOfYear(data.Date) - 2, data.Quantity, x))
										?.ToList());
								}

							}
						}
					}
				}
			}

			return ResponseModel<int>.SuccessResponse(0);
		}

		public ResponseModel<int> Validate()
		{
			throw new NotImplementedException();
		}
		static CapacityRequiredEntity getCapacity(int week, decimal quantity, Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity detailsEntity)
		{
			return new CapacityRequiredEntity
			{
				ArchiveTime = null,
				ArchiveUserId = null,
				Attendance = quantity * getTotalTimeOperation(detailsEntity, quantity) / 60,
				AvailableHrDaily = 0,
				AvailableSrDaily = 0,
				CountryId = detailsEntity.CountryId,
				CountryName = "",
				CreationTime = DateTime.Now,
				CreationUserId = -1,
				DepartementId = detailsEntity.DepartementId,
				DepartementName = "",
				Factor1HrDaily = 0,
				Factor2HrDaily = 0,
				Factor3SrDaily = 0,
				FormToolInsert = detailsEntity.FromToolInsert,
				HallId = detailsEntity.HallId,
				HallName = "",
				HrHardResourcesNumber = 0,
				Id = -1,
				IsArchived = false,
				LastUpdateTime = null,
				LastUpdateUserId = null,
				OperationId = detailsEntity.StandardOperationId,
				OperationName = "",
				PlanCapacity = 0,
				ProductivityHrDaily = 0,
				ProductivitySrDaily = 0,
				RequiredEmployees = 0,
				ShiftsNumberWeekly = 0,
				SoftRessourcesNumberDaily = 0,
				SourceId = detailsEntity.Id,
				SpecialHoursWeekly = 0,
				SpecialShiftsWeekly = 0,
				Version = 0,
				WeekFirstDay = DateTime.Today,
				WeekLastDay = DateTime.Today,
				WeekNumber = week,
				WorkAreaId = detailsEntity.WorkAreaId,
				WorkAreaName = "",
				WorkStationId = detailsEntity.WorkStationMachineId,
				WorkStationName = detailsEntity.WorkStationMachineId.ToString(),
				Year = 2021,
			};
		}
		static decimal getTotalTimeOperation(Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity workSchedule, decimal faQuantity)
		{
			// Formula taken from excel doc
			try
			{
				workSchedule.TotalTimeOperation = 0;
				// Lot
				if(workSchedule.RelationOperationTime == 0)
				{
					workSchedule.TotalTimeOperation =
						workSchedule.Amount * workSchedule.OperationTimeSeconds / 60 / faQuantity
						+ workSchedule.SetupTimeMinutes / faQuantity;
				}
				else
				{
					// Piece
					if(workSchedule.RelationOperationTime == 1)
					{
						workSchedule.TotalTimeOperation =
							workSchedule.Amount * workSchedule.OperationTimeSeconds / 60
							+ workSchedule.SetupTimeMinutes / faQuantity;
					}
				}

				return Math.Truncate(workSchedule.TotalTimeOperation * 100000) / 100000;
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
				return 0m;
			}
		}
	}
}
