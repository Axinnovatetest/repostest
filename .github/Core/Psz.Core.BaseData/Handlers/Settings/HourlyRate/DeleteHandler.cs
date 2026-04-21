using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.HourlyRate
{
	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int data)
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
				// -
				var entity = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.GetWithTransaction(this._data, botransaction.connection, botransaction.transaction);
				var responseBody = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.DeleteWithTransaction(this._data, botransaction.connection, botransaction.transaction);

				// -
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(ObjectLogHelper.getLog(this._user, responseBody,
						  $"",
						  $"{entity.ProductionSiteName} | {entity.HourlyRate}",
						  $"", Enums.ObjectLogEnums.Objects.ArticleConfig_HourlyRates.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Delete),
						  botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //

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

			var entity = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.Get(this._data);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}

			var prodExtEntities = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByProductionPlace1(entity.ProductionSiteId ?? -1);
			if(prodExtEntities?.Count > 0)
			{
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(prodExtEntities.Select(x => x.ArticleId).Distinct().ToList());
				if(articleEntities != null && articleEntities.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Hourly Rate [{(entity.ProductionSiteName)}] has articles [{(string.Join(", ", articleEntities.Take(5).Select(x => x.ArtikelNummer)))}]. Please change/delete the articles first.");
				}
			}
			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
