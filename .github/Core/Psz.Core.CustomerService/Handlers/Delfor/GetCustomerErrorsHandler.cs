using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetCustomerErrorsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DelforErrorsModel>>>
	{

		private DelforErrorsRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetCustomerErrorsHandler(Identity.Models.UserModel user, DelforErrorsRequestModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<DelforErrorsModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<DelforErrorsModel>();
				var errors = string.IsNullOrWhiteSpace(_data.Duns)
					? Infrastructure.Data.Access.Tables.CTS.ErrorAccess.GetWoBuyerDUNS()
					: Infrastructure.Data.Access.Tables.CTS.ErrorAccess.GetByBuyerDUNS(new List<string> { _data.Duns });

				if(errors != null && errors.Count > 0)
				{
					errors = errors.Where(e => !e.Validated.HasValue || !e.Validated.Value).ToList();
					foreach(var item in errors)
					{
						var sender = "";
						var customer = -1;
						if(string.IsNullOrWhiteSpace(_data.Duns))
						{

						}
						else
						{
							var customerAdress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByDunsNumber(_data.Duns);
							if(customerAdress != null)
							{
								sender = customerAdress.Name1;
								customer = customerAdress.Kundennummer ?? -1;
							}
							else
							{
								var adressenExtension = Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.GetByDuns(_data.Duns ?? "");
								var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(adressenExtension?.AdressenNr ?? -1);
								sender = adress?.Name1;
								customer = adress?.Kundennummer ?? -1;
							}
						}
						response.Add(new DelforErrorsModel
						{
							Id = item.Id,
							Sender = sender,
							Customer = customer,
							Date = item.ProcessTime,
							ErrorMessage = item.ErrorMessage,
							File = Path.GetFileName(item.FileName),
							DocumentNumber = item.Documentnumber,
						});
					}
				}

				return ResponseModel<List<DelforErrorsModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<DelforErrorsModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<DelforErrorsModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<DelforErrorsModel>>.SuccessResponse();
		}

	}
}
