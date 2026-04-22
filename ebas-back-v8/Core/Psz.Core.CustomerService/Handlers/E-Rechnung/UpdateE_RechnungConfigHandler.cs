using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class UpdateE_RechnungConfigHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private E_RechnungConfigRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateE_RechnungConfigHandler(Identity.Models.UserModel user, E_RechnungConfigRequestModel data)
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
				var entity = _data.ToEntity();
				entity.EmailBody = entity.EmailBody.Replace("<p>", "<p style='margin:0'>");
				var result = Infrastructure.Data.Access.Joins.CTS.Divers.UpdateE_Rechnung_Config(entity);

				return ResponseModel<int>.SuccessResponse(result);
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

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
