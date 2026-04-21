using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class UpdateOrderSupplierHandler: IHandle<UpdateOrderSupplierRequestModel, ResponseModel<UpdateOrderSupplierResponseModel>>
	{

		private UpdateOrderSupplierRequestModel data { get; set; }
		private UserModel user { get; set; }

		public UpdateOrderSupplierHandler(UserModel user, UpdateOrderSupplierRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<UpdateOrderSupplierResponseModel> Handle()
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

		private ResponseModel<UpdateOrderSupplierResponseModel> Perform(UserModel user, UpdateOrderSupplierRequestModel data)
		{

			var orderDb = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.Id);

			orderDb.Anrede = data.Type;
			orderDb.Vorname_NameFirma = data.Name;

			orderDb.Ansprechpartner = data.ContactPerson;
			orderDb.Abteilung = data.department;
			orderDb.Strasse_Postfach = data.StreetPO_Box;
			orderDb.Land_PLZ_Ort = data.CountryZIPLocation;
			orderDb.Briefanrede = data.letterSalutation;
			orderDb.Ihr_Zeichen = data.Your_sign;
			orderDb.Unser_Zeichen = data.Our_sign;

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);
			botransaction.beginTransaction();
			try
			{
				Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.UpdateWithTransaction(orderDb, botransaction.connection, botransaction.transaction);

				//Logging
				var _log = new LogHelper(
					orderDb.Nr,
					orderDb.Bestellung_Nr ?? -1,
					int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0,
					$"{orderDb.Typ}",
					LogHelper.LogType.MODIFICATIONORDER,
					"MTM",
					user)
						.LogMTM(orderDb.Nr);
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);
				// - 
				if(botransaction.commit())
				{
					return ResponseModel<UpdateOrderSupplierResponseModel>.SuccessResponse();
				}
				else
					return ResponseModel<UpdateOrderSupplierResponseModel>.FailureResponse("Transaction didn't commit.");
			} catch(Exception e)
			{
				botransaction.rollback();
				throw;
			}

		}

		public ResponseModel<UpdateOrderSupplierResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<UpdateOrderSupplierResponseModel>.AccessDeniedResponse();
			}
			if(user.Number == 0)
				return ResponseModel<UpdateOrderSupplierResponseModel>.FailureResponse("User need to have a User Number");


			var orderDb = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.Id);

			if(orderDb == null)
				return ResponseModel<UpdateOrderSupplierResponseModel>.FailureResponse("Order not found");

			if(orderDb.gebucht == true)
				return ResponseModel<UpdateOrderSupplierResponseModel>.FailureResponse("Can't edit Validated Order");


			return ResponseModel<UpdateOrderSupplierResponseModel>.SuccessResponse();
		}
	}
}
