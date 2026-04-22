using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Globalization;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class PrioEinkaufPDFHandler: IHandleAsync<PrioEinkaufPDFRequestModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		public PrioEinkaufPDFRequestModel _data { get; set; }
		public PrioEinkaufPDFHandler(UserModel user, PrioEinkaufPDFRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public async Task<ResponseModel<byte[]>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return await Perform();

			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		public async Task<ResponseModel<byte[]>> Perform()
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
			byte[] response = null;
			var reportParams = new Reporting.Models.ITextHeaderFooterProps
			{
				BodyData = null,
				BodyTemplate = "MTM_Prioeinkauf",
				DocumentTitle = "",
				FooterCenterText = "",
				FooterData = null,
				FooterLeftText = DateTime.Now.ToString("dddd, d. MMMM yyyy", new CultureInfo("de-DE")),
				FooterTemplate = "MTM_Footer",
				FooterWithCounter = false,
				HasFooter = true,
				HasHeader = false,
				HeaderFirstPageOnly = true,
				HeaderLogoWithCounter = false,
				HeaderLogoWithText = false,
				HeaderText = "",
				Logo = "",
				Rotate = false
			};
			if(_data.Type == 1)
			{
				var listAb = Infrastructure.Data.Access.Views.MTM.View_PrioeinkaufAccess.GetByLagererotId(_data.LagerId, filters);
				reportParams.BodyData = new Psz.Core.MaterialManagement.Reporting.Models.ABNotAvailable().GetData(listAb);
				return ResponseModel<byte[]>.SuccessResponse(await Psz.Core.MaterialManagement.Reporting.IText.GetItextPDF(reportParams));
			}
			else
			{
				var listDiff = Infrastructure.Data.Access.Views.MTM.View_PSZ_Disposition_Ab_Termin_zu_Spat_sqlAccess.GetByLagerortId(_data.LagerId, filters);
				reportParams.BodyData = new Psz.Core.MaterialManagement.Reporting.Models.DispositionDateDifference().GetData(listDiff);
				return ResponseModel<byte[]>.SuccessResponse(await Psz.Core.MaterialManagement.Reporting.IText.GetItextPDF(reportParams));
			}
		}
		public async Task<ResponseModel<byte[]>> ValidateAsync()
		{
			if(_user == null)
			{
				return await ResponseModel<byte[]>.AccessDeniedResponseAsync();
			}
			return await ResponseModel<byte[]>.SuccessResponseAsync();
		}
	}
}