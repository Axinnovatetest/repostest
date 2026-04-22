using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.CustomerItemNumber
{
	public class PromoteToCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.CustomerItemNumber.PromoteToCustomerRequestModel _data { get; set; }

		public PromoteToCustomerHandler(Identity.Models.UserModel user, Models.Settings.CustomerItemNumber.PromoteToCustomerRequestModel data)
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

				//TODO: - insert process here
				botransaction.beginTransaction();
				// -
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var responseBody = this._data.CustomerItemId;

				var entity = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetWithTransaction(this._data.CustomerItemId, botransaction.connection, botransaction.transaction);
				entity.Kundennummer = this._data.TargetCustomerNumber;
				entity.Kunde = this._data.TargetCustomerName;
				entity.Analyse = false;
				entity.AnalyseName = null;
				Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.UpdateWithTransaction(entity, botransaction.connection, botransaction.transaction);

				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetEFByKreis(entity.Nummerschlüssel, botransaction.connection, botransaction.transaction);
				if(articleEntities != null && articleEntities.Count > 0)
				{
					// -
					logs.AddRange(articleEntities.Select(x =>
						ObjectLogHelper.getLog(this._user, x.ArtikelNr,
					$"CustomerNumber",
						  $"{x.CustomerNumber}",
						  $"{this._data.TargetCustomerNumber}", Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit))?.ToList());

					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditCustomerWithTransaction(this._data.TargetCustomerNumber, articleEntities.Select(x => x.ArtikelNr).ToList(),
						botransaction.connection, botransaction.transaction);
				}

				logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Kunde",
						  $"{entity.Kundennummer}|{entity.Kunde}//{entity.AnalyseName}",
						  $"{this._data.TargetCustomerNumber}|{this._data.TargetCustomerName}", Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));

				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(responseBody);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var entity = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(this._data.CustomerItemId);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}
			if(entity.Analyse != true)
			{
				return ResponseModel<int>.FailureResponse("Item is not Analyse");
			}
			// - check target customer
			if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetKundenAddresses(this._data.TargetCustomerNumber) == null)
			{
				return ResponseModel<int>.FailureResponse($"Target customer [{this._data.TargetCustomerName}] not found");
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
