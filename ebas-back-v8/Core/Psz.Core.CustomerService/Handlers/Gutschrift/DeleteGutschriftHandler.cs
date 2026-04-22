using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class DeleteGutschriftHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteGutschriftHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
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

				var response = -1;

				var gutschriftItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(_data);
				var gutschriftEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
				response += Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Delete(_data);
				response += Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Delete(gutschriftItemsEntities?.Select(x => x.Nr)?.ToList());

				//logging
				var _log = new LogHelper(gutschriftEntity.Nr, (int)gutschriftEntity.Angebot_Nr, int.TryParse(gutschriftEntity.Projekt_Nr, out var v) ? v : 0, gutschriftEntity.Typ, LogHelper.LogType.DELETIONOBJECT, "CTS", _user)
					 .LogCTS(null, null, null, 0);
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var gutschriftEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if(gutschriftEntity == null)
				return ResponseModel<int>.FailureResponse("gutschrift not found .");
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
