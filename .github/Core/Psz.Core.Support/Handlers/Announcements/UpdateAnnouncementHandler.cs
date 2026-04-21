using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Handlers.Announcements
{
	public class UpdateAnnouncementHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private readonly AnnouncementModel _data;
		private readonly UserModel _user;

		public UpdateAnnouncementHandler(AnnouncementModel data, Identity.Models.UserModel user)
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
				//IsActive.HasValue && a.IsActive.Value
				if(_data.IsActive.HasValue && _data.IsActive.Value)
				{
					Infrastructure.Data.Access.Tables._Commun.GoLiveAnnouncementsAccess
						.DeactivateAllExcept(_data.Id);
				}

				var announcementEntity = this._data.ToEntity();
				var response = Infrastructure.Data.Access.Tables._Commun.GoLiveAnnouncementsAccess.Update(announcementEntity);
			

				return ResponseModel<int>.SuccessResponse(response);
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
