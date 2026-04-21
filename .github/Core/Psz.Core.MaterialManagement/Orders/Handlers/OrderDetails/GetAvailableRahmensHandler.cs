using Psz.Core.MaterialManagement.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class GetAvailableRahmensHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<InfoRahmennummerModel>>>
	{

		private InfoRahmenRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetAvailableRahmensHandler(Identity.Models.UserModel user, InfoRahmenRequestModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<InfoRahmennummerModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<InfoRahmennummerModel>();
				var artikelNr = _data.ArtikelNr.HasValue && _data.ArtikelNr.Value != 0
					? _data.ArtikelNr
					: Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(_data.PositionNr.Value).Artikel_Nr ?? -1;
				var BEposition = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(_data.PositionNr.Value);
				var bestellnummern = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetBySupplierIdArticleId(_data.SupplierId, artikelNr ?? -1);
				var ReplinshementDate = _data.ConfirmationDate.HasValue
					? _data.ConfirmationDate.Value
					: DateTime.Now.AddDays(bestellnummern.Wiederbeschaffungszeitraum.HasValue ? bestellnummern.Wiederbeschaffungszeitraum.Value : 0);
				var supplier = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummer(_data.SupplierId);
				var availableRAentities = Infrastructure.Data.Access.Joins.MTM.Order.InfoRahmennummerAccess.GetRahmennummer(artikelNr ?? -1, supplier.Nr, _data.Quantity, ReplinshementDate);
				response = availableRAentities?.Select(x => new InfoRahmennummerModel(x)).ToList();
				if((response == null || response.Count == 0) && BEposition?.RA_Pos_zu_Bestellposition.HasValue == true)
				{
					var RAposition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(BEposition?.RA_Pos_zu_Bestellposition ?? -1);
					var RAPositionExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(BEposition?.RA_Pos_zu_Bestellposition ?? -1);
					var ra = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(RAposition.AngebotNr ?? -1);
					response.Add(new InfoRahmennummerModel
					{
						AngeboteNr = ra.Bezug,
						PositionNr = RAposition?.Nr ?? -1,
						Position = RAposition?.Position ?? -1,
						Anzahl = RAposition?.Anzahl ?? -1,
						ExtensionDate = RAPositionExtension?.GultigBis, // - 2025-08-14 Hejdukova remove ExtDate 
						GultigAb = RAPositionExtension?.GultigAb,
						GultigBis = RAPositionExtension?.GultigBis,
						Nr = ra.Nr
					});
				}
				response = response.OrderBy(x => x.ExtensionDate).ToList();

				// - 
				return ResponseModel<List<InfoRahmennummerModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<InfoRahmennummerModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<InfoRahmennummerModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<InfoRahmennummerModel>>.SuccessResponse();
		}

	}
}
