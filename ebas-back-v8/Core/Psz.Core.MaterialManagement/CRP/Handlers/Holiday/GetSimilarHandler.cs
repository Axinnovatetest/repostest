using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Holiday;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Holiday
{
	public class GetSimilarHandler: IHandle<int, ResponseModel<List<Models.Holiday.HolidayModel>>>
	{
		private int id { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetSimilarHandler(int id, Core.Identity.Models.UserModel user)
		{
			this.id = id;
			this.user = user;
		}

		public ResponseModel<List<Models.Holiday.HolidayModel>> Handle()
		{
			try
			{
				if(user == null)
				{
					throw new SharedKernel.Exceptions.UnauthorizedException();
				}

				// -
				var holidayEntities = HolidayAccess.GetSimilar(this.id)
					?? new List<HolidayEntity>();
				var userEntities = UserAccess.Get(holidayEntities.Select(x => x.CreationUserId).ToList());
				for(int i = 0; i < holidayEntities.Count; i++)
				{
					holidayEntities[i].IsOverwritten = CreateHolidaysHandler.isHolidayOverwritten(holidayEntities[i].CountryId, holidayEntities[i].HallId, holidayEntities[i].Day);
				}

				// - 
				return ResponseModel<List<Models.Holiday.HolidayModel>>.SuccessResponse(
					holidayEntities.Select(x =>
						new HolidayModel(x, userEntities?.Find(y => y.Id == x.CreationUserId)?.Name, user.SelectedLanguage)).ToList());
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
