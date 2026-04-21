using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{

	public class PrioEinkaufHandler: IHandle<PrioEinkaufRequestModel, ResponseModel<PrioEinkaufResponseModel>>
	{

		private UserModel _user { get; set; }
		public PrioEinkaufRequestModel _data { get; set; }
		public PrioEinkaufHandler(UserModel user, PrioEinkaufRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<PrioEinkaufResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return Perform();

			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		public ResponseModel<PrioEinkaufResponseModel> Perform()
		{
			#region Filters
			var filters = new List<Infrastructure.Data.Access.Settings.FilterModel>();

			if(!string.IsNullOrWhiteSpace(_data.ArticleNummer))
				filters.Add(new Infrastructure.Data.Access.Settings.FilterModel { FirstFilterValue = _data.ArticleNummer, FilterFieldName = "Artikelnummer", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.String, QueryLevel = 0 });

			if(!string.IsNullOrWhiteSpace(_data.SupplierName))
				filters.Add(new Infrastructure.Data.Access.Settings.FilterModel { FirstFilterValue = _data.SupplierName, FilterFieldName = "Name1", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.String, QueryLevel = 0 });
			if(!string.IsNullOrWhiteSpace(_data.OrderId))
				filters.Add(new Infrastructure.Data.Access.Settings.FilterModel { FirstFilterValue = _data.OrderId, FilterFieldName = "[Bestellung-Nr]", FilterType = Infrastructure.Data.Access.Settings.FilterTypes.Number, QueryLevel = 0 });
			#endregion

			var listAb = Infrastructure.Data.Access.Views.MTM.View_PrioeinkaufAccess.GetByLagererotId(_data.LagerId, filters);
			var listDiff = Infrastructure.Data.Access.Views.MTM.View_PSZ_Disposition_Ab_Termin_zu_Spat_sqlAccess.GetByLagerortId(_data.LagerId, filters);


			var listAbResponce = new List<ABNotAvailable>();
			var listDiffResponce = new List<DispositionDateDifference>();

			foreach(var item in listAb)
			{
				var supplier = item.Name1;
				if(listAbResponce.Find(x => x.Name1.ToLower() == supplier.ToLower()) is null)
				{
					var supplierData = listAb.FindAll(x => x.Name1 == supplier);

					var ABNotAvailable = new ABNotAvailable();
					ABNotAvailable.Name1 = item.Name1;
					ABNotAvailable.Telefon = item.Telefon;
					ABNotAvailable.Fax = item.Fax;

					ABNotAvailable.Items = supplierData.Select(x => new ReportItem
					{
						Anzahl = x.Anzahl,
						Artikelnummer = x.Artikelnummer,
						BestellungNr = x.Bestellung_Nr,
						Bezeichnung_1 = x.Bezeichnung_1,
						Datum = x.Datum,
						Lagerort_id = x.Lagerort_id,
						Liefertermin = x.Liefertermin
					}).ToList();

					listAbResponce.Add(ABNotAvailable);
				}
			}

			foreach(var item in listDiff)
			{
				var supplier = item.Name1;
				if(listDiffResponce.Find(x => x.Name1.ToLower() == supplier.ToLower()) is null)
				{
					var supplierData = listDiff.FindAll(x => x.Name1 == supplier);

					var ABNotAvailable = new DispositionDateDifference();
					ABNotAvailable.Name1 = item.Name1;
					ABNotAvailable.Telefon = item.Telefon;
					ABNotAvailable.Fax = item.Fax;
					ABNotAvailable.Items = supplierData.Select(x => new ReortItemDateDifference
					{
						Anzahl = x.Anzahl,
						Artikelnummer = x.Artikelnummer,
						BestellungNr = x.Bestellung_Nr,
						Bezeichnung_1 = x.Bezeichnung_1,
						Datum = x.Datum,
						Lagerort_id = x.Lagerort_id,
						Liefertermin = x.Liefertermin,
						Bestatigter_Termin = x.Bestatigter_Termin
					}).ToList();

					listDiffResponce.Add(ABNotAvailable);
				}
			}

			PrioEinkaufResponseModel response = new PrioEinkaufResponseModel
			{
				ABNotAvailables = listAbResponce,
				DispositionDateDifferences = listDiffResponce
			};

			return ResponseModel<PrioEinkaufResponseModel>.SuccessResponse(response);
		}

		public ResponseModel<PrioEinkaufResponseModel> Validate()
		{

			if(_user == null)
			{
				return ResponseModel<PrioEinkaufResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<PrioEinkaufResponseModel>.SuccessResponse();
		}
	}
}
