using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	using Psz.Core.Apps.Purchase.Models.DeliveryNote;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetItemsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ItemModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }

		public GetItemsHandler(Identity.Models.UserModel user, int id)
		{
			_user = user;
			_data = id;
		}

		public ResponseModel<List<ItemModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			List<ItemModel> responseBody = null;
			var angeboteneArtikelEntites = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data);
			if(angeboteneArtikelEntites != null && angeboteneArtikelEntites.Count > 0)
			{
				responseBody = new List<ItemModel>();
				var artikelEntites = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(angeboteneArtikelEntites.Select(x => (int)x.ArtikelNr).ToList());
				foreach(var item in angeboteneArtikelEntites)
				{
					var artikelEntity = artikelEntites?.FirstOrDefault(x => x.ArtikelNr == item.ArtikelNr);
					responseBody.Add(new ItemModel
					{
						ItemNumber = artikelEntity.ArtikelNummer,
						Designation1 = item.Bezeichnung1,
						OriginalOrderQuantity = Convert.ToDecimal(item.OriginalAnzahl ?? 0),
						DesiredDate = Convert.ToDateTime(item.Wunschtermin ?? DateTime.MaxValue),
						DeliveredQuantity = Convert.ToDecimal(item.Geliefert ?? 0),
						OpenQuantity_Quantity = Convert.ToDecimal(item.Anzahl ?? 0),
						DeliveryDate = Convert.ToDateTime(item.Liefertermin ?? DateTime.MaxValue),
						ProductionNumber = Convert.ToInt32(item.Fertigungsnummer ?? 0),

						AktuelleLiefermenge = Convert.ToDecimal(item.Anzahl ?? 0),
						Done = Convert.ToBoolean(item.erledigt_pos ?? false),
						Standardversand = item.Versandarten_Auswahl,
						Versandatum = Convert.ToDateTime(item.Versanddatum_Auswahl ?? DateTime.MaxValue),
						termin_eingehalten = Convert.ToBoolean(item.termin_eingehalten ?? false),
						Index_Kunde = item.Index_Kunde,
						Index_Kunde_Datum = item.Index_Kunde_Datum,
					});
				}
				responseBody = responseBody?.OrderBy(o => o.Position).ToList();
			}

			return new ResponseModel<List<ItemModel>>
			{
				Success = true,
				Body = responseBody
			};
		}

		public ResponseModel<List<ItemModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<ItemModel>>.AccessDeniedResponse();
			}

			var deliveryEntitiy = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetDeliveryNotesByNr(this._data);
			if(deliveryEntitiy == null)
			{
				return new ResponseModel<List<ItemModel>>
				{
					Success = false,
					Errors = new List<ResponseModel<List<ItemModel>>.ResponseError>
					{
						new ResponseModel<List<ItemModel>>.ResponseError
						{
							Key ="",
							Value = "Delivery not found"
						}
					}
				};
			}

			return ResponseModel<List<ItemModel>>.SuccessResponse();
		}
	}
}
