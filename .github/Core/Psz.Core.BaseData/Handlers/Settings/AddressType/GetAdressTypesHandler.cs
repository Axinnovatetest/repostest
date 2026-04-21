using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.AddressType
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAdressTypesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAdressTypesHandler(Identity.Models.UserModel user)
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

				var addressTypeEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_typenAccess.Get();
				var S = addressTypeEntities.OrderBy(t => t.Bezeichnung != null)
				.ThenByDescending(t => t.Bezeichnung).ToArray();
				var responseBody = new List<KeyValuePair<int, string>>();

				foreach(var addressTypeEntity in S)
				{
					responseBody.Add(new KeyValuePair<int, string>(addressTypeEntity.ID_typ, $"{addressTypeEntity.ID_typ}||{addressTypeEntity.Bezeichnung}"));
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(responseBody);
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
