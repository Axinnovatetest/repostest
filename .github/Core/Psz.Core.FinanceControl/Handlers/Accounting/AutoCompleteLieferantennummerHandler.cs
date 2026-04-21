using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting
{
	public class AutoCompleteLieferantennummerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<int?>>>
	{

		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AutoCompleteLieferantennummerHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<int?>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var entites = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess2.GetLikeNumber(_data);
				var response = entites?.Select(x => x.nummer).ToList();

				return ResponseModel<List<int?>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<int?>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<int?>>.AccessDeniedResponse();
			}

			return ResponseModel<List<int?>>.SuccessResponse();
		}

	}
}
