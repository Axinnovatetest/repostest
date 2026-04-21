using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Handlers.Announcements
{
	public class AddAnnouncementHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private readonly AnnouncementModel _data;
		private readonly UserModel _user;

		public AddAnnouncementHandler(AnnouncementModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				

				var announcementEntity = this._data.ToEntity();
				var newId = Infrastructure.Data.Access.Tables._Commun.GoLiveAnnouncementsAccess.Insert(announcementEntity);

				if(_data.IsActive.HasValue && _data.IsActive.Value)
				{
					Infrastructure.Data.Access.Tables._Commun.GoLiveAnnouncementsAccess.DeactivateAllExcept(newId);
				}


				return ResponseModel<int>.SuccessResponse(newId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
