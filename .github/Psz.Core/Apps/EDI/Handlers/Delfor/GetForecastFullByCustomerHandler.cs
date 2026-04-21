using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using MoreLinq;
	using Psz.Core.Common.Models;
	using Psz.Core.CustomerService.Models.Delfor;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetForecastFullByCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DelforDocumentResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetForecastFullByCustomerHandler(Identity.Models.UserModel user, int customerId)
		{
			this._user = user;
			this._data = customerId;
		}

		public ResponseModel<List<DelforDocumentResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var response = new List<DelforDocumentResponseModel>();
				var customerEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data);
				var duns = customerEntity.Duns;

				var delforHeaders = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetDelforsByDuns(duns);
				if(delforHeaders != null && delforHeaders.Count > 0)
				{
					var customersDocuments = System.Linq.Enumerable.DistinctBy(delforHeaders.Select(x => new KeyValuePair<string, string>(x.DocumentNumber, x.BuyerDUNS)).ToList(),
						s => s.Key).ToList();
					foreach(var item in customersDocuments)
					{
						var documents = delforHeaders.Where(x => x.DocumentNumber == item.Key && x.BuyerDUNS == item.Value).ToList();
						var versions = documents.Select(x => new KeyValuePair<int, DateTime?>(x.ReferenceVersionNumber ?? -1, x.ReceivingDate)).ToList();
						var maxVersion = versions?.Select(x => x.Key).Max();
						var lastDocument = documents.FirstOrDefault(x => x.ReferenceVersionNumber == maxVersion);
						response.Add(new DelforDocumentResponseModel
						{
							Customer = lastDocument.BuyerPartyName,
							Consignee = $"{lastDocument.ConsigneePartyIdentification} | {lastDocument.ConsigneePartyName}".Trim(new char[] { ' ', '|' }),
							CustomerContact = lastDocument.BuyerContactName,
							CustomerNumber = this._data,
							DocumentNumber = item.Key,
							CustomerAdress = $"{lastDocument.ConsigneePostCode} {lastDocument.ConsigneeStreet} {lastDocument.ConsigneeCity} {lastDocument.SupplierCountryName}",
							Versions = versions,
							NumberOfVersions = documents.Select(x => x.ReferenceVersionNumber ?? -1).ToList().Count,
							LastVersion = versions?.FirstOrDefault(x => x.Key == maxVersion) ?? new KeyValuePair<int, DateTime?>(0, null)
						});
					}
				}

				return ResponseModel<List<DelforDocumentResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<DelforDocumentResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<DelforDocumentResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data) == null)
			{
				return ResponseModel<List<DelforDocumentResponseModel>>.FailureResponse("Customer not found");
			}

			return ResponseModel<List<DelforDocumentResponseModel>>.SuccessResponse();
		}
	}
}