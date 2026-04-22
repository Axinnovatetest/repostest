using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetWahrungHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetWahrungHandler(Identity.Models.UserModel user)
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

				var wahrungenEntities = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.GetCurrencies();
				var S = wahrungenEntities.OrderBy(t => t.Wahrung != null)
						.ThenBy(t => t.Land).ToArray();
				var response = new List<KeyValuePair<int, string>>();

				foreach(var wahrungenEntity in S)
				{
					response.Add(new KeyValuePair<int, string>(wahrungenEntity.Nr, $"{wahrungenEntity.Wahrung} ({wahrungenEntity.Symbol}) || {wahrungenEntity.Land}"));
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
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

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
