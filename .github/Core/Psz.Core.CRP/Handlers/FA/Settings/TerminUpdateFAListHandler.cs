using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Settings
{
	public class TerminUpdateFAListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, int>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public TerminUpdateFAListHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KeyValuePair<int, int>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<KeyValuePair<int, int>> response = new List<KeyValuePair<int, int>>();
				var falistEntity = Infrastructure.Data.Access.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.GetFAsOnly();
				if(falistEntity != null && falistEntity.Count > 0)
					response = falistEntity.Select(x => new KeyValuePair<int, int>(x, x)).ToList();

				return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, int>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, int>>>.AccessDeniedResponse();
			}
			return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse();
		}
	}
}