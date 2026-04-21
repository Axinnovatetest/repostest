namespace Psz.Core.MaterialManagement.Orders.Handlers
{
	using Infrastructure.Data.Access.Tables.MTM;
	using Psz.Core.MaterialManagement.Interfaces;

	public partial class OrderService: IOrderService
	{
		public ResponseModel<int> TogglePurchaseProject(UserModel user, int data)
		{
			var validate = Validate(user, data);
			if(validate.Success == false)
			{
				return validate;
			}
			// -

			return Perform(user, data);
		}
		public ResponseModel<int> Validate(UserModel user, int data)
		{
			if(user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(BestellungenAccess.Get(data) == null)
				return ResponseModel<int>.NotFoundResponse();
			if(user.Access.MaterialManagement.Purchasing.ProjectPurchaseSetOrder != true)
				return ResponseModel<int>.FailureResponse("User can't change [EK-Project] orders.");

			return ResponseModel<int>.SuccessResponse();
		}
		private ResponseModel<int> Perform(UserModel user, int data)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				var order = BestellungenAccess.GetWithTransaction(data, botransaction.connection, botransaction.transaction);
				BestellungenProcessing_LogAccess.InsertWithTransaction(new Helpers.LogHelper(
						data,
						order.Bestellung_Nr ?? 0,
						int.TryParse(order.Projekt_Nr, out var x) ? x : 0,
						order.Typ,
						Helpers.LogHelper.LogType.MODIFICATIONORDER,
						"MTM",
						user).LogMTM(data, $"[Purchase-Project] from {(order.ProjectPurchase ?? false)} to {!(order.ProjectPurchase ?? false)}"), botransaction.connection, botransaction.transaction);

				order.ProjectPurchase = !(order.ProjectPurchase ?? false);
				var status = BestellungenAccess.UpdatePurchaseProject(order, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //

				//-: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(status);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
