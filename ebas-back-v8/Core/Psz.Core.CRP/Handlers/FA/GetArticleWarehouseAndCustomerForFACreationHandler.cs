using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetArticleWarehouseAndCustomerForFACreationHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.FA.ArticleWarehouseAndCustomerForFACreationModel>>
	{
		private readonly UserModel _user;
		private readonly int _data;

		public GetArticleWarehouseAndCustomerForFACreationHandler(Identity.Models.UserModel user, int data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<ArticleWarehouseAndCustomerForFACreationModel> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data);
				var kreis = articleEntity.ArtikelNummer.Split('-')[0];
				var cutomerNumber = new KeyValuePair<int, string>();
				var warehouse = -1;
				var kreisEntity = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByKundeKreis(kreis);
				if(kreisEntity != null && kreisEntity.Count > 0)
				{
					var adressen = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(kreisEntity[0].Kundennummer ?? -1);
					var kunden = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(adressen?.Nr ?? -1);
					cutomerNumber = new KeyValuePair<int, string>(kunden?.Nummer ?? -1, $"{adressen?.Kundennummer} ||  {adressen?.Name1}");
				}
				var productionSite = new Core.CRP.Handlers.FA.GetFAArticleProductionSiteHandler(_user, _data).Handle();
				if(productionSite.Success)
					warehouse = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetHauptByStandard(productionSite.Body)?.Lagerort_id ?? -1;

				return ResponseModel<ArticleWarehouseAndCustomerForFACreationModel>.SuccessResponse(new ArticleWarehouseAndCustomerForFACreationModel
				{
					CutomerNumber = cutomerNumber,
					Warehouse = warehouse,
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<ArticleWarehouseAndCustomerForFACreationModel> Validate()
		{
			if(_user == null)
				return ResponseModel<ArticleWarehouseAndCustomerForFACreationModel>.AccessDeniedResponse();
			return ResponseModel<ArticleWarehouseAndCustomerForFACreationModel>.SuccessResponse();
		}
	}
}
