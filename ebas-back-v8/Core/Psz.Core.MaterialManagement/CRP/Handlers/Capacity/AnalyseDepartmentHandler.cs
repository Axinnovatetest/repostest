using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class AnalyseDepartmentHandler: IHandle<GetAnalyseReportRequestModel, ResponseModel<AnalyseDepartmentResponseModel>>
	{
		private GetAnalyseReportRequestModel data { get; set; }
		private Identity.Models.UserModel user { get; set; }

		public AnalyseDepartmentHandler(GetAnalyseReportRequestModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<AnalyseDepartmentResponseModel> Handle()
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

		public static ResponseModel<AnalyseDepartmentResponseModel> Perform(Identity.Models.UserModel user,
			GetAnalyseReportRequestModel data)
		{
			#region > Validation
			var countryEntity = CountryAccess.Get(data.CountryId);
			if(countryEntity == null || countryEntity.IsArchived)
			{
				return ResponseModel<AnalyseDepartmentResponseModel>.FailureResponse("Country is not found");
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
				null/*data.DepartementId*/,
				data.WorkAreaId,
				data.WorkStationId);

			var requestedCapacityEntities = CapacityRequiredAccess.Get(data.CountryId,
				data.Year,
				data.WeekFrom,
				weekUntil,
				data.OperationId,
				data.HallId,
				null/*data.DepartementId*/,
				data.WorkAreaId,
				data.WorkStationId);

			var departmentEntities = DepartmentAccess.Get()?.Where(x => x.IsArchived != true)?.ToList();

			var todaysWeekNumber = Helpers.DateTimeHelper.GetWeeksNumberInYear(DateTime.Today);

			var response = new AnalyseDepartmentResponseModel();

			var minValue = data.RoundToMinute.HasValue && data.RoundToMinute.Value ? Helpers.Config.MinAttendance : 0m;
			foreach(var department in departmentEntities)
			{
				var capacityItem = new AnalyseDepartmentResponseModel.Item()
				{
					DepartmentId = department.Id,
					DepartmentName = department.Name,
					Attendance = 0,
					PlanCapacity = 0,
					RequiredEmployees = 0,
				};
				foreach(var capacityEntity in capacityEntities.Where(e => e.DepartementId == department.Id && e.WeekNumber > todaysWeekNumber))
				{
					capacityItem.Attendance += capacityEntity.Attendance;
					capacityItem.PlanCapacity += capacityEntity.PlanCapacity;
					capacityItem.RequiredEmployees += capacityEntity.RequiredEmployees;
				}

				var requestedCapacityItem = new AnalyseDepartmentResponseModel.Item()
				{
					DepartmentId = department.Id,
					DepartmentName = department.Name,
					Attendance = 0,
					PlanCapacity = 0,
					RequiredEmployees = 0,
				};
				foreach(var requestedCapacityEntity in requestedCapacityEntities.Where(e => e.DepartementId == department.Id))
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

			return ResponseModel<AnalyseDepartmentResponseModel>.SuccessResponse(response);
		}

		public ResponseModel<AnalyseDepartmentResponseModel> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
