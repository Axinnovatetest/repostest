using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Holiday;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Holiday
{
	public class Get2PreviousYearsHolidayHandler: IHandle<int, ResponseModel<List<Models.Holiday.PreviousHolidayResponseModel>>>
	{
		private bool distinct { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public Get2PreviousYearsHolidayHandler(bool distinct, Core.Identity.Models.UserModel user)
		{
			this.distinct = distinct;
			this.user = user;
		}

		public ResponseModel<List<Models.Holiday.PreviousHolidayResponseModel>> Handle()
		{
			try
			{
				if(user == null)
				{
					throw new SharedKernel.Exceptions.UnauthorizedException();
				}

				// -
				var holidayEntities = (this.distinct == true
					? HolidayAccess.Get2PreviousYear()
					: HolidayAccess.GetPrevious(DateTime.Today.Year - 1))
					?? new List<HolidayEntity>();
				var userEntities = UserAccess.Get(holidayEntities.Select(x => x.CreationUserId).ToList());
				for(int i = 0; i < holidayEntities.Count; i++)
				{
					holidayEntities[i].IsOverwritten = CreateHolidaysHandler.isHolidayOverwritten(holidayEntities[i].CountryId, holidayEntities[i].HallId, holidayEntities[i].Day);
				}

				// - 
				return ResponseModel<List<Models.Holiday.PreviousHolidayResponseModel>>.SuccessResponse(
					holidayEntities.Select(x =>
						new PreviousHolidayResponseModel(x, userEntities?.Find(y => y.Id == x.CreationUserId)?.Name, user.SelectedLanguage)).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Holiday.PreviousHolidayResponseModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
