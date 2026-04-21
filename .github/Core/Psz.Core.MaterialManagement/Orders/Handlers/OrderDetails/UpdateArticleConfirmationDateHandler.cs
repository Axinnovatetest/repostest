using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class UpdateArticleConfirmationDateHandler: IHandle<UpdateArticleConfirmationDateRequestModel, ResponseModel<int>>
	{
		private UpdateArticleConfirmationDateRequestModel data { get; set; }
		private UserModel user { get; set; }
		public UpdateArticleConfirmationDateHandler(UserModel user, UpdateArticleConfirmationDateRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private ResponseModel<int> Perform(UserModel user, UpdateArticleConfirmationDateRequestModel data)
		{
			var oldBAPostionEntity = new Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity();
			if(data.PositionId != -1 && data.PositionId != 0)
				oldBAPostionEntity = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.PositionId);


			var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);
			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(oldBAPostionEntity.Bestellung_Nr ?? -1);
			botransaction.beginTransaction();
			int updated = 0;
			try
			{
				updated = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.UpdateConfirmationDate(this.data.PositionId, this.data.ConfirmationDate, botransaction.connection, botransaction.transaction);
				var _log = new LogHelper(
						bestellung.Nr,
						oldBAPostionEntity.Bestellung_Nr ?? -1,
						0,
						$"{bestellung.Typ}",
						LogHelper.LogType.MODIFICATIONPOS,
						"MTM",
						user).LogMTM(oldBAPostionEntity.Nr);
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

			} catch
			{
				botransaction.rollback();
				throw;
			}

			if(botransaction.commit())
			{
				return ResponseModel<int>.SuccessResponse(updated);
			}
			else
				return ResponseModel<int>.FailureResponse("Transaction didn't commit.");
		}
		public ResponseModel<int> Validate()
		{
			if(user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(user.Number == 0)
				return ResponseModel<int>.FailureResponse("User need to have a User Number");

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.OrderId);

			if(bestellung is null)
				return ResponseModel<int>.FailureResponse("Order not found");

			if(bestellung is not null && bestellung.gebucht == true)
				return ResponseModel<int>.FailureResponse("Can't edit Validated Order");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
