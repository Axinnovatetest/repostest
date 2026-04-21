using Psz.Core.BaseData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Handlers.Announcements
{
	public class GetAnnouncementHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<AnnouncementModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		//public string _data { get; set; }

		public GetAnnouncementHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		//	this._data = _data;
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

				var allAnnouncements = Infrastructure.Data.Access.Tables._Commun.GoLiveAnnouncementsAccess
					.Get()
					.Where(a => a.IsActive.HasValue && a.IsActive.Value)  
			        .ToList();
				;
				var readEntries = Infrastructure.Data.Access.Tables._Commun.GoLiveAnnouncements_ReadAccess
					.Get()
					.Where(r => r.UserId == _user.Id)
					.Select(r => r.AnnouncementId)
					.ToList();

				var unreadAnnouncements = allAnnouncements
					.Where(a => !readEntries.Contains(a.Id))
					.Select(a => new AnnouncementModel(a))
					.ToList();

				return ResponseModel<List<AnnouncementModel>>.SuccessResponse(unreadAnnouncements);
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
