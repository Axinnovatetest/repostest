using System;

namespace Psz.Core.BaseData.Handlers.Article.Purchase.CustomPrice
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Purchase.CustomPriceModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.Purchase.CustomPriceModel data)
		{
			_user = user;
			_data = data;
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

				#region >>> Logs & Notifications <<<
				var purchasePrice = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetWithTransaction(this._data.OrderNumber, botransaction.connection, botransaction.transaction);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(purchasePrice.ArtikelNr ?? -1, botransaction.connection, botransaction.transaction);
				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr,
					$"Article Custom Purchase Price",
					$"",
					$"{this._data.Price}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Add), botransaction.connection, botransaction.transaction);
				#endregion Logs & Notifications

				var result = Infrastructure.Data.Access.Tables.BSD.Bestellnummern_StaffelpreiseAccess.InsertWithTransaction(this._data.ToEntity(), botransaction.connection, botransaction.transaction);

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
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.Get(this._data.OrderNumber) == null)
			{
				return ResponseModel<int>.FailureResponse($"Bestellnummer [{this._data.OrderNumber}] not found!");
			}
			if(Infrastructure.Data.Access.Tables.BSD.Bestellnummern_StaffelpreiseAccess.GetByOrderNumberAndQuantity(this._data.OrderNumber, this._data.QuantityFrom) != null)
			{
				return ResponseModel<int>.FailureResponse($"Custom price for quantity [{this._data.QuantityFrom}] exists !");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
