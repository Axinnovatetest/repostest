using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class ErledigtUpdateHandler: IHandle<GetByIdRequestModel, ResponseModel<int>>
	{

		private int data { get; set; }
		private UserModel user { get; set; }

		public ErledigtUpdateHandler(UserModel user, int data)
		{
			this.data = data;
			this.user = user;
		}
		private ResponseModel<int> Perform()
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
						user).LogMTM(data, $"Erledigt from {(order.erledigt ?? false)} to {!(order.erledigt ?? false)}"), botransaction.connection, botransaction.transaction);

				order.erledigt = !(order.erledigt ?? false);
				var status = BestellungenAccess.UpdateErledigt(order, botransaction.connection, botransaction.transaction);

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

		public ResponseModel<int> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var order = BestellungenAccess.Get(data);
			if(order == null)
				return ResponseModel<int>.NotFoundResponse();
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
