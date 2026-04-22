using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Handlers.Announcements
{
	public class GetAnnouncementAdminHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<AnnouncementModel>>>
	{
		private readonly UserModel _user;

		public GetAnnouncementAdminHandler(Identity.Models.UserModel user)
		{
			_user = user;
		}

		public ResponseModel<List<AnnouncementModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<AnnouncementModel>();
				var announcemententity = Infrastructure.Data.Access.Tables._Commun.GoLiveAnnouncementsAccess
					.Get()
					//.Where(a => a.IsActive == true) 
			  //      .ToList();
				;
				if(announcemententity != null && announcemententity.Count > 0)
					response = announcemententity.Select(k => new AnnouncementModel(k)).ToList();

				return ResponseModel<List<AnnouncementModel>>.SuccessResponse(response);


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<AnnouncementModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<AnnouncementModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<AnnouncementModel>>.SuccessResponse();
		}
	}
}
