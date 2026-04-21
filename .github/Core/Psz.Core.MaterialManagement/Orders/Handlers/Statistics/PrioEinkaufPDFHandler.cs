using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{

	public class PrioEinkaufPDFHandler: IHandle<PrioEinkaufPDFRequestModel, ResponseModel<byte[]>>
	{

		private UserModel _user { get; set; }
		public PrioEinkaufPDFRequestModel _data { get; set; }
		public PrioEinkaufPDFHandler(UserModel user, PrioEinkaufPDFRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<byte[]> Handle()
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
		public ResponseModel<byte[]> Perform()
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
			if(_data.Type == 1)
			{
				var listAb = Infrastructure.Data.Access.Views.MTM.View_PrioeinkaufAccess.GetByLagererotId(_data.LagerId, filters);

				var data = new Infrastructure.Services.Reporting.IText.PRS.ABNotAvailable().GetData(listAb);
				return ResponseModel<byte[]>.SuccessResponse(Infrastructure.Services.Reporting.IText.PRS.GetKeineABVorhanden(data));
			}
			else
			{
				var listDiff = Infrastructure.Data.Access.Views.MTM.View_PSZ_Disposition_Ab_Termin_zu_Spat_sqlAccess.GetByLagerortId(_data.LagerId, filters);
				var data = new Infrastructure.Services.Reporting.IText.PRS.DispositionDateDifference().GetData(listDiff);
				return ResponseModel<byte[]>.SuccessResponse(Infrastructure.Services.Reporting.IText.PRS.GetAbTermin(data));
			}
		}

		public ResponseModel<byte[]> Validate()
		{

			if(_user == null)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}
			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
