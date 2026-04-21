using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetBlanketTypesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }


		public GetBlanketTypesHandler(Identity.Models.UserModel user)
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

				// -
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
									Enum.GetValues(typeof(Enums.BlanketEnums.Types)).Cast<Enums.BlanketEnums.Types>()
									?.Select(x => new KeyValuePair<int, string>((int)x, x.GetDescription()))
									?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}



			// -
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
