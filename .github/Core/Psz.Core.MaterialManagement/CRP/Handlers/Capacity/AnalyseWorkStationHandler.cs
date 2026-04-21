using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class AnalyseWorkStationHandler: IHandle<GetAnalyseReportRequestModel, ResponseModel<AnalyseWorkStationResponseModel>>
	{
		private GetAnalyseReportRequestModel data { get; set; }
		private Identity.Models.UserModel user { get; set; }

		public AnalyseWorkStationHandler(GetAnalyseReportRequestModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<AnalyseWorkStationResponseModel> Handle()
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
					throw;// e;
				}
			}
		}

		public static ResponseModel<AnalyseWorkStationResponseModel> Perform(Identity.Models.UserModel user,
			GetAnalyseReportRequestModel data)
		{
			#region > Validation
			var countryEntity = CountryAccess.Get(data.CountryId);
			if(countryEntity == null || countryEntity.IsArchived)
			{
				return ResponseModel<AnalyseWorkStationResponseModel>.FailureResponse("Country is not found");
			}
			#endregion

			var wideView = (data.WeekUntil ?? Helpers.DateTimeHelper.GetWeeksNumberInYear(new DateTime(DateTime.Today.Year, 12, 31))) - data.WeekFrom > 4;

			var weekUntil = data.WeekUntil ?? (data.WeekFrom + (6 * 4));
			if(weekUntil > 53)
			{
				weekUntil = 53;
			}

			var capacityEntities = CapacityAccess.Get(data.CountryId,
				data.Year,
				data.WeekFrom,
				weekUntil,
				data.OperationId,
				data.HallId,
				data.DepartementId,
				data.WorkAreaId,
				null/*data.WorkStationId*/);

			var requestedCapacityEntities = CapacityRequiredAccess.Get(data.CountryId,
				data.Year,
				data.WeekFrom,
				weekUntil,
				data.OperationId,
				data.HallId,
				data.DepartementId,
				data.WorkAreaId,
				null/*data.WorkStationId*/);

			var workStationEntities = WorkStationMachineAccess.Get()?.Where(x => x.IsArchived != true
				&& data.CountryId == x.CountryId
				&& (!data.HallId.HasValue || data.HallId.HasValue && data.HallId.Value == x.HallId)
				)?.ToList();

			// - add template for operations w/o WS
			int fakeId = -1;
			workStationEntities.Add(new Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity
			{
				CountryId = data.CountryId,
				CreationTime = DateTime.Now,
				CreationUserId = -2,
				DeleteTime = null,
				DeleteUserId = null,
				HallId = data.HallId ?? -1,
				Id = fakeId,
				IsArchived = false,
				LastEditTime = DateTime.Now,
				LastEditUserId = -2,
				Name = "<Empty>",
				Type = 0,
				WorkAreaId = data.WorkAreaId ?? -1
			});
			foreach(var item in capacityEntities)
			{
				if(!item.WorkStationId.HasValue)
					item.WorkStationId = fakeId;
			}
			foreach(var item in requestedCapacityEntities)
			{
				if(!item.WorkStationId.HasValue)
					item.WorkStationId = fakeId;
			}

			var todaysWeekNumber = Helpers.DateTimeHelper.GetWeeksNumberInYear(DateTime.Today);

			var response = new AnalyseWorkStationResponseModel();

			var minValue = data.RoundToMinute.HasValue && data.RoundToMinute.Value ? Helpers.Config.MinAttendance : 0m;
			foreach(var workStation in workStationEntities)
			{
				var capacityItem = new AnalyseWorkStationResponseModel.Item()
				{
					WorkStationId = workStation.Id,
					WorkStationName = workStation.Name,
					Attendance = 0,
					PlanCapacity = 0,
					RequiredEmployees = 0,
				};
				foreach(var capacityEntity in capacityEntities.Where(e => e.WorkStationId == workStation.Id && e.WeekNumber > todaysWeekNumber))
				{
					capacityItem.Attendance += capacityEntity.Attendance;
					capacityItem.PlanCapacity += capacityEntity.PlanCapacity;
					capacityItem.RequiredEmployees += capacityEntity.RequiredEmployees;
				}

				var requestedCapacityItem = new AnalyseWorkStationResponseModel.Item()
				{
					WorkStationId = workStation.Id,
					WorkStationName = workStation.Name,
					Attendance = 0,
					PlanCapacity = 0,
					RequiredEmployees = 0,
				};
				foreach(var requestedCapacityEntity in requestedCapacityEntities.Where(e => e.WorkStationId == workStation.Id))
				{
					requestedCapacityItem.Attendance += requestedCapacityEntity.Attendance;
					requestedCapacityItem.PlanCapacity += requestedCapacityEntity.PlanCapacity;
					requestedCapacityItem.RequiredEmployees += requestedCapacityEntity.RequiredEmployees;
				}
				// -
				if(capacityItem.Attendance >= minValue || requestedCapacityItem.Attendance >= minValue)
				{
					capacityItem.Attendance = Common.Helpers.MathHelper.RoundDecimal(capacityItem.Attendance, Helpers.Config.DecimalPart);
					capacityItem.RequiredEmployees = Common.Helpers.MathHelper.RoundDecimal(capacityItem.RequiredEmployees, Helpers.Config.DecimalPart);
					response.Capacities.Add(capacityItem);
					response.RequestedCapacities.Add(requestedCapacityItem);
				}
			}

			return ResponseModel<AnalyseWorkStationResponseModel>.SuccessResponse(response);
		}

		public ResponseModel<AnalyseWorkStationResponseModel> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
