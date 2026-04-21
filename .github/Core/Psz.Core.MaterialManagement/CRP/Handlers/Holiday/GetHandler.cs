using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Holiday;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Holiday
{
	public class GetHandler: IHandle<int, ResponseModel<List<Models.Holiday.HolidayModel>>>
	{
		private Models.Holiday.GetModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetHandler(Models.Holiday.GetModel data,
			Core.Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<List<Models.Holiday.HolidayModel>> Handle()
		{
			try
			{
				if(user == null)
				{
					throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
				}

				if(this.data.Year < 1900 || 9999 < this.data.Year)
				{
					return ResponseModel<List<Models.Holiday.HolidayModel>>.FailureResponse("Year: Invalid value");
				}

				var fromDay = new DateTime(this.data.Year, 01, 01);
				var untilDay = new DateTime(this.data.Year, 12, 31);

				var holidayEntities = HolidayAccess.GetBy_CountryId_HallId_DayRange(this.data.CurrentCountryId,
					this.data.CurrentHallId,
					fromDay,
					untilDay,
					null,
					null);

				if(this.data.WeekEndsOnly)
					holidayEntities = holidayEntities?.Where(x => x.Day.DayOfWeek == DayOfWeek.Saturday || x.Day.DayOfWeek == DayOfWeek.Sunday)?.ToList();

				var creationUserIds = holidayEntities.Select(e => e.CreationUserId).Distinct().ToList();
				var creationUserEntities = Infrastructure.Data.Access.Tables.WPL.UserAccess.Get(creationUserIds);
				var specialShifts = Infrastructure.Data.Access.Tables.MTM.CapacityAccess.GetSpecialShifts(this.data.Year, this.data.CurrentCountryId, this.data.CurrentHallId, null)
					?? new List<Infrastructure.Data.Entities.Tables.MTM.CapacityEntity>();

				var response = new List<Models.Holiday.HolidayModel>();

				foreach(var holidayEntity in holidayEntities)
				{
					var creationUserName = creationUserEntities.Find(e => e.Id == holidayEntity.CreationUserId)?.Name;
					var weekSpecialShifts = specialShifts.FindAll(x => x.WeekNumber == holidayEntity.WeekNumber);

					holidayEntity.IsOverwritten = (holidayEntity.Day.DayOfWeek == DayOfWeek.Saturday || holidayEntity.Day.DayOfWeek == DayOfWeek.Sunday)
						&& weekSpecialShifts != null && weekSpecialShifts.Count > 0;
					response.Add(new HolidayModel(holidayEntity, creationUserName, user.SelectedLanguage));
				}

				return ResponseModel<List<Models.Holiday.HolidayModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Holiday.HolidayModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
