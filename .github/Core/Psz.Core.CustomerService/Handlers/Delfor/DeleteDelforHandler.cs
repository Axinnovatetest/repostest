using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class DeleteDelforHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private DeleteDelforModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteDelforHandler(Identity.Models.UserModel user, DeleteDelforModel data)
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

				var response = 0;
				var logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
				var header = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(_data.HeaderId);

				var headerId = header.Id;
				var headerDocument = header.DocumentNumber;
				var headerCustomerNumber = header.PSZCustomernumber;
				var headerVersion = header.ReferenceVersionNumber;

				var lineItems = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetByHeaderId(_data.HeaderId);
				var lineItemPlans = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByLineItems(lineItems.Select(x => x.Id).ToList());
				logs.Add(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
				{
					AngebotNr = null,
					DateTime = DateTime.Now,
					LogObject = "DeliveryForcat",
					LogText = $"Delivery Forcast {headerDocument} Version {header.ReferenceVersionNumber} deleted",
					LogType = "DELETE_DLF",
					Nr = (int)headerId,
					Origin = "CTS",
					ProjektNr = null,
					UserId = _user.Id,
					Username = _user.Name
				});
				//deleting current version
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				try
				{
					botransaction.beginTransaction();

					#region // -- transaction-based logic -- //

					response += Infrastructure.Data.Access.Tables.CTS.HeaderAccess.DeleteWithTransaction(header.Id, botransaction.connection, botransaction.transaction);
					response += Infrastructure.Data.Access.Tables.CTS.LineItemAccess.DeleteWithTransaction(lineItems.Select(x => x.Id).ToList(), botransaction.connection, botransaction.transaction);
					response += Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.DeleteWithTransaction(lineItemPlans.Select(x => x.Id).ToList(), botransaction.connection, botransaction.transaction);

					if(_data.DeleteAllVersions)
					{
						//deleting all version
						var versionsHeaders = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetDeflorOtherversions(headerDocument, headerCustomerNumber ?? -1, headerVersion ?? -1, true, true,
							botransaction.connection, botransaction.transaction);
						if(versionsHeaders != null && versionsHeaders.Count > 0)
						{
							var lineItemsVersions = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetByHeaderId(versionsHeaders.Select(x => x.Id).ToList(),
								botransaction.connection, botransaction.transaction);
							var lineItemPlansVersions = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByLineItems(lineItemsVersions.Select(x => x.Id).ToList(),
								botransaction.connection, botransaction.transaction);

							response += Infrastructure.Data.Access.Tables.CTS.HeaderAccess.DeleteWithTransaction(versionsHeaders.Select(x => x.Id).ToList(),
								 botransaction.connection, botransaction.transaction);
							response += Infrastructure.Data.Access.Tables.CTS.LineItemAccess.DeleteWithTransaction(lineItemsVersions.Select(x => x.Id).ToList(),
								 botransaction.connection, botransaction.transaction);
							response += Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.DeleteWithTransaction(lineItemPlansVersions.Select(x => x.Id).ToList(),
								 botransaction.connection, botransaction.transaction);

							foreach(var item in versionsHeaders)
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
						}
					}
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);

					#endregion // -- transaction-based logic -- //
					if(botransaction.commit())
					{
						return ResponseModel<int>.SuccessResponse(response);
					}
					else
						return ResponseModel<int>.FailureResponse("Transaction Error");
				} catch(Exception e)
				{
					botransaction.rollback();
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
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