using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.CapacityPlan;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlan
{
	public class GetNewPlanFirstWeekHandler: IHandle<NewPlanFirstWeekRequestModel, ResponseModel<int>>
	{
		private NewPlanFirstWeekRequestModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetNewPlanFirstWeekHandler(NewPlanFirstWeekRequestModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.HolidayLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}


					int firstKW = 1;
					if(this.data.Year == DateTime.Today.Year)
					{
						firstKW = Math.Max(firstKW, Helpers.DateTimeHelper.GetIso8601WeekOfYear(DateTime.Today));

						// -
						var capacityEntities = CapacityAccess.GetBYCountryHallYear(this.data.CountryId,
						this.data.HallId, this.data.Year);
						if(capacityEntities != null && capacityEntities.Count > 0)
						{
							firstKW = Helpers.Config.GetCapacityLastEditableWeek() + 1;
						}
					}

					// - Edit next Year on late December
					if(this.data.Year == DateTime.Today.Year + 1 && Helpers.Config.GetLastCapacityEditableDay().Year == DateTime.Today.Year + 1)
					{
						firstKW = 1;
						var capacityEntities = CapacityAccess.GetBYCountryHallYear(this.data.CountryId,
							this.data.HallId, this.data.Year);
						if(capacityEntities != null && capacityEntities.Count > 0)
						{
							firstKW = Helpers.Config.GetCapacityLastEditableWeek() + 1;
						}
					}


					return ResponseModel<int>.SuccessResponse(firstKW);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<int> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
