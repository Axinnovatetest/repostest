using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetArchivedHeaderHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DelforCustomerResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private bool _data { get; set; }
		public GetArchivedHeaderHandler(Identity.Models.UserModel user, bool data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<List<DelforCustomerResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<DelforCustomerResponseModel>();
				var customersNummers = _user.IsGlobalDirector || _user.SuperAdministrator || _user.Access.Purchase.AllCustomers ?
					 Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get()?.Select(c => c.Nummer ?? -1).OrderBy(y => y).ToList()
					 : Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(_user.Id)?.Select(c => c.CustomerNumber).OrderBy(y => y).ToList();

				var customersAdresses = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNummers)?.ToList();
				var delforHeaders = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetDelforsByCustomerNumber(customersAdresses?.Select(x => x.Nr)?.ToList(), _data, true);

				if(delforHeaders != null && delforHeaders.Count > 0)
				{
					var customers = delforHeaders.Select(x => x.PSZCustomernumber).Distinct().ToList();

					foreach(var customer in customers)
					{
						var adressEntity = customersAdresses.FirstOrDefault(x => x.Nr == customer);
						var documents = delforHeaders.Where(x => x.PSZCustomernumber == customer)?.ToList();
						// - 
						if(documents?.Count > 0)
						{
							response.Add(new DelforCustomerResponseModel
							{
								CustomerName = documents[0].BuyerPartyName,
								ConsigneeName = documents[0].ConsigneePartyName,
								CustomerContact = documents[0].BuyerContactName,
								CustomerNumber = adressEntity?.Kundennummer ?? 0,
								CustomerId = adressEntity?.Nr ?? 0,
								CustomerAdress = $"{documents[0].ConsigneePostCode} {documents[0].ConsigneeStreet} {documents[0].ConsigneeCity} {documents[0].SupplierCountryName}",
								DocumentCount = documents.Count,
							});
						}
					}
				}

				// -
				return ResponseModel<List<DelforCustomerResponseModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<DelforCustomerResponseModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<DelforCustomerResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<DelforCustomerResponseModel>>.SuccessResponse();
		}

	}
}
