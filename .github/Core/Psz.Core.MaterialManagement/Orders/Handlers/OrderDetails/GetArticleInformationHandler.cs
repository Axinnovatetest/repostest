using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class GetArticleInformationHandler: IHandle<GetArticleInformationRequestModel, ResponseModel<GetArticleInformationResponseModel>>
	{
		private GetArticleInformationRequestModel data { get; set; }
		private UserModel user { get; set; }
		public GetArticleInformationHandler(UserModel user, GetArticleInformationRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<GetArticleInformationResponseModel> Handle()
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
		private ResponseModel<GetArticleInformationResponseModel> Perform(UserModel user, GetArticleInformationRequestModel data)
		{
			var bestellArtikel = new Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity();
			if(data.Nr != -1 && data.Nr != 0)
				bestellArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.Nr);

			var LagerorteList = Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.Get(Module.LagerortIds);
			var bestellnummern = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetBySupplierIdArticleId(data.SupplierId, data.ArticleId);

			if(bestellArtikel is not null && bestellArtikel.Nr != 0)
			{
				var inforRahmmen = Infrastructure.Data.Access.Joins.MTM.Order.InfoRahmennummerAccess.GetRahmennummer(data.ArticleId, data.SupplierId, (bestellArtikel?.Anzahl ?? -1), bestellArtikel?.Liefertermin ?? DateTime.Today);
				var currentRahmen = "";
				if(bestellArtikel.RA_Pos_zu_Bestellposition.HasValue && bestellArtikel.RA_Pos_zu_Bestellposition.Value > 0)
				{
					var currentRaPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(bestellArtikel.RA_Pos_zu_Bestellposition.Value);
					var currentRa = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(currentRaPos.AngebotNr ?? -1);
					currentRahmen = $"{currentRa?.Bezug} || Pos.{currentRaPos?.Position}";
				}
				var bestellnummernStaffelpreise = Infrastructure.Data.Access.Tables.MTM.Bestellnummern_StaffelpreiseAccess.GetByBestellnummernNumber(bestellnummern.Nr);

				var response = new GetArticleInformationResponseModel(bestellArtikel, bestellnummernStaffelpreise, LagerorteList, inforRahmmen, bestellnummern, currentRahmen);

				return ResponseModel<GetArticleInformationResponseModel>.SuccessResponse(response);
			}
			else
			{
				var bestellnummernStaffelpreise = Infrastructure.Data.Access.Tables.MTM.Bestellnummern_StaffelpreiseAccess.GetByBestellnummernNumber(bestellnummern?.Nr ?? -1);

				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data.ArticleId);
				var ReplinshementDate = bestellArtikel?.Liefertermin ?? (DateTime.Now.AddDays(bestellnummern.Wiederbeschaffungszeitraum.HasValue ? bestellnummern.Wiederbeschaffungszeitraum.Value : 0));
				var inforRahmmen = Infrastructure.Data.Access.Joins.MTM.Order.InfoRahmennummerAccess.GetRahmennummer(data.ArticleId, data.SupplierId,
					Decimal.TryParse((bestellnummern?.Mindestbestellmenge ?? -1).ToString(), out decimal quantity) ? quantity : 0,
					ReplinshementDate);
				// - 2023-08-25 - CoC
				if(!string.IsNullOrWhiteSpace(article?.CocVersion))
				{
					var cocEntity = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.GetByVersion(article.CocVersion);
					bestellArtikel.CocVersion = $"{(cocEntity?.Count > 0 ? cocEntity[0].Name : "")} {article?.CocVersion}".Trim();
				}
				var response = new GetArticleInformationResponseModel(bestellArtikel, article, bestellnummern, bestellnummernStaffelpreise, inforRahmmen, LagerorteList);

				return ResponseModel<GetArticleInformationResponseModel>.SuccessResponse(response);
			}
		}
		public ResponseModel<GetArticleInformationResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<GetArticleInformationResponseModel>.AccessDeniedResponse();
			}
			if(data.IsEdit)
			{
				var bestellArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.Nr);
				if(bestellArtikel == null)
				{
					return ResponseModel<GetArticleInformationResponseModel>.FailureResponse("Position doesn't exist.");
				}
			}
			return ResponseModel<GetArticleInformationResponseModel>.SuccessResponse();
		}
	}
}