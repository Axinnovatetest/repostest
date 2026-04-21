using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class Dispows120DetailsLieferantenAndOrderHandler: IHandle<Dispows120DetailsAllLieferantenAndordersRequestModel, ResponseModel<Dispows120DetailsAllLieferantenModel>>
	{

		private Dispows120DetailsAllLieferantenAndordersRequestModel _data { get; set; }
		private UserModel _user { get; set; }
		public Dispows120DetailsLieferantenAndOrderHandler(UserModel user, Dispows120DetailsAllLieferantenAndordersRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Dispows120DetailsAllLieferantenModel> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this._user, this._data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<Dispows120DetailsAllLieferantenModel> Perform(UserModel user, Dispows120DetailsAllLieferantenAndordersRequestModel data)
		{
			try
			{
				/*	//Dispows120DetailsLieferantenAndOrderModel

					var fetchedLieferanten = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispowsAccess.Getws120Lieferanten(data.artikelnummer);
					var fetchedOffneBestellungen = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispowsAccess.Getws120OffneBestellungen(data.artikelnummer);

					var suppliers = fetchedLieferanten.Select(x=> new Dispows120DetailsLieferantenModel(x)).ToList();
					var fetchedOpenOrders = fetchedOffneBestellungen.Select(x=> new Dispows120DetailsOffenBestellungenModel(x)).ToList();
					var LieferantenNrs = suppliers.Select(x => x.Lieferanten_Nr).Distinct().ToList();

					Dispows120DetailsAllLieferantenModel restoreturn = new Dispows120DetailsAllLieferantenModel();

					foreach(var item in LieferantenNrs)
					{
						restoreturn.Lieferantens.Add(new Dispows120DetailsLieferantenAndOrderModel(suppliers, fetchedOpenOrders,item));
						//var x = new Dispows120DetailsLieferantenAndOrderModel(suppliers, fetchedOpenOrders,item);
					}


					return ResponseModel<Dispows120DetailsAllLieferantenModel>.SuccessResponse(restoreturn);*/

				return null;

			} catch(Exception ex)
			{
				throw;
			}
		}
		public ResponseModel<Dispows120DetailsAllLieferantenModel> Validate()
		{
			if(_user == null)
			{
				return ResponseModel<Dispows120DetailsAllLieferantenModel>.AccessDeniedResponse();
			}
			return ResponseModel<Dispows120DetailsAllLieferantenModel>.SuccessResponse();
		}
	}
}
