using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class GetPositionInformationHandler: IHandle<GetArticleInformationRequestModel, ResponseModel<GetArticleInformationResponseModel>>
	{
		private int data { get; set; }
		private UserModel user { get; set; }
		public GetPositionInformationHandler(UserModel user, int data)
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
		private ResponseModel<GetArticleInformationResponseModel> Perform(UserModel user, int data)
		{
			var bestellArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data);
			var bestellungEntity = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(bestellArtikel?.Bestellung_Nr ?? -1);
			var inforRahmmen = Infrastructure.Data.Access.Joins.MTM.Order.InfoRahmennummerAccess.GetRahmennummer(bestellArtikel.Artikel_Nr ?? 0, bestellungEntity.Lieferanten_Nr ?? 0, (bestellArtikel?.Anzahl ?? -1), bestellArtikel?.Liefertermin ?? DateTime.Today);
			var currentRahmen = "";
			if(bestellArtikel.RA_Pos_zu_Bestellposition.HasValue && bestellArtikel.RA_Pos_zu_Bestellposition.Value > 0)
			{
				var currentRaPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(bestellArtikel.RA_Pos_zu_Bestellposition.Value);
				var currentRa = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(currentRaPos.AngebotNr ?? -1);
				currentRahmen = $"{currentRa?.Bezug} || Pos.{currentRaPos?.Position}";
			}

			return ResponseModel<GetArticleInformationResponseModel>.SuccessResponse(new GetArticleInformationResponseModel(bestellArtikel, currentRahmen));
		}
		public ResponseModel<GetArticleInformationResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<GetArticleInformationResponseModel>.AccessDeniedResponse();
			}
			var bestellArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data);
			if(bestellArtikel == null)
			{
				return ResponseModel<GetArticleInformationResponseModel>.FailureResponse("Position doesn't exist.");
			}
			return ResponseModel<GetArticleInformationResponseModel>.SuccessResponse();
		}
	}
}