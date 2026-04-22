using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class DeleteDelforInListhandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private DeleteDelforInListmodel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteDelforInListhandler(Identity.Models.UserModel user, DeleteDelforInListmodel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
				var documents = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetDeflorOtherversions(_data.DocumentNumber, _data.Customernumber, -1, false, true);
				if(documents != null && documents.Count > 0)
				{
					var lineItems = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetByHeaderId(documents.Select(x => x.Id).ToList());
					var lineItemPlans = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByLineItems(lineItems?.Select(x => x.Id).ToList());
					foreach(var item in documents)
					{
						logs.Add(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
						{
							AngebotNr = null,
							DateTime = DateTime.Now,
							LogObject = "DeliveryForcat",
							LogText = $"Delivery Forcast {item.DocumentNumber} Version {item.ReferenceVersionNumber} deleted",
							LogType = "DELETE_DLF",
							Nr = (int)item.Id,
							Origin = "CTS",
							ProjektNr = null,
							UserId = _user.Id,
							Username = _user.Name
						});
					}
					var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
					botransaction.beginTransaction();
					Infrastructure.Data.Access.Tables.CTS.HeaderAccess.DeleteWithTransaction(documents.Select(x => x.Id).ToList(), botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.CTS.LineItemAccess.DeleteWithTransaction(lineItems?.Select(x => x.Id).ToList(), botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.CTS.HeaderAccess.DeleteWithTransaction(lineItemPlans.Select(x => x.Id).ToList(), botransaction.connection, botransaction.transaction);
					if(botransaction.commit())
					{
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(logs);
					}
					else
						return ResponseModel<int>.FailureResponse("Transaction Error");
				}
				return ResponseModel<int>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
