using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class UpdateRahmenPositionCommentHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly UpdateRahmenPositionCommentRequestModel _data;

		public UpdateRahmenPositionCommentHandler(UserModel user, UpdateRahmenPositionCommentRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
				var extensionPosition = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.Get(_data.PositionExtensionId);
				var position = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(extensionPosition.AngeboteArtikelNr);
				var rahmen = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(position.AngebotNr ?? -1);
				if(extensionPosition.Comment != _data.Comment)
					logs.Add(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
					{
						AngebotNr = rahmen.Angebot_Nr,
						DateTime = DateTime.Now,
						LogObject = "Rahmenauftrag",
						LogText = $"[Rahmenauftrag] [{rahmen.Angebot_Nr}] Position [{position.Position}] Modified-[Comment]",
						LogType = "MODIFICATIONPOS",
						Nr = rahmen.Nr,
						Origin = "MTM",
						PositionNr = position.Nr,
						ProjektNr = int.TryParse(rahmen.Projekt_Nr, out var p) ? p : null,
						UserId = _user.Id,
						Username = _user.Name
					});
				if(extensionPosition.AB_nummer != _data.ABNummer)
					logs.Add(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
					{
						AngebotNr = rahmen.Angebot_Nr,
						DateTime = DateTime.Now,
						LogObject = "Rahmenauftrag",
						LogText = $"[Rahmenauftrag] [{rahmen.Angebot_Nr}] Position [{position.Position}] Modified-[AB_nummer] changed from [{extensionPosition.AB_nummer}] to [{_data.ABNummer}]",
						LogType = "MODIFICATIONPOS",
						Nr = rahmen.Nr,
						Origin = "MTM",
						PositionNr = position.Nr,
						ProjektNr = int.TryParse(rahmen.Projekt_Nr, out var p) ? p : null,
						UserId = _user.Id,
						Username = _user.Name
					});
				extensionPosition.Comment = _data.Comment;
				extensionPosition.AB_nummer = _data.ABNummer;
				var response = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.Update(extensionPosition);
				if(logs != null && logs.Count > 0)
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(logs);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(_user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			return ResponseModel<int>.SuccessResponse();
		}
	}
}