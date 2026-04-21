using MoreLinq;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class SearchManualDeflorHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DelforDocumentResponseModel>>>
	{

		private SearchManualDelforRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public SearchManualDeflorHandler(Identity.Models.UserModel user, SearchManualDelforRequestModel data)
		{
			this._user = user;
			this._data = data;
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

				var response = new List<DelforDocumentResponseModel>();
				var delforHeaders = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetManualDelfors();
				var userCustomers = new Psz.Core.CustomerService.Handlers.OrderProcessing.GetMyCustomersHandler(null, _user).Handle().Body;
				var userCustomerNumbers = userCustomers?.Select(c => c.CustomerNumber).ToList();
				delforHeaders = delforHeaders?.Where(d => userCustomerNumbers.Contains(d.PSZCustomernumber ?? -1)).ToList();

				if(delforHeaders != null && delforHeaders.Count > 0)
				{
					var customers = delforHeaders.Select(x => x.PSZCustomernumber).Distinct().ToList();

					foreach(var customer in customers)
					{
						var documents = delforHeaders.Where(x => x.PSZCustomernumber == customer).Select(y => y.DocumentNumber).Distinct().ToList();
						foreach(var document in documents)
						{
							var docInfo = delforHeaders.Where(x => x.PSZCustomernumber == customer && x.DocumentNumber == document).Distinct().ToList();
							var versions = docInfo.Select(x => new KeyValuePair<int, DateTime?>(x.ReferenceVersionNumber ?? -1, x.ReceivingDate)).ToList();
							var maxVersion = versions?.Select(x => x.Key).Max();
							response.Add(new DelforDocumentResponseModel
							{
								Customer = docInfo[0].BuyerPartyName,
								Consignee = docInfo[0].ConsigneePartyName,
								CustomerContact = docInfo[0].BuyerContactName,
								CustomerNumber = customer,
								DocumentNumber = document,
								CustomerAdress = $"{docInfo[0].ConsigneePostCode} {docInfo[0].ConsigneeStreet} {docInfo[0].ConsigneeCity} {docInfo[0].SupplierCountryName}",
								Versions = versions,
								NumberOfVersions = docInfo.Select(x => x.ReferenceVersionNumber ?? -1).ToList().Count,
								LastVersion = versions?.FirstOrDefault(x => x.Key == maxVersion) ?? new KeyValuePair<int, DateTime?>(0, null)
							});
						}
					}
				}

				if(!string.IsNullOrEmpty(_data.Customer) && !string.IsNullOrWhiteSpace(_data.Customer))
					response = response.Where(x => x.Customer.ToLower().Contains(_data.Customer.ToLower()) || x.Consignee.ToLower().Contains(_data.Customer.ToLower())).ToList();


				if(!string.IsNullOrEmpty(_data.DocumentNummer) && !string.IsNullOrWhiteSpace(_data.DocumentNummer))
					response = response.Where(x => x.DocumentNumber.ToLower().Contains(_data.DocumentNummer.ToLower())).ToList();


				return ResponseModel<List<DelforDocumentResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<DelforDocumentResponseModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<DelforDocumentResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<DelforDocumentResponseModel>>.SuccessResponse();
		}

	}
}
