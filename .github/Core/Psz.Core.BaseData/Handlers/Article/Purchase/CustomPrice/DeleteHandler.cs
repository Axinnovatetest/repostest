using System;

namespace Psz.Core.BaseData.Handlers.Article.Purchase.CustomPrice
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public DeleteHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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

				// -
				botransaction.beginTransaction();
				var customPrice = Infrastructure.Data.Access.Tables.BSD.Bestellnummern_StaffelpreiseAccess.GetWithTransaction(this._data, botransaction.connection, botransaction.transaction);

				#region >>> Logs & Notifications <<<
				var purchasePrice = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetWithTransaction(customPrice.nummer ?? -1, botransaction.connection, botransaction.transaction);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(purchasePrice.ArtikelNr ?? -1, botransaction.connection, botransaction.transaction);
				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr,
					$"Article Custom Purchase Price",
					$"{customPrice.Einkaufspreis}",
					$"",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Delete), botransaction.connection, botransaction.transaction);
				#endregion Logs & Notifications

				var result = Infrastructure.Data.Access.Tables.BSD.Bestellnummern_StaffelpreiseAccess.DeleteWithTransaction(this._data, botransaction.connection, botransaction.transaction);

				//handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(result);
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
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.Bestellnummern_StaffelpreiseAccess.Get(this._data) == null)
			{
				return ResponseModel<int>.FailureResponse("Custom price not found !");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
