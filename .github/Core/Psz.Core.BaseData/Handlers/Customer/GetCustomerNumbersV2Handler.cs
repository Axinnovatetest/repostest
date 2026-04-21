using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetCustomerNumbersV2Handler: IHandle<Identity.Models.UserModel,
		ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		public GetCustomerNumbersV2Handler(Identity.Models.UserModel user, string data)
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

				if(string.IsNullOrWhiteSpace(this._data) || this._data.Trim().Length < 2)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(new List<KeyValuePair<int, string>>());
				}

				var responseBody = new List<KeyValuePair<int, string>>();

				var adressenEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetLikeCustomerNumber(this._data.Trim().ToLower())
					?? new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
				var kundeEntities = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByAddressNr(adressenEntities.Select(x => x.Nr).ToList());

				if(adressenEntities != null && adressenEntities.Count > 0)
				{
					foreach(var adressenEntity in adressenEntities)
					{
						var kundeEntity = kundeEntities.FirstOrDefault(x => x.Nummer == adressenEntity.Nr);
						responseBody.Add(new KeyValuePair<int, string>(kundeEntity?.Nr ?? -1, $"{adressenEntity.Kundennummer} || {adressenEntity.Name1.Trim()}"));
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
