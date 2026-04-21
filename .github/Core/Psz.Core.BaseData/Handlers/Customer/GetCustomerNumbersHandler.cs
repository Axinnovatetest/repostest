using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetCustomerNumbersHandler: IHandle<Identity.Models.UserModel,
		ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		public GetCustomerNumbersHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
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

				if(this._data.Trim().Length < 2)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(new List<KeyValuePair<int, string>>());
				}

				var responseBody = new List<KeyValuePair<int, string>>();

				var adressenEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetLikeCustomerNumber(this._data.Trim().ToLower());
				// var adressenEntities = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetLikeNumber(this._data.Trim().ToLower());

				if(adressenEntities != null && adressenEntities.Count > 0)
				{
					foreach(var adressenEntity in adressenEntities)
					{
						responseBody.Add(new KeyValuePair<int, string>(adressenEntity.Nr, ((int)adressenEntity.Kundennummer).ToString()));

					}
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

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
