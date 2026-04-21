using Infrastructure.Data.Entities.Tables._Commun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Handlers.Announcements
{
	public class MarkAnnouncementAsReadHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly int _announcementId;

		public MarkAnnouncementAsReadHandler(Identity.Models.UserModel user,int announcementId)
		{
			this._user = user;
			this._announcementId = announcementId;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validation = this.Validate();
				if(!validation.Success)
					return validation;

				var existing = Infrastructure.Data.Access.Tables._Commun.GoLiveAnnouncementsAccess
					.GetById(this._announcementId);

				if(existing != null)
				{
					var newRead = new GoLiveAnnouncements_ReadEntity
					{
						UserId = _user.Id,
						Username = _user.Name,
						AnnouncementId = _announcementId,
						ReadAt = DateTime.Now
					};

					Infrastructure.Data.Access.Tables._Commun.GoLiveAnnouncements_ReadAccess.Insert(newRead);
				}

				return ResponseModel<int>.SuccessResponse(_announcementId);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null || this._announcementId<=0)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
