using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class GetAnalyseReportHandler: IHandle<GetAnalyseReportRequestModel, ResponseModel<GetAnalyseReportResponseModel>>
	{
		private GetAnalyseReportRequestModel data { get; set; }
		private Identity.Models.UserModel user { get; set; }

		public GetAnalyseReportHandler(GetAnalyseReportRequestModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<GetAnalyseReportResponseModel> Handle()
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

		public static ResponseModel<GetAnalyseReportResponseModel> Perform(Identity.Models.UserModel user,
			GetAnalyseReportRequestModel data)
		{
			#region > Validation
			var countryEntity = CountryAccess.Get(data.CountryId);
			if(countryEntity == null || countryEntity.IsArchived)
			{
				return ResponseModel<GetAnalyseReportResponseModel>.FailureResponse("Country is not found");
			}
			#endregion

			var wideView = (data.WeekUntil ?? Helpers.DateTimeHelper.GetWeeksNumberInYear(new DateTime(data.Year, 12, 31))) - data.WeekFrom > 4;

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
				data.WorkStationId);

			#region // - Add Holidays - //
			capacityEntities = Helpers.Capacity.AddHolidays(
				capacityEntities, data.CountryId, data.HallId, data.Year, data.WeekFrom, weekUntil);
			#endregion Holidays

			var requestedCapacityEntities = CapacityRequiredAccess.Get(data.CountryId,
				data.Year,
				data.WeekFrom,
				weekUntil,
				data.OperationId,
				data.HallId,
				data.DepartementId,
				data.WorkAreaId,
				data.WorkStationId);

			var countryEntities = CountryAccess.Get();
			var hallEntities = HallAccess.Get();
			var lagerortId = getLagerortId(countryEntities?.Find(x => x.Id == data.CountryId)?.Name, hallEntities?.Find(x => x.Id == data.HallId)?.Name);

			var faultyFertigungEntities =
					Infrastructure.Data.Access.Joins.MTM.CRP.FertigungFaultyTimeAccess.Get(
					lagerortId,
					Helpers.DateTimeHelper.FirstDateOfWeekISO8601(data.Year, data.WeekFrom),
					Helpers.DateTimeHelper.FirstDateOfWeekISO8601(data.Year, weekUntil)
				)
				?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.FertigungFaultyTimeEntity>();

			var response = new GetAnalyseReportResponseModel();

			response.FaultyFAs = faultyFertigungEntities.Select(x => new GetAnalyseReportResponseModel.FaItem(Helpers.DateTimeHelper.GetWeeksNumberInYear(x.FaDate.HasValue ? x.FaDate.Value : DateTime.MinValue), x))?.ToList();

			for(int weekNumber = data.WeekFrom; weekNumber < (weekUntil + 1); weekNumber++)
			{
				var capacityItem = new GetAnalyseReportResponseModel.Item()
				{
					WeekNumber = weekNumber,
					Attendance = 0,
					PlanCapacity = 0,
					RequiredEmployees = 0,
					FaultyFARequiredCapacities = 0
				};
				foreach(var capacityEntity in capacityEntities.Where(e => e.WeekNumber == weekNumber))
				{
					capacityItem.Attendance += capacityEntity.Attendance;
					capacityItem.PlanCapacity += capacityEntity.PlanCapacity;
					capacityItem.RequiredEmployees += capacityEntity.RequiredEmployees;
				}

				// - ignore capacity for past date!
				if(data.Year <= DateTime.Today.Year)
				{
					if(weekNumber < Helpers.DateTimeHelper.GetWeeksNumberInYear(DateTime.Today))
					{
						capacityItem.Attendance = 0;
						capacityItem.PlanCapacity = 0;
					}
				}

				if(data.RoundToMinute.HasValue && data.RoundToMinute.Value)
				{
					if(capacityItem.Attendance <= Helpers.Config.MinAttendance)
					{
						capacityItem.Attendance = 0;
					}
					if(capacityItem.RequiredEmployees <= Helpers.Config.MinAttendance)
					{
						capacityItem.RequiredEmployees = 0;
					}
					if(capacityItem.PlanCapacity <= Helpers.Config.MinAttendance)
					{
						capacityItem.PlanCapacity = 0;
					}
				}
				capacityItem.Attendance = Common.Helpers.MathHelper.RoundDecimal(capacityItem.Attendance, Helpers.Config.DecimalPart);
				capacityItem.RequiredEmployees = Common.Helpers.MathHelper.RoundDecimal(capacityItem.RequiredEmployees, Helpers.Config.DecimalPart);
				response.Capacities.Add(capacityItem);

				var requestedCapacityItem = new GetAnalyseReportResponseModel.Item()
				{
					WeekNumber = weekNumber,
					Attendance = 0,
					PlanCapacity = 0,
					RequiredEmployees = 0,
					FaultyFARequiredCapacities = 0
				};
				foreach(var requestedCapacityEntity in requestedCapacityEntities.Where(e => e.WeekNumber == weekNumber))
				{
					requestedCapacityItem.Attendance += requestedCapacityEntity.Attendance;
					requestedCapacityItem.PlanCapacity += requestedCapacityEntity.PlanCapacity;
					requestedCapacityItem.RequiredEmployees += requestedCapacityEntity.RequiredEmployees;
				}
				foreach(var faultyFA in faultyFertigungEntities.Where(e => e.FaDate.HasValue && Helpers.DateTimeHelper.GetIso8601WeekOfYear(e.FaDate.Value) == weekNumber))
				{
					requestedCapacityItem.FaultyFARequiredCapacities += faultyFA.FaTotalTime;
				}

				if(data.RoundToMinute.HasValue && data.RoundToMinute.Value)
				{
					if(requestedCapacityItem.Attendance <= Helpers.Config.MinAttendance)
					{
						requestedCapacityItem.Attendance = 0;
					}
					if(requestedCapacityItem.RequiredEmployees <= Helpers.Config.MinAttendance)
					{
						requestedCapacityItem.RequiredEmployees = 0;
					}
					if(requestedCapacityItem.PlanCapacity <= Helpers.Config.MinAttendance)
					{
						requestedCapacityItem.PlanCapacity = 0;
					}
					if(requestedCapacityItem.FaultyFARequiredCapacities <= Helpers.Config.MinAttendance)
					{
						requestedCapacityItem.FaultyFARequiredCapacities = 0;
					}
				}

				requestedCapacityItem.Attendance = Common.Helpers.MathHelper.RoundDecimal(requestedCapacityItem.Attendance, Helpers.Config.DecimalPart);
				requestedCapacityItem.RequiredEmployees = Common.Helpers.MathHelper.RoundDecimal(requestedCapacityItem.RequiredEmployees, Helpers.Config.DecimalPart);
				requestedCapacityItem.FaultyFARequiredCapacities = Common.Helpers.MathHelper.RoundDecimal(requestedCapacityItem.FaultyFARequiredCapacities, Helpers.Config.DecimalPart);

				response.RequestedCapacities.Add(requestedCapacityItem);
			}


			// - squeeze value to first delivery delay, if wideView
			if(wideView)
			{
				int i = 0;
				for(; i < response.RequestedCapacities.Count; i++)
				{
					if(response.RequestedCapacities[i].Attendance > 0)
						break;
				}

				if(i < response.RequestedCapacities.Count - 1)
				{
					i = i > 0 ? i - 1 : i; // >>> include prev element for better visualisation
					var _response = new GetAnalyseReportResponseModel();
					_response.Capacities = response.Capacities.Where((x, idx) => idx >= i)?.ToList();
					_response.RequestedCapacities = response.RequestedCapacities.Where((x, idx) => idx >= i)?.ToList();
					_response.FaultyFAs = response.FaultyFAs;
					response = _response;
				}
			}

			// -
			return ResponseModel<GetAnalyseReportResponseModel>.SuccessResponse(response);
		}

		public ResponseModel<GetAnalyseReportResponseModel> Validate()
		{
			throw new NotImplementedException();
		}

		internal static List<int> getLagerortId(string countryName, string hallName)
		{
			if(countryName?.ToLower()?.StartsWith("alb") == true)
			{
				return new List<int> { 26 };
			}
			else if(countryName?.ToLower()?.StartsWith("cz") == true)
			{
				if(hallName?.ToLower()?.Contains("1") == true)
					return null;

				if(hallName?.ToLower()?.Contains("2") == true)
					return null;

				return new List<int> { 6 };
			}
			else if(countryName?.ToLower()?.StartsWith("ger") == true)
			{
				return new List<int> { 15 };
			}
			else if(countryName?.ToLower()?.StartsWith("tun") == true)
			{
				if(hallName?.ToLower()?.Contains("1") == true)
					return new List<int> { 7 };

				if(hallName?.ToLower()?.Contains("2") == true)
					return new List<int> { 60 };

				if(hallName?.ToLower()?.Contains("3") == true)
					return new List<int> { 42 };

				return new List<int> { 7, 60, 42 };
			}

			return null;
		}
	}
}
