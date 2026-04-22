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
	public class GetArchivedDocumentsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DelforDocumentResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private DelforDocumentRequestModel _data { get; set; }
		public GetArchivedDocumentsHandler(Identity.Models.UserModel user, DelforDocumentRequestModel data)
		{
			this._user = user;
			_data = data;
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
				var delforHeaders = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetDelforsByCustomerNumber(_data.CustomerId, _data.IsManual, true);
				if(delforHeaders != null && delforHeaders.Count > 0)
				{
					var customers = delforHeaders.Select(x => x.PSZCustomernumber).Distinct().ToList();

					foreach(var customer in customers)
					{
						var documents = delforHeaders.Where(x => x.PSZCustomernumber == customer)?.Select(y => y.DocumentNumber).Distinct()?.ToList();
						// - 
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
								DocumentNumber = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.RemoveSuffixForArchive(document),
								DocumentNumber_Archived = document,
								CustomerAdress = $"{docInfo[0].ConsigneePostCode} {docInfo[0].ConsigneeStreet} {docInfo[0].ConsigneeCity} {docInfo[0].SupplierCountryName}",
								Versions = versions,
								NumberOfVersions = docInfo.Select(x => x.ReferenceVersionNumber ?? -1).ToList().Count,
								LastVersion = versions?.FirstOrDefault(x => x.Key == maxVersion) ?? new KeyValuePair<int, DateTime?>(0, null)
							});
						}
					}
				}

				// -
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
