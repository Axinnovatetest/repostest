using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.MaterialManagement.CRP.Models.Holiday;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Holiday
{
	public class GetDetailsHandler: IHandle<int, ResponseModel<Models.Holiday.HolidayModel>>
	{
		private int id { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetDetailsHandler(int id,
			Core.Identity.Models.UserModel user)
		{
			this.id = id;
			this.user = user;
		}

		public ResponseModel<Models.Holiday.HolidayModel> Handle()
		{
			try
			{
				if(user == null)
				{
					throw new SharedKernel.Exceptions.UnauthorizedException();
				}

				var holidayEntity = HolidayAccess.Get(this.id);

				if(holidayEntity == null || holidayEntity.IsArchived)
				{
					throw new SharedKernel.Exceptions.NotFoundException();
				}

				// MISSING: check user's hall access
				holidayEntity.IsOverwritten = CreateHolidaysHandler.isHolidayOverwritten(holidayEntity.CountryId, holidayEntity.HallId, holidayEntity.Day);
				var response = new HolidayModel(holidayEntity, UserAccess.Get(holidayEntity.CreationUserId)?.Name, user.SelectedLanguage);

				return ResponseModel<Models.Holiday.HolidayModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Holiday.HolidayModel> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
