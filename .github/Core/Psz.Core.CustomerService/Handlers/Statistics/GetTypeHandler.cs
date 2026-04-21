using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using static Infrastructure.Services.ActiveDirectory.ActiveDirectoryManager;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetTypeHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private bool _data { get; set; }
		public GetTypeHandler(Identity.Models.UserModel user, bool filterByUser)
		{
			this._user = user;
			this._data = filterByUser;
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

				var items = Enum.GetValues(typeof(Enums.OrderEnums.Types)).Cast<Enums.OrderEnums.Types>().ToList()
								   ?.Select(x => new KeyValuePair<int, string>((int)x, x.GetDescription()))
								   ?.Where(y =>
									y.Key == (int)Enums.OrderEnums.Types.Confirmation
										|| y.Key == (int)Enums.OrderEnums.Types.Contract
										|| y.Key == (int)Enums.OrderEnums.Types.Credit
										|| y.Key == (int)Enums.OrderEnums.Types.Invoice
										|| y.Key == (int)Enums.OrderEnums.Types.forecast
										)?.ToList();

				if(this._data && !this._user.IsGlobalDirector && !this._user.SuperAdministrator)
				{
					// - no access to CTS
					if(this._user.Access == null || this._user.Access.CustomerService == null || this._user.Access.CustomerService.ModuleActivated != true)
					{
						items = new List<KeyValuePair<int, string>>();
					}
					else
					{
						if(this._user.Access.CustomerService.ConfirmationCreate != true)
						{
							items = items?.Where(x => x.Key != (int)Enums.OrderEnums.Types.Confirmation)?.ToList();
						}
						if(this._user.Access.CustomerService.Rahmen != true || this._user.Access.CustomerService.RahmenAdd != true)
						{
							items = items?.Where(x => x.Key != (int)Enums.OrderEnums.Types.Contract)?.ToList();
						}
						if(this._user.Access.CustomerService.ConfirmationDeliveryNote != true && this._user.Access.CustomerService.DeliveryNoteCreate != true)
						{
							items = items?.Where(x => x.Key != (int)Enums.OrderEnums.Types.Delivery)?.ToList();
						}

						if(this._user.Access.CustomerService.GutschriftCreate != true)
						{
							items = items?.Where(x => x.Key != (int)Enums.OrderEnums.Types.Credit)?.ToList();
						}
						if(_user.Access.CustomerService.ForcastCreate != true)
						{
							items = items?.Where(x => x.Key != (int)Enums.OrderEnums.Types.forecast)?.ToList();
						}
						if(_user.Access.CustomerService.RechnungManualCreation != true)
						{
							items = items?.Where(x => x.Key != (int)Enums.OrderEnums.Types.Invoice)?.ToList();
						}
					}
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(items);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}