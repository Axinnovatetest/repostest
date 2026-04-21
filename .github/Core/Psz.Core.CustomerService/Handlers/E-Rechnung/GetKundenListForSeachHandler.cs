using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class GetKundenListForSeachHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public GetKundenListForSeachHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var kundenList = Infrastructure.Data.Access.Joins.CTS.Divers.GetKundenListForE_Rechnung();
				var result = kundenList?.Select(x =>
				new KeyValuePair<int, string>(x.value2 ?? -1, $"{x.value1}||{x.value3}||{x.value4}")).ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}

	}
}
