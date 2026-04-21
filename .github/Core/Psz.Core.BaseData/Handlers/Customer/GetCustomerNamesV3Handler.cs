using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetCustomerNamesV3Handler: IHandle<Identity.Models.UserModel,
		ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		public GetCustomerNamesV3Handler(Identity.Models.UserModel user, string data)
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

				this._data = this._data ?? "";

				var responseBody = new List<KeyValuePair<int, string>>();

				var adressenEntities = new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
				adressenEntities.AddRange(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetLikeCustomerName(this._data.Trim().ToLower())
					?? new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>());
				adressenEntities.AddRange(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetLikeCustomerNumber(this._data.Trim().ToLower())
					?? new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>());

				if(adressenEntities != null && adressenEntities.Count > 0)
				{
					adressenEntities = adressenEntities?.DistinctBy(x => x.Nr).ToList();
					var kundeEntities = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByAddressNr(adressenEntities.Select(x => x.Nr).ToList());
					var results = new List<KeyValuePair<int, string>>();
					foreach(var adressenEntity in adressenEntities)
					{
						var kundeEntity = kundeEntities.FirstOrDefault(x => x.Nummer == adressenEntity.Nr);
						results.Add(new KeyValuePair<int, string>(kundeEntity?.Nr ?? -1, $"{adressenEntity.Kundennummer} || {(adressenEntity.Adresstyp == 3 ? "[Lieferadresse] " : "")}{adressenEntity.Name1.Trim()}"));
					}
					responseBody = results.Where(x => x.Key > 0).OrderBy(x => x.Key).ToList();
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
