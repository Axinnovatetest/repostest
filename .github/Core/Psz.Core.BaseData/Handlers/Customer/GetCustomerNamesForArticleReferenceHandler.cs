using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Psz.Core.Common.Models;
using Psz.Core.BaseData.Models;

namespace Psz.Core.BaseData.Handlers.Customer
{
	public class GetCustomerNamesForArticleReferenceHandler: IHandle<Identity.Models.UserModel,
		ResponseModel<List<GetCustomerNamesForArticleReferenceModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private GetCustomerNamesForArticleReferenceRequestModel _data { get; set; }
		public GetCustomerNamesForArticleReferenceHandler(Identity.Models.UserModel user, GetCustomerNamesForArticleReferenceRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<GetCustomerNamesForArticleReferenceModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//if(string.IsNullOrWhiteSpace(this._data.searchtext) || this._data.searchtext.Trim().Length < 2)
				//{
				//	return ResponseModel<List<GetCustomerNamesForArticleReferenceModel>>.SuccessResponse(new List<GetCustomerNamesForArticleReferenceModel>());
				//}

				var customertoExeclude = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.GetByArtikelId(this._data.ArtikelId);
				var customerNumberstoExeclude = new List<int?>();
				if(customertoExeclude != null && customertoExeclude.Count > 0)
				{
					customerNumberstoExeclude = customertoExeclude.Select(x => x.CustomerNumber).ToList();
				}

				var responseBody = new List<GetCustomerNamesForArticleReferenceModel>();

				var adressenEntities = new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
				adressenEntities.AddRange(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetLikeCustomerName(this._data.searchtext.Trim().ToLower())
					?? new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>());
				adressenEntities.AddRange(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetLikeCustomerNumber(this._data.searchtext.Trim().ToLower())
					?? new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>());

				if(adressenEntities != null && adressenEntities.Count > 0)
				{
					adressenEntities = adressenEntities?.DistinctBy(x => x.Nr).ToList();
					var kundeEntities = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByAddressNr(adressenEntities.Select(x => x.Nr).ToList());
					var results = new List<GetCustomerNamesForArticleReferenceModel>();
					foreach(var adressenEntity in adressenEntities.Where(x => x.Adresstyp != 3))
					{
						var kundeEntity = kundeEntities.FirstOrDefault(x => x.Nummer == adressenEntity.Nr);
						results.Add(
							new GetCustomerNamesForArticleReferenceModel()
							{
								CustomerName = (adressenEntity.Adresstyp == 3 ? "[Lieferadresse] " : "") + adressenEntity.Name1.Trim(),
								CustomerNumber = adressenEntity.Kundennummer,
								CustomerId = adressenEntity.Nr
							}
								);
					}
					if(customerNumberstoExeclude.Count > 0)
					{
						results = results.Where(x => !customerNumberstoExeclude.Contains(x.CustomerNumber)).ToList();
					}
					responseBody = results.Where(x => x.CustomerId > 0).OrderBy(x => x.CustomerName).ToList();
				}
				return ResponseModel<List<GetCustomerNamesForArticleReferenceModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<GetCustomerNamesForArticleReferenceModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<GetCustomerNamesForArticleReferenceModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetCustomerNamesForArticleReferenceModel>>.SuccessResponse();
		}
	}
}
