using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class ToggleStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private ToggleStatusRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public ToggleStatusHandler(Identity.Models.UserModel user, ToggleStatusRequestModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				var docs = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetByDocumentAndCustomer(_data.DocumentNumber, _data.CustomerId, null, botransaction.connection, botransaction.transaction);
				if(docs?.Count > 0)
				{
					var _documentNumber = "";
					if(_data.Done)
					{// - archiving
						_documentNumber = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.AddSuffixForArchive(_data.DocumentNumber, botransaction.connection, botransaction.transaction);
					}
					else
					{// - unarchiving
						_documentNumber = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.RemoveSuffixForArchive(_data.DocumentNumber);
					}

					var header = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.ToggleStatus(docs?.Select(x => x.Id)?.ToList(), _documentNumber, _data.Done, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.CTS.LineItemAccess.ToggleStatus(docs?.Select(x => x.Id)?.ToList(), _documentNumber, botransaction.connection, botransaction.transaction);

					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(docs?.Select(x =>
					new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
					{
						AngebotNr = null,
						DateTime = DateTime.Now,
						LogObject = "DeliveryForcat",
						LogText = $"Delivery Forcast {_data.DocumentNumber} Version {x.ReferenceVersionNumber} set to [{(_data.Done ? "closed" : "open")}]",
						LogType = "UPDATE_DLF",
						Nr = (int)x.Id,
						Origin = "CTS",
						ProjektNr = null,
						UserId = _user.Id,
						Username = _user.Name
					})?.ToList(), botransaction.connection, botransaction.transaction);

					//TODO: handle transaction state (success or failure)
					if(botransaction.commit())
					{
						return ResponseModel<int>.SuccessResponse(header);
					}
					else
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
					}
				}

				// - 
				return ResponseModel<int>.SuccessResponse(0);

				#endregion // -- transaction-based logic -- //

			} catch(Exception e)
			{
				botransaction.rollback();
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

			// when reactivating, check conflicts
			if(_data.Done == false)
			{
				var existingDocuments = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetByDocumentAndCustomer(
					 Infrastructure.Data.Access.Tables.CTS.HeaderAccess.RemoveSuffixForArchive(_data.DocumentNumber), _data.CustomerId, false);
				if(existingDocuments?.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"An active document [{existingDocuments[0].DocumentNumber}] already exists for customer [{existingDocuments[0].BuyerPartyName}]. Please archive it first.");
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
